using System.Net;
using System.Text.Json;
using Azure;
using ExchangeRates.Data;
using ExchangeRates.Json.Contracts.Requests;
using ExchangeRates.Json.Contracts.Responses;
using ExchangeRates.Json.Utilities;
using ExchangeRates.Json.Utilities.Constants;
using ExchangeRates.Json.Utilities.Mappers;
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
        private readonly RequestValidationService validationService;
        private readonly StatisticsCollectorService statisticsCollectorService;

        public GetExchangeRatesFunction(
            ILoggerFactory loggerFactory,
            RequestValidationService validationService,
            StatisticsCollectorService statisticsCollectorService)
        {
            this.logger = loggerFactory.CreateLogger<GetExchangeRatesFunction>();
            this.validationService = validationService;
            this.statisticsCollectorService = statisticsCollectorService;
        }

        [Function("currentExchangeRate")]
        public async Task<Contracts.Responses.Response<ExchangeRateResponse>> GetCurrentExchangeRate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [FromBody] LatestExchangeRateRequest exchangeRateRequest)
        {
            await this.statisticsCollectorService.SaveRequestHistoryAsync(exchangeRateRequest.ToDTO());

            if (!this.validationService.IsRequestedCurrencySupported(exchangeRateRequest.Currency))
            {
                return ResponseFactory.CreateErrorResponse<ExchangeRateResponse>($"Exchange rate for {exchangeRateRequest.Currency} is not supported");
            }

            this.logger.LogInformation($"[{DateTime.Now}] Fetching JSON data for currency {exchangeRateRequest.Currency} from cache");

            var exchangeRate = await this.statisticsCollectorService.GetCurrentExchangeRateAsync(exchangeRateRequest.Currency);

            if (exchangeRate == null)
            {
                return ResponseFactory.CreateErrorResponse<ExchangeRateResponse>($"Exchange rate for {exchangeRateRequest.Currency} is not available in our system");
            }

            return ResponseFactory.CreateSuccessResponse(
                $"Successfully fetched JSON data for {exchangeRate.Value.Currency} exchange rate",
                new ExchangeRateResponse { Base = exchangeRate.Value.Currency, RatesJson = exchangeRate.Value.RatesJson });
        }

        [Function("exchangeRateHistory")]
        public async Task<Contracts.Responses.Response<ExchangeRateHistoryResponse>> GetExchangeRateHistory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req,
            [FromBody] ExchangeRateHistoryRequest exchangeRateRequest)
        {
            await this.statisticsCollectorService.SaveRequestHistoryAsync(exchangeRateRequest.ToDto());

            if (!this.validationService.IsRequestedCurrencySupported(exchangeRateRequest.Currency))
            {
                return ResponseFactory.CreateErrorResponse<ExchangeRateHistoryResponse>($"Exchange rate history for {exchangeRateRequest.Currency} is not supported");
            }

            var exchangeRateHistory = await this.statisticsCollectorService.GetExchangeRateHistoryAsync(exchangeRateRequest.Currency, exchangeRateRequest.Period);

            if (exchangeRateHistory.Count == 0)
            {
                return ResponseFactory.CreateErrorResponse<ExchangeRateHistoryResponse>($"Exchange rate history for {exchangeRateRequest.Currency} is not available in our system");
            }

            return ResponseFactory.CreateSuccessResponse(
                $"Successfully fetched JSON data for exchange rate history for {exchangeRateRequest.Currency} within the last {exchangeRateRequest.Period} hours",
                new ExchangeRateHistoryResponse { ExchangeRateHistory = exchangeRateHistory });
        }
    }
}
