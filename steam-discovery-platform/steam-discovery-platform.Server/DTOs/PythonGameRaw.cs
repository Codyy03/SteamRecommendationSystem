using System.Text.Json.Serialization;

namespace steam_discovery_platform.Server.DTOs
{
    public class PythonGameRaw
    {
        [JsonPropertyName("appid")]
        public int Appid { get; set; }

        [JsonPropertyName("final_score")]
        public double FinalScore { get; set; }
    }
}
