using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Tests.TestInfrastructure;
using System.Net;
using System.Text.Json;

namespace steam_discovery_platform.Server.Tests.IntegrationTests
{
    /// <summary>
    /// Contains integration tests for the ApplicationsController, 
    /// verifying the correctness of API endpoints using a seeded in-memory database.
    /// </summary>
    public class ApplicationsControllerTests
    {
        /// <summary>
        /// Verifies that the GetGames endpoint returns a successful 200 OK response 
        /// with a valid list of game applications.
        /// </summary>
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

        /// <summary>
        /// Verifies that filtering applications by a valid genre returns a 200 OK response 
        /// and a list of games matching the specified criteria.
        /// </summary>
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

        /// Verifies that providing a non-existent or invalid genre returns a 200 OK response 
        /// with an empty list, ensuring the API handles zero results gracefully.
        /// </summary>
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

        /// <summary>
        /// Verifies that the getGameDetails endpoint returns detailed information 
        /// for a specific game ID, including prices and descriptions.
        /// </summary>
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
