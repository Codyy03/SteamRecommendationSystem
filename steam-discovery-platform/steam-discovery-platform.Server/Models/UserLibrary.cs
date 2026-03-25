using System;
using System.Collections.Generic;

namespace steam_discovery_platform.Server.Models;

public partial class UserLibrary
{
    public Guid UserId { get; set; }

    public int Appid { get; set; }

    public bool? IsFavorite { get; set; }

    public DateTime? AddedAt { get; set; }

    public virtual Application App { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
