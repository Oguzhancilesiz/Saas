using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Saas.Application.Abstractions;
using Saas.Infrastructure.AppRuntime;
using Saas.Infrastructure.Auth;
using Saas.Infrastructure.Persistence;
using Saas.Infrastructure.Services;
using Saas.Infrastructure.Time;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Saas.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddDbContext<SaasDbContext>(opt =>
                opt.UseSqlServer(config.GetConnectionString("SaasDb")));

            services.AddIdentityCore<AppUser>(opt =>
            {
                opt.User.RequireUniqueEmail = true;
            })
            .AddRoles<IdentityRole<Guid>>()
            .AddEntityFrameworkStores<SaasDbContext>();

            // JWT auth
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(opt =>
                {
                    opt.TokenValidationParameters = new()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config["Jwt:Issuer"],
                        ValidAudience = config["Jwt:Audience"],
                        IssuerSigningKey = key
                    };
                });

            // Common services
            services.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
            services.AddScoped<IPlanService, PlanService>();
            services.AddScoped<ISubscriptionService, SubscriptionService>();
            services.AddScoped<IJwtTokenService, JwtTokenService>();
            services.AddScoped<AppContextAccessor>();
            services.AddScoped<IAppContext>(sp => sp.GetRequiredService<AppContextAccessor>());
            services.AddScoped<IUsageService, UsageService>();
            services.AddScoped<IFeatureGate, FeatureGate>();
            services.AddScoped<IPricingService, PricingService>();

            return services;
        }
    }

}
