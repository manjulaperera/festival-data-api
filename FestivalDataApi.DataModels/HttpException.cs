using Microsoft.Extensions.Logging;
using System;
using System.Net;

namespace FestivalDataApi.DataModels
{
    public class HttpException : Exception
    {
        private const int UnexpectedErrorCode = 12899;
        private const string UnexpectedErrorMessage = "Unexpected error";

        public int Code { get; set; }
        public HttpStatusCode HttpStatusCode { get; set; }

        public string Description { get; set; }

        public HttpException(ILogger logger, HttpStatusCode statusCode, int? code, string description, Exception innerException = null)
        {
            if (innerException == null)
                logger.LogError(description);
            else
                logger.LogError(innerException, description);

            Code = code ?? UnexpectedErrorCode;
            HttpStatusCode = statusCode;
            Description = description ?? UnexpectedErrorMessage;
        }
    }
}
