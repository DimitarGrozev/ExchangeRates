using ExchangeRates.Data.Models;
using Fixerr.Models;
using System.Runtime.CompilerServices;
using System.Text.Json;

namespace ExchangeRatesCollector.Utilities
{
    public static class LatestRateExtensions
    {
        public static ExchangeRate ToExchangeRate(this LatestRate? latestRate)
        {
            if (latestRate == null)
                return null;

            return new ExchangeRate
            {
                Base = latestRate.Base,
                Date = latestRate.Date,
                Timestamp = latestRate.TimeStamp,
                RatesJson = JsonSerializer.Serialize(latestRate.Rates)
            };
        }

        public static List<ExchangeRate> ToExchangeRate(this LatestRate?[] latestRates)
        {
            return latestRates.Select(ToExchangeRate).ToList();
        }
    }
}
