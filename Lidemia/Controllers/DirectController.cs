// Created on 23/11/2026 19:47 by Laserson

using FluentValidation;
using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;
using Lidemia.Core.Models.Misc;
using Lidemia.Core.Models.Service;
using Lidemia.Hubs;
using Lidemia.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace Lidemia.Controllers;

[CustomerOrSupplierAuthorize]
[Route("[controller]")]
public class DirectController : BaseController
{
    private readonly IChatService chatService;
    private readonly IProductService productService;
    private readonly ISupplierOrderService supplierOrderService;
    private readonly IValidator<SendChatMessageRequestDto> validator;
    private readonly IHubContext<DirectChatHub> hubContext;
    private readonly ILogger<DirectController> logger;

    public DirectController(
        IChatService chatService,
        IProductService productService,
        ISupplierOrderService supplierOrderService,
        IValidator<SendChatMessageRequestDto> validator,
        IHubContext<DirectChatHub> hubContext,
        ILogger<DirectController> logger)
    {
        this.chatService = chatService;
        this.productService = productService;
        this.supplierOrderService = supplierOrderService;
        this.validator = validator;
        this.hubContext = hubContext;
        this.logger = logger;
    }

    [HttpGet]
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    public async Task<IActionResult> Index([FromQuery] long? chatId, CancellationToken cancellationToken)
    {
        var role = EntityType;
        var entityId = RequireEntityId;

        if (role is not (EntityType.Customer or EntityType.Supplier))
        {
            return RedirectToAction("Index", "Home");
        }

        var chats = await this.chatService.GetChats(entityId, role, cancellationToken);
        this.logger.LogInformation("The chat list for {Role} {EntityId} contains {ChatCount} conversation(s)",
            role, entityId, chats.Count);
        ChatModel? activeChat;

        if (chatId != null)
        {
            activeChat = chats.FirstOrDefault(x => x.Id == chatId);

            if (activeChat == null)
            {
                return NotFoundView;
            }
        }
        else
        {
            activeChat = chats.FirstOrDefault();
        }

        IReadOnlyList<ChatMessageModel> messages = [];
        IReadOnlyDictionary<long, ChatProductDto> products = new Dictionary<long, ChatProductDto>();
        IReadOnlyDictionary<long, ChatOrderDto> orders = new Dictionary<long, ChatOrderDto>();

        if (activeChat != null)
        {
            await this.chatService.MarkRead(activeChat.Id, entityId, role, cancellationToken);
            activeChat.UnreadCount = 0;

            messages = await this.chatService.GetMessages(activeChat.Id, entityId, role, cancellationToken);
            products = await this.GetMessageProducts(messages, cancellationToken);
            orders = await this.GetMessageOrders(messages, activeChat, role, cancellationToken);
        }

        var viewModel = new DirectIndexViewModel(chats, activeChat, messages, role, products, orders);
        return View(viewModel);
    }

    [HttpGet("Start/{supplierId:long}")]
    [CustomerAuthorize]
    public async Task<IActionResult> Start([FromRoute] long supplierId, CancellationToken cancellationToken)
    {
        var result = await this.chatService.GetOrCreateChat(RequireEntityId, supplierId, cancellationToken);

        if (result.IsFailed)
        {
            return NotFoundView;
        }

        var chat = await this.chatService.GetChat(result.Value, RequireEntityId, EntityType.Customer, cancellationToken);

        if (chat == null)
        {
            return NotFoundView;
        }

        // The supplier may not have any message from this customer yet, the conversation still
        // has to show up in the supplier chat list so the supplier is able to reply.
        await this.hubContext.Clients.Group(DirectChatHub.GetEntityGroupName(EntityType.Supplier, chat.SupplierId))
            .SendAsync("ChatCreated", this.CreateChatSummary(chat), cancellationToken);

        return RedirectToAction(nameof(Index), new { chatId = chat.Id });
    }

