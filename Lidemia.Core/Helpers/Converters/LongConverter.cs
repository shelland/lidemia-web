// Created on 10/10/2020 21:16 by Andrey Laserson

using System.Text.Json;
using System.Text.Json.Serialization;
using Lidemia.Core.Extensions;

namespace Lidemia.Core.Helpers.Converters;

public class LongConverter : JsonConverter<long>
{
    public override long Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return long.Parse(reader.GetString().NotNull());
    }

    public override void Write(Utf8JsonWriter writer, long value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}