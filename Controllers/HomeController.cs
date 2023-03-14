using ArbisSalesManagers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ArbisSalesManagers.Controllers;

[Authorize(Policy = "RegularUser")]
public class HomeController : Controller {
  public async Task<IActionResult> Index() => View();
  [AllowAnonymous]
  public async Task<IActionResult> Error() => View();
}