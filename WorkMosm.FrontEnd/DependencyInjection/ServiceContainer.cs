using Microsoft.AspNetCore.Components.Authorization;
using WorkMosm.FrontEnd.Auth;
using WorkMosm.FrontEnd.Services;
using WorkMosm.FrontEnd.Services.Interfaces;

namespace WorkMosm.FrontEnd.DependencyInjection
{
    public static class ServiceContainer
    {
        public static IServiceCollection AddFrontEndServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddTransient<AuthenticationHandler>();
            services.AddHttpClient("WorkMosm.ServerAPI",
                client => client.BaseAddress = new Uri(configuration["ApiBaseUrl"] ?? "http://localhost:5000"))
                .AddHttpMessageHandler<AuthenticationHandler>();

            services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>()
                .CreateClient("WorkMosm.ServerAPI"));

            services.AddScoped<UsersService>();
            services.AddScoped<ApiClient>();
            services.AddScoped<IUsersService, UsersService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<IVehicleService, VehicleService>();

            services.AddAuthorizationCore();
            services.AddScoped<CustomAuthStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<CustomAuthStateProvider>());

            return services;
        }
    }
}
