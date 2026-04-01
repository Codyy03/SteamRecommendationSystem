using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Tests.TestInfrastructure;
using System.Net;
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

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

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

        [Fact]
        public async Task GetApplicationsByGenre_ReturnOk()
        {
            var client = new SeededDbFactory().CreateClient();

            var response = await client.GetAsync("/api/applications/getGamesByGenre?genre=action");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

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


        [Fact]
        public async Task GetApplicationsByGenre_ReturnNull()
        {
            var client = new SeededDbFactory().CreateClient();

            var response = await client.GetAsync("/api/applications/getGamesByGenre?genre=asdas");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var content = await response.Content.ReadAsStringAsync();

            var games = JsonSerializer.Deserialize<List<GameInfoDTO>>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(games);
            Assert.Empty(games);
        }

        [Fact]
        public async Task GetApplicationsDetails_ReturnOk()
        {
            var client = new SeededDbFactory().CreateClient();

            var response = await client.GetAsync("/api/applications/getGameDetails?id=10");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var content = await response.Content.ReadAsStringAsync();

            var game = JsonSerializer.Deserialize<GameDetailsDTO>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(game);

            Assert.False(string.IsNullOrEmpty(game.Name));
            Assert.False(string.IsNullOrEmpty(game.HeaderImage));
            Assert.False(string.IsNullOrEmpty(game.Type));
            Assert.False(string.IsNullOrEmpty(game.ShortDescription));
            Assert.True(game.Appid > 0);
            Assert.True(game.FinalPrice > 0);
            Assert.True(game.ReleaseDate > DateOnly.MinValue);
        }
    }
}
