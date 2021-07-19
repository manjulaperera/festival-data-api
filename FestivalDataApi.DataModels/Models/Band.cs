using System.Text.Json.Serialization;

namespace FestivalDataApi.DataModels.Models
{
    public class Band
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("recordLabel")]
        public string RecordLabel { get; set; }
    }
}
