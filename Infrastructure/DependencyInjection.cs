using Application.Contracts.Repository;
using Infrastructure.Persistance.Connectrion.DBContext;
using Infrastructure.Persistance.Repository;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddJWTAuthentication(configuration);

            string connectionString = configuration.GetConnectionString("EnetityExampleDB");
            services.AddDbContext<AppDBContext>(option =>
            {
                option.UseSqlServer(connectionString, b =>
                {
                    b.MigrationsAssembly("AuthenticationTrial2");
                });

            });
            services.AddScoped<IUserRepository, UserRepository>();
            return services;
        }
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
            var cookieName = jwtSettings["CookieName"];
            services.AddAuthentication((options) =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters()
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtSettings["Issuer"],
                        ValidAudience = jwtSettings["Audience"],
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ClockSkew = TimeSpan.Zero
                    };

                    options.Events = new JwtBearerEvents
                    {
                        OnMessageReceived = context =>
                        {
                            if (context.Request.Cookies.TryGetValue(cookieName, out var token))
                            {
                                context.Token = token;
                            }
                            return Task.CompletedTask;
                        },
                        OnChallenge = context =>
                        {
                            // 1. Suppress the default 401 empty response
                            context.HandleResponse();

                            // 2. Check if it's an API/AJAX call vs a normal browser page navigation
                            var isApiRequest = context.Request.Path.StartsWithSegments("/api") ||
                                               context.Request.Headers.Accept.ToString().Contains("application/json");

                            if (isApiRequest)
                            {
                                context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                            }
                            else
                            {
                                // 3. Redirect normal page visits to the login page (preserving ReturnUrl)
                                var returnUrl = Uri.EscapeDataString(context.Request.Path + context.Request.QueryString);
                                context.Response.Redirect($"/login?returnUrl={returnUrl}");
                            }

                            return Task.CompletedTask;
                        }
                    };
                });
            return services;
        }
    }
}
