namespace steam_discovery_platform.Server.DTOs
{
    public class UserLibraryGameDTO
    {
        public Guid UserID { get; set; }
        public int Appid { get; set; }
        public bool IsFavorite {  get; set; }
        public DateTime AddedAt { get; set; }
    }
}
