using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Saas.Application.Abstractions;

namespace Saas.Api.Authorization
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true)]
    public sealed class RequireAppAttribute : Attribute, IAsyncAuthorizationFilter
    {
        private readonly string _code;
        public RequireAppAttribute(string appCode) => _code = appCode;

        public Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            var appCtx = context.HttpContext.RequestServices.GetRequiredService<IAppContext>();
            if (!string.Equals(appCtx.AppCode, _code, StringComparison.OrdinalIgnoreCase))
            {
                context.Result = new ContentResult
                {
                    StatusCode = StatusCodes.Status403Forbidden,
                    Content = $"This endpoint requires app '{_code}'."
                };
            }
            return Task.CompletedTask;
        }
    }
}
