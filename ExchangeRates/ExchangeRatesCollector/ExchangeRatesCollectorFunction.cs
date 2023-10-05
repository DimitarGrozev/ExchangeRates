using System;
using ExchangeRates.Data;
using ExchangeRatesCollector.Utilities;
using Fixerr;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesCollector
{
    public class ExchangeRatesCollectorFunction
    {
        private readonly ILogger _logger;
        private readonly IFixerClient fixerClient;
        private readonly ExchangeRatesDbContext dbContext;

        public ExchangeRatesCollectorFunction(
            ILoggerFactory loggerFactory,
            IFixerClient fixerClient,
            ExchangeRatesDbContext dbContext)
        {
            _logger = loggerFactory.CreateLogger<ExchangeRatesCollectorFunction>();
            this.fixerClient = fixerClient;
            this.dbContext = dbContext;
        }

        [Function("GetExchangeRates")]
        public async Task Run([TimerTrigger("%TimerSchedule%")] MyInfo myTimer)
        {
            _logger.LogInformation($"[{DateTime.Now}] Fetching exchange rates...");

            var rates = await this.fixerClient.GetLatestRateAsync();

            if (rates != null)
            {
                var exchangeRate = rates.ToExchangeRate();
                await this.dbContext.ExchangeRates.AddAsync(exchangeRate);
                await this.dbContext.SaveChangesAsync();
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
