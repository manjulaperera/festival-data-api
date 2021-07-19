using System.Collections.Generic;

namespace FestivalDataApi.DataModels.Dto
{
    public class BandDto
    {
        public string Name { get; set; }
        public List<string> Festivals { get; set; }
    }
}
