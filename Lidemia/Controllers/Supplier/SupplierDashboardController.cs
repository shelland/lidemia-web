// Created on 01/09/2026 14:28 by Laserson

using Lidemia.Core.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers.Supplier;

[SupplierAuthorize]
[Route("Supplier/Dashboard")]
public class SupplierDashboardController : Controller
{
}