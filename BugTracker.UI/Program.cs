using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BugTracker.Data.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace BugTracker.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();//.Run();

            using (var scope = host.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                //try
                //{
                var serviceProvider = services.GetRequiredService<IServiceProvider>();
                var configuration = services.GetRequiredService<IConfiguration>();
                DbInitializer.PopulateRoles(serviceProvider, configuration).GetAwaiter().GetResult();
                //}
                //catch (Exception e)
                //{
                //    var logger = services.GetRequiredService<ILogger<Program>>();
                //    logger.LogError(e, "An exception occurred while populating roles");
                //}
            }

            host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
