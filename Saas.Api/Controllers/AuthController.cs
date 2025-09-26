using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Saas.Api.Contracts.Auth;
using Saas.Application.Abstractions;
using Saas.Infrastructure.Auth;

namespace Saas.Api.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public sealed class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _users;
        private readonly IJwtTokenService _jwt;

        public AuthController(UserManager<AppUser> users, IJwtTokenService jwt)
        {
            _users = users; _jwt = jwt;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest req)
        {
            var user = new AppUser { Id = Guid.NewGuid(), UserName = req.Email, Email = req.Email };
            var result = await _users.CreateAsync(user, req.Password);
            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var token = _jwt.CreateToken(user.Id, user.Email!, Array.Empty<string>());
            return Ok(new AuthResponse { Token = token, UserId = user.Id, Email = user.Email! });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest req)
        {
            var user = await _users.FindByEmailAsync(req.Email);
            if (user is null) return Unauthorized();

            if (!await _users.CheckPasswordAsync(user, req.Password))
                return Unauthorized();

            var roles = await _users.GetRolesAsync(user);
            var token = _jwt.CreateToken(user.Id, user.Email!, roles);
            return Ok(new AuthResponse { Token = token, UserId = user.Id, Email = user.Email! });
        }
        [HttpPost("register-or-login")]
        public async Task<IActionResult> RegisterOrLogin([FromBody] RegisterRequest req)
        {
            var user = await _users.FindByEmailAsync(req.Email);
            if (user is null)
            {
                user = new AppUser { Id = Guid.NewGuid(), UserName = req.Email, Email = req.Email };
                var create = await _users.CreateAsync(user, req.Password);
                if (!create.Succeeded) return BadRequest(create.Errors);
            }
            else
            {
                if (!await _users.CheckPasswordAsync(user, req.Password))
                    return Unauthorized(); // güvenlik: yanlış şifreye token yok
            }
            var token = _jwt.CreateToken(user.Id, user.Email!, Array.Empty<string>());
            return Ok(new AuthResponse { Token = token, UserId = user.Id, Email = user.Email! });
        }

    }
}
