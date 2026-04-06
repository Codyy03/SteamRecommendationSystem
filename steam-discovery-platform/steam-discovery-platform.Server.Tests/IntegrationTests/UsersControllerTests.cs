using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Tests.TestInfrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace steam_discovery_platform.Server.Tests.IntegrationTests
{
    public class UsersControllerTests
    {
        [Fact]
        public async Task GetUserReturn_OK()
        {
            var clinet = new SeededDbFactory().CreateClient();

            var response = await clinet.GetAsync("/api/users/getUser?id=00000000-0000-0000-0000-000000000001");

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            Assert.Equal("application/json; charset=utf-8", response.Content.Headers.ContentType?.ToString());

            var content = await response.Content.ReadAsStringAsync();

            var user = JsonSerializer.Deserialize<UserDTO>(content,
                new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

            Assert.NotNull(user);

            Assert.False(string.IsNullOrEmpty(user.Email));
            Assert.False(string.IsNullOrEmpty(user.Username));
            Assert.True(user.CreatedAt >  DateTime.MinValue);
        }

        [Fact]
        public async Task CreateUser_ReturnCreated()
        {
            var client = new SeededDbFactory().CreateClient();

            var dto = new UserRegisterDTO
            {
                Email = "ada2@email.com",
                userName = "Test",
                Password = "123456"
            };

            var response = await client.PostAsJsonAsync("/api/users/register", dto);

            Assert.Equal(HttpStatusCode.Created, response.StatusCode);

            var created = await response.Content.ReadFromJsonAsync<UserDTO>();

            Assert.NotNull(created);
            Assert.Equal("Test", created!.Username);
            Assert.Equal("ada2@email.com", created.Email);
        }

        [Fact]
        public async Task Login_ShouldReturnTokens_WhenCredentialsAreValid()
        {
            var client = new SeededDbFactory().CreateClient();

            var loginDto = new { UserName = "admin_test", Password = "Admin123!" };

            var response = await client.PostAsJsonAsync("/api/Users/login", loginDto);

            // Assert
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadFromJsonAsync<LoginResponseDTO>();

            Assert.NotNull(content.AccessToken);
            Assert.Equal("admin_test", content.Username);
        }

        [Fact]
        public async Task Login_ShouldReturnUnauthorized_WhenPasswordIsWrong()
        {
            var client = new SeededDbFactory().CreateClient();

            var loginDto = new { UserName = "admin_test", Password = "WrongPassword" };

            var response = await client.PostAsJsonAsync("/api/Users/login", loginDto);

            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }
    }
}
