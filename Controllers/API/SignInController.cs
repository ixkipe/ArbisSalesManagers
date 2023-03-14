using System.Net.Mime;
using System.Security.Cryptography;
using ArbisSalesManagers.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
[AllowAnonymous]
public class SignInController : ControllerBase {
  private readonly IDbConnectionProvider _provider;

  public SignInController(IDbConnectionProvider provider)
  {
    _provider = provider;
  }

  [HttpPost]
  [Consumes(MediaTypeNames.Application.Json)]
  public async Task<IActionResult> SignIn(CredentialDTO user) {
    if (HttpContext.User.Identity.IsAuthenticated) return BadRequest("Already authenticated.");
    if (!ModelState.IsValid) return NotFound("Either the user has not been found or the password is incorrect.");

    var userFromDb = await this._provider.Connection.QueryFirstOrDefaultAsync<Credential>(
      Queries.FetchUser,
      new {
        username = user.Username
      }
    );

    using (HMACSHA512 hmac = new HMACSHA512(userFromDb.Password_Salt)) {
      var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(user.Password));
      if (!hash.SequenceEqual(userFromDb.Password_Hash)) return NotFound("Either the user has not been found or the password is incorrect.");

      var claims = new List<Claim>() {
        new Claim(ClaimTypes.Name, userFromDb.Is_Admin ? "admin" : "user")
      };
      var identity = new ClaimsIdentity(claims, "AmoAsteriskAuthCookie");
      ClaimsPrincipal principal = new(identity);

      await HttpContext.SignInAsync("AmoAsteriskAuthCookie", principal);

      return Ok($"Welcome back, {user.Username}!");
    }
  }
}