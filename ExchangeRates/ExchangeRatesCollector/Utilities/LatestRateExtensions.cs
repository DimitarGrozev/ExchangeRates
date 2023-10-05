using ExchangeRates.Data.Models;
using Fixerr.Models;
using System.Text.Json;

namespace ExchangeRatesCollector.Utilities
{
    public static class LatestRateExtensions
    {
        public static ExchangeRate ToExchangeRate(this LatestRate latestRate)
        {
            return new ExchangeRate
            {
                Base = latestRate.Base,
                Date = latestRate.Date,
                Timestamp = latestRate.TimeStamp,
                RatesJson = JsonSerializer.Serialize(latestRate.Rates)
            };
        }
    }
}
