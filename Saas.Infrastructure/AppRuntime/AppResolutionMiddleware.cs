using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Saas.Infrastructure.Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure.AppRuntime
{
    public sealed class AppResolutionMiddleware
    {
        private readonly RequestDelegate _next;

        public const string HeaderName = "X-App-Id";      // Guid
        public const string HeaderCode = "X-App-Code";    // "SPIN", "DIET"

        public AppResolutionMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext ctx, SaasDbContext db, AppContextAccessor accessor)
        {
            // 1) Önce Guid başlığa bak
            if (ctx.Request.Headers.TryGetValue(HeaderName, out var idVals)
                && Guid.TryParse(idVals.ToString(), out var appId))
            {
                var exists = await db.Apps.AsNoTracking().AnyAsync(x => x.Id == appId);
                if (!exists)
                {
                    ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await ctx.Response.WriteAsync("Invalid X-App-Id");
                    return;
                }
                accessor.AppId = appId;
                // Code’u da çekmek istersen:
                accessor.AppCode = await db.Apps.AsNoTracking().Where(x => x.Id == appId).Select(x => x.Code).FirstAsync();
                await _next(ctx);
                return;
            }

            // 2) Alternatif: Code başlığı
            if (ctx.Request.Headers.TryGetValue(HeaderCode, out var codeVals))
            {
                var code = codeVals.ToString().Trim();
                var app = await db.Apps.AsNoTracking().FirstOrDefaultAsync(x => x.Code == code);
                if (app is null)
                {
                    ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
                    await ctx.Response.WriteAsync("Invalid X-App-Code");
                    return;
                }
                accessor.AppId = app.Id;
                accessor.AppCode = app.Code;
                await _next(ctx);
                return;
            }

            // 3) Hiçbiri yok: reddet
            ctx.Response.StatusCode = StatusCodes.Status400BadRequest;
            await ctx.Response.WriteAsync("Missing X-App-Id or X-App-Code header");
        }
    }

}
