namespace steam_discovery_platform.Server.DTOs
{
    public class GameInfoDTO
    {
        public int Appid { get; set; }

        public string Name { get; set; } = null!;

        public string? Type { get; set; }
    }
}
