using System.Xml.Serialization;

namespace ExchangeRates.Xml.Contracts.Requests
{
    [XmlRoot]
    public class LatestExchangeRateRequest
    {
        [XmlElement]
        public Guid RequestId { get; set; }

        [XmlElement]
        public string ClientId { get; set; }

        [XmlElement]
        public string Currency { get; set; }
    }
}
