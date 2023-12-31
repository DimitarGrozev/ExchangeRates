﻿using ExchangeRates.Json.Contracts.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExchangeRates.Json.Utilities
{
    internal static class ResponseFactory
    {
        internal static Response<T> CreateSuccessResponse<T>(string message, T value) where T : ResponseEntitiy, new()
        {
            return new Response<T>
            {
                Message = message,
                Value = value
            };
        }

        internal static Response<T> CreateErrorResponse<T>(string message) where T : ResponseEntitiy, new()
        {
            return new Response<T>
            {
                Message = message,
                IsSuccsessful = false,
                Value = default
            };
        }
    }
}
