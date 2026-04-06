using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using steam_discovery_platform.Server.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace steam_discovery_platform.Server.Tests.TestInfrastructure
{
    /// <summary>
    /// A custom <see cref="WebApplicationFactory{TEntryPoint}"/> that configures
    /// an in-memory database named "TestDb" and pre-populates it with sample
    /// data for integration testing.
    /// The database is cleared before seeding to ensure a consistent state
    /// across tests.
    /// </summary>
    public class SeededDbFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. Comprehensive cleanup of all options assigned by the Npgsql provider
                var descriptorsToRemove = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<SteamdbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    (d.ServiceType.FullName != null && d.ServiceType.FullName.Contains("IDbContextOptionsConfiguration"))).ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                // Additionally, we protect ourselves by removing the default DbConnection object, if any
                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(System.Data.Common.DbConnection));
                if (dbConnectionDescriptor != null)
                {
                    services.Remove(dbConnectionDescriptor);
                }

                // 2. Unique database name for each Factory instance solves concurrent test errors
                string uniqueDbName = $"TestDb_{Guid.NewGuid()}";

                services.AddDbContext<SteamdbContext>(options =>
                {
                    options.UseInMemoryDatabase(uniqueDbName);
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SteamdbContext>();

                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.ChangeTracker.Clear();
                db.Applications.RemoveRange(db.Applications);
                db.SaveChanges();

                var action = new Genre { Id = 1, Name = "Action" };
                var rpg = new Genre { Id = 2, Name = "RPG" };
                var freeToPlay = new Genre { Id = 3, Name = "Free to Play" };

                var single = new Category { Id = 1, Name = "Single-player" };
                var achievements = new Category { Id = 2, Name = "Steam Achievements" };
                var controller = new Category { Id = 3, Name = "Full controller support" };
                var multi = new Category { Id = 4, Name = "Multi-player" };
                var crossPlatform = new Category { Id = 5, Name = "Cross-Platform Multiplayer" };
                var workshop = new Category { Id = 6, Name = "Steam Workshop" };
                var inApp = new Category { Id = 7, Name = "In-App Purchases" };

                var app1 = new Application
                {
                    Appid = 10,
                    Name = "Cyberpunk 2077",
                    Type = "game",
                    IsFree = false,
                    ReleaseDate = new DateOnly(2020, 12, 10),
                    ShortDescription = "Cyberpunk...",
                    HeaderImage = "https://cdn.akamai.steamstatic.com/steam/apps/1091500/header.jpg",
                    MetacriticScore = 86,
                    RecommendationsTotal = 650000,
                    FinalPrice = 19900,
                    Currency = "PLN",
                    SupportsWindows = true,
                    SupportsMac = false,
                    SupportsLinux = true,
                    CreatedAt = DateTime.UtcNow,

                    Developers = new List<Developer> { new Developer { Id = 1, Name = "CD PROJEKT RED" } },
                    Publishers = new List<Publisher> { new Publisher { Id = 1, Name = "CD PROJEKT RED" } }
                };

                app1.Genres.Add(rpg);
                app1.Genres.Add(action);
                app1.Categories.Add(single);
                app1.Categories.Add(achievements);
                app1.Categories.Add(controller);

                var app2 = new Application
                {
                    Appid = 20,
                    Name = "Counter-Strike 2",
                    Type = "game",
                    IsFree = true,
                    ReleaseDate = new DateOnly(2023, 09, 27),
                    ShortDescription = "CS2...",
                    HeaderImage = "https://cdn.akamai.steamstatic.com/steam/apps/730/header.jpg",
                    MetacriticScore = 82,
                    RecommendationsTotal = 7500000,
                    FinalPrice = 0,
                    Currency = "USD",
                    SupportsWindows = true,
                    SupportsMac = false,
                    SupportsLinux = true,
                    CreatedAt = DateTime.UtcNow,

                    Developers = new List<Developer> { new Developer { Id = 2, Name = "Valve" } },
                    Publishers = new List<Publisher> { new Publisher { Id = 2, Name = "Valve" } }
                };

                app2.Genres.Add(action);
                app2.Genres.Add(freeToPlay);
                app2.Categories.Add(multi);
                app2.Categories.Add(crossPlatform);
                app2.Categories.Add(workshop);
                app2.Categories.Add(inApp);

                db.Applications.AddRange(app1, app2);
                db.SaveChanges();

                var passwordHasher = new Microsoft.AspNetCore.Identity.PasswordHasher<User>();

                Guid user1Id = new Guid("00000000-0000-0000-0000-000000000001");
                var adminUser = new User
                {
                    Id = user1Id,
                    Username = "admin_test",
                    Email = "admin@test.pl",
                    Role = "Admin",
                    CreatedAt = DateTime.UtcNow
                };
                adminUser.PasswordHash = passwordHasher.HashPassword(adminUser, "Admin123!");

                var user2Id = Guid.NewGuid();
                var regularUser = new User
                {
                    Id = user2Id,
                    Username = "user_test",
                    Email = "user@test.pl",
                    Role = "User",
                    CreatedAt = DateTime.UtcNow
                };
                regularUser.PasswordHash = passwordHasher.HashPassword(regularUser, "User123!");

                db.Users.AddRange(adminUser, regularUser);
                db.SaveChanges();
            });
        }
    }
}
