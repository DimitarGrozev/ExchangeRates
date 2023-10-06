using System.Net;
using Azure;
using ExchangeRates.Data;
using ExchangeRates.Json.Contracts.Requests;
using ExchangeRates.Json.Contracts.Responses;
using ExchangeRates.Json.Utilities;
using ExchangeRates.Json.Utilities.Constants;
using ExchangeRates.Services;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;
using FromBodyAttribute = Microsoft.Azure.Functions.Worker.Http.FromBodyAttribute;

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
        public async Task<Contracts.Responses.Response<ExchangeRateResponse>> GetCurrentExchangeRate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [FromBody] LatestExchangeRateRequest exchangeRateRequest)
        {
            var currencySymbol = exchangeRateRequest.Currency;

            await this.dbContext.RequestHistory.AddAsync(new Data.Models.RequestHistory
            {
                ClientId = exchangeRateRequest.ClientId,
                Currency = currencySymbol,
                ExitService = ExchangeRatesConstants.ExitServiceName,
                RequestId = exchangeRateRequest.RequestId,
                Timestamp = exchangeRateRequest.Timestamp
            });
            await this.dbContext.SaveChangesAsync();


            if (!this.validationService.IsRequestedCurrencySupported(currencySymbol))
            {
                return ResponseFactory.CreateErrorResponse<ExchangeRateResponse>($"Exchange rate for {currencySymbol} is not supported");
            }

            this.logger.LogInformation($"[{DateTime.Now}] Fetching JSON data for currency {currencySymbol} from cache");
            
            var redisCache = this.redisConnection.GetDatabase();
            var cachedCurrencyRates = await redisCache.StringGetAsync(currencySymbol);

            if (cachedCurrencyRates.HasValue)
            {
                return ResponseFactory.CreateSuccessResponse(
                    $"Successfully fetched JSON data for {currencySymbol} exchange rate from cache",
                    new ExchangeRateResponse { Base = currencySymbol, RatesJson = cachedCurrencyRates.ToString()});
            }

            var exchangeRate = await this.dbContext.ExchangeRates
                .Where(exchangeRate => exchangeRate.Base == currencySymbol)
                .OrderBy(exchangeRate => exchangeRate.Timestamp)
                .FirstOrDefaultAsync();

            if(exchangeRate == null)
            {
                return ResponseFactory.CreateErrorResponse<ExchangeRateResponse>($"Exchange rate for {currencySymbol} is not available in our system");
            }


            return ResponseFactory.CreateSuccessResponse(
                $"Successfully fetched JSON data for {currencySymbol} exchange rate from db",
                new ExchangeRateResponse { Base = currencySymbol, RatesJson = exchangeRate.RatesJson});
        }
    }
}
