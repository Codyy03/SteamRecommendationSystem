using steam_discovery_platform.Server.DTOs;
using steam_discovery_platform.Server.Models;

namespace steam_discovery_platform.Server.Interfaces
{
    public interface IUserLibraryService
    {
        /// <summary>
        /// Adds a new game to the user's personal library collection with the current timestamp.
        /// </summary>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="userGameDTO">Data transfer object containing the Steam AppID of the game.</param>
        /// <returns>The created UserLibrary entity record.</returns>
        public Task<UserLibrary> AddGameToLibrary(Guid userId, UserGameDTO userGameDTO);

        /// <summary>
        /// Retrieves the complete list of games in the user's library, joining data with the main Applications table 
        /// to include game details like names, images, and genres.
        /// </summary>
        /// <param name="userId">The unique identifier of the user whose library is being requested.</param>
        /// <returns>A list of enriched UserLibraryGameDTO objects.</returns>
        public Task<List<UserLibraryGameDTO>> GetUserLibraryGames(Guid userId);

        /// <summary>
        /// Permanently removes a specific game from the user's library.
        /// </summary>
        /// <param name="appid">The Steam Application ID of the game to be removed.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        public Task RemoveGameFromLibrary(int appid, Guid userId);

        /// <summary>
        /// Updates the 'Favorite' status of a game within the user's library.
        /// </summary>
        /// <param name="appid">The Steam Application ID of the game.</param>
        /// <param name="userId">The unique identifier of the user.</param>
        /// <param name="isFavorite">The new status to set (true for favorite, false otherwise).</param>
        public Task ChangeGameFavoriteStatus(int appid, Guid userId, bool isFavorite);
    }
}
