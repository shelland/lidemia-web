// Created on 02/09/2026 19:59 by Laserson

using Lidemia.Core.Models.Base;
using Lidemia.Core.Models.Misc.Meta;

namespace Lidemia.Core.Models.Domain;

public class SupplierModel : AbstractEntity<long>
{
    public string Name { get; set; } = string.Empty;
    
    public UserModel? User { get; set; }

    public SupplierStatsModel? Stats { get; set; }

    public string? LogoUrl { get; set; }

    public SupplierMetadata Metadata { get; set; } = null!;
}