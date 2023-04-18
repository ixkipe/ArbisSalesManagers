using ArbisSalesManagers.Models;
using Microsoft.AspNetCore.Mvc;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using AmoAsterisk.DbAccess;

namespace ArbisSalesManagers.Controllers;

[AllowAnonymous]
public class LoginController : Controller {
  private readonly IMysqlMiscConnectionProvider _provider;

  public LoginController(IMysqlMiscConnectionProvider provider)
  {
    _provider = provider;
  }

  [HttpGet]
  public async Task<IActionResult> Index() {
    if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction(actionName: nameof(HomeController.Index), controllerName: "Home");

    return View();
  }

  [HttpPost]
  [ValidateAntiForgeryToken]
  public async Task<IActionResult> Index(CredentialDTO user) {
    if (HttpContext.User.Identity.IsAuthenticated) return RedirectToAction(actionName: nameof(HomeController.Index), controllerName: "Home");
    if (!ModelState.IsValid) return View(model: user);

    var userFromDb = await this._provider.Connection.QueryFirstOrDefaultAsync<Credential>(
      Queries.FetchUser,
      new {
        username = user.Username
      }
    );

    using (HMACSHA512 hmac = new HMACSHA512(userFromDb.Password_Salt)) {
      var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
      if (!hash.SequenceEqual(userFromDb.Password_Hash)) return View();

      var claims = new List<Claim>() {
        new Claim(ClaimTypes.Name, userFromDb.Is_Admin ? "admin" : "user")
      };
      var identity = new ClaimsIdentity(claims, "AmoAsteriskAuthCookie");
      ClaimsPrincipal principal = new(identity);

      await HttpContext.SignInAsync("AmoAsteriskAuthCookie", principal);

      return RedirectToAction(actionName: nameof(HomeController.Index), controllerName: "Home");
    }
  }
}