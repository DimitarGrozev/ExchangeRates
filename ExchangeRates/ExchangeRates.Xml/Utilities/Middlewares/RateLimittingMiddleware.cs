using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Hosting;
using System.Net;
using ThrottlingTroll;

namespace ExchangeRates.Xml.Utilities.Middlewares
{
    public static class RateLimittingMiddleware
    {
        public static void UseRateLimitting(this IFunctionsWorkerApplicationBuilder builder, HostBuilderContext context)
        {
            builder.UseThrottlingTroll(context, options =>
            {
               options.Config = new ThrottlingTrollConfig
               {
                   Rules = new[]
                   {
                      new ThrottlingTrollRule
                      {
                          UriPattern = "api/",
                          HeaderValue = "60",
                          Method = "POST",
                          LimitMethod = new FixedWindowRateLimitMethod
                          {
                              PermitLimit = 2,
                              IntervalInSeconds = 60
                          },
                          IdentityIdExtractor = (request) =>
                          {
                              return ((IIncomingHttpRequestProxy)request).Request.Query["clientId"];
                          }
                      }
                  }
               };
               options.ResponseFabric = async (limitExceededResult, requestProxy, responseProxy, requestAborted) =>
               {
                   responseProxy.StatusCode = (int)HttpStatusCode.TooManyRequests;

                   responseProxy.SetHttpHeader("Retry-After", limitExceededResult.RetryAfterHeaderValue);

                   await responseProxy.WriteAsync("Too many requests. Try again later.");
               };
            });
        }
    }
}
