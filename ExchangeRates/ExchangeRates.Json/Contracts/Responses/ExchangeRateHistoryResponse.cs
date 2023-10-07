using ExchangeRates.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Json.Contracts.Responses
{
    public class ExchangeRateHistoryResponse: ResponseEntitiy
    {
        public IEnumerable<ExchangeRate> ExchangeRateHistory { get; set; }
    }
}
