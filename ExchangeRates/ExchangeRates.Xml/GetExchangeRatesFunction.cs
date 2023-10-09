using System.Diagnostics.Contracts;
using System.Net;
using ExchangeRates.Services;
using ExchangeRates.Xml.Contracts.Requests;
using ExchangeRates.Xml.Contracts.Responses;
using ExchangeRates.Xml.Utilities;
using ExchangeRates.Xml.Utilities.Extensions;
using ExchangeRates.Xml.Utilities.Mappers;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;

namespace ExchangeRates.Xml
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
        public async Task<HttpResponseData> GetCurrentExchangeRate(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var exchangeRateRequest = await req.ToReqestObject<LatestExchangeRateRequest>();

            await this.statisticsCollectorService.SaveRequestHistoryAsync(exchangeRateRequest.ToDTO());

            if (!this.validationService.IsRequestedCurrencySupported(exchangeRateRequest.Currency))
            {
                return await ResponseFactory.CreateErrorResponse<ExchangeRateResponse>($"Exchange rate for {exchangeRateRequest.Currency} is not supported", req);
            }

            this.logger.LogInformation($"[{DateTime.Now}] Fetching JSON data for currency {exchangeRateRequest.Currency} from cache");

            var exchangeRate = await this.statisticsCollectorService.GetCurrentExchangeRateAsync(exchangeRateRequest.Currency);

            if (exchangeRate == null)
            {
                return await ResponseFactory.CreateErrorResponse<ExchangeRateResponse>($"Exchange rate for {exchangeRateRequest.Currency} is not available in our system", req);
            }

            return await ResponseFactory.CreateSuccessResponse(
                $"Successfully fetched JSON data for {exchangeRate.Value.Currency} exchange rate",
                new ExchangeRateResponse { Base = exchangeRate.Value.Currency, RatesJson = exchangeRate.Value.RatesJson },
                req);
        }

        [Function("exchangeRateHistory")]
        public async Task<HttpResponseData> GetExchangeRateHistory(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post")] HttpRequestData req)
        {
            var exchangeRateRequest = await req.ToReqestObject<ExchangeRateHistoryRequest>();

            await this.statisticsCollectorService.SaveRequestHistoryAsync(exchangeRateRequest.ToDto());

            if (!this.validationService.IsRequestedCurrencySupported(exchangeRateRequest.Currency))
            {
                return await ResponseFactory.CreateErrorResponse<ExchangeRateHistoryResponse>($"Exchange rate history for {exchangeRateRequest.Currency} is not supported", req);
            }

            var exchangeRateHistory = await this.statisticsCollectorService.GetExchangeRateHistoryAsync(exchangeRateRequest.Currency, exchangeRateRequest.Period);

            if (exchangeRateHistory.Count == 0)
            {
                return await ResponseFactory.CreateErrorResponse<ExchangeRateHistoryResponse>($"Exchange rate history for {exchangeRateRequest.Currency} is not available in our system", req);
            }

            return await ResponseFactory.CreateSuccessResponse(
                $"Successfully fetched JSON data for exchange rate history for {exchangeRateRequest.Currency} within the last {exchangeRateRequest.Period} hours",
                new ExchangeRateHistoryResponse { ExchangeRateHistory = exchangeRateHistory },
                req);
        }
    }
}
