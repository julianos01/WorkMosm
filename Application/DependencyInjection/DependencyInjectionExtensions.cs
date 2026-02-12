using Application.UseCases.DeactivateUser;
using Application.UseCases.GetUserProfile;
using Application.UseCases.LoginUser;
using Application.UseCases.RegisterUser;
using Application.UseCases.UpdateUser;
using Application.UseCases.UpdateUserProfile;
using Microsoft.Extensions.DependencyInjection;

namespace Application.DependencyInjection
{
    public static class DependencyInjectionExtensions
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
            services.AddScoped<ILoginUserUseCase, LoginUserUseCase>();  
            services.AddScoped<IGetUserProfileUseCase, GetUserProfileUseCase>();
            services.AddScoped<IUpdateUserProfile, UpdateUserProfileUseCase>();
            services.AddScoped<IDeactivateUserUseCase, DeactivateUserUseCase>();

            return services;
        }
    }
}
