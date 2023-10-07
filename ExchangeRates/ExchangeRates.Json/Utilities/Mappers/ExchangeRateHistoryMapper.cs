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
                Timestamp = request.Timestamp,
                ExitServiceName = ExchangeRatesConstants.ExitServiceName
            };
        }
    }
}
