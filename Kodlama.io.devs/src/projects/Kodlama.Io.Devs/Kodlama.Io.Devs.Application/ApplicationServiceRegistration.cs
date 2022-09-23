using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Core.Application.Pipelines.Validation;
using FluentValidation;
using System.Reflection;
using Kodlama.Io.Devs.Application.Features.Languages.Rules;
using Kodlama.Io.Devs.Application.Features.OperationClaims.Rules;
using Kodlama.Io.Devs.Application.Features.UserOperationClaims.Rules;
using Kodlama.Io.Devs.Application.Features.Authorizations.Rules;
using Kodlama.Io.Devs.Application.Features.GitHubs.Rules;
using Kodlama.Io.Devs.Application.Features.Users.Rules;
using Kodlama.Io.Devs.Application.Features.LanguageTechnologies.Rules;
using Core.Application.Pipelines.Authorization;

namespace Application
{
    public static class ApplicationServiceRegistration
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {

            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            services.AddMediatR(Assembly.GetExecutingAssembly());

            services.AddScoped<LanguageBusinessRules>();
            services.AddScoped<LanguageBusinessRulesMessages>();

            services.AddScoped<OperationClaimBusinessRules>();
            services.AddScoped<OperationClaimBusinessRulesMessages>();

            services.AddScoped<UserOperationClaimBusinessRules>();
            services.AddScoped<UserOperationClaimBusinessRulesMessages>();

            services.AddScoped<AuthorizationBusinessRules>();
            services.AddScoped<AuthorizationBusinessRulesMessages>();

            services.AddScoped<UserBusinessRules>();
            services.AddScoped<UserBusinessRulesMessages>();

            services.AddScoped<LanguageTechnologyBusinessRules>();
            services.AddScoped<LanguageTechnologyBusinessRulesMessages>();

            services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(AuthorizationBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CachingBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(CacheRemovingBehavior<,>));
            //services.AddTransient(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehavior<,>));

            return services;
        }
    }
}