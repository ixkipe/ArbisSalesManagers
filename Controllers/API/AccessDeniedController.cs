using Microsoft.AspNetCore.Mvc;

public class AccessDeniedController : ControllerBase {
  [HttpGet]
  public async Task<IActionResult> Index() => Unauthorized("You don't have access to this resource.");
}