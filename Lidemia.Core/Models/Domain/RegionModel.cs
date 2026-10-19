// Created on 12/10/2026 22:45 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class RegionModel : AbstractEntity<Guid>
{
    public string Name { get; set; } = string.Empty;

    public Guid CountryId { get; set; }
}