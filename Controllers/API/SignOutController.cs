using ArbisSalesManagers.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SignOutController : ControllerBase {
  [HttpGet]
  public async Task<IActionResult> Index() {
    await HttpContext.SignOutAsync("AmoAsteriskAuthCookie");
    return Ok("See you around.");
  }
}