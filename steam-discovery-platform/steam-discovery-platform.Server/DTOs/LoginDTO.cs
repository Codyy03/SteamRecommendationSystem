using System.Text.Json.Serialization;

namespace steam_discovery_platform.Server.DTOs
{
    public class LoginDTO
    {
        [JsonPropertyName("userName")]
        public string UserName { get; set; }
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
