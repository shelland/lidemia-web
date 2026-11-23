// Created on 23/11/2026 19:40 by Laserson

using FluentResults;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Common.Mapper;
using Lidemia.Common.Mapper.Abstract;
using Lidemia.Core.Enums;
using Lidemia.Core.Errors;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Misc;
using Lidemia.DataAccess.Entities;
using Lidemia.DataAccess.Repository.Abstract;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Scrutor;

namespace Lidemia.Common.BusinessLogic.Services.Data;

[ServiceDescriptor<IChatService>(ServiceLifetime.Scoped)]
public class ChatService : IChatService
{
    private const int MessagesLimit = 200;

    private readonly IChatRepository chatRepository;
    private readonly IChatMessageRepository chatMessageRepository;
    private readonly IChatMapper chatMapper;
    private readonly IChatMessageMapper chatMessageMapper;
    private readonly IProductService productService;
    private readonly ISupplierOrderService supplierOrderService;
    private readonly ILogger<ChatService> logger;

    public ChatService(
        IChatRepository chatRepository,
        IChatMessageRepository chatMessageRepository,
        IChatMapper chatMapper,
        IChatMessageMapper chatMessageMapper,
        IProductService productService,
        ISupplierOrderService supplierOrderService,
        ILogger<ChatService> logger)
    {
        this.chatRepository = chatRepository;
        this.chatMessageRepository = chatMessageRepository;
        this.chatMapper = chatMapper;
        this.chatMessageMapper = chatMessageMapper;
        this.productService = productService;
        this.supplierOrderService = supplierOrderService;
        this.logger = logger;
    }

    public async Task<Result<long>> GetOrCreateChat(long customerId, long supplierId, CancellationToken cancellationToken)
    {
        var chat = await this.chatRepository.GetOrCreate(customerId, supplierId, cancellationToken);

        if (chat == null)
        {
            return Result.Fail(CommonErrors.NotFoundError);
        }

        return Result.Ok(chat.Id);
    }

    public async Task<IReadOnlyList<ChatModel>> GetChats(long entityId, EntityType entityType, CancellationToken cancellationToken)
    {
        IReadOnlyList<ChatEntity> chats = entityType switch
        {
            EntityType.Customer => await this.chatRepository.GetCustomerChats(entityId, cancellationToken),
            EntityType.Supplier => await this.chatRepository.GetSupplierChats(entityId, cancellationToken),
            _ => []
        };

        if (chats.Count == 0)
        {
            return [];
        }

        var unreadCounts = await this.chatMessageRepository.GetUnreadCounts(chats.Select(x => x.Id).ToList(), entityType, cancellationToken);
        var models = this.chatMapper.MapList(chats);

        foreach (var model in models)
        {
            model.UnreadCount = unreadCounts.GetValueOrDefault(model.Id);
        }

        return models;
    }

    public async Task<ChatModel?> GetChat(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken)
    {
        var chat = await this.chatRepository.GetChat(chatId, entityId, entityType, cancellationToken);

        if (chat == null)
        {
            return null;
        }

        var model = this.chatMapper.Map(chat);
        var unreadCounts = await this.chatMessageRepository.GetUnreadCounts([chat.Id], entityType, cancellationToken);
        model.UnreadCount = unreadCounts.GetValueOrDefault(model.Id);

        return model;
    }

    public async Task<IReadOnlyList<ChatMessageModel>> GetMessages(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken)
    {
        var hasAccess = await this.chatRepository.HasAccess(chatId, entityId, entityType, cancellationToken);

        if (!hasAccess)
        {
            return [];
        }

        var messages = await this.chatMessageRepository.GetMessages(chatId, MessagesLimit, cancellationToken);
        return this.chatMessageMapper.MapList(messages);
    }

    public async Task<Result<ChatMessageModel>> SendMessage(long chatId, long entityId, EntityType entityType, ChatMessageContent content,
        CancellationToken cancellationToken)
    {
        var chat = await this.chatRepository.GetChat(chatId, entityId, entityType, cancellationToken);

        if (chat == null)
        {
            return Result.Fail(CommonErrors.NotFoundError);
        }

        ChatMessageContent normalizedContent;
        string? lastMessageText;

        switch (content.Type)
        {
            case ChatMessageType.Message:
                var message = content.Text?.Trim();

                if (string.IsNullOrEmpty(message))
                {
                    return Result.Fail(new Error("direct.message.empty"));
                }

                normalizedContent = ChatMessageContent.CreateText(message);
                lastMessageText = message;
                break;

            case ChatMessageType.Product:
                var product = content.ProductId.HasValue
                    ? await this.productService.GetById(content.ProductId.Value, cancellationToken)
                    : null;

                // Only products of the supplier the chat belongs to can be shared in the conversation.
                if (product == null || product.Supplier.Id != chat.SupplierId)
                {
                    return Result.Fail(new Error("direct.message.product.invalid"));
                }

                normalizedContent = ChatMessageContent.CreateProduct(product.Id);
                lastMessageText = product.Title;
                break;

            case ChatMessageType.Order:
                var order = content.OrderId.HasValue
                    ? await this.supplierOrderService.GetCustomerSupplierOrder(content.OrderId.Value, chat.CustomerId, chat.SupplierId,
                        cancellationToken)
                    : null;

                // Only orders the customer placed with the supplier the chat belongs to can be shared.
                if (order == null)
                {
                    return Result.Fail(new Error("direct.message.order.invalid"));
                }

                normalizedContent = ChatMessageContent.CreateOrder(order.Order.Id);
                lastMessageText = order.Order.Number;
                break;

            default:
                return Result.Fail(new Error("direct.message.type.unsupported"));
        }

        var entity = await this.chatMessageRepository.Add(chatId, entityType, entityId, normalizedContent, lastMessageText,
            cancellationToken);
        this.logger.LogInformation("A new {MessageType} message {MessageId} was added to chat {ChatId} by {EntityType} {EntityId}",
            normalizedContent.Type, entity.Id, chatId, entityType, entityId);

        return Result.Ok(this.chatMessageMapper.Map(entity));
    }

    public async Task MarkRead(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken)
    {
        var hasAccess = await this.chatRepository.HasAccess(chatId, entityId, entityType, cancellationToken);

        if (!hasAccess)
        {
            return;
        }

        await this.chatMessageRepository.MarkRead(chatId, entityType, cancellationToken);
    }

    public Task<bool> HasAccess(long chatId, long entityId, EntityType entityType, CancellationToken cancellationToken)
    {
        return this.chatRepository.HasAccess(chatId, entityId, entityType, cancellationToken);
    }
}