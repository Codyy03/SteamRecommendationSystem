namespace steam_discovery_platform.Server.DTOs
{
    public class UserLibraryGameDTO
    {
        public bool? IsFavorite {  get; set; }
        public DateTime? AddedAt { get; set; }
        public GameInfoDTO Game { get; set; }
    }
}
