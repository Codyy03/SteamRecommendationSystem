using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Moq;
using Moq.Protected;
using steam_discovery_platform.Server.Controllers;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;
using steam_discovery_platform.Server.Services;
using System.Net;
using System.Net.Http.Json;
namespace steam_discovery_platform.Server.Tests.IntegrationTests
{
    public class PythonRecommendationControllerTests
    {
        [Fact]
        public async Task GetPythonRecommendationGames_ReturnsOk_WhenGamesFound()
        {
            // Arrange
            var mockService = new Mock<IPythonRecommendationService>();
            var expectedGames = new List<GameInfoDTO> { new GameInfoDTO { Appid = 1, Name = "Test Game" } };

            mockService.Setup(s => s.GetRecommendationsAsync(It.IsAny<string>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<float>(), It.IsAny<int>()))
                       .ReturnsAsync(expectedGames);

            var controller = new PythonRecommendationController(mockService.Object);

            // Act
            var result = await controller.GetPythonRecommendationGames("Cyberpunk", 0.4f, 0.3f, 0.15f, 5);

            // Assert
            var okResult = result.Result.Should().BeOfType<OkObjectResult>().Subject;
            var model = okResult.Value.Should().BeAssignableTo<List<GameInfoDTO>>().Subject;
            model.Should().HaveCount(1);
            model[0].Name.Should().Be("Test Game");
        }

        [Fact]
        public async Task GetRecommendationsAsync_ReturnsMappedGames_FromMockedPython()
        {
            // 1. Setup InMemory Database
            var options = new DbContextOptionsBuilder<SteamDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb")
                .Options;

            using var context = new SteamDbContext(options);
            context.Applications.Add(new Application { Appid = 10, Name = "Witcher 3", HeaderImage = "img" });
            await context.SaveChangesAsync();

            // 2. Mock HttpClient
            var handlerMock = new Mock<HttpMessageHandler>();
            var response = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = JsonContent.Create(new { recommendations = new[] { new { appid = 10 } } })
            };

            handlerMock.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response);

            var httpClient = new HttpClient(handlerMock.Object);
            var service = new PythonRecommendationService(httpClient, null, context);

            // 3. Act
            var result = await service.GetRecommendationsAsync("Witcher", 0.5f, 0.5f, 0.5f, 1);

            // 4. Assert
            result.Should().NotBeEmpty();
            result[0].Appid.Should().Be(10);
            result[0].Name.Should().Be("Witcher 3");
        }
    }
}
