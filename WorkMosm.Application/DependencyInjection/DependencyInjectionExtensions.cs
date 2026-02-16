using Application.UseCases.LoginUser;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using WorkMosm.Application.UseCases.DeactivateUser;
using WorkMosm.Application.UseCases.GetUserProfile;
using WorkMosm.Application.UseCases.LoginUser;
using WorkMosm.Application.UseCases.RegisterUser;
using WorkMosm.Application.UseCases.UpdateUser;

namespace WorkMosm.Application.DependencyInjection
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

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

            return services;
        }
    }
}
