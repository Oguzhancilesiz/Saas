using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Saas.Application.Abstractions;
using Saas.Application.Contracts.Subscriptions;
using System.Security.Claims;

namespace Saas.Api.Controllers
{
    [ApiController]
    [Route("api/subscriptions")]
    public sealed class SubscriptionsController : ControllerBase
    {
        private readonly ISubscriptionService _subs;

        public SubscriptionsController(ISubscriptionService subs) => _subs = subs;

        //[HttpPost]
        //public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest req, CancellationToken ct)
        //{
        //    var result = await _subs.SubscribeAsync(req, ct);
        //    return Ok(result);
        //}
        [Authorize]
        [HttpPost]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeRequest req, CancellationToken ct)
        {
            var userId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            var result = await _subs.SubscribeAsync(req with { UserId = userId }, ct);
            return Ok(result);
        }
        [HttpGet("{userId:guid}/apps/{appId:guid}")]
        public async Task<IActionResult> GetUserSubs([FromRoute] Guid userId, [FromRoute] Guid appId, CancellationToken ct)
        {
            var items = await _subs.GetUserSubscriptionsAsync(userId, appId, ct);
            return Ok(items);
        }
    }
}
