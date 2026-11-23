// Created on 23/11/2026 20:30 by Laserson

using System.Text.Json.Serialization;

namespace Lidemia.Core.Enums;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ChatMessageType
{
    Message = 1,
    Order = 2,
    Product = 3
}