    [HttpGet("Products")]
    public async Task<ResultInfo> Products([FromQuery] long chatId, CancellationToken cancellationToken)
    {
        var role = EntityType;

        if (role is not (EntityType.Customer or EntityType.Supplier))
        {
            return new ErrorResultInfo(["errors.unauthorized"]);
        }

        var chat = await this.chatService.GetChat(chatId, RequireEntityId, role, cancellationToken);

        if (chat == null)
        {
            return new ErrorResultInfo(["direct.chat.accessDenied"]);
        }

        var products = await this.productService.GetSupplierCatalog(chat.SupplierId, cancellationToken);
        var dtos = products.Select(this.CreateProductDto).ToList();

        return new ResultInfo<IReadOnlyList<ChatProductDto>>(dtos);
    }

    [HttpGet("Orders")]
    public async Task<ResultInfo> Orders([FromQuery] long chatId, CancellationToken cancellationToken)
    {
        var role = EntityType;

        if (role is not (EntityType.Customer or EntityType.Supplier))
        {
            return new ErrorResultInfo(["errors.unauthorized"]);
        }

        var chat = await this.chatService.GetChat(chatId, RequireEntityId, role, cancellationToken);

        if (chat == null)
        {
            return new ErrorResultInfo(["direct.chat.accessDenied"]);
        }

        var orders = await this.supplierOrderService.GetCustomerSupplierOrders(chat.CustomerId, chat.SupplierId, cancellationToken);
        var dtos = orders.Select(x => this.CreateOrderDto(x, role)).ToList();

        return new ResultInfo<IReadOnlyList<ChatOrderDto>>(dtos);
    }

    [HttpPost("Send")]
    public async Task<ResultInfo> Send([FromBody] SendChatMessageRequestDto request, CancellationToken cancellationToken)
    {
        var validationResult = await this.validator.ValidateAsync(request, cancellationToken);

        if (!validationResult.IsValid)
        {
            return new ErrorResultInfo(validationResult.GetErrors());
        }

        var role = EntityType;
        var entityId = RequireEntityId;

        if (role is not (EntityType.Customer or EntityType.Supplier))
        {
            return new ErrorResultInfo(["errors.unauthorized"]);
        }

        var content = request.Type switch
        {
            ChatMessageType.Product => ChatMessageContent.CreateProduct(request.ProductId.GetValueOrDefault()),
            ChatMessageType.Order => ChatMessageContent.CreateOrder(request.OrderId.GetValueOrDefault()),
            _ => ChatMessageContent.CreateText(request.Text ?? string.Empty)
        };

        var result = await this.chatService.SendMessage(request.ChatId, entityId, role, content, cancellationToken);

        if (result.IsFailed)
        {
            return new ErrorResultInfo(result.Errors.Select(x => x.Message));
        }

        var message = result.Value;
        var chat = await this.chatService.GetChat(request.ChatId, entityId, role, cancellationToken);

        if (chat == null)
        {
            return new ErrorResultInfo();
        }

        var product = message.Type == ChatMessageType.Product && message.ProductId.HasValue
            ? await this.productService.GetById(message.ProductId.Value, cancellationToken)
            : null;

        var order = message.Type == ChatMessageType.Order && message.OrderId.HasValue
            ? await this.supplierOrderService.GetCustomerSupplierOrder(message.OrderId.Value, chat.CustomerId, chat.SupplierId,
                cancellationToken)
            : null;

        var dto = this.CreateMessageDto(message, chat, product, order, role);

        await this.hubContext.Clients.Group(DirectChatHub.GetChatGroupName(chat.Id))
            .SendAsync("ReceiveMessage", dto, cancellationToken);

        var recipientType = message.SenderType == EntityType.Customer ? EntityType.Supplier : EntityType.Customer;
        var recipientId = message.SenderType == EntityType.Customer ? chat.SupplierId : chat.CustomerId;

        var summary = this.CreateChatSummary(chat) with
        {
            LastMessageText = chat.LastMessageText,
            LastMessageDate = message.CreateDate
        };

        await this.hubContext.Clients.Group(DirectChatHub.GetEntityGroupName(recipientType, recipientId))
            .SendAsync("ChatUpdated", summary, cancellationToken);

        return new ResultInfo<ChatMessageDto>(dto);
    }

    [HttpPost("Read")]
    public async Task<ResultInfo> Read([FromQuery] long chatId, CancellationToken cancellationToken)
    {
        var role = EntityType;

        if (role is not (EntityType.Customer or EntityType.Supplier))
        {
            return new ErrorResultInfo(["errors.unauthorized"]);
        }

        await this.chatService.MarkRead(chatId, RequireEntityId, role, cancellationToken);
        return ResultInfo.Ok;
    }

