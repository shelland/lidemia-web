// Created on 23/09/2023 15:11 by shell

using System.Text.Json;
using System.Text.Json.Serialization;
using Lidemia.Core.Helpers.Converters;

namespace Lidemia.Core.Helpers;

public class CommonJsonOptions
{
    static CommonJsonOptions()
    {
        Options = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true,
            NumberHandling = JsonNumberHandling.AllowReadingFromString,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            Converters =
            {
                new LongConverter()
            }
        };
    }

    public static JsonSerializerOptions Options { get; }
}