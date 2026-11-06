// Created on 05/11/2026 21:20 by Laserson

using Lidemia.Web.BusinessLogic.Services.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace Lidemia.Controllers;

[Route("SignOut")]
public class SignOutController : ControllerBase
{
    private readonly IWebAuthService webAuthService;

    public SignOutController(IWebAuthService webAuthService)
    {
        this.webAuthService = webAuthService;
    }

    [HttpGet]
    public async Task<IActionResult> SignOut(CancellationToken cancellationToken)
    {
        await this.webAuthService.SignOut(cancellationToken);

        return RedirectToAction("Index", "Home", new
        {
            signOut = 1
        });
    }
}