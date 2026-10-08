// Created on 02/09/2026 19:59 by Laserson

using Lidemia.Core.Models.Base;
using System.Text.Json.Serialization;

namespace Lidemia.Core.Models.Domain;

public class ProductModel : AbstractEntity<long>
{
    public string? Title { get; set; }

    [JsonIgnore]
    public long SupplierId { get; set; }

    public string[] Tags { get; set; } = [];
}