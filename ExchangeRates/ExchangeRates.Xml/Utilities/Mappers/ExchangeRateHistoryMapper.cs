using ExchangeRates.Services.DTOs;
using ExchangeRates.Xml.Contracts.Requests;
using ExchangeRates.Xml.Utilities.Constants;

namespace ExchangeRates.Xml.Utilities.Mappers
{
    public static class ExchangeRateHistoryMapper
    {
        public static ExchangeRateHistoryDTO ToDto(this ExchangeRateHistoryRequest request)
        {
            return new ExchangeRateHistoryDTO
            {
                Currency = request.Currency,
                ClientId = request.ClientId,
                Period = request.Period,
                RequestId = request.RequestId,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds(),
                ExitServiceName = ExchangeRatesConstants.ExitServiceName
            };
        }
    }
}
