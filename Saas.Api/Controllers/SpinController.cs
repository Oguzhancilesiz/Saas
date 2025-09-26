using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Api.Authorization;
using Saas.Application.Abstractions;
using System.Security.Claims;

namespace Saas.Api.Controllers
{
    [ApiController]
    [Route("api/spin")]
    [Authorize]
    [RequireApp("SPIN")] // bu endpoint sadece SPIN app için
    public sealed class SpinController : ControllerBase
    {
        private readonly IAppContext _appCtx;
        private readonly IUsageService _usage;

        public SpinController(IAppContext appCtx, IUsageService usage)
        {
            _appCtx = appCtx; _usage = usage;
        }

        public sealed record StartSpinRequest(int ParticipantCount);

        [HttpPost("start")]
        public async Task<IActionResult> Start([FromBody] StartSpinRequest req, CancellationToken ct)
        {
            if (_appCtx.AppId is null) return BadRequest("App not resolved");
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            const string feature = "SPIN_START";

            // Gate ile kontrol + log
            var gate = HttpContext.RequestServices.GetRequiredService<IFeatureGate>();
            var ok = await gate.CheckAndLogAsync(userId, _appCtx.AppId.Value, feature, 1, ct);
            if (!ok) return StatusCode(429, "Limit reached for SPIN_START");

            var winnerIndex = Random.Shared.Next(0, Math.Max(req.ParticipantCount, 1));
            return Ok(new { winnerIndex });
        }
        [Authorize]
        [HttpGet("whoami")]
        public IActionResult WhoAmI()
        {
            var nameId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var email = User.FindFirstValue(ClaimTypes.Email);
            var roles = User.FindAll(ClaimTypes.Role).Select(r => r.Value).ToArray();

            return Ok(new { nameId, email, roles });
        }
    }
}