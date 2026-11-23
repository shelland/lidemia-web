// Created on 23/11/2026 19:38 by Laserson

using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Models.Domain;
using Lidemia.DataAccess.Entities;
using Microsoft.Extensions.DependencyInjection;
using Scrutor;

namespace Lidemia.Common.Mapper;

public interface IChatMessageMapper : IMapper<ChatMessageEntity, ChatMessageModel>;

[ServiceDescriptor<IChatMessageMapper>(ServiceLifetime.Scoped)]
public class ChatMessageMapper : IChatMessageMapper
{
    public ChatMessageModel Map(ChatMessageEntity source)
    {
        return new ChatMessageModel
        {
            Id = source.Id,
            ChatId = source.ChatId,
            SenderType = source.SenderType,
            SenderId = source.SenderId,
            Type = source.Text.Type,
            Text = source.Text.Text,
            ProductId = source.Text.ProductId,
            OrderId = source.Text.OrderId,
            IsRead = source.IsRead,
            CreateDate = source.CreateDate,
            UpdateDate = source.UpdateDate
        };
    }
}
