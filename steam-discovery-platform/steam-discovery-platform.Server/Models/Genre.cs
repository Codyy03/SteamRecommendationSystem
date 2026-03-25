using System;
using System.Collections.Generic;

namespace steam_discovery_platform.Server.Models;

public partial class Genre
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Application> Apps { get; set; } = new List<Application>();
}
