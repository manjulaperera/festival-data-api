using Microsoft.Extensions.Logging;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace FestivalDataApi.Utilities
{
    /// <summary>
    /// This is an exception handler middleware
    /// </summary>
    public class ExceptionApiHandler : DelegatingHandler
    {
        private readonly ILogger<ExceptionApiHandler> _logger;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="logger"></param>
        public ExceptionApiHandler(ILogger<ExceptionApiHandler> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            try
            {
                var response = await base.SendAsync(request, cancellationToken);
                return response;
            }

            catch (Exception ex)
            {
                _logger.LogError(ex, $"[{request.RequestUri.Host}] Request {request.Method} {request.RequestUri} has failed");
                throw;
            }
        }
    }
}
