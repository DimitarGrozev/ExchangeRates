using System;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Logging;

namespace ExchangeRatesCollector
{
    public class ExchangeRatesCollectorFunction
    {
        [FunctionName("ExchangeRatesCollector")]
        public void Run([TimerTrigger("0 */5 * * * *")]TimerInfo myTimer, ILogger log)
        {
            log.LogInformation($"Fetching exchange rates: {DateTime.Now}");
        }
    }
}
