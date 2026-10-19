using System.Text.Json.Serialization;

namespace Lidemia.Core.Models.Base;

public class AbstractEntity<T> : AbstractBaseEntity<T> where T : notnull
{
    [JsonIgnore]
    public DateTimeOffset CreateDate { get; set; }

    [JsonIgnore]
    public DateTimeOffset? UpdateDate { get; set; }
}