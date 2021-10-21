using CompLang.BLL.Interfaces.Providers;
using CompLang.BLL.Interfaces.Services;
using CompLang.BLL.Mapping;
using CompLang.BLL.Providers;
using CompLang.BLL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CompLang.BLL.Configuration
{
    public static class DependenciesConfiguration
    {
        public static IServiceCollection AddMappingConfig(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(
                 c => c.AddProfile<MappingConfiguration>(),
                 typeof(MappingConfiguration));

            return serviceCollection;
        }

        public static IServiceCollection AddServices(this IServiceCollection services)
        {
            services = services.AddTransient<IParseTextService, ParseTextService>();
            services = services.AddTransient<IWordService, WordService>();
            services = services.AddScoped<ITextService, TextService>();
            return services;
        }

        public static IServiceCollection AddProviders(this IServiceCollection services)
        {
            services = services.AddScoped<ITextProvider, TextProvider>();
            services = services.AddScoped<IWordProvider, WordProvider>();
            services = services.AddScoped<IWordUsageProvider, WordUsageProvider>();
            return services;
        }
    }
}
