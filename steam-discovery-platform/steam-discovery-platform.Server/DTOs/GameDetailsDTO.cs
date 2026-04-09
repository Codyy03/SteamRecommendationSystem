namespace steam_discovery_platform.Server.DTOs
{
    public class GameDetailsDTO
    {
        public int Appid { get; set; }

        public string Name { get; set; } = null!;

        public string? Type { get; set; }

        public bool? IsFree { get; set; }

        public DateOnly? ReleaseDate { get; set; }

        public string? ShortDescription { get; set; }

        public string? HeaderImage { get; set; }

        public short? MetacriticScore { get; set; }

        public int? RecommendationsTotal { get; set; }

        public int? FinalPrice { get; set; }

        public string? Currency { get; set; }

        public bool? SupportsWindows { get; set; }

        public bool? SupportsMac { get; set; }

        public bool? SupportsLinux { get; set; }

        public string? PcRequirements { get; set; }
        public DateTime? CreatedAt { get; set; }
        public string? Developers { get; set; }
        public string? Publishers { get; set; }
        public string? Categories { get; set; }
        public string? Genres { get; set; }
        public bool IsInLibrary { get; set; }
    }
}
