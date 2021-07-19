using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace FestivalDataApi.DataModels.Models
{
    public class MusicFestival
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("bands")]
        public List<Band> Bands { get; set; }
    }
}
