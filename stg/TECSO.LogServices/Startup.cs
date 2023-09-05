using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
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
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Services;
using ROSBUS.infra.Data.Contexto;
using ROSBUS.infra.Data.Repositories;
using Swashbuckle.AspNetCore.Swagger;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.LogServices.MappingConfig;

namespace TECSO.LogServices
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
            services.AddDbContext<ErrorDBContext>(options =>
               options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));



            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();
                c.SwaggerDoc("v1", new Info { Title = "Logging API", Version = "v1" }); 

                var security = new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", new string[] { } }
                };


                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey",

                });

               
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

            services.AddCors();

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile<MappingProfile>();

            });

            //services.AddAutoMapper(cfg =>
            //{
            //    cfg.AddCollectionMappers();
            //    //cfg.SetGeneratePropertyMaps<GenerateEntityFrameworkPrimaryKeyPropertyMaps<AdminContext>>();
            //    // Configuration code                
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

            //Errors
            services.AddTransient<IErrorAppService, ErrorAppService>();
            services.AddTransient<IErrorService, ErrorService>();
            services.AddTransient<IErrorRepository, ErrorRepository>();

            //Logs
            services.AddTransient<ILogAppService, LogAppService>();
            services.AddTransient<ILogService, LogService>();
            services.AddTransient<ILogRepository, LogRepository>();

            services.AddTransient<IErrorDbContext, ErrorDBContext>();


            services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.IAuthService, AuthService>();

            services.AddTransient<ILogger, LogServicetxt>();
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
