using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Helpers;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Numerics;

namespace steam_discovery_platform.Server.Services
{
    public class UserService : IUserService
    {
        readonly SteamdbContext context;
        readonly JwtTokenHelper jwtHelper;

        public UserService(SteamdbContext context, JwtTokenHelper jwtHelper)
        {
            this.context = context;
            this.jwtHelper = jwtHelper;
        }

        public async Task<User> CreateUser(UserRegisterDTO userRegisterDTO)
        {
            if (await context.Users.AnyAsync(u => u.Username == userRegisterDTO.userName))
                throw new Exception("User name must be unique");

            if (await context.Users.AnyAsync(u => u.Email == userRegisterDTO.Email))
                throw new Exception("Email must be unique");

            var hasher = new PasswordHasher<User>();

            User user = new User
            {
                Email = userRegisterDTO.Email,
                PasswordHash = hasher.HashPassword(null!, userRegisterDTO.Password),
                CreatedAt = DateTime.UtcNow,
                Username = userRegisterDTO.userName
            };

            context.Add(user);
            await context.SaveChangesAsync();

            return user;
        }

        public async Task<UserDTO> GetUser(Guid id)
        {
            UserDTO? userDTO = await context.Users
                .Where(u => u.Id == id)
                .Select(u => new UserDTO
                {
                    Username = u.Username,
                    CreatedAt = u.CreatedAt,
                    Email = u.Email
                }).FirstOrDefaultAsync();

            if (userDTO == null)
                throw new Exception("User do not exists");

            return userDTO;
        }

        public async Task<LoginResponseDTO> Login(LoginDTO loginDTO)
        {
            User? user = await context.Users.FirstOrDefaultAsync(u => u.Username == loginDTO.UserName);

            if (user == null) throw new Exception("Invalid credentials");

            PasswordHasher<User> hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password);

            if (user == null || hasher.VerifyHashedPassword(user, user.PasswordHash, loginDTO.Password) == PasswordVerificationResult.Failed)
            {
                throw new Exception("Invalid username or password");
            }

            var oldTokens = context.RefreshTokens.Where(rt => rt.UserId == user.Id);
            context.RefreshTokens.RemoveRange(oldTokens);

            var accessToken = jwtHelper.GenerateJwtToken(user.Id.ToString(), user.Email, user.Username, user.Role);
            var refreshTokenValue = jwtHelper.GenerateRefreshToken();

            var refreshTokenEntity = new RefreshToken
            {
                Id = Guid.NewGuid(),
                Token = refreshTokenValue,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                UserId = user.Id
            };

            context.RefreshTokens.Add(refreshTokenEntity);
            await context.SaveChangesAsync();

            return new LoginResponseDTO
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue,
                Username = user.Username
            };
        }
    }
}
