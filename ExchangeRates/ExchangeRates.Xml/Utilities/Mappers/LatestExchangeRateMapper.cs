using ExchangeRates.Services.DTOs;
using ExchangeRates.Xml.Contracts.Requests;
using ExchangeRates.Xml.Utilities.Constants;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Xml.Utilities.Mappers
{
    public static class LatestExchangeRateMapper
    {
        public static LatestExchangeRateDTO ToDTO(this LatestExchangeRateRequest request)
        {
            return new LatestExchangeRateDTO
            {
                ClientId = request.ClientId,
                Currency = request.Currency,
                ExitServiceName = ExchangeRatesConstants.ExitServiceName,
                RequestId = request.RequestId,
                Timestamp = DateTimeOffset.UtcNow.ToUnixTimeSeconds()
            };
        }
    }
}
