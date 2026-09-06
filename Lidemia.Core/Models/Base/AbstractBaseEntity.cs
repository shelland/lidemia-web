namespace Lidemia.Core.Models.Base;

public class AbstractBaseEntity<T> : AbstractIdEntity<T> where T : notnull
{
    public bool IsActive { get; set; }
}