using FestivalDataApi.DataModels;
using FestivalDataApi.DataModels.Models;
using FestivalDataApi.Utilities.Helpers;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FestivalDataApi.UnitTests.TestData
{
    public class RequestAndOkResult
    {
        public RequestAndOkResult()
        {
            ClientResponse = new List<MusicFestival>();
        }

        public RecordLabelsRequest Request { get; set; }
        public IEnumerable<MusicFestival> ClientResponse { get; set; }
        public RecordLabelsResponse FinalResponse { get; set; }

        public async Task<string> GetSerializedClientResponse()
        {
            return await ClientResponse.ToJson();
        }
    }
}
