using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.service.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Repositories.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Interfaces.Services.AppInspectores;
using ROSBUS.Admin.Domain.Services;
using ROSBUS.Admin.Domain.Services.AppInspectores;
using ROSBUS.infra.Data.Contexto;
using ROSBUS.infra.Data.Repositories;
using ROSBUS.Infra.Data.Repositories.AppInspectores;
using ROSBUS.WebService.Geo.MappingConfig;
using ROSBUS.WebService.Shared;
using Swashbuckle.AspNetCore.Swagger;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Caching;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.Geo
{
    public class Startup
    {
        IHostingEnvironment environment;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            environment = env;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {


            var entorno = Configuration.GetValue<string>("Entorno");

            services.AddDbContext<AdminContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



            services.AddSwaggerGen(c =>
            {

                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("v1", new Info { Title = "Rosario Bus Geolocalization (" + entorno + ")", Version = "v1" });

                var security = new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", new string[] { } }
                };

                // Define the BearerAuth scheme that's in use
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",

                });

                // Assign scope requirements to operations based on AuthorizeAttribute
                c.AddSecurityRequirement(security);


            });

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

            services.AddCors(

                options =>
                {
                    options.AddPolicy("AllowAllOrigins",
                    builder =>
                    {
                        builder
                        //.WithOrigins(_appConfiguration["App:CorsOrigins"].Split(",", StringSplitOptions.RemoveEmptyEntries).Select(o => o.RemovePostFix("/")).ToArray())
                        .AllowAnyOrigin() //TODO: Will be replaced by above when Microsoft releases microsoft.aspnetcore.cors 2.0 - https://github.com/aspnet/CORS/pull/94
                        .AllowAnyHeader()
                        .AllowAnyMethod();
                    });
                }
                );
            services.AddCors();
            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile<MappingProfile>();

            });



            services.AddMvc(
                 options =>
                 {
                     options.Filters.Add(new HttpGlobalExceptionFilter(environment));
                 })
               .AddRazorPagesOptions(options =>
               {

                   options.Conventions.AuthorizeFolder("/Account/Manage");
                   options.Conventions.AuthorizePage("/Account/Logout");
               })
               .AddJsonOptions(options =>

                    options.SerializerSettings.ContractResolver = new DefaultContractResolver()
                    ); ;

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            //Data Base
            services.AddScoped<IAdminDbContext>(provider => provider.GetService<AdminContext>());
            services.AddScoped<IAdminDBResilientTransaction, AdminDBResilientTransaction>();


            services.AddTransient<ROSBUS.Admin.Domain.Url.IAppUrlService, AngularAppUrlService>();
            services.AddTransient<ROSBUS.Admin.Domain.Url.IWebUrlService, WebUrlService>();


            //Agregar servicios

            //Geolocalizacion
            services.AddTransient<IGeoAppService, GeoAppService>();
            services.AddTransient<IGeoService, GeoService>();
            services.AddTransient<IGeoRepository, GeoRepository>();

            //PermissionAppService
            services.AddTransient<IPermissionAppService, PermissionAppService>();
            services.AddTransient<IPermissionRepository, PermissionsRepository>();
            services.AddTransient<IPermissionService, PermissionService>();


            services.AddScoped<TECSO.FWK.Domain.Interfaces.Services.IPermissionProvider, PermissionProvider>();

            services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.IAuthService, AuthService>();

            services.AddSingleton<TECSO.FWK.Caching.Configuration.ICachingConfiguration, TECSO.FWK.Caching.Configuration.CachingConfiguration>();
            services.AddSingleton<ICacheManager, TECSO.FWK.Caching.Memory.MemoryCacheManager>();

            ///////////LOG///////////////
            if (environment.IsDevelopment())
            {
                services.AddTransient<ILogger, TECSO.FWK.AppService.LogServiceDebug>();
                //services.AddTransient<ILogger, LogServicetxt>();
            }
            else
            {
                //services.AddTransient<ILogger, ErrorHttpAppService>();
                services.AddTransient<ILogger, LogServicetxt>();
            }

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IServiceProvider svp)
        {
            if (env.IsDevelopment())
            {
                app.UseBrowserLink();
                app.UseDeveloperExceptionPage();
                app.UseDatabaseErrorPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
            }


            app.UseStaticFiles();

            app.UseAuthentication();

            app.UseCors(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod());

            app.UseMvc(routes =>
            {

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });

            var pathBase = Configuration["APPL_PATH"];
            if (pathBase == "/")
                pathBase = "";
            if (!string.IsNullOrEmpty(pathBase))
            {
                app.UsePathBase(pathBase);
            }

            var enableSwagger = Configuration.GetValue<bool>("EnableSwagger");

            if (enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "API Error V1");
                });

            }

            ServiceProviderResolver.ServiceProvider = app.ApplicationServices;
        }
    }
}
