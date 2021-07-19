namespace FestivalDataApi.DataModels
{
    /// <summary>
    /// Error response returned to the caller
    /// </summary>
    public class ErrorResponse
    {
        public int Code { get; set; }
        public string Message { get; set; }
    }
}
