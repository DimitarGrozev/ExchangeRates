using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Services.DTOs
{
    public class ExchangeRateHistoryDTO
    {
        public Guid RequestId { get; set; }

        public long Timestamp { get; set; }

        public string ClientId { get; set; }

        public string Currency { get; set; }

        public int Period { get; set; }

        public string ExitServiceName { get; set; }
    }
}
