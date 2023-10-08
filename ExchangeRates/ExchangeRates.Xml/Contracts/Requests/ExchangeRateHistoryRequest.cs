namespace ExchangeRates.Xml.Contracts.Requests
{
    public class ExchangeRateHistoryRequest
    {
            public Guid RequestId { get; set; }

            public string ClientId { get; set; }

            public string Currency { get; set; }

            public int Period { get; set; }
    }
}
