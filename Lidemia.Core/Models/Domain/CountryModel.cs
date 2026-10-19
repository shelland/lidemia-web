// Created on 12/10/2026 22:44 by Laserson

using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Domain;

public class CountryModel : AbstractEntity<Guid>
{
    public string Name { get; set; } = string.Empty;
}