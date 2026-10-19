using System.Text.Json.Serialization;

namespace Lidemia.Core.Models.Base;

public class AbstractBaseEntity<T> : AbstractIdEntity<T> where T : notnull
{
    [JsonIgnore]
    public bool IsActive { get; set; }
}