    private async Task<IReadOnlyDictionary<long, ChatProductDto>> GetMessageProducts(IReadOnlyList<ChatMessageModel> messages,
        CancellationToken cancellationToken)
    {
        var productIds = messages
            .Where(x => x is { Type: ChatMessageType.Product, ProductId: not null })
            .Select(x => x.ProductId.GetValueOrDefault())
            .Distinct()
            .ToArray();

        if (productIds.Length == 0)
        {
            return new Dictionary<long, ChatProductDto>();
        }

        var products = await this.productService.GetProducts(productIds, cancellationToken);

        return products.ToDictionary(x => x.Id, this.CreateProductDto);
    }

    private async Task<IReadOnlyDictionary<long, ChatOrderDto>> GetMessageOrders(IReadOnlyList<ChatMessageModel> messages,
        ChatModel chat, EntityType role, CancellationToken cancellationToken)
    {
        var orderIds = messages
            .Where(x => x is { Type: ChatMessageType.Order, OrderId: not null })
            .Select(x => x.OrderId.GetValueOrDefault())
            .Distinct()
            .ToArray();

        var orders = new Dictionary<long, ChatOrderDto>();

        foreach (var orderId in orderIds)
        {
            var order = await this.supplierOrderService.GetCustomerSupplierOrder(orderId, chat.CustomerId, chat.SupplierId,
                cancellationToken);

            if (order != null)
            {
                orders[orderId] = this.CreateOrderDto(order, role);
            }
        }

        return orders;
    }

    private ChatMessageDto CreateMessageDto(ChatMessageModel message, ChatModel chat, ProductModel? product,
        SupplierOrderModel? order, EntityType role)
    {
        return new ChatMessageDto(
            Id: message.Id.ToString(),
            ChatId: chat.Id.ToString(),
            CustomerId: chat.CustomerId.ToString(),
            SupplierId: chat.SupplierId.ToString(),
            SenderId: message.SenderId.ToString(),
            SenderType: message.SenderType.ToString(),
            Type: message.Type.ToString(),
            Text: message.Text,
            Product: product != null ? this.CreateProductDto(product) : null,
            Order: order != null ? this.CreateOrderDto(order, role) : null,
            CreateDate: message.CreateDate);
    }

    private ChatProductDto CreateProductDto(ProductModel product)
    {
        var price = product.HasCurrentDiscount && product.DiscountPrice != null ? product.DiscountPrice : product.Price;

        return new ChatProductDto(
            Id: product.Id.ToString(),
            Title: product.Title,
            Price: price.GetDisplayValue(),
            PhotoUrl: product.PrimaryPhoto?.SmallThumbPath ?? product.PrimaryPhoto?.OriginalFilePath,
            Url: this.Url.Action("Details", "Products", new { id = product.Id, slug = product.Slug }));
    }

    private ChatOrderDto CreateOrderDto(SupplierOrderModel order, EntityType role)
    {
        var items = order.Items.Items;
        var totalSum = items.Sum(x => x.Quantity * (x.DiscountPriceForSingle ?? x.PriceForSingle));

        // Both participants open the order in the section they are allowed to see.
        var url = role == EntityType.Supplier
            ? this.Url.Action("Details", "SupplierOrders", new { id = order.Id })
            : this.Url.Action("Details", "CustomerOrders", new { id = order.Order.Id });

        return new ChatOrderDto(
            Id: order.Order.Id.ToString(),
            Number: order.Order.Number,
            Status: order.Status.GetLocalizedDisplayName(),
            TotalSum: totalSum.ToPrice().GetDisplayValue(),
            ItemCount: items.Count,
            CreateDate: order.Order.CreateDate,
            Url: url);
    }

    private ChatSummaryDto CreateChatSummary(ChatModel chat)
    {
        return new ChatSummaryDto(
            ChatId: chat.Id.ToString(),
            CustomerId: chat.CustomerId.ToString(),
            SupplierId: chat.SupplierId.ToString(),
            CustomerName: chat.CustomerName,
            SupplierName: chat.SupplierName,
            LastMessageText: chat.LastMessageText,
            LastMessageDate: chat.LastMessageDate);
    }
}
