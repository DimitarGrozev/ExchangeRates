using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Json.Contracts.Responses
{
    public class ExchangeRateResponse : ResponseEntity
    {
        public string? Base { get; set; }

        public string RatesJson { get; set; }
    }
}
