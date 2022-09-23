using Kodlama.Io.Devs.Application.Services.Repositories;
using Kodlama.Io.Devs.Persistence.Contexts;
using Kodlama.Io.Devs.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Security.Claims;

namespace Kodlama.Io.Devs.Persistence
{
    public static class PersistenceServiceRegistration
    {
        public static IServiceCollection AddPersistenceServices(this IServiceCollection services,
                                                                IConfiguration configuration)
        {
            services.AddDbContext<BaseDbContext>(options =>
                                                     options.UseNpgsql(
                                                         configuration.GetConnectionString("PostgreSql")));

            services.AddScoped<ILanguageRepository, LanguageRepository>();
            services.AddScoped<IOperationClaimRepository, OperationClaimRepository>();
            services.AddScoped<IUserOperationClaimRepository, UserOperationClaimRepository>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<ILanguageTechnologyRepository, LanguageTechnologyRepository>();
            services.AddScoped<IDeveloperRepository, DeveloperRepository>();

            return services;
        }
    }
}
