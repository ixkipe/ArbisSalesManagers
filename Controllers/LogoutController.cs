using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArbisSalesManagers.Controllers;

[Authorize]
public class LogoutController : Controller {
  public async Task<IActionResult> Index() {
    await HttpContext.SignOutAsync("AmoAsteriskAuthCookie");
    return RedirectToAction(actionName: nameof(LoginController.Index), controllerName: "Login");
  }
}