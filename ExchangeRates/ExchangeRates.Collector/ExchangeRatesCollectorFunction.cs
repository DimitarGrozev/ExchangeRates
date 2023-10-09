using Fixerr;
using Fixerr.Models;
using ExchangeRates.Configuration;
using ExchangeRates.Data;
using ExchangeRatesCollector.Utilities;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using StackExchange.Redis;
using ExchangeRates.Services;

namespace ExchangeRatesCollector
{
    public class ExchangeRatesCollectorFunction
    {
        private readonly ILogger logger;
        private readonly IFixerClient fixerClient;
        private readonly IOptions<ExchangeRatesConfiguration> exchangeRatesConfig;
        private readonly ExchangeRatesDbContext dbContext;
        private readonly ConnectionMultiplexer redisConnection;
        private readonly MessageClientService messageClient;

        public ExchangeRatesCollectorFunction(
            ILoggerFactory loggerFactory,
            IFixerClient fixerClient,
            IOptions<ExchangeRatesConfiguration> exchangeRatesConfig,
            ExchangeRatesDbContext dbContext,
            ConnectionMultiplexer redisConnection,
            MessageClientService messageClient)
        {
            this.logger = loggerFactory.CreateLogger<ExchangeRatesCollectorFunction>();
            this.fixerClient = fixerClient;
            this.exchangeRatesConfig = exchangeRatesConfig;
            this.dbContext = dbContext;
            this.redisConnection = redisConnection;
            this.messageClient = messageClient;
        }

        [Function("FetchExchangeRates")]
        public async Task FetchExchangeRates([TimerTrigger("%TimerSchedule%")] MyInfo myTimer)
        {
            this.logger.LogInformation($"[{DateTime.Now}] Fetching exchange rates...");

            var redisCache = this.redisConnection.GetDatabase();
            var latestRateTasks = new List<Task<LatestRate?>>();

            foreach (var currency in this.exchangeRatesConfig.Value.FollowedCurrencies)
            {
                latestRateTasks.Add(this.fixerClient.GetLatestRateAsync(baseCurrency: currency));
            }

            // fetch new
            var latestRates = await Task.WhenAll(latestRateTasks);

            if (latestRates != null && latestRates.Length > 0)
            {
                //save to db and cache
                var exchangeRates = latestRates.ToExchangeRate()
                    .Where(exchangeRate => exchangeRate != null)
                    .ToList();

                await this.dbContext.ExchangeRates.AddRangeAsync(exchangeRates);
                await this.dbContext.SaveChangesAsync();

                // can set cache expiry but if trigger is delayed we might not hit it on time so just overwrite on new request
                exchangeRates.ForEach(exchangeRate =>
                    redisCache.StringSet(exchangeRate.Base, exchangeRate.RatesJson));

                // publish message to rabbitMQ
                this.messageClient.SendMessage(exchangeRates);
            }

            this.logger.LogInformation($"[{DateTime.Now}] Fetching completed...");
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
