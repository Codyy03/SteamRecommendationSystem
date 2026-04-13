using FluentAssertions;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Models;
using steam_discovery_platform.Server.Tests.TestInfrastructure;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;

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

        [Fact]
        public async Task GetMe_Unauthorized_WithoutToken()
        {
            var client = new SeededDbFactory().CreateClient();

            var response = await client.GetAsync("/api/users/me");

            Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
        }

        [Fact]
        public async Task GetMe_ReturnUser_WhenAuthorized()
        {
            var factory = new SeededDbFactory();
            var client = factory.CreateClient();

            var token = TestJwtTokenHelper.GenerateTestToken("00000000-0000-0000-0000-000000000001", "admin@test.pl", "admin_test", "Admin");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/users/me");

            // Assert
            response.EnsureSuccessStatusCode();

            var dto = await response.Content.ReadFromJsonAsync<UserDTO>();

            Assert.NotNull(dto);
            Assert.Equal("admin_test", dto.Username);
            Assert.Equal("admin@test.pl", dto.Email);
            Assert.Equal("Admin", dto.Role);
        }

        [Fact]
        public async Task UpdateUser_ReturnOk()
        {
            var factory = new SeededDbFactory();
            var client = factory.CreateClient();

            var token = TestJwtTokenHelper.GenerateTestToken("00000000-0000-0000-0000-000000000001", "admin@test.pl", "admin_test", "Admin");
            client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue("Bearer", token);

            var response = await client.GetAsync("/api/users/me");
            response.EnsureSuccessStatusCode();

            var dto = await response.Content.ReadFromJsonAsync<UserDTO>();
            Assert.NotNull(dto);

            var updateDTO = new UpdateUserDTO
            {
                UserId = dto.Id,
                UserName = "asddd",
                Email = "new@email.com"
            };

            var updateResponse = await client.PutAsJsonAsync($"/api/users/update", updateDTO);

            Assert.Equal(HttpStatusCode.OK, updateResponse.StatusCode);

            var updatedData = await updateResponse.Content.ReadFromJsonAsync<UserDTO>();
            Assert.NotNull(updatedData);
            Assert.Equal("asddd", updatedData!.Username);
            Assert.Equal("new@email.com", updatedData!.Email);
        }

        [Fact]
        public async Task ChangePassword_ReturnOk()
        {
            var factory = new SeededDbFactory();
            var client = factory.CreateClient();

            var userId = "00000000-0000-0000-0000-000000000001";
            var token = TestJwtTokenHelper.GenerateTestToken(userId, "admin@test.pl", "admin_test", "Admin");
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var changePasswordDto = new ChangePasswordDto
            {
                Password = "Admin123!", // Stare hasło z SeededDbFactory
                NewPassword = "NewSecurePassword123!" // Nowe hasło, które chcemy ustawić
            };

            var response = await client.PutAsJsonAsync("/api/users/passwordReset", changePasswordDto);
            response.StatusCode.Should().Be(HttpStatusCode.NoContent);

            using var scope = factory.Services.CreateScope();
            var dbContext = scope.ServiceProvider.GetRequiredService<SteamdbContext>();

            // Musimy pobrać użytkownika PONOWNIE z bazy, żeby mieć świeże dane po SaveChangesAsync()
            var userFromDb = await dbContext.Users.FindAsync(Guid.Parse(userId));

            var hasher = new PasswordHasher<User>();
            var verificationResult = hasher.VerifyHashedPassword(userFromDb!, userFromDb!.PasswordHash, "NewSecurePassword123!");

            verificationResult.Should().Be(PasswordVerificationResult.Success);

        }
    }
}
