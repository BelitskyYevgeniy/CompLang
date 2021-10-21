using CompLang.DAL.Context;
using CompLang.DAL.Interfaces.Repository;
using CompLang.DAL.Repository;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace CompLang.DAL.Configuration
{
    public static class DependenciesConfiguration
    {
        public static IServiceCollection AddRepositories(this IServiceCollection services, string connectionString)
        {
            AddRepositoryContext(services, connectionString);
            services.AddScoped<IWordRepository, WordRepository>();
            services.AddScoped<ITextRepository, TextRepository>();
            services.AddScoped<IWordUsageRepository, WordUsageRepository>();
            return services;
        }
        public static IServiceCollection AddRepositoryContext(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<DictionaryDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }
    }
}
