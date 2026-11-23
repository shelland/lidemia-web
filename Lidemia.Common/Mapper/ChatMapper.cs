// Created on 23/11/2026 19:38 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IChatMapper : IMapper<ChatEntity, ChatModel>;

[ServiceDescriptor<IChatMapper>(ServiceLifetime.Scoped)]
public class ChatMapper : IChatMapper
{
    public ChatModel Map(ChatEntity source)
    {
        return new ChatModel
        {
            Id = source.Id,
            CustomerId = source.CustomerId,
            SupplierId = source.SupplierId,
            CustomerName = $"{source.Customer.FirstName} {source.Customer.LastName}".Trim(),
            SupplierName = source.Supplier.Name,
            LastMessageText = source.LastMessageText,
            LastMessageDate = source.LastMessageDate,
            CreateDate = source.CreateDate,
            UpdateDate = source.UpdateDate
        };
    }
}