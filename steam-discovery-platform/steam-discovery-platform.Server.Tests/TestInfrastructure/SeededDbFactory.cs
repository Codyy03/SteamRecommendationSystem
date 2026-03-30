using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using steam_discovery_platform.Server.Models;
namespace MediCare.Server.Tests.TestInfrastructure
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
                var descriptor = services.SingleOrDefault(
                    d => d.ServiceType == typeof(DbContextOptions<SteamDbContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                services.AddDbContext<SteamDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDb");
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SteamDbContext>();
                db.Database.EnsureDeleted();
                db.Database.EnsureCreated();

                db.ChangeTracker.Clear();

                db.Applications.RemoveRange(db.Applications);


                db.SaveChanges();


                db.Applications.AddRange(
                 new Application
                 {
                     Appid = 10,
                     Name = "Cyberpunk 2077",
                     Type = "game",
                     IsFree = false,
                     ReleaseDate = new DateOnly(2020, 12, 10),
                     ShortDescription = "Cyberpunk 2077 is an open-world, action-adventure RPG set in the megalopolis of Night City, where you play as a cyberpunk mercenary wrapped up in a do-or-die fight for survival.",
                     HeaderImage = "https://cdn.akamai.steamstatic.com/steam/apps/1091500/header.jpg",
                     MetacriticScore = 86,
                     RecommendationsTotal = 650000,
                     FinalPrice = 19900,
                     Currency = "PLN",
                     SupportsWindows = true,
                     SupportsMac = false,
                     SupportsLinux = true,
                     CreatedAt = DateTime.UtcNow,
                     PcRequirements = "{\"os_min\": \"Windows 10\", \"os_rec\": \"Windows 11\", \"memory_min\": \"12 GB RAM\", \"graphics_min\": \"GTX 1060\", \"processor_min\": \"Core i5-3570K\", \"processor_rec\": \"Core i7-12700K\"}",

                     Developers = new List<Developer> { new Developer { Name = "CD PROJEKT RED" } },
                     Publishers = new List<Publisher> { new Publisher { Name = "CD PROJEKT RED" } },
                     Genres = new List<Genre> {
                        new Genre { Name = "RPG" },
                        new Genre { Name = "Action" }
                     },
                     Categories = new List<Category> {
                        new Category { Name = "Single-player" },
                        new Category { Name = "Steam Achievements" },
                        new Category { Name = "Full controller support" }
                     }
                 },
                 new Application
                 {
                     Appid = 20,
                     Name = "Counter-Strike 2",
                     Type = "game",
                     IsFree = true,
                     ReleaseDate = new DateOnly(2023, 09, 27),
                     ShortDescription = "For over two decades, Counter-Strike has offered an elite competitive experience, one shaped by millions of players from across the globe.",
                     HeaderImage = "https://cdn.akamai.steamstatic.com/steam/apps/730/header.jpg",
                     MetacriticScore = 82,
                     RecommendationsTotal = 7500000,
                     FinalPrice = 0,
                     Currency = "USD",
                     SupportsWindows = true,
                     SupportsMac = false,
                     SupportsLinux = true,
                     CreatedAt = DateTime.UtcNow,
                     PcRequirements = "{\"os_min\": \"Windows 10\", \"memory_min\": \"8 GB RAM\", \"graphics_min\": \"GTX 660\", \"processor_min\": \"Core i5-2500K\"}",

                     Developers = new List<Developer> { new Developer { Name = "Valve" } },
                     Publishers = new List<Publisher> { new Publisher { Name = "Valve" } },
                     Genres = new List<Genre> {
                         new Genre { Name = "Action" },
                         new Genre { Name = "Free to Play" }
                     },

                     Categories = new List<Category> {
                        new Category { Name = "Multi-player" },
                        new Category { Name = "Cross-Platform Multiplayer" },
                        new Category { Name = "Steam Workshop" },
                        new Category { Name = "In-App Purchases" }
                     }
                 }
             );

                db.SaveChanges();
            });
        }
    }
}
