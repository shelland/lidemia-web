// Created on 20/11/2021 12:43 by Andrey Laserson

using Lidemia.Core.Models.Base;
using Lidemia.DataAccess.Abstract;
using Lidemia.DataAccess.Entities.Meta;

namespace Lidemia.DataAccess.Entities;

public class OrderEntity : IDbEntity, IHasId<long>, IHasMetadata<OrderMetadata>
{
    public long Id { get; set; }

    public string Number { get; set; } = string.Empty;

    public long CustomerId { get; set; }

    public CustomerEntity Customer { get; set; } = null!;

    public string? Comment { get; set; }

    public bool IsFinishConfirmed { get; set; }
    
    public OrderMetadata Metadata { get; set; } = null!;
    
    public IEnumerable<OrderItemEntity> Items { get; set; } = null!;

    public CustomerAddressEntity? CustomerAddress { get; set; }

    public Guid? CustomerAddressId { get; set; }

    public int RowVersion { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset CreateDate { get; set; }

    public DateTimeOffset? UpdateDate { get; set; }
}