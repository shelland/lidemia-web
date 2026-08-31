namespace Lidemia.DataAccess.Abstract;

public interface IDbEntity
{
    int RowVersion { get; set; }

    bool IsActive { get; set; }

    DateTimeOffset CreateDate { get; set; }

    DateTimeOffset? UpdateDate { get; set; }
}