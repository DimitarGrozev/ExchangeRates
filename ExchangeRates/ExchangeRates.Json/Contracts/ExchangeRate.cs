using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Json.Contracts
{
    public class ExchangeRate: ResponseEntity
    {
        public string? Base { get; set; }

        public string RatesJson { get; set; }
    }
}
