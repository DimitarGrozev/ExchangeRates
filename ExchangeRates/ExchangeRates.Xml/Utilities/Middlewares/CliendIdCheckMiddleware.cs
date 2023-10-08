using System.Net;
using ExchangeRates.Configuration;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Azure.Functions.Worker.Middleware;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ExchangeRates.Xml.Utilities.Middlewares
{
    public class ClientRegisteredMiddleware : IFunctionsWorkerMiddleware
    {
        private readonly IOptions<RegisteredUsersConfiguration> configuration;

        public ClientRegisteredMiddleware(IOptions<RegisteredUsersConfiguration> configuration)
        {
            this.configuration = configuration;
        }

        public async Task Invoke(FunctionContext context, FunctionExecutionDelegate next)
        {
            try
            {
                var request = await context.GetHttpRequestDataAsync();
                var cliendId = request.Query["clientId"];

                if (string.IsNullOrEmpty(cliendId))
                {
                    var res = request!.CreateResponse();
                    await res.WriteStringAsync("Please regiser in our site and provide the API key as a query parameter");
                    res.StatusCode = HttpStatusCode.Unauthorized;
                    context.GetInvocationResult().Value = res;

                    return;
                }

                if (!this.configuration.Value.RegisteredUsers.Contains(cliendId))
                {
                    var res = request!.CreateResponse();
                    await res.WriteStringAsync("Invalid API key");
                    res.StatusCode = HttpStatusCode.Unauthorized;
                    context.GetInvocationResult().Value = res;

                    return;
                }

                await next(context);
            }
            catch (Exception ex)
            {
                var logger = context.GetLogger(context.FunctionDefinition.Name);
                logger.LogError("Unexpected Error in {0}: {1}", context.FunctionDefinition.Name, ex.Message);
            }
        }
    }
}