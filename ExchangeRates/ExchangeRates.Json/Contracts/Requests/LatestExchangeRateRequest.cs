using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Json.Contracts.Requests
{
    public class LatestExchangeRateRequest
    {
        public Guid RequestId { get; set; }

        public long Timestamp { get; set; }

        public string ClientId { get; set; }

        public string Currency { get; set; }
    }
}
