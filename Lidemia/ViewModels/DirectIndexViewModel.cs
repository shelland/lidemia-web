// Created on 23/11/2026 19:43 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Models.Domain;
using Lidemia.Core.Models.Dto;

namespace Lidemia.ViewModels;

public record DirectIndexViewModel
(
    IReadOnlyList<ChatModel> Chats,
    ChatModel? ActiveChat,
    IReadOnlyList<ChatMessageModel> Messages,
    EntityType CurrentRole,
    IReadOnlyDictionary<long, ChatProductDto> Products,
    IReadOnlyDictionary<long, ChatOrderDto> Orders
);
