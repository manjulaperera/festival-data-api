using FestivalDataApi.DataModels;
using System.Net;
using System.Net.Http;
using System.Text.Json;

namespace FestivalDataApi.UnitTests.TestData
{
    public class RequestAndExpectedResult
    {
        public RecordLabelsRequest Request { get; set; }
        public HttpStatusCode ResponseStatusCode { get; set; }
        public int Code { get; set; }
        public string Description { get; set; }
        public string ReturnedValue { get; set; }

        public HttpResponseMessage Response
        {
            get
            {
                var apiError = new CodingTestServiceError { Code = Code, Description = Description };
                return new HttpResponseMessage(ResponseStatusCode)
                {
                    Content = new StringContent(JsonSerializer.Serialize(apiError), System.Text.Encoding.UTF8, "application/json")
                };
            }
        }
    }
}
