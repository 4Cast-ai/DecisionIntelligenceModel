﻿using Microsoft.Extensions.Diagnostics.HealthChecks;
using Newtonsoft.Json;

namespace Infrastructure.Models
{
    public class ApiValidationError
    {
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public string Field { get; }

        public string Message { get; }

        public ApiValidationError(string field, string message)
        {
            Field = field != string.Empty ? field : null;
            Message = message;
        }

        public override string ToString()
        {
            return $"Field: {Field}, Message: {Message}";
        }
    }
}
