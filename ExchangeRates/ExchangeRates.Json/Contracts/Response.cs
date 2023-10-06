using ExchangeRates.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Json.Contracts
{
    public class Response<T> where T: ResponseEntity
    {
        public string Message { get; set; }

        public bool IsSuccsessful { get; set; } = true;

        public T Value { get; set; }
    }
}
