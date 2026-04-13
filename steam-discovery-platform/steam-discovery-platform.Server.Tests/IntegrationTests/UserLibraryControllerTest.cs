using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Models;
using steam_discovery_platform.Server.Services;
using steam_discovery_platform.Server.Tests.TestInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace steam_discovery_platform.Server.Tests.IntegrationTests
{
    public class UserLibraryControllerTest
    {
        [Fact]
        public async Task GetGameStatus_ReturnOK()
        {
            var factory = new SeededDbFactory();
            var client = factory.CreateClient();

            var userId = "00000000-0000-0000-0000-000000000001";
            var token = TestJwtTokenHelper.GenerateTestToken(userId, "admin@test.pl", "admin_test", "Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.PutAsync("/api/usersLibrary/updateFavoriteGame/10?isFavorite=false", null);
            // Assert
            response.EnsureSuccessStatusCode();

            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SteamdbContext>();

            var libraryEntry = await dbContext.UserLibraries
                .FirstOrDefaultAsync(ul => ul.UserId == Guid.Parse(userId) && ul.Appid == 10);

            // Upewniamy się, że gra tam jest i status IsFavorite faktycznie zmienił się na false
            libraryEntry.Should().NotBeNull();
            libraryEntry!.IsFavorite.Should().BeFalse();

        }

        [Fact]
        public async Task GetUserLibraryGames_ReturnOk()
        {
            var factory = new SeededDbFactory();
            var client = factory.CreateClient();

            var userId = "00000000-0000-0000-0000-000000000001";
            var token = TestJwtTokenHelper.GenerateTestToken(userId, "admin@test.pl", "admin_test", "Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/userLibrary/userLibrary");

            response.EnsureSuccessStatusCode();
            var games = await response.Content.ReadFromJsonAsync<List<UserLibraryGameDTO>>();

            // Weryfikacja
            games.Should().NotBeNull();
            games.Should().HaveCount(1);

            var libraryGame = games!.First();
            libraryGame.Game.Appid.Should().Be(10);
            libraryGame.Game.Name.Should().Be("Cyberpunk 2077");
            libraryGame.IsFavorite.Should().BeTrue(); 

            // Sprawdzamy czy JOIN z gatunkami zadziałał
            libraryGame.Genres.Should().Contain("RPG");
            libraryGame.Genres.Should().Contain("Action");
        }

        [Fact]
        public async Task AddGameToUserLibrary_ReturnsNoContent_AndAddsRecordToDb()
        {
            // 1. Arrange
            var factory = new SeededDbFactory();
            var client = factory.CreateClient();

            var userId = "00000000-0000-0000-0000-000000000001"; // ID Admina
            var token = TestJwtTokenHelper.GenerateTestToken(userId, "admin@test.pl", "admin_test", "Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var newGameDto = new UserGameDTO { Appid = 20 }; // Chcemy dodać CS2

            // 2. Act
            var response = await client.PostAsJsonAsync("/api/UserLibrary/addGameToLibrary", newGameDto);

            // 3. Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // 4. Verify in DB
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SteamdbContext>();

            var exists = await dbContext.UserLibraries
                .AnyAsync(ul => ul.UserId == Guid.Parse(userId) && ul.Appid == 20);

            exists.Should().BeTrue();
        }

        [Fact]
        public async Task RemoveGameFromLibrary_ReturnsNoContent_AndRemovesRecordFromDb()
        {
            // 1. Arrange
            var factory = new SeededDbFactory();
            var client = factory.CreateClient();

            var userId = "00000000-0000-0000-0000-000000000001";
            var token = TestJwtTokenHelper.GenerateTestToken(userId, "admin@test.pl", "admin_test", "Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var appidToRemove = 10; // Cyberpunk

            // 2. Act
            var response = await client.DeleteAsync($"/api/usersLibrary/delete/{appidToRemove}");

            // 3. Assert
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            // 4. Verify in DB
            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SteamdbContext>();

            var exists = await dbContext.UserLibraries
                .AnyAsync(ul => ul.UserId == Guid.Parse(userId) && ul.Appid == appidToRemove);

            exists.Should().BeFalse();
        }
    }
}
