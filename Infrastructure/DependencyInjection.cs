using Application.Contracts.Repository;
using Infrastructure.Persistance.Connectrion.DBContext;
using Infrastructure.Persistance.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Text;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,IConfiguration configuration)
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
        public static IServiceCollection AddJWTAuthentication(this IServiceCollection services,IConfiguration configuration)
        {
            var jwtSettings = configuration.GetSection("JWT");
            var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]!);
            var cookieName = jwtSettings["CookieName"];
            return services;
        }
    }
}
