// Created on 03/09/2026 18:42 by Laserson

using Lidemia.Core.Enums;
using Lidemia.Core.Extensions;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

public class BaseController : Controller
{
    protected long? EntityId => this.HttpContext.User.GetEntityId();

    protected long RequireEntityId => EntityId.NotNull().Value;

    protected EntityType EntityType => this.HttpContext.User.GetEntityRole();

    public IActionResult NotFoundView => View("NotFound");
}