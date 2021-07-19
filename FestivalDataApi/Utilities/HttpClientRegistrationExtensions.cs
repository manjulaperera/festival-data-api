using FestivalDataApi.BusinessLogic.Clients;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Refit;
using System;
using System.Net.Http;

namespace FestivalDataApi.Utilities
{
    /// <summary>
    /// This is an extension method to add http client registration methods
    /// This also contains Polly wait and retry and circuitbreaker policies 
    /// </summary>
    public static class HttpClientRegistrationExtensions
    {
        private static TimeSpan DurationBeforeRecover => new TimeSpan(0, 0, 10);

        public static void AddHttpClientRegistry(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.AddTransient<ExceptionApiHandler>();

            services.AddCodingTestServiceClient(configuration);
        }

        private static void AddCodingTestServiceClient(this IServiceCollection services,
            IConfigurationRoot configuration)
        {
            services.AddRefitClient<ICodingTestServiceClient>()
                .ConfigureHttpClient(c =>
                {
                    c.BaseAddress = new Uri(configuration.GetValue<string>("ApplicationSettings:CodingTestServiceUrl"));
                    c.Timeout = TimeSpan.FromSeconds(10);
                })
                .SetHandlerLifetime(TimeSpan.FromMinutes(5))
                .AddTransientHttpErrorPolicy(ConfigureResiliencePolicy);
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureResiliencePolicy(
            PolicyBuilder<HttpResponseMessage> policyBuilder)
        {
            return Policy
                .WrapAsync(ConfigureWaitAndRetryPolicy(policyBuilder), ConfigureCircuitBreakerPolicy(policyBuilder));
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureWaitAndRetryPolicy(
            PolicyBuilder<HttpResponseMessage> policyBuilder)
        {
            return policyBuilder
                .WaitAndRetryAsync(new[]
                {
                    // retry with 2 secs appart for three times
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(4),
                    TimeSpan.FromSeconds(6),
                });
        }

        private static IAsyncPolicy<HttpResponseMessage> ConfigureCircuitBreakerPolicy(
            PolicyBuilder<HttpResponseMessage> policyBuilder)
        {
            // more details on how circuit breaks work https://github.com/App-vNext/Polly/wiki/Advanced-Circuit-Breaker
            // TLDR; in plain words, if in 5 second, 20 or calls are made and if the failure rate is more equal or more then 50%, it will break from 10 seconds
            return policyBuilder.AdvancedCircuitBreakerAsync(0.5, TimeSpan.FromSeconds(5), 20, DurationBeforeRecover);
        }
    }
}
