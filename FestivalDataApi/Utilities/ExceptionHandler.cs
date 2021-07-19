using FestivalDataApi.DataModels;
using FestivalDataApi.DataModels.Enums;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace FestivalDataApi.Utilities
{
    /// <summary>
    /// Exception handler middleware
    /// </summary>
    public class ExceptionHandler
    {
        private readonly RequestDelegate _request;
        private readonly JsonSerializerSettings _serializerSettings = new JsonSerializerSettings
        {
            ContractResolver = new CamelCasePropertyNamesContractResolver()
        };

        public ExceptionHandler(RequestDelegate request)
        {
            _request = request;
        }

        /// <summary>
        /// This handles exceptions globally
        /// </summary>
        /// <param name="context"></param>
        /// <param name="logger"></param>
        /// <returns></returns>
        public async Task Invoke(HttpContext context, ILogger<ExceptionHandler> logger)
        {
            try
            {
                await _request(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex, logger);
            }
        }

        private readonly string _generalErrorMessage = "We were unable to process your request, please try again";

        private async Task HandleExceptionAsync(HttpContext context, Exception exception, ILogger logger)
        {
            string result;
            if (exception is HttpException)
            {
                var ex = (HttpException)exception;

                var response = new ErrorResponse
                {
                    Code = ex.Code,
                    Message = ex.Description
                };

                context.Response.StatusCode = (int)ex.HttpStatusCode;
                result = JsonConvert.SerializeObject(response, _serializerSettings);
            }
            else
            {
                logger.LogError(exception, $"Internal Server Error {context.Request.Path}");

                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

                var response = new RequestOutcome
                {
                    StatusId = (int)ServiceStatusOutcomes.Failed,
                    StatusMessage = _generalErrorMessage
                };

                result = JsonConvert.SerializeObject(response, _serializerSettings);
            }

            context.Response.ContentType = MediaTypeNames.Application.Json;

            await context.Response.WriteAsync(result);
        }
    }
}
