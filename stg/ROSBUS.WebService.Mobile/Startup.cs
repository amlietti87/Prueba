using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.service;
using ROSBUS.Admin.Domain.Emailing;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Services;
using ROSBUS.infra.Data.Contexto;
using ROSBUS.infra.Data.Repositories;
using ROSBUS.WebService.Shared;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Caching.Configuration;
using TECSO.FWK.Domain;
using TECSO.FWK.Caching;
using TECSO.FWK.Caching.Memory;
using ROSBUS.Operaciones.AppService.Interface;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using ROSBUS.Operaciones.AppService;
using ROSBUS.Operaciones.Domain.Services;
using ROSBUS.infra.Data.Operaciones.Repositories;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.Mobile
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

            services.AddSwaggerGen(c =>
            {

                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("v1", new Info { Title = "Rosario Bus Mobile (" + entorno + ")", Version = "v1" });

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

            //AutoMapper.Mapper.Initialize(cfg =>
            //{
            //    cfg.AddCollectionMappers();
            //    cfg.AddProfile<MappingProfile>();

            //});



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

      
            //email
            services.AddTransient<TECSO.FWK.Domain.Mail.Smtp.ISmtpEmailSenderConfiguration, TECSO.FWK.Domain.Mail.Smtp.SmtpEmailSenderConfiguration>();

            services.AddTransient<IEmailTemplateProvider, EmailTemplateProvider>();
            services.AddTransient<TECSO.FWK.Domain.Mail.IEmailSender, TECSO.FWK.Domain.Mail.Smtp.SmtpEmailSender>();



            services.AddSingleton<ICachingConfiguration, CachingConfiguration>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();


            services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.IAuthService, AuthService>();

            services.AddTransient<IBanderaAppService, BanderaHttpAppService>();
            services.AddTransient<IAuthAppService, AuthHttpAppService>();
            services.AddTransient<ILineaAppService, LineaHttpAppService>();
            services.AddTransient<IPlaSentidoBanderaAppService, SentidoBanderaHttpAppService>();

            
            ///////////LOG///////////////
            if (environment.IsDevelopment())
            {
                services.AddTransient<ILogger, TECSO.FWK.AppService.LogServiceDebug>();
                //services.AddTransient<ILogger, LogServicetxt>();
            }
            else
            {
                services.AddTransient<ILogger, ErrorHttpAppService>();
                //services.AddTransient<ILogger, LogServicetxt>();
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

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "API Mobile V1");
            });

            ServiceProviderResolver.ServiceProvider = app.ApplicationServices;

            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            ServiceProviderResolver.ServiceProvider.GetService<ICachingConfiguration>().ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = defaultSlidingExpireTime;
            });



        }
    }

    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        public void Apply(Operation operation, OperationFilterContext context)
        {
            var actionAttrs = context.ApiDescription.ActionAttributes();
            if (actionAttrs.OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var controllerAttrs = context.ApiDescription.ControllerAttributes();
            var actionAuthorizeAttrs = actionAttrs.OfType<ApiAuthorizeAttribute>();

            if (!actionAuthorizeAttrs.Any() && controllerAttrs.OfType<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var controllerAuthorizeAttrs = controllerAttrs.OfType<AuthorizeAttribute>();
            if (controllerAuthorizeAttrs.Any() || actionAuthorizeAttrs.Any())
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });

                //var permissions = controllerAuthorizeAttrs.Union(actionAuthorizeAttrs)
                //    .SelectMany(p => p.Permissions)
                //    .Distinct();

                //if (permissions.Any())
                //{
                //    operation.Responses.Add("403", new Response { Description = "Forbidden" });
                //}

                //operation.Security = new List<IDictionary<string, IEnumerable<string>>>
                //{
                //    new Dictionary<string, IEnumerable<string>>
                //    {
                //        { "bearerAuth", permissions }
                //    }
                //};
            }
        }
    }
}
