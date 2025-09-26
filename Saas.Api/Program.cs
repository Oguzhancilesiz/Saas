
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Saas.Api.Swagger;
using Saas.Application;
using Saas.Infrastructure;
using Saas.Infrastructure.AppRuntime;
using Saas.Infrastructure.Persistence;
namespace Saas.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            builder.Services.AddDbContext<SaasDbContext>(opt =>
                opt.UseSqlServer(builder.Configuration.GetConnectionString("SaasDb")));

            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);

            builder.Services.AddControllers();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "SaaS API", Version = "v1" });

                // Bearer þemasý
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Bearer token"
                });

                // ÖNEMLÝ: Operation'lara baðlamak için reference ile ekle
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });

                // Eðer eklediysen kalsýn:
                c.OperationFilter<AddAppHeadersOperationFilter>();
            });


            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();

            // App kimliði sadece spin/diet için zorunlu
            app.UseWhen(
                ctx => ctx.Request.Path.StartsWithSegments("/api/spin")
                       || ctx.Request.Path.StartsWithSegments("/api/diet"),
                branch => branch.UseMiddleware<AppResolutionMiddleware>());

            app.MapControllers();
            app.Run();
        }
    }
}
