using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.EquivalencyExpression;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Serialization;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.service;
using ROSBUS.Admin.AppService.service.FirmaDigital;
using ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory;
using ROSBUS.Admin.AppService.service.http;
using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Emailing;
using ROSBUS.Admin.Domain.Interfaces;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Services;
using ROSBUS.infra.Data.Contexto;
using ROSBUS.infra.Data.Operaciones.Repositories;
using ROSBUS.infra.Data.Repositories;
using ROSBUS.Infra.Data.Repositories;
using ROSBUS.Operaciones.AppService;
using ROSBUS.Operaciones.AppService.Interface;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using ROSBUS.Operaciones.Domain.Services;
using ROSBUS.WebService.FirmaDigital.MappingConfig;
using ROSBUS.WebService.Shared;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.AspNetCore.SwaggerGen;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.AppService;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Caching;
using TECSO.FWK.Caching.Configuration;
using TECSO.FWK.Caching.Memory;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.WebService.FirmaDigital
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

            services.AddDbContext<AdminContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<OperacionesRBContext>(options => options.UseSqlServer(Configuration.GetConnectionString("OperacionesRB")));
            services.AddDbContext<AdjuntosContext>(options => options.UseSqlServer(Configuration.GetConnectionString("Adjuntos")));

            services.AddSwaggerGen(c =>
            {
                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("v1", new Info { Title = "Rosario Bus FirmaDigital (" + entorno + ")", Version = "v1" });

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

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile<MappingProfile>();

            });


            services.AddMvc(options => {
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
            services.AddTransient<ISignalRHttpService, SignalRHttpService>();

            
            //FD
            services.AddTransient<IFdDocumentosProcesadosAppService, FdDocumentosProcesadosAppService>();
            services.AddTransient<IFdDocumentosProcesadosService, FdDocumentosProcesadosService>();
            services.AddTransient<IFdDocumentosProcesadosRepository, FdDocumentosProcesadosRepository>();

            services.AddTransient<IFdDocumentosErrorAppService, FdDocumentosErrorAppService>();
            services.AddTransient<IFdDocumentosErrorService, FdDocumentosErrorService>();
            services.AddTransient<IFdDocumentosErrorRepository, FdDocumentosErrorRepository>();

            services.AddTransient<IFdAccionesAppService, FdAccionesAppService>();
            services.AddTransient<IFdAccionesService, FdAccionesService>();
            services.AddTransient<IFdAccionesRepository, FdAccionesRepository>();

            services.AddTransient<IFdAccionesPermitidasAppService, FdAccionesPermitidasAppService>();
            services.AddTransient<IFdAccionesPermitidasService, FdAccionesPermitidasService>();
            services.AddTransient<IFdAccionesPermitidasRepository, FdAccionesPermitidasRepository>();


            services.AddTransient<IFdDocumentosProcesadosHistoricoAppService, FdDocumentosProcesadosHistoricoAppService>();
            services.AddTransient<IFdDocumentosProcesadosHistoricoService, FdDocumentosProcesadosHistoricoService>();
            services.AddTransient<IFdDocumentosProcesadosHistoricoRepository, FdDocumentosProcesadosHistoricoRepository>();

            services.AddTransient<IFdEstadosAppService, FdEstadosAppService>();
            services.AddTransient<IFdEstadosService, FdEstadosService>();
            services.AddTransient<IFdEstadosRepository, FdEstadosRepository>();

            services.AddTransient<IFdTiposDocumentosAppService, FdTiposDocumentosAppService>();
            services.AddTransient<IFdTiposDocumentosService, FdTiposDocumentosService>();
            services.AddTransient<IFdTiposDocumentosRepository, FdTiposDocumentosRepository>();


            services.AddTransient<IFdFirmadorAppService, FdFirmadorAppService>();
            services.AddTransient<IFdFirmadorService, FdFirmadorService>();
            services.AddTransient<IFdFirmadorRepository, FdFirmadorRepository>();

            services.AddTransient<IFdFirmadorLogAppService, FdFirmadorLogAppService>();
            services.AddTransient<IFdFirmadorLogService, FdFirmadorLogService>();
            services.AddTransient<IFdFirmadorLogRepository, FdFirmadorLogRepository>();


            services.AddTransient<IEmpleadosAppService, EmpleadosAppService>();
            services.AddTransient<IEmpleadosService, EmpleadosService>();
            services.AddTransient<IEmpleadosRepository, EmpleadosRepository>();

            services.AddTransient<IEmpresaAppService, EmpresaAppService>();
            services.AddTransient<IEmpresaService, EmpresaService>();
            services.AddTransient<IEmpresaRepository, EmpresaRepository>();

            services.AddTransient<IFdCertificadosAppService, FdCertificadosAppService>();
            services.AddTransient<IFdCertificadosService, FdCertificadosService>();
            services.AddTransient<IFdCertificadosRepository, FdCertificadosRepository>();

            //Notification
            services.AddTransient<INotificationAppService, NotificationAppService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationRepository, NotificationRepository>();

            //Adjuntos
            services.AddTransient<IAdjuntosAppService, AdjuntosAppService>();
            services.AddTransient<IAdjuntosService, AdjuntosService>();
            services.AddTransient<IAdjuntosRepository, AdjuntosRepository>();


            


            ////Ejemplo de inyeccion de HttpAppService de una Entidad (Denuncia)
            ////services.AddTransient<IDenunciaHttpAppService, DenunciaHttpAppService>();

            //SysParameteres and SysDataTypes
            services.AddTransient<ISysParametersAppService, SysParametersAppService>();
            services.AddTransient<ISysParametersService, SysParametersService>();
            services.AddTransient<ISysParametersRepository, SysParametersRepository>();
            services.AddTransient<ISysDataTypesAppService, SysDataTypesAppService>();
            services.AddTransient<ISysDataTypesService, SysDataTypesService>();
            services.AddTransient<ISysDataTypesRepository, SysDataTypesRepository>();

            services.AddTransient<IErrorDbContext, ErrorDBContext>();

            services.AddScoped<TECSO.FWK.Domain.Interfaces.Services.IPermissionProvider, PermissionProvider>();

            //PermissionAppService
            services.AddTransient<IPermissionAppService, PermissionAppService>();
            services.AddTransient<IPermissionRepository, PermissionsRepository>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.IAuthService, AuthService>();
            services.AddTransient<IAuthAppService, AuthHttpAppService>();
            

            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<IUserAppService, UserAppService>();
            services.AddTransient<ILdapRepository, LdapRepository>();

            services.AddSingleton<ICachingConfiguration, CachingConfiguration>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            //
            services.AddTransient<ROSBUS.Admin.Domain.Url.IWebUrlService, WebUrlService>();
            services.AddTransient<IUserEmailer, ROSBUS.Admin.Domain.Interfaces.Mail.UserEmailer>();
            services.AddTransient<IDefaultEmailer, ROSBUS.Admin.Domain.Interfaces.Mail.DefaultEmailer>();

            services.AddTransient<IPermissionProvider, PermissionProvider>();



            services.AddTransient<IAccion_RespuestaMinisterio, Accion_RespuestaMinisterio>();
            services.AddTransient<IAccion_FirmarEmpleado, Accion_FirmarEmpleado>();
            services.AddTransient<IAccion_FirmarEmpleador, Accion_FirmarEmpleador>();
            services.AddTransient<IAccion_AprobarDocumento, Accion_AprobarDocumento>();
            services.AddTransient<IAccion_VerDetalleDocumento, Accion_VerDetalleDocumento>();
            services.AddTransient<IAccion_RechazarDocumento, Accion_RechazarDocumento>();
            services.AddTransient<IAccion_AbrirArchivo, Accion_AbrirArchivo>();
            services.AddTransient<IAccion_DescargarArchivo, Accion_DescargarArchivo>();
            services.AddTransient<IAccion_RevisarArchivo, Accion_RevisarArchivo>();
            services.AddTransient<IAccion_EnviarCorreo, Accion_EnviarCorreo>();
            services.AddTransient<IAccionesFactory, AccionesFactory>();
            services.AddTransient<IFirmadorHelper, FirmadorHelper>();
            


            //services.AddScoped<IAccionesFactory, AccionesFactory>(
            //    serviceProvider => new AccionesFactory(serviceProvider.GetService<IServiceProvider>())
            //);



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

            //app.UseStatusCodePages(async context =>
            //{
            //    if (context.HttpContext.Response.StatusCode == 401)
            //    {
            //        await context.HttpContext.Response.WriteAsync("{ error = 'El Usuario no esta autorizado.' }");
            //    }
            //});

            app.UseStaticFiles();

            app.UseFileServer(new FileServerOptions
            {
                FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "JarFiles")),
                RequestPath = "/StaticFiles",
                EnableDirectoryBrowsing = true
            });

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
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "API FirmaDigital V1");
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
