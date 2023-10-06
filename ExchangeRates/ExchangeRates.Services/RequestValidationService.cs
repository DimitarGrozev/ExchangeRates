using ExchangeRates.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Services
{
    public class RequestValidationService
    {
        private readonly IOptions<ExchangeRatesConfiguration> configuration;

        public RequestValidationService(IOptions<ExchangeRatesConfiguration> configuration)
        {
            this.configuration = configuration;
        }

        public bool IsRequestedCurrencySupported(string currency)
        {
            return configuration.Value.FollowedCurrencies.Contains(currency);
        }
    }
}
