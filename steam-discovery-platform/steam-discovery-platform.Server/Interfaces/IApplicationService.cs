using Microsoft.AspNetCore.Mvc;
using steam_discovery_platform.Server.DTOs;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IApplicationService
    {
        Task<List<GameInfoDTO>> GetTopGamesAsync(int count);
        Task<List<GameInfoDTO>> GetGamesByGenreAsync(int count, string genre);

        Task<GameDetailsDTO> GetGameDetails(int id, Guid userId);
    }
}
