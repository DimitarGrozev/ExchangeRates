using System;
using Fixerr;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesCollector
{
    public class ExchangeRatesCollectorFunction
    {
        private readonly ILogger _logger;
        private readonly IFixerClient fixerClient;

        public ExchangeRatesCollectorFunction(
            ILoggerFactory loggerFactory,
            IFixerClient fixerClient)
        {
            _logger = loggerFactory.CreateLogger<ExchangeRatesCollectorFunction>();
            this.fixerClient = fixerClient;
        }

        [Function("GetExchangeRates")]
        public void Run([TimerTrigger("%TimerSchedule%")] MyInfo myTimer)
        {
            _logger.LogInformation($"[{DateTime.Now}] Fetching exchange rates...");
            var rates = this.fixerClient.GetLatestRateStringAsync();
            _logger.LogInformation($"[{DateTime.Now}] {rates}");

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
