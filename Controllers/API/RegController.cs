using System.Net.Mime;
using System.Security.Cryptography;
using ArbisSalesManagers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// !!!
// testing only, delete from final version

[ApiController]
[Route("api/[controller]")]
public class RegController : ControllerBase {
  private readonly IDbConnectionProvider _provider;

  public RegController(IDbConnectionProvider provider)
  {
    _provider = provider;
  }

  [HttpGet]
  public async Task<IActionResult> DummyResponse() => new JsonResult( new { message = "This is a dummy response. Find something better to do, slacker." });

  [HttpPost]
  [Consumes(MediaTypeNames.Application.Json)]
  [Authorize(Policy = "MustBeAdmin")]
  public async Task<IActionResult> Register([FromBody] RegisterDto creds) {
    if (this._provider.Connection.QueryFirst<int>(Queries.CheckIfUserExists, new { username = creds.Username }) > 0) {
      return BadRequest("User already exists.");
    }

    using HMACSHA512 hmac = new();
    var salt = hmac.Key;
    var hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(creds.Password));

    var result = await this._provider.Connection.ExecuteAsync(
      Queries.Register,
      new {
        username = creds.Username,
        salt = salt,
        hash = hash,
        is_admin = creds.Is_Admin
      }
    );

    return Ok($"User {creds.Username} successfully created.");
  }
}