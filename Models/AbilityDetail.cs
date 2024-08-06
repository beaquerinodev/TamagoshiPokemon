using System.Text.Json.Serialization;

namespace Models
{
    public class AbilityDetail
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
