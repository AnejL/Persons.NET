using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Persons.NET.Configuration;
using Persons.NET.Services;
using Persons.NET.Stores;
using Persons.NET.ViewModels;
using Persons.NET.Views;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace Persons.NET
{
    public partial class App : Application
    {
        public static IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            var builder = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("AppSettings.json", optional: false, reloadOnChange: true);

            this.Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            this.ConfigureServices(serviceCollection);

            ServiceProvider = serviceCollection.BuildServiceProvider();

            var window = ServiceProvider.GetRequiredService<MainWindow>();
            window.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<AppSettings>
                (Configuration.GetSection(nameof(AppSettings)));

            // inject Services
            services.AddSingleton<FileService>();
            services.AddSingleton<PersonsService>();
            services.AddSingleton<NavigationStore>();

            // inject View Models
            services.AddTransient<MainViewModel>();
            services.AddTransient<DashboardViewModel>();
            services.AddTransient<AddPersonViewModel>();
            services.AddTransient<EditPersonViewModel>();

            // inject Views and Window
            services.AddSingleton<MainWindow>();
            services.AddTransient<DashboardView>();
            services.AddTransient<PersonView>();
        }
    }
}
