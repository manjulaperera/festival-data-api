using System.Collections.Generic;

namespace FestivalDataApi.DataModels.Dto
{
    public class RecordLabelDto
    {
        public RecordLabelDto()
        {
            Bands = new List<BandDto>();
        }

        public string RecordLabel { get; set; }
        public List<BandDto> Bands { get; set; }
    }
}
