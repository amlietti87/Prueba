using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.EquivalencyExpression;
using Hangfire;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.AppService.service;
using ROSBUS.Admin.AppService.service.http;
using ROSBUS.Admin.Domain.ParametersHelper;
using ROSBUS.WebService.ART;
using ROSBUS.WebService.Schedules.Tasks;
using Swashbuckle.AspNetCore.Swagger;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Caching;
using TECSO.FWK.Caching.Configuration;
using TECSO.FWK.Caching.Memory;
using TECSO.FWK.Domain;


namespace ROSBUS.WebService.Schedules
{
    public class Startup
    {
        public IConfiguration Configuration { get; }

        private readonly IHostingEnvironment environment;

        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            environment = env;
        }


        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var entorno = Configuration.GetValue<string>("Entorno");


            services.AddSwaggerGen(c =>
            {

                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("v1", new Info { Title = "Rosario Bus Schedule (" + entorno + ")", Version = "v1" });

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

            services.AddHangfire(x => x.UseSqlServerStorage(Configuration.GetConnectionString("DefaultConnection")));
            services.AddHangfireServer();


            
            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();
            services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.IAuthService, AuthService>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();
            services.AddSingleton<ICachingConfiguration, CachingConfiguration>();


            services.AddTransient<IAuthAppService, AuthHttpAppService>();

            services.AddTransient<IDenunciasAppService, DenunciasHttpAppService>();
            services.AddTransient<IFdDocumentosProcesadosAppService, ImportadorDocumentosFDHttpAppService>();
            
            services.AddTransient<ISysParametersAppService, SysParametersHttpAppService>();
            services.AddTransient<IParametersHelper, SysParametersHttpAppService>();

            services.AddTransient<ImportadorDenuniasTask>();
            services.AddTransient<ImportadorDocumentosTask>();
            services.AddTransient<ImportadorDocumentosFDHttpAppService>();

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime applicationLifetime)
        {
            applicationLifetime.ApplicationStopping.Register(OnShutdown);

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
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "API Schedules V1");
                });
            }
            //GlobalJobFilters.Filters.Add(new AutomaticRetryAttribute {  Attempts = 0 });
            

            ServiceProviderResolver.ServiceProvider = app.ApplicationServices;

            var defaultSlidingExpireTime = TimeSpan.FromHours(24);
            ServiceProviderResolver.ServiceProvider.GetService<ICachingConfiguration>().ConfigureAll(cache =>
            {
                cache.DefaultSlidingExpireTime = defaultSlidingExpireTime;
            });



            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                Authorization = Enumerable.Repeat(new MyAuthorizationFilter(), 1)
            });

            ServiceProviderResolver.ServiceProvider.GetService<ImportadorDenuniasTask>().Init();
            ServiceProviderResolver.ServiceProvider.GetService<ImportadorDocumentosTask>().Init();


        }

        private void OnShutdown()
        {
            
        }
    }

    public class MyAuthorizationFilter : IDashboardAuthorizationFilter
    {
        public bool Authorize(DashboardContext context)
        {
            var httpContext = context.GetHttpContext();

            // Allow all authenticated users to see the Dashboard (potentially dangerous).
            //return httpContext.User.Identity.IsAuthenticated;
            return true;
        }
    }
}
