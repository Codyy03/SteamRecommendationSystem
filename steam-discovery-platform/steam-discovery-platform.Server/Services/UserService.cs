using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Interfaces;
using steam_discovery_platform.Server.Models;
using System.Numerics;

namespace steam_discovery_platform.Server.Services
{
    public class UserService : IUserService
    {
        readonly SteamDbContext context;

        public UserService(SteamDbContext context)
        {
            this.context = context;
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
    }
}
