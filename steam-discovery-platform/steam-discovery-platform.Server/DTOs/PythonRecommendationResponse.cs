using System.Text.Json.Serialization;

namespace steam_discovery_platform.Server.DTOs
{
    public class PythonRecommendationResponse
    {
        [JsonPropertyName("is_cold_start")]
        public bool IsColdStart { get; set; }

        [JsonPropertyName("base_game")]
        public string BaseGame { get; set; }

        [JsonPropertyName("recommendations")]
        public List<PythonGameRaw> Recommendations { get; set; }
    }
}
