using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using DataBase.Data;
using DataBase.Seeding;

namespace DrugStoreApplication
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //inilaize Values 
            var Host = CreateHostBuilder(args).Build();
            using var scope = Host.Services.CreateScope();
            var services = scope.ServiceProvider;
            var loggerFactory = services.GetRequiredService<ILoggerProvider>();
            var logger = loggerFactory.CreateLogger("app");
            try
            {

                //paramerter using in the function
                var roleManger = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManger = services.GetRequiredService<UserManager<PharmcyInfo>>(); 
                //Call the fun
                await Ruless.AddRuleAsync(roleManger);
                await Ruless.AddAdmin(userManger);

                logger.LogInformation("Data Seed");
            }
            catch (Exception ex)
            {
                logger.LogWarning(ex, "An error Accured while Seeding the data");
            }

            Host.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
