using Microsoft.AspNetCore.Mvc;
using Saas.Application.Abstractions;
using Saas.Application.Contracts.Plans;

namespace Saas.Api.Controllers
{
    [ApiController]
    [Route("api/plans")]
    public sealed class PlansController : ControllerBase
    {
        private readonly IPlanService _plans;
        public PlansController(IPlanService plans) => _plans = plans;

        [HttpGet]
        public async Task<IActionResult> List(CancellationToken ct) =>
            Ok(await _plans.ListAsync(ct));

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreatePlanRequest req, CancellationToken ct)
        {
            var created = await _plans.CreateAsync(req, ct);
            return CreatedAtAction(nameof(List), new { id = created.Id }, created);
        }
    }
}
