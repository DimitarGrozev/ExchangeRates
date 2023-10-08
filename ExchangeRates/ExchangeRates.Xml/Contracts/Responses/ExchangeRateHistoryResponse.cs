using ExchangeRates.Data.Models;
using System.Xml.Serialization;

namespace ExchangeRates.Xml.Contracts.Responses
{
    [XmlRoot]
    public class ExchangeRateHistoryResponse: ResponseEntitiy
    {
        [XmlArray]
        public List<ExchangeRate> ExchangeRateHistory { get; set; }
    }
}
