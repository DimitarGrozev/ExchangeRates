using ExchangeRates.Data;
using ExchangeRates.Data.Models;
using ExchangeRates.Services.DTOs;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;
using System.Text.Json;

namespace ExchangeRates.Services
{
    public class StatisticsCollectorService
    {
        private readonly ExchangeRatesDbContext dbContext;
        private readonly ConnectionMultiplexer redisConnection;

        public StatisticsCollectorService(
            ExchangeRatesDbContext dbContext,
            ConnectionMultiplexer redisConnection)
        {
            this.dbContext = dbContext;
            this.redisConnection = redisConnection;
        }

        public async Task SaveRequestHistoryAsync(LatestExchangeRateDTO latestExchangeRate)
        {
            await this.dbContext.RequestHistory.AddAsync(new Data.Models.RequestHistory
            {
                ClientId = latestExchangeRate.ClientId,
                Currency = latestExchangeRate.Currency,
                ExitService = latestExchangeRate.ExitServiceName,
                RequestId = latestExchangeRate.RequestId,
                Timestamp = latestExchangeRate.Timestamp,
                ExitServiceName = latestExchangeRate.ExitServiceName,
                RequestType = RequestType.Current
            });

            await this.dbContext.SaveChangesAsync();
        }

        public async Task SaveRequestHistoryAsync(ExchangeRateHistoryDTO exchangeRateHistory)
        {
            await this.dbContext.RequestHistory.AddAsync(new Data.Models.RequestHistory
            {
                ClientId = exchangeRateHistory.ClientId,
                Currency = exchangeRateHistory.Currency,
                ExitService = exchangeRateHistory.ExitServiceName,
                RequestId = exchangeRateHistory.RequestId,
                Timestamp = exchangeRateHistory.Timestamp,
                ExitServiceName = exchangeRateHistory.ExitServiceName,
                AdditionalParameters = JsonSerializer.Serialize(new Dictionary<string, object> { { nameof(exchangeRateHistory.Period), exchangeRateHistory.Period } }),
                RequestType = RequestType.History
            });

            await this.dbContext.SaveChangesAsync();
        }

        public async Task<(string Currency, string RatesJson)?> GetCurrentExchangeRateAsync(string currency)
        {
            var redisCache = this.redisConnection.GetDatabase();
            var cachedCurrencyRates = await redisCache.StringGetAsync(currency);

            if (cachedCurrencyRates.HasValue)
            {
                return (currency, cachedCurrencyRates);
            }

            var exchangeRate = await this.dbContext.ExchangeRates
                .Where(exchangeRate => exchangeRate.Base == currency)
                .OrderBy(exchangeRate => exchangeRate.Timestamp)
                .FirstOrDefaultAsync();

            if (exchangeRate == null)
            {
                return null;
            }

            return (currency, exchangeRate.RatesJson);
        }

        public async Task<List<ExchangeRate>> GetExchangeRateHistoryAsync(string currency, int requestedPeriod)
        {
            long requestedFromTimestamp = DateTimeOffset.UtcNow.AddHours(-requestedPeriod).ToUnixTimeSeconds();

            return await this.dbContext.ExchangeRates
                .Where(exchangeRate => exchangeRate.Base == currency && exchangeRate.Timestamp >= requestedFromTimestamp)
                .OrderBy(exchangeRate => exchangeRate.Timestamp)
                .ToListAsync();
        }
    }
}
