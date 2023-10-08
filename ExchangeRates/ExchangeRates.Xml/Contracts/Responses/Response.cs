using System.Xml.Serialization;

namespace ExchangeRates.Xml.Contracts.Responses
{
    [XmlRoot]
    public class Response<T> where T : ResponseEntitiy
    {
        [XmlElement]
        public string Message { get; set; }

        [XmlElement]
        public bool IsSuccsessful { get; set; } = true;

        [XmlElement]
        public T Value { get; set; }
    }
}
