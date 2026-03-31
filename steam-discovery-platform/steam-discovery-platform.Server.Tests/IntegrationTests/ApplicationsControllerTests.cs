using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Tests.TestInfrastructure;
using System.Text.Json;

namespace steam_discovery_platform.Server.Tests.IntegrationTests
{
    public class ApplicationsControllerTests
    {
        [Fact]
        public async Task GetApplications_ReturnOk()
        {
            var client = new SeededDbFactory().CreateClient();

            var response = await client.GetAsync("/api/applications/getGames");

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var content = await response.Content.ReadAsStringAsync();

            var games = JsonSerializer.Deserialize<List<GameInfoDTO>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(games);

            Assert.All(games, g =>
            {
                Assert.False(string.IsNullOrEmpty(g.Name));
                Assert.False(string.IsNullOrEmpty(g.HeaderImage));
                Assert.False(string.IsNullOrEmpty(g.Type));
                Assert.True(g.Appid > 0);
            });
        }
    }
}