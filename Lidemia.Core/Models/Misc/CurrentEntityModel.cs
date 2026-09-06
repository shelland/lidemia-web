// Created on 20/12/2021 23:13 by shell

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Base;

namespace Lidemia.Core.Models.Misc;

public class CurrentEntityModel : AbstractIdEntity<long>
{
    public string Name { get; set; } = string.Empty;
    
    public EntityType Role { get; set; }
}