using System.Net;
using Azure;
using ExchangeRates.Data;
using ExchangeRates.Json.Contracts;
using ExchangeRates.Json.Utilities;
using ExchangeRates.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace ExchangeRates.Json
{
    public class GetExchangeRatesFunction
    {
        private readonly ILogger logger;
        private readonly ExchangeRatesDbContext dbContext;
        private readonly ConnectionMultiplexer redisConnection;
        private readonly RequestValidationService validationService;

        public GetExchangeRatesFunction(
            ILoggerFactory loggerFactory,
            ExchangeRatesDbContext dbContext,
            ConnectionMultiplexer redisConnection,
            RequestValidationService validationService)
        {
            logger = loggerFactory.CreateLogger<GetExchangeRatesFunction>();
            this.dbContext = dbContext;
            this.redisConnection = redisConnection;
            this.validationService = validationService;
        }

        [Function("GetCurrentExchangeRate")]
        public async Task<Contracts.Response<ExchangeRate>> GetCurrentExchangeRate([HttpTrigger(AuthorizationLevel.Anonymous, "get")] HttpRequestData req)
        {
            var currencySymbol = System.Web.HttpUtility.ParseQueryString(req.Url.Query)["currency"] ?? string.Empty;

            if (!this.validationService.IsRequestedCurrencySupported(currencySymbol))
            {
                return ResponseFactory.CreateErrorResponse<ExchangeRate>($"Exchange rate for {currencySymbol} is not supported");
            }

            this.logger.LogInformation($"[{DateTime.Now}] Fetching JSON data for currency {currencySymbol} from cache");
            
            var redisCache = this.redisConnection.GetDatabase();
            var cachedCurrencyRates = await redisCache.StringGetAsync(currencySymbol);

            if (cachedCurrencyRates.HasValue)
            {
                return ResponseFactory.CreateSuccessResponse<ExchangeRate>(
                    $"Successfully fetched JSON data for {currencySymbol} exchange rate from cache",
                    new ExchangeRate { Base = currencySymbol, RatesJson = cachedCurrencyRates.ToString()});
            }

            var exchangeRate = await this.dbContext.ExchangeRates
                .Where(exchangeRate => exchangeRate.Base == currencySymbol)
                .OrderBy(exchangeRate => exchangeRate.Timestamp)
                .FirstOrDefaultAsync();

            if(exchangeRate == null)
            {
                return ResponseFactory.CreateErrorResponse<ExchangeRate>($"Exchange rate for {currencySymbol} is not available in our system");
            }

            return ResponseFactory.CreateSuccessResponse<ExchangeRate>(
                $"Successfully fetched JSON data for {currencySymbol} exchange rate from db",
                new ExchangeRate { Base = currencySymbol, RatesJson = exchangeRate.RatesJson});
        }
    }
}
