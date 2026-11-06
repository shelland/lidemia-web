// Created on 22/11/2021 22:49 by Andrey Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;
using Lidemia.DataAccess.Entities.Meta;

namespace Lidemia.DataAccess.Entities;

public class CustomerEntity : IDbEntity, IHasId<long>, IHasMetadata<CustomerMetadata>
{
    public long Id { get; set; }

    public long UserId { get; set; }

    public UserEntity User { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public CustomerMetadata Metadata { get; set; } = null!;

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}