using System;
using ExchangeRates.Data;
using ExchangeRatesCollector.Configurations;
using ExchangeRatesCollector.Utilities;
using Fixerr;
using Fixerr.Models;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;

namespace ExchangeRatesCollector
{
    public class ExchangeRatesCollectorFunction
    {
        private readonly ILogger _logger;
        private readonly IFixerClient fixerClient;
        private readonly IOptions<ExchangeRatesConfiguration> exchangeRatesConfig;
        private readonly ExchangeRatesDbContext dbContext;
        private readonly ConnectionMultiplexer redisConnection;

        public ExchangeRatesCollectorFunction(
            ILoggerFactory loggerFactory,
            IFixerClient fixerClient,
            IOptions<ExchangeRatesConfiguration> exchangeRatesConfig,
            ExchangeRatesDbContext dbContext,
            ConnectionMultiplexer redisConnection)
        {
            _logger = loggerFactory.CreateLogger<ExchangeRatesCollectorFunction>();
            this.fixerClient = fixerClient;
            this.exchangeRatesConfig = exchangeRatesConfig;
            this.dbContext = dbContext;
            this.redisConnection = redisConnection;
        }

        [Function("GetExchangeRates")]
        public async Task GetExchangeRates([TimerTrigger("%TimerSchedule%")] MyInfo myTimer)
        {
            _logger.LogInformation($"[{DateTime.Now}] Fetching exchange rates...");

            var redisCache = this.redisConnection.GetDatabase();
            var latestRateTasks = new List<Task<LatestRate?>>();

            foreach (var currency in this.exchangeRatesConfig.Value.FollowedCurrencies)
            {
                latestRateTasks.Add(this.fixerClient.GetLatestRateAsync(baseCurrency: currency));
            }

            var latestRates = await Task.WhenAll(latestRateTasks);

            if (latestRates != null && latestRates.Length > 0)
            {
                var exchangeRates = latestRates.ToExchangeRate().Where(exchangeRate => exchangeRate != null).ToList();
                await this.dbContext.ExchangeRates.AddRangeAsync(exchangeRates);
                await this.dbContext.SaveChangesAsync();

                exchangeRates.ForEach(exchangeRate => redisCache.StringSet(exchangeRate.Base, exchangeRate.RatesJson));
            }

            _logger.LogInformation($"[{DateTime.Now}] Fetching completed...");
        }
    }

    public class MyInfo
    {
        public MyScheduleStatus ScheduleStatus { get; set; }

        public bool IsPastDue { get; set; }
    }

    public class MyScheduleStatus
    {
        public DateTime Last { get; set; }

        public DateTime Next { get; set; }

        public DateTime LastUpdated { get; set; }
    }
}
