using System.Text.Json.Serialization;

namespace Models
{
    public class Ability
    {
        [JsonPropertyName("ability")]
        public AbilityDetail AbilityDetail { get; set; }
    }
}
