using Microsoft.AspNetCore.Mvc;
using Saas.Domain.Apps;
using Saas.Infrastructure.Persistence;

namespace Saas.Api.Controllers
{
    [ApiController]
    [Route("api/apps")]
    public class AppsController : ControllerBase
    {
        private readonly SaasDbContext _db;
        public AppsController(SaasDbContext db) => _db = db;

        [HttpPost("seed")]
        public async Task<IActionResult> Seed()
        {
            if (!_db.Apps.Any())
            {
                _db.Apps.AddRange(new App("Hesap Çarkı", "SPIN"), new App("Diyet", "DIET"));
                await _db.SaveChangesAsync();
            }
            return Ok();
        }

        [HttpGet]
        public IActionResult Get() => Ok(_db.Apps.ToList());
    }
}
