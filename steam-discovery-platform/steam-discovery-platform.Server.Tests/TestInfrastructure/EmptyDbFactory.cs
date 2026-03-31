using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Tests.TestInfrastructure
{
    /// <summary>
    /// Configures a test web host with an empty in-memory database.
    /// A unique database name is used for each run to ensure data isolation.
    /// </summary>
    public class EmptyDbFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptors = services
                    .Where(d => d.ServiceType == typeof(DbContextOptions<SteamDbContext>))
                    .ToList();

                foreach (var descriptor in descriptors)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<SteamDbContext>(options =>
                {
                    options.UseInMemoryDatabase(Guid.NewGuid().ToString());
                });

                services.AddDbContext<SteamDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestDatabase_" + Guid.NewGuid().ToString());
                });

                var sp = services.BuildServiceProvider();
                using var scope = sp.CreateScope();
                var db = scope.ServiceProvider.GetRequiredService<SteamDbContext>();
                db.Database.EnsureCreated();
            });
        }
    }
}
