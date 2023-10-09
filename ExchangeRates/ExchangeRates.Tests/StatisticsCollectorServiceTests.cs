using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ExchangeRates.Data;
using ExchangeRates.Services;
using ExchangeRates.Services.DTOs;
using Microsoft.EntityFrameworkCore;
using StackExchange.Redis;

namespace ExchangeRates.Tests
{
    [TestFixture]
    public class StatisticsCollectorServiceTests
    {
        private ExchangeRatesDbContext dbContext;
        private ConnectionMultiplexer redisConnection;
        private StatisticsCollectorService service;

        [SetUp]
        public async Task Setup()
        {
            var options = new DbContextOptionsBuilder<ExchangeRatesDbContext>()
                .UseInMemoryDatabase(databaseName: "InMemoryDatabase")
                .Options;
            dbContext = new ExchangeRatesDbContext(options);

            redisConnection = ConnectionMultiplexer.Connect("localhost");

            service = new StatisticsCollectorService(dbContext, redisConnection);
        }

        [TearDown]
        public void TearDown()
        {
            dbContext.Dispose();
            redisConnection.Dispose();
        }

        [Test]
        public async Task SaveRequestHistoryAsync_Saves_Request_History()
        {
            // Arrange
            var request = new LatestExchangeRateDTO
            {
                ClientId = "123",
                Currency = "EUR",
                ExitServiceName = "Exit_Service_1",
                RequestId = Guid.NewGuid(),
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };

            // Act
            await service.SaveRequestHistoryAsync(request);

            // Assert
            var savedEntry = await dbContext.RequestHistory.FirstOrDefaultAsync();
            Assert.NotNull(savedEntry);
            Assert.Equals(request.ClientId, savedEntry.ClientId);
            Assert.Equals(request.Currency, savedEntry.Currency);
        }

        [Test]
        public async Task GetCurrentExchangeRateAsync_Returns_Cached_Data_If_Available()
        {
            // Arrange
            var currency = "USD";
            var cachedRates = "{\"AED\":3.893613,\"AFN\":79.975022,\"ALL\":105.680208,\"AMD\":441.717275,\"ANG\":1.901944,\"AOA\":878.256865,\"ARS\":369.396319,\"AUD\":1.661002,\"AWG\":1.908093}";
            var redisCache = redisConnection.GetDatabase();
            await redisCache.StringSetAsync(currency, cachedRates);

            // Act
            var result = await service.GetCurrentExchangeRateAsync(currency);

            // Assert
            Assert.NotNull(result);
            Assert.Equals(currency, result.Value.Currency);
            Assert.Equals(cachedRates, result.Value.RatesJson);
        }
    }
}
