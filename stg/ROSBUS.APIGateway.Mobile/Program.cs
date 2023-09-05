using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Administration;
using Microsoft.Extensions.DependencyInjection;


namespace AdministrationApi
{
  public class Program
{
        public static void Main(string[] args)
        {
            IWebHostBuilder builder = new WebHostBuilder();
            builder.ConfigureServices(s =>
            {
                s.AddSingleton(builder);
            });
            builder.UseKestrel()
                   .UseContentRoot(Directory.GetCurrentDirectory())
                   .UseStartup<Startup>()
                   .UseUrls("http://localhost:5000")
                    .ConfigureLogging((hostingContext, logging) =>
                   {
                       logging.AddConsole();
                   });

            var host = builder.Build();
            host.Run();
        }




        //public static void Main(string[] args)
        //{
        //    new WebHostBuilder()
        //       .UseKestrel()
        //       .UseUrls("http://localhost:5000")
        //       .UseContentRoot(Directory.GetCurrentDirectory())
        //       .ConfigureAppConfiguration((hostingContext, config) =>
        //       {
        //           config
        //               .SetBasePath(hostingContext.HostingEnvironment.ContentRootPath)
        //               .AddJsonFile("appsettings.json", true, true)
        //               .AddJsonFile($"appsettings.{hostingContext.HostingEnvironment.EnvironmentName}.json", true, true)
        //               .AddJsonFile("ocelot.json", true, true)
        //               .AddEnvironmentVariables();
        //       })
        //       .ConfigureServices(s =>
        //       {
        //           s.AddOcelot()
        //               .AddAdministration("/administration", "secret");
        //       })
        //       .ConfigureLogging((hostingContext, logging) =>
        //       {
        //           logging.AddConsole();
        //       })
        //       .UseIISIntegration()
        //       .Configure(app =>
        //       {
        //           app.UseOcelot().Wait();
        //       })
        //       .Build()
        //       .Run();
        //}
    }
}
