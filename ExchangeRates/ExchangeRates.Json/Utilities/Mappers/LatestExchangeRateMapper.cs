using ExchangeRates.Json.Contracts.Requests;
using ExchangeRates.Json.Utilities.Constants;
using ExchangeRates.Services.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Json.Utilities.Mappers
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
                Timestamp = request.Timestamp,
            };
        }
    }
}
