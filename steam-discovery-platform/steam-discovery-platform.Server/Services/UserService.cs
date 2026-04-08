using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Helpers;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Numerics;
using steam_discovery_platform.Server.Validation;

namespace steam_discovery_platform.Server.Services
{
    public class UserService : IUserService
    {
        readonly SteamdbContext context;
        readonly JwtTokenHelper jwtHelper;
        DataValidation dataValidation = new DataValidation();

        public UserService(SteamdbContext context, JwtTokenHelper jwtHelper)
        {
            this.context = context;
            this.jwtHelper = jwtHelper;
        }

        public async Task<User> CreateUser(UserRegisterDTO userRegisterDTO)
        {
            var nameErrors = dataValidation.ValidateName(userRegisterDTO.userName);
            if (nameErrors.Any())
                throw new ValidationException(string.Join(" ", nameErrors));

            var emailErrors = dataValidation.ValidateEmail(userRegisterDTO.Email);
            if (emailErrors.Any())
                throw new ValidationException(string.Join(" ", emailErrors));

            var passwordErrors = dataValidation.ValidatePassword(userRegisterDTO.Password);
            if (passwordErrors.Any())
                throw new ValidationException(string.Join(" ", passwordErrors));

            if (await context.Users.AnyAsync(u => u.Username == userRegisterDTO.userName))
                throw new ValidationException("User name must be unique");

            if (await context.Users.AnyAsync(u => u.Email == userRegisterDTO.Email))
                throw new ValidationException("Email must be unique");

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

        public async Task<UserDTO> GetMe(Guid userId)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null) throw new ValidationException("User not found");

            return new UserDTO
            {
                CreatedAt = user.CreatedAt,
                Username = user.Username,
                Email = user.Email,
                Role = user.Role
            };
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
                throw new ValidationException("User do not exists");

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
                throw new ValidationException("Invalid username or password");
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

        public async Task<UserDTO> UpdateUser(UpdateUserDTO updateUserDTO)
        {
            var existing = await context.Users.FindAsync(updateUserDTO.UserId);

            if (existing == null)
                throw new Exception("Error");

            var NameErrors = dataValidation.ValidateName(updateUserDTO.UserName);

            if (NameErrors.Any())
                throw new ValidationException(string.Join(" ", NameErrors));

            var EmailErrors = dataValidation.ValidateEmail(updateUserDTO.Email);

            if (EmailErrors.Any())
                throw new ValidationException(string.Join(" ", EmailErrors));

            existing.Username = updateUserDTO.UserName;
            existing.Email = updateUserDTO.Email;

            await context.SaveChangesAsync();

            return new UserDTO
            {
                Username = existing.Username,
                CreatedAt = existing.CreatedAt,
                Email = existing.Email,
            };
        }
    }
}
