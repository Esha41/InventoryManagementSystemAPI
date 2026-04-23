using Microsoft.Extensions.DependencyInjection;
using Ettad.CrossCutting.Comman.Time;
using Ettad.Services.Helpers;
//using Ettad.Services.Implementation;
using Ettad.Services.Interfaces;
using Ettad.Services.Mapper;
using Ettad.User.Services.Interfaces;
using Ettad.CrossCutting.Comman.Time;
using Ettad.User.Services.DTO;
using Ettad.Application.Common.Interfaces;
using Ettad.User.Services.Services;

namespace Ettad.User.Services
{
    public static class ModuleServicesDependences
    {
        public static IServiceCollection AddReposetoriesServices(this IServiceCollection service)
        {
            service.AddAutoMapper(typeof(MappingProfile));
           // service.AddTransient<IAuthenticationService, AuthenticationService>();
            service.AddTransient<IHelpureService, HelpureService>();
            service.AddScoped<IAccountServices, AccountServices>();
            service.AddScoped<ISettingsProvider, SettingsProvider>();
            service.AddScoped<ILdapAuthenticator, LdapAuthenticator>();
            service.AddScoped<IDateTimeProvider, SystemDateTimeProvider>();
            service.AddScoped<IJwtServices, JwtServices>();
            service.AddScoped<IRoleService, RoleService>();
            service.AddScoped<IUserService, UserService>();
            service.AddTransient<IPermissionService, PermissionService>();
            service.AddSingleton<ICaptchaService, CaptchaService>();
            service.AddScoped<ITokenBlacklistService, TokenBlacklistService>();
            service.AddScoped<IAdminAnalyticsService, AdminAnalyticsService>();
            service.AddScoped<IUserDelegationService, UserDelegationService>();
            service.AddScoped<IOnboardingService, OnboardingService>();

            service.AddScoped<ICurrentUserService, CurrentUserService>();




            return service;
        }
    }
}
