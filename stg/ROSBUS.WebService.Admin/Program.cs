using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace ROSBUS.WebService.Admin
{
    public class Program
    {
        public static void Main(string[] args)
        {
            BuildWebHost(args).Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {

            return WebHost.CreateDefaultBuilder(args)
                      .UseStartup<Startup>()
                      .ConfigureLogging((hostingContext, logging) =>
                      {
                          logging.AddConfiguration(hostingContext.Configuration.GetSection("Logging"));
                          logging.AddConsole(options => options.IncludeScopes = true);
                          logging.AddDebug();
                      })
                      .Build();

        }

    }
}
