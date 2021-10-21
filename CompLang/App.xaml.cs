using Microsoft.Extensions.DependencyInjection;
using System.Windows;
using CompLang.BLL.Configuration;
using CompLang.DAL.Configuration;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.IO;
using CompLang.Interfaces.Managers;
using CompLang.Managers;

namespace CompLang
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private ServiceProvider _serviceProvider;
        public static readonly CancellationToken Ct = new CancellationTokenSource().Token;
        public static IConfiguration Configuration { get; private set; }

        public App()
        {
            ServiceCollection services = new ServiceCollection();
            ConfigureServices(services);
            _serviceProvider = services.BuildServiceProvider();


        }

        private void ConfigureServices(IServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();

            services.AddSingleton<IPaginationManager, PaginationManager>();


            services.AddSingleton(Configuration);
            services.AddRepositories(Configuration.GetConnectionString("DefaultConnection"));
            services.AddMappingConfig();
            services.AddProviders();
            services.AddServices();

            services.AddSingleton<MainWindow>();
        }


        protected void OnStartup(object sender, StartupEventArgs e)
        {
            var mainWindow = _serviceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }
}
