using FestivalDataApi.DataModels.Enums;

namespace FestivalDataApi.DataModels
{
    public class RequestOutcome
    {
        public RequestOutcome()
        {
            StatusId = (int)ServiceStatusOutcomes.NoResponse;
        }

        public int StatusId { get; set; } // 0 => Good, > or < 0 = Bad
        public string StatusMessage { get; set; } // An error message if it was not successful
    }
}
