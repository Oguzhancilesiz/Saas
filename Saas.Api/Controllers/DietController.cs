using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Api.Authorization;
using Saas.Application.Abstractions;
using System.Security.Claims;

namespace Saas.Api.Controllers
{
    [ApiController]
    [Route("api/diet")]
    [Authorize]
    [RequireApp("DIET")]
    public sealed class DietController : ControllerBase
    {
        private readonly IAppContext _appCtx;
        private readonly IUsageService _usage;

        public DietController(IAppContext appCtx, IUsageService usage)
        {
            _appCtx = appCtx; _usage = usage;
        }

        public sealed record CreateProgramRequest(Guid PatientId, string Title);

        [HttpPost("programs")]
        public async Task<IActionResult> CreateProgram([FromBody] CreateProgramRequest req, CancellationToken ct)
        {
            if (_appCtx.AppId is null) return BadRequest("App not resolved");
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            // Örn: Aylık 50 program oluşturma limiti
            var monthStart = new DateTime(DateTime.UtcNow.Year, DateTime.UtcNow.Month, 1);
            var nextMonth = monthStart.AddMonths(1).AddTicks(-1);

            var used = await _usage.CountAsync(userId, _appCtx.AppId.Value, "DIET_PROGRAM_CREATE", monthStart, nextMonth, ct);
            if (used >= 50) return StatusCode(429, "Monthly create limit reached");

            await _usage.LogAsync(userId, _appCtx.AppId.Value, "DIET_PROGRAM_CREATE", 1, ct);

            const string feature = "DIET_PROGRAM_CREATE";
            var gate = HttpContext.RequestServices.GetRequiredService<IFeatureGate>();
            var ok = await gate.CheckAndLogAsync(userId, _appCtx.AppId.Value, feature, 1, ct);
            if (!ok) return StatusCode(429, "Limit reached for DIET_PROGRAM_CREATE");


            // Burada gerçek program entity’siyle devam edeceğiz. Şimdilik sahte ID dönelim.
            return Ok(new { programId = Guid.NewGuid(), usedThisMonth = used + 1, title = req.Title, patientId = req.PatientId });


        }
    }
}
