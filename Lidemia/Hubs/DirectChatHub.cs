// Created on 23/11/2026 19:45 by Laserson

using Lidemia.Common.BusinessLogic.Services.Data.Abstract;
using Lidemia.Core.Attributes;
using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Microsoft.AspNetCore.SignalR;

namespace Lidemia.Hubs;

[CustomerOrSupplierAuthorize]
public class DirectChatHub : Hub
{
    private readonly IChatService chatService;

    public DirectChatHub(IChatService chatService)
    {
        this.chatService = chatService;
    }

    public static string GetChatGroupName(long chatId) => $"direct-chat-{chatId}";

    public static string GetEntityGroupName(EntityType entityType, long entityId) => $"direct-entity-{entityType}-{entityId}";

    public override async Task OnConnectedAsync()
    {
        var entityId = this.Context.User?.GetEntityId();
        var entityType = entityId != null ? this.Context.User?.GetEntityRole() : null;

        if (entityId != null && entityType is EntityType.Customer or EntityType.Supplier)
        {
            await this.Groups.AddToGroupAsync(this.Context.ConnectionId, GetEntityGroupName(entityType.Value, entityId.Value));
        }

        await base.OnConnectedAsync();
    }

    public async Task JoinChat(long chatId)
    {
        var entityId = this.Context.User?.GetEntityId() ?? throw new HubException("direct.chat.unauthorized");
        var entityType = this.Context.User.NotNull().GetEntityRole();
        var hasAccess = await this.chatService.HasAccess(chatId, entityId, entityType, CancellationToken.None);

        if (!hasAccess)
        {
            throw new HubException("direct.chat.accessDenied");
        }

        await this.Groups.AddToGroupAsync(this.Context.ConnectionId, GetChatGroupName(chatId));
    }

    public Task LeaveChat(long chatId)
    {
        return this.Groups.RemoveFromGroupAsync(this.Context.ConnectionId, GetChatGroupName(chatId));
    }
}