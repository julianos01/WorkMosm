using Application.Interfaces.Security;
using Domain.Ports;
using Infrastructure.Configurations;
using Infrastructure.Persistence;
using Infrastructure.Security;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {

        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            var connectionString = configuration.GetConnectionString("Postgres");
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new Exception("ConnectionString Missing");
            }
            services.AddDbContext<AppDbContext>(options =>
            options.UseNpgsql(connectionString));

            services.AddScoped<IUserRepository, UserRepository>();

            services.Configure<JwtSettings>(configuration.GetSection("Jwt"));

            services.AddScoped<ITokenGenerator, JwtTokenGenerator>();
            services.AddScoped<IPasswordHasher, BCryptPasswordHasher>();

            return services;
        }
    }
}
