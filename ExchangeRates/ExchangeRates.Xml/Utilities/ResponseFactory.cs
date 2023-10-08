using ExchangeRates.Xml.Contracts.Responses;
using Google.Protobuf.Compiler;
using Microsoft.Azure.Functions.Worker.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ExchangeRates.Xml.Utilities
{
    internal static class ResponseFactory
    {
        internal static async Task<HttpResponseData> CreateSuccessResponse<T>(string message, T value, HttpResponseData response) where T : ResponseEntitiy, new()
        {
            var body =  new Response<T>
            {
                Message = message,
                Value = value
            };

            var serializer = new XmlSerializer(typeof(Response<T>));
            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw))
                {
                    serializer.Serialize(writer, body);
                    await response.WriteStringAsync(sw.ToString());
                }
            }

            response.Headers.Add("Content-Type", "application/xml");

            return response;
        }

        internal static async Task<HttpResponseData> CreateErrorResponse<T>(string message, HttpResponseData response) where T : ResponseEntitiy, new()
        {
            var body = new Response<T>
            {
                Message = message,
                IsSuccsessful = false,
                Value = default
            };

            var serializer = new XmlSerializer(typeof(Response<T>));
            using (var sw = new StringWriter())
            {
                using (var writer = XmlWriter.Create(sw))
                {
                    serializer.Serialize(writer, body);
                    await response.WriteStringAsync(sw.ToString());
                }
            }

            response.Headers.Add("Content-Type", "application/xml");

            return response;
        }
    }
}
