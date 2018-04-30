using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.IO;

namespace BestProductsApp.Function
{
    class Program
    {
        static void Main(string[] args)
        {
            IServiceCollection services = new ServiceCollection();
            ConfigureServices(services);

            var configuration = new JobHostConfiguration();
            configuration.JobActivator = new CustomJobActivator(services.BuildServiceProvider());
            Console.WriteLine("Job Started");
            configuration.UseTimers();
            var host = new JobHost(configuration);
            host.RunAndBlock();
        }

        private static void ConfigureServices(IServiceCollection services)
        {
            bool IsDevelopment = true;
#if DEBUG
            IsDevelopment = true;
#else
            IsDevelopment = false;
#endif
            // Optional: Setup your configuration:
            var Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile((IsDevelopment) ? "appsettings.Development.json" : "appsettings.json", optional: false, reloadOnChange: true)
                .Build();
            services.Configure<IConfigurationRoot>(Configuration);
            services.AddScoped<Functions, Functions>();
            
            // One more thing - tell azure where your azure connection strings are
            Environment.SetEnvironmentVariable("AzureWebJobsDashboard", Configuration.GetValue<string>("AzureStorage:ConnectionString"));
            Environment.SetEnvironmentVariable("AzureWebJobsStorage", Configuration.GetValue<string>("AzureStorage:ConnectionString"));
        }
    }
}
