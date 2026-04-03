using System;
using System.Collections.Generic;

namespace steam_discovery_platform.Server.Models;

public partial class Application
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

    public string? Background { get; set; }

    public virtual ICollection<UserLibrary> UserLibraries { get; set; } = new List<UserLibrary>();

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Developer> Developers { get; set; } = new List<Developer>();

    public virtual ICollection<Genre> Genres { get; set; } = new List<Genre>();

    public virtual ICollection<Platform> Platforms { get; set; } = new List<Platform>();

    public virtual ICollection<Publisher> Publishers { get; set; } = new List<Publisher>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
