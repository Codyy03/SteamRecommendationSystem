using System;
using System.Collections.Generic;

namespace steam_discovery_platform.Server.Models;

public partial class User
{
    public Guid Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string PasswordHash { get; set; } = null!;

    public DateTime? CreatedAt { get; set; }

    public virtual ICollection<UserLibrary> UserLibraries { get; set; } = new List<UserLibrary>();

    public virtual ICollection<Application> Apps { get; set; } = new List<Application>();
}
