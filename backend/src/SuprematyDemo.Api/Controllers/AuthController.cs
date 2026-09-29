using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SuprematyDemo.Application.Auth;
namespace SuprematyDemo.Api.Controllers;
[ApiController, Route("api/auth")]
public sealed class AuthController(IAuthService auth, IWebHostEnvironment environment) : ControllerBase
{
    [HttpPost("signup"), AllowAnonymous] public async Task<ActionResult<AuthResponse>> SignUp(SignUpRequest request, CancellationToken ct) { try { return Ok(await auth.SignUpAsync(request, ct)); } catch (ArgumentException e) { return BadRequest(new ProblemDetails{Title="Validation failed",Detail=e.Message,Status=400}); } catch (InvalidOperationException e) { return Conflict(new ProblemDetails{Title="Account already exists",Detail=e.Message,Status=409}); } }
    [HttpPost("login"), AllowAnonymous] public async Task<ActionResult<AuthResponse>> Login(LoginRequest request, CancellationToken ct) { try { return Ok(await auth.LoginAsync(request, ct)); } catch (UnauthorizedAccessException) { return Unauthorized(new ProblemDetails{Title="Authentication failed",Detail="Invalid email or password.",Status=401}); } }
    [HttpPost("forgot-password"), AllowAnonymous] public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword(ForgotPasswordRequest request, CancellationToken ct) => Ok(await auth.ForgotPasswordAsync(request, environment.IsDevelopment(), ct));
    [HttpPost("reset-password"), AllowAnonymous] public async Task<IActionResult> ResetPassword(ResetPasswordRequest request, CancellationToken ct) { try { await auth.ResetPasswordAsync(request, ct); return NoContent(); } catch (Exception e) when (e is ArgumentException or InvalidOperationException) { return BadRequest(new ProblemDetails{Title="Password reset failed",Detail=e.Message,Status=400}); } }
    [HttpGet("me"), Authorize] public async Task<ActionResult<UserDto>> Me(CancellationToken ct) { var value=User.FindFirstValue(ClaimTypes.NameIdentifier); return Guid.TryParse(value,out var id) && await auth.GetUserAsync(id,ct) is { } user ? Ok(user) : Unauthorized(); }
}
