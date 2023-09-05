using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Ocelot.Administration;
using Ocelot.DependencyInjection;
using Ocelot.Middleware;
using Ocelot.Provider.Polly;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdministrationApi
{
    public class Startup
    {

        IHostingEnvironment environment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            environment = env;

            var builder = new Microsoft.Extensions.Configuration.ConfigurationBuilder();
            builder.SetBasePath(env.ContentRootPath)
                           .AddJsonFile("appsettings.json", true, true)
                           .AddJsonFile($"appsettings.{env.EnvironmentName}.json", true, true)
                           .AddJsonFile("ocelot.json", true, true)
                           .AddEnvironmentVariables();


            ConfigurationR = builder.Build();

        }

        public IConfiguration Configuration { get; }
        public IConfigurationRoot ConfigurationR { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            var identityUrl = Configuration.GetValue<string>("IdentityUrl");
                       

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = identityUrl,
                    ValidAudience = identityUrl,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("veryVerySecretKey"))
                };
            });

            services.AddOcelot(ConfigurationR)
                    .AddPolly();
        }

        public async void Configure(IApplicationBuilder app, IHostingEnvironment env)
        {
            await app.UseOcelot();
        }

    }
}
