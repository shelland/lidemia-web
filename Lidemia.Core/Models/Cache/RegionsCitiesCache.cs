// Created on 13/10/2026 21:38 by Laserson

namespace Lidemia.Core.Models.Cache;

public class RegionsCitiesCache
{
    public Dictionary<Guid, string> Regions { get; set; } = new();

    public Dictionary<Guid, KeyValuePair<Guid, string>> Cities { get; set; } = new();
}