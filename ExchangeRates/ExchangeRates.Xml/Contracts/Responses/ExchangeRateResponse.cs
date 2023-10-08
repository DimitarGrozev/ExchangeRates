using System.Xml.Serialization;

namespace ExchangeRates.Xml.Contracts.Responses
{
    [XmlRoot]
    public class ExchangeRateResponse : ResponseEntitiy
    {
        [XmlElement]
        public string? Base { get; set; }

        [XmlElement]
        public string RatesJson { get; set; }
    }
}
