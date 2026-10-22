// Created on 12/10/2026 22:46 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class CityModel : AbstractEntity<long>
{
    public string Name { get; set; } = string.Empty;

    public long RegionId { get; set; }
}