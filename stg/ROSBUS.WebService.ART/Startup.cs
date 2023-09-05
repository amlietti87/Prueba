using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Interface.ART;
using ROSBUS.Admin.AppService.service.ART;
using ROSBUS.Admin.Domain.Emailing;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Repositories.ART;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Interfaces.Services.ART;
using ROSBUS.Admin.Domain.Services;
using ROSBUS.Admin.Domain.Services.ART;
using ROSBUS.infra.Data.Contexto;
using ROSBUS.infra.Data.Repositories;
using ROSBUS.Infra.Data.Repositories.ART;
using ROSBUS.WebService.Shared;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.AppService;
using TECSO.FWK.Caching;
using TECSO.FWK.Caching.Configuration;
using TECSO.FWK.Caching.Memory;
using TECSO.FWK.Domain;
using TECSO.FWK.AppService.Interface;
using ROSBUS.Admin.AppService.service;
using ROSBUS.ART.AppService.Interface;
using ROSBUS.ART.AppService;
using ROSBUS.ART.Domain.Services;
using ROSBUS.ART.Domain.Interfaces.Services;
using ROSBUS.ART.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain;
using ROSBUS.Infra.Data.Repositories;

namespace ROSBUS.WebService.ART
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

        public void ConfigureServices(IServiceCollection services)
        {
            var entorno = Configuration.GetValue<string>("Entorno");

            services.AddDbContext<AdminContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<OperacionesRBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OperacionesRB")));
            services.AddDbContext<AdjuntosContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Adjuntos")));

            services.AddSwaggerGen(c =>
            {

                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("v1", new Info { Title = "Rosario Bus ART (" + entorno + ")", Version = "v1" });

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
                    options.AddPolicy("AllowAll",
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

            //services.AddCors();


            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile<MappingProfile>();

            });


            services.AddMvc(options => {
                options.Filters.Add(new VersionActionFilter(Configuration));
                options.Filters.Add(new HttpGlobalExceptionFilter(environment));
            }).AddRazorPagesOptions(options => 
            {
                options.Conventions.AuthorizeFolder("/Account/Manage");
                options.Conventions.AuthorizePage("/Account/Logout");
            }).AddJsonOptions(options => 
            {
                options.SerializerSettings.ContractResolver = new DefaultContractResolver();
            }); 

            services.AddSingleton<Microsoft.AspNetCore.Http.IHttpContextAccessor, Microsoft.AspNetCore.Http.HttpContextAccessor>();

            //Data Base
            services.AddScoped<IAdminDbContext>(provider => provider.GetService<AdminContext>());
            services.AddScoped<IAdminDBResilientTransaction, AdminDBResilientTransaction>();
            //Data Base
            services.AddScoped<IOperacionesRBDbContext>(provider => provider.GetService<OperacionesRBContext>());
            services.AddScoped<IOperacionesRBDBResilientTransaction, OperacionesRBContext.OperacionesRBDBResilientTransaction>();
            //Data Base
            services.AddScoped<IAdjuntosDbContext>(provider => provider.GetService<AdjuntosContext>());
            services.AddScoped<IAdjuntosDBResilientTransaction, AdjuntosContext.AdjuntosDBResilientTransaction>();

            //email
            services.AddTransient<TECSO.FWK.Domain.Mail.Smtp.ISmtpEmailSenderConfiguration, TECSO.FWK.Domain.Mail.Smtp.SmtpEmailSenderConfiguration>();

            services.AddTransient<IEmailTemplateProvider, EmailTemplateProvider>();
            services.AddTransient<TECSO.FWK.Domain.Mail.IEmailSender, TECSO.FWK.Domain.Mail.Smtp.SmtpEmailSender>();


            //URL : TODO Ver como implementar
            // services.AddTransient<ROSBUS.Admin.Domain.Url.IAppUrlService, AngularAppUrlService>();
            // services.AddTransient<ROSBUS.Admin.Domain.Url.IWebUrlService, WebUrlService>();

            //Contingencias
            services.AddTransient<IContingenciasAppService, ContingenciasAppService>();
            services.AddTransient<IContingenciasService, ContingenciasService>();
            services.AddTransient<IContingenciasRepository, ContingenciasRepository>();

            //Denuncias
            services.AddTransient<IDenunciasAppService, DenunciasAppService>();
            services.AddTransient<IDenunciasService, DenunciasService>();
            services.AddTransient<IDenunciasRepository, DenunciasRepository>();

            //Reporter http
            services.AddTransient<IReporterHttpAppService, ReporterHttpAppService>();

            //
            services.AddTransient<ROSBUS.Admin.Domain.Url.IWebUrlService, WebUrlService>();
            services.AddTransient<IUserEmailer, ROSBUS.Admin.Domain.Interfaces.Mail.UserEmailer>();
            services.AddTransient<IDefaultEmailer, ROSBUS.Admin.Domain.Interfaces.Mail.DefaultEmailer>();

            //SysParameteres and SysDataTypes
            services.AddTransient<ISysParametersAppService, SysParametersAppService>();
            services.AddTransient<ISysParametersService, SysParametersService>();
            services.AddTransient<ISysParametersRepository, SysParametersRepository>();
            services.AddTransient<ISysDataTypesAppService, SysDataTypesAppService>();
            services.AddTransient<ISysDataTypesService, SysDataTypesService>();
            services.AddTransient<ISysDataTypesRepository, SysDataTypesRepository>();


            //Estados
            services.AddTransient<Admin.AppService.Interface.IEstadosAppService, Admin.AppService.EstadosAppService>();
            services.AddTransient<Admin.Domain.Interfaces.Services.IEstadosService, Admin.Domain.Services.EstadosService>();
            services.AddTransient<Admin.Domain.Interfaces.Repositories.IEstadosRepository, infra.Data.Repositories.EstadosRepository>();

            //Empresa
            services.AddTransient<IEmpresaAppService, EmpresaAppService>();
            services.AddTransient<IEmpresaService, EmpresaService>();
            services.AddTransient<IEmpresaRepository, EmpresaRepository>();


            //DenunciasNotificaciones
            services.AddTransient<IDenunciasNotificacionesAppService, DenunciasNotificacionesAppService>();
            services.AddTransient<IDenunciasNotificacionesService, DenunciasNotificacionesService>();
            services.AddTransient<IDenunciasNotificacionesRepository, DenunciasNotificacionesRepository>();

            //Estados
            services.AddTransient<Admin.AppService.Interface.ART.IEstadosAppService, Admin.AppService.service.ART.EstadosAppService>();
            services.AddTransient<Admin.Domain.Interfaces.Services.ART.IEstadosService, Admin.Domain.Services.ART.EstadosService>();
            services.AddTransient<Admin.Domain.Interfaces.Repositories.ART.IEstadosRepository, Infra.Data.Repositories.ART.EstadosRepository>();

            //CroCroquis
            services.AddTransient<Operaciones.AppService.Interface.IEmpleadosAppService, Operaciones.AppService.EmpleadosAppService>();
            services.AddTransient<Operaciones.Domain.Interfaces.Repositories.IEmpleadosRepository, infra.Data.Operaciones.Repositories.EmpleadosRepository>();
            services.AddTransient<IEmpleadosService, Operaciones.Domain.Services.EmpleadosService>();

            //Localidades
            services.AddTransient<ILocalidadesAppService, LocalidadesAppService>();
            services.AddTransient<ILocalidadesService, LocalidadesService>();
            services.AddTransient<ILocalidadesRepository, LocalidadesRepository>();

            //MotivosAudiencias
            services.AddTransient<IMotivosAudienciasAppService, MotivosAudienciasAppService>();
            services.AddTransient<IMotivosAudienciasService, MotivosAudienciasService>();
            services.AddTransient<IMotivosAudienciasRepository, MotivosAudienciasRepository>();

            //MotivosNotificaciones
            services.AddTransient<IMotivosNotificacionesAppService, MotivosNotificacionesAppService>();
            services.AddTransient<IMotivosNotificacionesService, MotivosNotificacionesService>();
            services.AddTransient<IMotivosNotificacionesRepository, MotivosNotificacionesRepository>();

            //Patologias
            services.AddTransient<IPatologiasAppService, PatologiasAppService>();
            services.AddTransient<IPatologiasService, PatologiasService>();
            services.AddTransient<IPatologiasRepository, PatologiasRepository>();

            //PrestadoresMedicos
            services.AddTransient<IPrestadoresMedicosAppService, PrestadoresMedicosAppService>();
            services.AddTransient<IPrestadoresMedicosService, PrestadoresMedicosService>();
            services.AddTransient<IPrestadoresMedicosRepository, PrestadoresMedicosRepository>();


            //Tratamientos
            services.AddTransient<ITratamientosAppService, TratamientosAppService>();
            services.AddTransient<ITratamientosService, TratamientosService>();
            services.AddTransient<ITratamientosRepository, TratamientosRepository>();


            //TiposAcuerdo
            services.AddTransient<ITiposAcuerdoAppService, TiposAcuerdoAppService>();
            services.AddTransient<ITiposAcuerdoService, TiposAcuerdoService>();
            services.AddTransient<ITiposAcuerdoRepository, TiposAcuerdoRepository>();

            //TiposReclamo
            services.AddTransient<ITiposReclamoAppService, TiposReclamoAppService>();
            services.AddTransient<ITiposReclamoService, TiposReclamoService>();
            services.AddTransient<ITiposReclamoRepository, TiposReclamoRepository>();

            //CausasReclamo
            services.AddTransient<ICausasReclamoAppService, CausasReclamoAppService>();
            services.AddTransient<ICausasReclamoService, CausasReclamoService>();
            services.AddTransient<ICausasReclamoRepository, CausasReclamoRepository>();


            //Adjuntos
            services.AddTransient<IAdjuntosAppService, AdjuntosAppService>();
            services.AddTransient<IAdjuntosService, AdjuntosService>();
            services.AddTransient<IAdjuntosRepository, AdjuntosRepository>();

            //Adjuntos
            services.AddTransient<IReclamosAppService, ReclamosAppService>();
            services.AddTransient<IReclamosService, ReclamosService>();
            services.AddTransient<IReclamosRepository, ReclamosRepository>();

            //Adjuntos
            services.AddTransient<ISiniestrosAppService, SiniestrosAppService>();
            services.AddTransient<ISiniestrosService, SiniestrosService>();
            services.AddTransient<ISiniestrosRepository, SiniestrosRepository>();

            //RubrosSalariales
            services.AddTransient<IRubrosSalarialesAppService, RubrosSalarialesAppService>();
            services.AddTransient<IRubrosSalarialesService, RubrosSalarialesService>();
            services.AddTransient<IRubrosSalarialesRepository, RubrosSalarialesRepository>();

            services.AddTransient<IErrorDbContext, ErrorDBContext>();

            services.AddScoped<TECSO.FWK.Domain.Interfaces.Services.IPermissionProvider, PermissionProvider>();
            
            //PermissionAppService
            services.AddTransient<IPermissionAppService, PermissionAppService>();
            services.AddTransient<IPermissionRepository, PermissionsRepository>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.IAuthService, AuthService>();


            services.AddSingleton<ICachingConfiguration, CachingConfiguration>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();



            ///////////LOG///////////////
            if (environment.IsDevelopment())
            {
                //services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.ILogger, TECSO.FWK.AppService.LogServiceDebug>();
                services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.ILogger, LogServicetxt>();
            }
            else
            {
                services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.ILogger, LogServicetxt>();
                //services.AddTransient<ILogger, LogServicetxt>();
            }

        }

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
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "API ART V1");
                });
            }
           

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
