using ExchangeRates.Xml.Contracts.Requests;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ExchangeRates.Xml.Utilities.Extensions
{
    public static class HttpRequestDataExtensions
    {
        public static async Task<T> ToReqestObject<T>(this HttpRequestData req) where T : class, new()
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();

            T requestObject;
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            using (StringReader reader = new StringReader(requestBody))
            {
                requestObject = (T)serializer.Deserialize(reader);
            }

            return requestObject;
        }
    }
}
