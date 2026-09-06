namespace Lidemia.Core.Models.Base;

public class AbstractEntity<T> : AbstractBaseEntity<T> where T : notnull
{
    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}