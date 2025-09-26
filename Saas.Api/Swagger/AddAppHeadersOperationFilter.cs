using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Saas.Api.Swagger
{
    public sealed class AddAppHeadersOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation op, OperationFilterContext ctx)
        {
            op.Parameters ??= new List<OpenApiParameter>();
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "X-App-Code",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema { Type = "string" },
                Description = "Uygulama kodu (örn: SPIN, DIET)"
            });
            op.Parameters.Add(new OpenApiParameter
            {
                Name = "X-App-Id",
                In = ParameterLocation.Header,
                Required = false,
                Schema = new OpenApiSchema { Type = "string", Format = "uuid" },
                Description = "Uygulama Guid’ı (X-App-Code yerine kullanılabilir)"
            });
        }
    }
}
