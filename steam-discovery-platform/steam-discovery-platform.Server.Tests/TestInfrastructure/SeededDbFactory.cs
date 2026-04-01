using System;
using System.Linq; // Wymagane dla metod rozszerzeń LINQ
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using steam_discovery_platform.Server.Models;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
namespace steam_discovery_platform.Server.Tests.TestInfrastructure
{
    public class SeededDbFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                // 1. Kompleksowe czyszczenie wszystkich opcji przypisanych przez providera Npgsql
                var descriptorsToRemove = services.Where(d =>
                    d.ServiceType == typeof(DbContextOptions<SteamDbContext>) ||
                    d.ServiceType == typeof(DbContextOptions) ||
                    (d.ServiceType.FullName != null && d.ServiceType.FullName.Contains("IDbContextOptionsConfiguration"))).ToList();

                foreach (var descriptor in descriptorsToRemove)
                {
                    services.Remove(descriptor);
                }

                // Dodatkowo zabezpieczamy się usunięciem domyślnego obiektu DbConnection, jeśli taki został
                var dbConnectionDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(System.Data.Common.DbConnection));
                if (dbConnectionDescriptor != null)
                {
                    services.Remove(dbConnectionDescriptor);
                }

                // 2. Unikalna nazwa bazy dla każdej instancji Factory rozwiązuje błędy przy testach współbieżnych
                string uniqueDbName = $"TestDb_{Guid.NewGuid()}";

                services.AddDbContext<SteamDbContext>(options =>
                {
                    options.UseInMemoryDatabase(uniqueDbName);
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SteamDbContext>();

                // 3. Reset bazy i seedowanie danych (ta część zostaje bez zmian)
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
            });
        }
    }
}
