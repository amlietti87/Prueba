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
using ROSBUS.ART.Domain.Interfaces.Services;
using ROSBUS.ART.Domain.Services;
using ROSBUS.ART.Domain.Interfaces.Repositories;

namespace ROSBUS.WebService.Siniestros
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
            services.AddDbContext<OperacionesRBContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("OperacionesRB")));
            services.AddDbContext<AdjuntosContext>(options =>
            options.UseSqlServer(Configuration.GetConnectionString("Adjuntos")));

            services.AddSwaggerGen(c =>
            {

                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("v1", new Info { Title = "Rosario Bus Siniestros (" + entorno + ")", Version = "v1" });

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

            AutoMapper.Mapper.Initialize(cfg =>
            {
                cfg.AddCollectionMappers();
                cfg.AddProfile<MappingProfile>();

            });



            services.AddMvc(
                 options =>
                 {
                     options.Filters.Add(new VersionActionFilter(Configuration));
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

            //Conductor
            services.AddTransient<IConductoresAppService, ConductoresAppService>();
            services.AddTransient<IConductoresService, ConductoresService>();
            services.AddTransient<IConductoresRepository, ConductoresRepository>();

            //Consecuencias
            services.AddTransient<IConsecuenciasAppService, ConsecuenciasAppService>();
            services.AddTransient<IConsecuenciasService, ConsecuenciasService>();
            services.AddTransient<IConsecuenciasRepository, ConsecuenciasRepository>();

            //SancionSugerida
            services.AddTransient<ISancionSugeridaAppService, SancionSugeridaAppService>();
            services.AddTransient<ISancionSugeridaService, SancionSugeridaService>();
            services.AddTransient<ISancionSugeridaRepository, SancionSugeridaRepository>();

            //TipoColision
            services.AddTransient<ITipoColisionAppService, TipoColisionAppService>();
            services.AddTransient<ITipoColisionService, TipoColisionService>();
            services.AddTransient<ITipoColisionRepository, TipoColisionRepository>();

            //Categorias
            services.AddTransient<ICategoriasAppService, CategoriasAppService>();
            services.AddTransient<ICategoriasService, CategoriasService>();
            services.AddTransient<ICategoriasRepository, CategoriasRepository>();

            //FactoresIntervinientes
            services.AddTransient<IFactoresIntervinientesAppService, FactoresIntervinientesAppService>();
            services.AddTransient<IFactoresIntervinientesService, FactoresIntervinientesService>();
            services.AddTransient<IFactoresIntervinientesRepository, FactoresIntervinientesRepository>();


            //Seguros
            services.AddTransient<ICiaSegurosAppService, CiaSegurosAppService>();
            services.AddTransient<ICiaSegurosService, CiaSegurosService>();
            services.AddTransient<ICiaSegurosRepository, CiaSegurosRepository>();

            //Abogados
            services.AddTransient<IAbogadosAppService, AbogadosAppService>();
            services.AddTransient<IAbogadosService, AbogadosService>();
            services.AddTransient<IAbogadosRepository, AbogadosRepository>();

            //Juzgados
            services.AddTransient<IJuzgadosAppService, JuzgadosAppService>();
            services.AddTransient<IJuzgadosService, JuzgadosService>();
            services.AddTransient<IJuzgadosRepository, JuzgadosRepository>();

            //TipoLesionado
            services.AddTransient<ITipoLesionadoAppService, TipoLesionadoAppService>();
            services.AddTransient<ITipoLesionadoService, TipoLesionadoService>();
            services.AddTransient<ITipoLesionadoRepository, TipoLesionadoRepository>();

            //Lesionados
            services.AddTransient<ILesionadosAppService, LesionadosAppService>();
            services.AddTransient<ILesionadosService, LesionadosService>();
            services.AddTransient<ILesionadosRepository, LesionadosRepository>();

            //TipoMueble
            services.AddTransient<ITipoMuebleAppService, TipoMuebleAppService>();
            services.AddTransient<ITipoMuebleService, TipoMuebleService>();
            services.AddTransient<ITipoMuebleRepository, TipoMuebleRepository>();

            //MuebleInmueble
            services.AddTransient<IMuebleInmuebleAppService, MuebleInmuebleAppService>();
            services.AddTransient<IMuebleInmuebleService, MuebleInmuebleService>();
            services.AddTransient<IMuebleInmuebleRepository, MuebleInmuebleRepository>();

            //Involucrado
            services.AddTransient<IInvolucradosAppService, InvolucradosAppService>();
            services.AddTransient<IInvolucradosService, InvolucradosService>();
            services.AddTransient<IInvolucradosRepository, InvolucradosRepository>();

            //TipoInvolucrado
            services.AddTransient<ITipoInvolucradoAppService, TipoInvolucradoAppService>();
            services.AddTransient<ITipoInvolucradoService, TipoInvolucradoService>();
            services.AddTransient<ITipoInvolucradoRepository, TipoInvolucradoRepository>();

            //Practicantes
            services.AddTransient<IPracticantesAppService, PracticantesAppService>();
            services.AddTransient<IPracticantesService, PracticantesService>();
            services.AddTransient<IPracticantesRepository, PracticantesRepository>();

            //TipoDni
            services.AddTransient<ITipoDniAppService, TipoDniAppService>();
            services.AddTransient<ITipoDniService, TipoDniService>();
            services.AddTransient<ITipoDniRepository, TipoDniRepository>();


            //Vehiculos
            services.AddTransient<IVehiculosAppService, VehiculosAppService>();
            services.AddTransient<IVehiculosService, VehiculosService>();
            services.AddTransient<IVehiculosRepository, VehiculosRepository>();


            //Siniestros
            services.AddTransient<ISiniestrosAppService, SiniestrosAppService>();
            services.AddTransient<ISiniestrosService, SiniestrosService>();
            services.AddTransient<ISiniestrosRepository, SiniestrosRepository>();

            //Reclamos
            services.AddTransient<IReclamosAppService, ReclamosAppService>();
            services.AddTransient<IReclamosService, ReclamosService>();
            services.AddTransient<IReclamosRepository, ReclamosRepository>();

            //Siniestros consecuencias
            services.AddTransient<ISiniestrosConsecuenciasAppService, SiniestrosConsecuenciasAppService>();
            services.AddTransient<ISiniestrosConsecuenciasService, SiniestrosConsecuenciasService>();
            services.AddTransient<ISiniestrosConsecuenciasRepository, SiniestrosConsecuenciasRepository>();

            //Reporter http
            services.AddTransient<IReporterHttpAppService, ReporterHttpAppService>();

            //Siniestro
            services.AddTransient<ISiniestrosConsecuenciasAppService, SiniestrosConsecuenciasAppService>();
            services.AddTransient<ISiniestrosConsecuenciasService, SiniestrosConsecuenciasService>();
            services.AddTransient<ISiniestrosConsecuenciasRepository, SiniestrosConsecuenciasRepository>();


            //Causas
            services.AddTransient<ICausasAppService, CausasAppService>();
            services.AddTransient<ICausasService, CausasService>();
            services.AddTransient<ICausasRepository, CausasRepository>();

            //SubCausas
            services.AddTransient<ISubCausasAppService, SubCausasAppService>();
            services.AddTransient<ISubCausasService, SubCausasService>();
            services.AddTransient<ISubCausasRepository, SubCausasRepository>();

            //Estados
            services.AddTransient<IEstadosAppService, EstadosAppService>();
            services.AddTransient<IEstadosService, EstadosService>();
            services.AddTransient<IEstadosRepository, EstadosRepository>();

            //SubEstados
            services.AddTransient<ISubEstadosAppService, SubEstadosAppService>();
            services.AddTransient<ISubEstadosService, SubEstadosService>();
            services.AddTransient<ISubEstadosRepository, SubEstadosRepository>();

            //Localidades
            services.AddTransient<ILocalidadesAppService, LocalidadesAppService>();
            services.AddTransient<ILocalidadesService, LocalidadesService>();
            services.AddTransient<ILocalidadesRepository, LocalidadesRepository>();

            //Provincias
            services.AddTransient<IProvinciasAppService, ProvinciasAppService>();
            services.AddTransient<IProvinciasService, ProvinciasService>();
            services.AddTransient<IProvinciasRepository, ProvinciasRepository>();


            //Empleados
            services.AddTransient<IEmpleadosAppService, EmpleadosAppService>();
            services.AddTransient<IEmpleadosService, EmpleadosService>();
            services.AddTransient<IEmpleadosRepository, EmpleadosRepository>();

            //Empleados
            services.AddTransient<ILegajosEmpleadoAppService, LegajosEmpleadoAppService>();
            services.AddTransient<ILegajosEmpleadoService, LegajosEmpleadoService>();
            services.AddTransient<ILegajosEmpleadoRepository, LegajosEmpleadoRepository>();

            //ConductasNormas
            services.AddTransient<IConductasNormasAppService, ConductasNormasAppService>();
            services.AddTransient<IConductasNormasService, ConductasNormasService>();
            services.AddTransient<IConductasNormasRepository, ConductasNormasRepository>();

            //CCoches
            services.AddTransient<ICCochesAppService, CCochesAppService>();
            services.AddTransient<ICCochesService, CCochesService>();
            services.AddTransient<ICCochesRepository, CCochesRepository>();

            //BandaSiniestral
            services.AddTransient<IBandaSiniestralAppService, BandaSiniestralAppService>();
            services.AddTransient<IBandaSiniestralService, BandaSiniestralService>();
            services.AddTransient<IBandaSiniestralRepository, BandaSiniestralRepository>();


            //BandaSiniestral
            services.AddTransient<ITipoDanioAppService, TipoDanioAppService>();
            services.AddTransient<ITipoDanioService, TipoDanioService>();
            services.AddTransient<ITipoDanioRepository, TipoDanioRepository>();

            //ReclamosHistoricos
            services.AddTransient<IReclamosHistoricosAppService, ReclamosHistoricosAppService>();
            services.AddTransient<IReclamosHistoricosService, ReclamosHistoricosService>();
            services.AddTransient<IReclamosHistoricosRepository, ReclamosHistoricosRepository>();

            //services.AddTransient<ROSBUS.Admin.Domain.Url.IWebUrlService, WebUrlService>();

            //services.AddTransient<Admin.Domain.IUserEmailer, ROSBUS.Admin.Domain.Interfaces.Mail.UserEmailer>();

            //Usuario
            //services.AddTransient<IUserAppService, UserAppService>();
            //services.AddTransient<IUserService, UserService>();
            //services.AddTransient<IUserRepository, UserRepository>();
            //services.AddTransient<ILdapRepository, LdapRepository>();


            //RoleService
            //services.AddTransient<IRoleAppService, RoleAppService>();
            //services.AddTransient<IRoleService, RoleService>();
            //services.AddTransient<IRoleRepository, RoleRepository>();
            //services.AddTransient<IPermissionRepository, PermissionsRepository>();
            //services.AddTransient<IPermissionService, PermissionService>();

            //Adjuntos
            services.AddTransient<IAdjuntosAppService, AdjuntosAppService>();
            services.AddTransient<IAdjuntosService, AdjuntosService>();
            services.AddTransient<IAdjuntosRepository, AdjuntosRepository>();

            //Adjuntos
            services.AddTransient<ICroCroquisAppService, CroCroquisAppService>();
            services.AddTransient<ICroCroquisService, CroCroquisService>();
            services.AddTransient<ICroCroquisRepository, CroCroquisRepository>();

            services.AddTransient<IErrorDbContext, ErrorDBContext>();

            services.AddScoped<TECSO.FWK.Domain.Interfaces.Services.IPermissionProvider, PermissionProvider>();
            

            //PermissionAppService
            services.AddTransient<IPermissionAppService, PermissionAppService>();
            services.AddTransient<IPermissionRepository, PermissionsRepository>();
            services.AddTransient<IPermissionService, PermissionService>();
            services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.IAuthService, AuthService>();


            services.AddSingleton<ICachingConfiguration, CachingConfiguration>();
            services.AddSingleton<ICacheManager, MemoryCacheManager>();

            //Tipos de reclamos
            services.AddTransient<ITiposReclamoService, TiposReclamoService>();
            services.AddTransient<ITiposReclamoRepository, TiposReclamoRepository>();

            //Sucursales
            services.AddTransient<IsucursalesService, sucursalesService>();
            services.AddTransient<IsucursalesRepository, sucursalesRepository>();

            //Empresa
            services.AddTransient<IEmpresaService, EmpresaService>();
            services.AddTransient<IEmpresaRepository, EmpresaRepository>();

            //CausasReclamos    
            services.AddTransient<ICausasReclamoService, CausasReclamoService>();
            services.AddTransient<ICausasReclamoRepository, CausasReclamoRepository>();

            //TiposAcuerdo
            services.AddTransient<ITiposAcuerdoService, TiposAcuerdoService>();
            services.AddTransient<ITiposAcuerdoRepository, TiposAcuerdoRepository>();

            //RubrosSalariales
            services.AddTransient<IRubrosSalarialesService, RubrosSalarialesService>();
            services.AddTransient<IRubrosSalarialesRepository, RubrosSalarialesRepository>();

            //Denuncias
            services.AddTransient<Admin.Domain.Interfaces.Services.ART.IDenunciasService, Admin.Domain.Services.ART.DenunciasService>();
            services.AddTransient<Admin.Domain.Interfaces.Repositories.ART.IDenunciasRepository, Infra.Data.Repositories.ART.DenunciasRepository>();


            //SysParameteres and SysDataTypes
            services.AddTransient<ISysParametersAppService, SysParametersAppService>();
            services.AddTransient<ISysParametersService, SysParametersService>();
            services.AddTransient<ISysParametersRepository, Infra.Data.Repositories.SysParametersRepository>();
            services.AddTransient<ISysDataTypesAppService, SysDataTypesAppService>();
            services.AddTransient<ISysDataTypesService, SysDataTypesService>();
            services.AddTransient<ISysDataTypesRepository, Infra.Data.Repositories.SysDataTypesRepository>();

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
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "API Siniestros V1");
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
