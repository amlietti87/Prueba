using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Swashbuckle.AspNetCore.Swagger;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService;
using ROSBUS.Admin.Domain.Services;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.infra.Data.Contexto;
using AutoMapper;
using Microsoft.AspNetCore.Mvc.Formatters;
using TECSO.FWK.ApiServices.Filters;
using ROSBUS.Admin.Domain;
using TECSO.FWK.Domain;
using AutoMapper.EquivalencyExpression;
using AutoMapper.EntityFramework;
using TECSO.FWK.Domain.Extensions;
using Newtonsoft.Json.Serialization;
using System.Globalization;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Authorization;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
//using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService;
using ROSBUS.Admin.AppService.service;
using System.IO;
using TECSO.FWK.AppService.Interface;
using Microsoft.EntityFrameworkCore.Design;
using TECSO.FWK.Domain.Interfaces;
using ROSBUS.Admin.Domain.Emailing;
using Microsoft.Extensions.Caching.Memory;
using TECSO.FWK.Caching;
using ROSBUS.WebService.Shared;
using ROSBUS.Operaciones.AppService.Interface;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Operaciones.Repositories;
using ROSBUS.Operaciones.Domain.Services;
using ROSBUS.Infra.Data.Repositories;
using BolSectoresTarifarioService = ROSBUS.Admin.Domain.Services.BolSectoresTarifarioService;
using ROSBUS.Admin.Domain.ParametersHelper;
using TECSO.FWK.Domain.Interfaces.Services;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ROSBUS.Admin.AppService.Interface.AppInspectores;
using ROSBUS.Admin.AppService.service.AppInspectores;
using ROSBUS.Admin.Domain.Services.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Services.AppInspectores;
using ROSBUS.Infra.Data.Repositories.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Repositories.AppInspectores;
using ROSBUS.Admin.Domain.Maps;
using ROSBUS.Admin.Domain.Interfaces;
using ROSBUS.Admin.AppService.Interface.AppInspectores;

namespace ROSBUS.WebService.Admin
{
    public class Startup
    {
        IHostingEnvironment environment;
        public Startup(IConfiguration configuration, IHostingEnvironment env)
        {
            Configuration = configuration;
            environment = env;


        }
        private IConfiguration Configuration { get; }

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

            var version = string.Format("v{0}", Configuration.GetValue<string>("Version"));
            services.AddSwaggerGen(c =>
            {

                c.DescribeAllEnumsAsStrings();

                c.SwaggerDoc("v1", new Info { Title = "Rosario Bus Admin (" + entorno + ")", Version = version });

                var security = new Dictionary<string, IEnumerable<string>> {
                    { "Bearer", new string[] { } }
                };

                // Define the BearerAuth scheme that's in use
                c.AddSecurityDefinition("Bearer", new ApiKeyScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                    Name = "Authorization",
                    In = "header",
                    Type = "apiKey"
                });

                c.AddSecurityRequirement(security);
                // Assign scope requirements to operations based on AuthorizeAttribute
                //c.OperationFilter<SecurityRequirementsOperationFilter>();


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



            services.AddMemoryCache();

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
                    );





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

            // services.AddScoped<IAdminDbContext>(DbContextActivator.CreateInstance(typeof(AdminContext)));

            // services.AddScoped<IAdminDbContext, AdminContext>();



            services.AddTransient<TECSO.FWK.Domain.Mail.Smtp.ISmtpEmailSenderConfiguration, TECSO.FWK.Domain.Mail.Smtp.SmtpEmailSenderConfiguration>();
            services.AddTransient<IEmailTemplateProvider, EmailTemplateProvider>();
            services.AddTransient<TECSO.FWK.Domain.Mail.IEmailSender, TECSO.FWK.Domain.Mail.Smtp.SmtpEmailSender>();

            services.AddTransient<ROSBUS.Admin.Domain.Url.IAppUrlService, AngularAppUrlService>();
            services.AddTransient<ROSBUS.Admin.Domain.Url.IWebUrlService, WebUrlService>();

            //
            services.AddTransient<IUserEmailer, ROSBUS.Admin.Domain.Interfaces.Mail.UserEmailer>();
            services.AddTransient<IDefaultEmailer, ROSBUS.Admin.Domain.Interfaces.Mail.DefaultEmailer>();
            //Adjuntos
            services.AddTransient<IAdjuntosAppService, AdjuntosAppService>();
            services.AddTransient<IAdjuntosService, AdjuntosService>();
            services.AddTransient<IAdjuntosRepository, AdjuntosRepository>();

            //Usuario
            services.AddTransient<IUserAppService, UserAppService>();
            services.AddTransient<IUserService, UserService>();
            services.AddTransient<IUserRepository, UserRepository>();
            services.AddTransient<ILdapRepository, LdapRepository>();

            //Usuario
            services.AddTransient<IRoleAppService, RoleAppService>();
            services.AddTransient<IRoleService, RoleService>();
            services.AddTransient<IRoleRepository, RoleRepository>();


            //PermissionAppService
            services.AddTransient<IPermissionAppService, PermissionAppService>();
            services.AddTransient<IPermissionRepository, PermissionsRepository>();
            services.AddTransient<IPermissionService, PermissionService>();

            //Involucrado
            services.AddTransient<IInvolucradosAppService, InvolucradosAppService>();
            services.AddTransient<IInvolucradosService, InvolucradosService>();
            services.AddTransient<IInvolucradosRepository, InvolucradosRepository>();

            //Linea
            services.AddTransient<ILineaAppService, LineaAppService>();
            services.AddTransient<ILineaService, LineaService>();
            services.AddTransient<ILineaRepository, LineaRepository>();

            //InfInformes
            services.AddTransient<IInfInformesAppService, InfInformesAppService>();
            services.AddTransient<IInfInformesService, InfInformesService>();
            services.AddTransient<IInfInformesRepository, InfInformesRepository>();

            //InsDesvios
            services.AddTransient<IInsDesviosAppService, InsDesviosAppService>();
            services.AddTransient<IInsDesviosService, InsDesviosService>();
            services.AddTransient<IInsDesviosRepository, InsDesviosRepository>();

            //TipoLinea
            services.AddTransient<ITipoLineaAppService, TipoLineaAppService>();
            services.AddTransient<ITipoLineaService, TipoLineaService>();
            services.AddTransient<ITipoLineaRepository, TipoLineaRepository>();


            //Empresa
            services.AddTransient<IEmpresaAppService, EmpresaAppService>();
            services.AddTransient<IEmpresaService, EmpresaService>();
            services.AddTransient<IEmpresaRepository, EmpresaRepository>();

            //MotivoInfra
            services.AddTransient<IMotivoInfraAppService, MotivoInfraAppService>();
            services.AddTransient<IMotivoInfraService, MotivoInfraService>();
            services.AddTransient<IMotivoInfraRepository, MotivoInfraRepository>();

            //GrupoLineas
            services.AddTransient<IGrupoLineasAppService, GrupoLineasAppService>();
            services.AddTransient<IGrupoLineasService, GrupoLineasService>();
            services.AddTransient<IGrupoLineasRepository, GrupoLineasRepository>();

            //TiposDeDias
            services.AddTransient<ITiposDeDiasAppService, TiposDeDiasAppService>();
            services.AddTransient<ITiposDeDiasService, TiposDeDiasService>();
            services.AddTransient<ITiposDeDiasRepository, TiposDeDiasRepository>();

            //TiposDeParada
            services.AddTransient<ITipoParadaAppService, TipoParadaAppService>();
            services.AddTransient<ITipoParadaService, TipoParadaService>();
            services.AddTransient<ITipoParadaRepository, TipoParadaRepository>();

            //TiempoEsperadoDeCarga
            services.AddTransient<ITiempoEsperadoDeCargaAppService, TiempoEsperadoDeCargaAppService>();
            services.AddTransient<ITiempoEsperadoDeCargaService, TiempoEsperadoDeCargaService>();
            services.AddTransient<ITiempoEsperadoDeCargaRepository, TiempoEsperadoDeCargaRepository>();

            //RamalColor
            services.AddTransient<IRamalColorAppService, RamalColorAppService>();
            services.AddTransient<IRamalColorService, RamalColorService>();
            services.AddTransient<IRamalColorRepository, RamalColorRepository>();


            //Bandera
            services.AddTransient<IBanderaAppService, BanderaAppService>();
            services.AddTransient<IBanderaService, BanderaService>();
            services.AddTransient<IBanderaRepository, BanderaRepository>();

            //Rutas
            services.AddTransient<IRutasAppService, RutasAppService>();
            services.AddTransient<IRutasService, RutasService>();
            services.AddTransient<IRutasRepository, RutasRepository>();


            //EstadoRuta
            services.AddTransient<IEstadoRutaAppService, EstadoRutaAppService>();
            services.AddTransient<IEstadoRutaService, EstadoRutaService>();
            services.AddTransient<IEstadoRutaRepository, EstadoRutaRepository>();

            //Estados
            services.AddTransient<IEstadosAppService, EstadosAppService>();
            services.AddTransient<IEstadosService, EstadosService>();
            services.AddTransient<IEstadosRepository, EstadosRepository>();

            //Puntos
            services.AddTransient<IPuntosAppService, PuntosAppService>();
            services.AddTransient<IPuntosService, PuntosService>();
            services.AddTransient<IPuntosRepository, PuntosRepository>();


            //Sector
            services.AddTransient<ISectorAppService, SectorAppService>();
            services.AddTransient<ISectorService, SectorService>();
            services.AddTransient<ISectorRepository, SectorRepository>();

            //Taller
            services.AddTransient<IGalponAppService, GalponAppService>();
            services.AddTransient<IGalponService, GalponService>();
            services.AddTransient<IGalponRepository, GalponRepository>();


            //TipoBandera
            services.AddTransient<ITipoBanderaAppService, TipoBanderaAppService>();
            services.AddTransient<ITipoBanderaService, TipoBanderaService>();
            services.AddTransient<ITipoBanderaRepository, TipoBanderaRepository>();


            //Coordenadas
            services.AddTransient<ICoordenadasAppService, CoordenadasAppService>();
            services.AddTransient<ICoordenadasService, CoordenadasService>();
            services.AddTransient<ICoordenadasRepository, CoordenadasRepository>();

            //sucursales
            services.AddTransient<IsucursalesAppService, sucursalesAppService>();
            services.AddTransient<IsucursalesService, sucursalesService>();
            services.AddTransient<IsucursalesRepository, sucursalesRepository>();


            //sucursalesxLineas
            services.AddTransient<IsucursalesxLineasAppService, sucursalesxLineasAppService>();
            services.AddTransient<IsucursalesxLineasService, sucursalesxLineasService>();
            services.AddTransient<IsucursalesxLineasRepository, sucursalesxLineasRepository>();


            //HBanderasTup
            services.AddTransient<IHBanderasTupAppService, HBanderasTupAppService>();
            services.AddTransient<IHBanderasTupService, HBanderasTupService>();
            services.AddTransient<IHBanderasTupRepository, HBanderasTupRepository>();

            //BandasHorarias
            services.AddTransient<IBandasHorariasService, BandasHorariasService>();
            services.AddTransient<IBandasHorariasRepository, BandasHorariasRepository>();

            //HBanderasTup
            services.AddTransient<IHFechasAppService, HFechasAppService>();
            services.AddTransient<IHFechasService, HFechasService>();
            services.AddTransient<IHFechasRepository, HFechasRepository>();

            //PlaMinutosPorSector
            services.AddTransient<IPlaMinutosPorSectorAppService, PlaMinutosPorSectorAppService>();
            services.AddTransient<IPlaMinutosPorSectorService, PlaMinutosPorSectorService>();
            services.AddTransient<IPlaMinutosPorSectorRepository, PlaMinutosPorSectorRepository>();


            //PlaSentidoBandera
            services.AddTransient<IPlaSentidoBanderaAppService, PlaSentidoBanderaAppService>();
            services.AddTransient<IPlaSentidoBanderaService, PlaSentidoBanderaService>();
            services.AddTransient<IPlaSentidoBanderaRepository, PlaSentidoBanderaRepository>();

            //DshDashboard
            services.AddTransient<IDshDashboardAppService, DshDashboardAppService>();
            services.AddTransient<IDshDashboardRepository, DshDashboardRepository>();
            services.AddTransient<IDshDashboardService, DshDashboardService>();


            //HServicios
            services.AddTransient<IHServiciosAppService, HServiciosAppService>();
            services.AddTransient<IHServiciosRepository, HServiciosRepository>();
            services.AddTransient<IHServiciosService, HServiciosService>();

            //InfResponsables
            services.AddTransient<IInfResponsablesAppService, InfResponsablesAppService>();
            services.AddTransient<IInfResponsablesRepository, InfResponsablesRepository>();
            services.AddTransient<IInfResponsablesService, InfResponsablesService>();

            //HServicios
            services.AddTransient<IPlaLineaAppService, PlaLineaAppService>();
            services.AddTransient<IPlaLineaRepository, PlaLineaRepository>();
            services.AddTransient<IPlaLineaService, PlaLineaService>();

            
            //HFechasConfi
            services.AddTransient<IHFechasConfiAppService, HFechasConfiAppService>();
            services.AddTransient<IHFechasConfiRepository, HFechasConfiRepository>();
            services.AddTransient<IHFechasConfiService, HFechasConfiService>();


            //PlaEstadoHorarioFecha
            services.AddTransient<IPlaEstadoHorarioFechaAppService, PlaEstadoHorarioFechaAppService>();
            services.AddTransient<IPlaEstadoHorarioFechaRepository, PlaEstadoHorarioFechaRepository>();
            services.AddTransient<IPlaEstadoHorarioFechaService, PlaEstadoHorarioFechaService>();


            //SubGalpon
            services.AddTransient<ISubGalponAppService, SubGalponAppService>();
            services.AddTransient<ISubGalponRepository, SubGalponRepository>();
            services.AddTransient<ISubGalponService, SubGalponService>();


            //HMediasVueltas
            services.AddTransient<IHMediasVueltasAppService, HMediasVueltasAppService>();
            services.AddTransient<IHMediasVueltasRepository, HMediasVueltasRepository>();
            services.AddTransient<IHMediasVueltasService, HMediasVueltasService>();

            //HMediasVueltas
            services.AddTransient<IHHorariosConfiAppService, HHorariosConfiAppService>();
            services.AddTransient<IHHorariosConfiRepository, HHorariosConfiRepository>();
            services.AddTransient<IHHorariosConfiService, HHorariosConfiService>();

            //HTposHoras
            services.AddTransient<IHTposHorasAppService, HTposHorasAppService>();
            services.AddTransient<IHTposHorasRepository, HTposHorasRepository>();
            services.AddTransient<IHTposHorasService, HTposHorasService>();

            //IHMinxtipoAppService
            services.AddTransient<IHMinxtipoAppService, HMinxtipoAppService>();
            services.AddTransient<IHMinxtipoRepository, HMinxtipoRepository>();
            services.AddTransient<IHMinxtipoService, HMinxtipoService>();

            //PlaDistribucionDeCochesPorTipoDeDia
            services.AddTransient<IPlaDistribucionDeCochesPorTipoDeDiaAppService, PlaDistribucionDeCochesPorTipoDeDiaAppService>();
            services.AddTransient<IPlaDistribucionDeCochesPorTipoDeDiaRepository, PlaDistribucionDeCochesPorTipoDeDiaRepository>();
            services.AddTransient<IPlaDistribucionDeCochesPorTipoDeDiaService, PlaDistribucionDeCochesPorTipoDeDiaService>();


            //BolBanderasCartel
            services.AddTransient<IBolBanderasCartelAppService, BolBanderasCartelAppService>();
            services.AddTransient<IBolBanderasCartelRepository, BolBanderasCartelRepository>();
            services.AddTransient<IBolBanderasCartelService, BolBanderasCartelService>();

            //BolSectoresTarifarios
            services.AddTransient<IBolSectoresTarifariosAppService, BolSectoresTarifariosAppService>();
            services.AddTransient<IBolSectoresTarifariosRepository, BolSectoresTarifariosRepository>();
            services.AddTransient<IBolSectoresTarifariosService, BolSectoresTarifarioService>();


            //CroTipoElemento
            services.AddTransient<ICroTipoElementoAppService, CroTipoElementoAppService>();
            services.AddTransient<ICroTipoElementoRepository, CroTipoElementoRepository>();
            services.AddTransient<ICroTipoElementoService, CroTipoElementoService>();

            //CroCroquis
            services.AddTransient<ICroCroquisAppService, CroCroquisAppService>();
            services.AddTransient<ICroCroquisRepository, CroCroquisRepository>();
            services.AddTransient<ICroCroquisService, CroCroquisService>();

            //CroCroquis //AddedHttpAppService
            services.AddTransient<IReporterHttpAppService, ReporterHttpAppService>();
            services.AddTransient<ISiniestrosAppService, SiniestrosAppService>();
            services.AddTransient<ISiniestrosRepository, SiniestrosRepository>();
            services.AddTransient<ISiniestrosService, SiniestrosService>();

            //Reclamos
            services.AddTransient<IReclamosAppService, ReclamosAppService>();
            services.AddTransient<IReclamosService, ReclamosService>();
            services.AddTransient<IReclamosRepository, ReclamosRepository>();

            //CroCroquis
            services.AddTransient<IEmpleadosAppService, Operaciones.AppService.EmpleadosAppService>();
            services.AddTransient<IEmpleadosRepository, EmpleadosRepository>();
            services.AddTransient<IEmpleadosService, EmpleadosService>();

            //CroCroquis
            services.AddTransient<ILocalidadesAppService, LocalidadesAppService>();
            services.AddTransient<ILocalidadesRepository, LocalidadesRepository>();
            services.AddTransient<ILocalidadesService, LocalidadesService>();

            //CroTipo
            services.AddTransient<ICroTipoAppService, CroTipoAppService>();
            services.AddTransient<ICroTipoRepository, CroTipoRepository>();
            services.AddTransient<ICroTipoService, CroTipoService>();

            //CroElemento
            services.AddTransient<ICroElemenetoAppService, CroElemenetoAppService>();
            services.AddTransient<ICroElemenetoRepository, CroElemenetoRepository>();
            services.AddTransient<ICroElemenetoService, CroElemenetoService>();

            //Configu
            //services.AddTransient<IConfiguAppService, ConfiguAppService>();
            //services.AddTransient<IConfiguService, ConfiguService>();
            //services.AddTransient<IConfiguRepository, ConfiguRepository>();

            //Grupos
            services.AddTransient<IGruposAppService, GruposAppService>();
            services.AddTransient<IGruposService, GruposService>();
            services.AddTransient<IGruposRepository, GruposRepository>();

            //Galpon
            services.AddTransient<IGalponAppService, GalponAppService>();
            services.AddTransient<IGalponService, GalponService>();
            services.AddTransient<IGalponRepository, GalponRepository>();

            //TalleresIvu
            services.AddTransient<IPlaTalleresIvuAppService, PlaTalleresIvuAppService>();
            services.AddTransient<IPlaTalleresIvuService, PlaTalleresIvuService>();
            services.AddTransient<IPlaTalleresIvuRepository, PlaTalleresIvuRepository>();

            //SubGalpon
            services.AddTransient<ISubGalponAppService, SubGalponAppService>();
            services.AddTransient<ISubGalponService, SubGalponService>();
            services.AddTransient<ISubGalponRepository, SubGalponRepository>();

            //Notification
            services.AddTransient<INotificationAppService, NotificationAppService>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<INotificationRepository, NotificationRepository>();

            //InspGruposInspectores
            services.AddTransient<IInspGruposInspectoresAppService, InspGruposInspectoresAppService>();
            services.AddTransient<IInspGruposInspectoresService, InspGruposInspectoresService>();
            services.AddTransient<IInspGruposInspectoresRepository, InspGruposInspectoresRepository>();

            //InspZonas
            services.AddTransient<IInspZonasAppService, InspZonasAppService>();
            services.AddTransient<IInspZonasService, InspZonasService>();
            services.AddTransient<IInspZonasRepository, InspZonasRepository>();

            //InspRangosHorario
            services.AddTransient<IInspRangosHorarioAppService, InspRangosHorarioAppService>();
            services.AddTransient<IInspRangosHorarioService, InspRangosHorarioService>();
            services.AddTransient<IInspRangosHorarioRepository, InspRangosHorarioRepository>();

            //InspTiposTarea
            services.AddTransient<IInspTiposTareaAppService, InspTiposTareaAppService>();
            services.AddTransient<IInspTiposTareaService, InspTiposTareaService>();
            services.AddTransient<IInspTiposTareaRepository, InspTiposTareaRepository>();

            //InspGruposInspectoresRangosHorario
            services.AddTransient<IInspGruposInspectoresRangosHorariosAppService, InspGruposInspectoresRangosHorariosAppService>();
            services.AddTransient<IInspGruposInspectoresRangosHorariosService, InspGruposInspectoresRangosHorariosService>();
            services.AddTransient<IInspGruposInspectoresRangosHorariosRepository, InspGruposInspectoresRangosHorariosRepository>();

            //InspGruposInspectoresZonas
            services.AddTransient<IInspGruposInspectoresZonasAppService, InspGruposInspectoresZonasAppService>();
            services.AddTransient<IInspGruposInspectoresZonasService, InspGruposInspectoresZonasService>();
            services.AddTransient<IInspGruposInspectoresZonasRepository, InspGruposInspectoresZonasRepository>();

            //InspGruposInspectoresTiposTarea
            services.AddTransient<IInspGruposInspectoresTareaAppService, InspGruposInspectoresTareaAppService>();
            services.AddTransient<IInspGruposInspectoresTareaService, InspGruposInspectoresTareaService>();
            services.AddTransient<IInspGruposInspectoresTareaRepository, InspGruposInspectoresTareaRepository>();

            //InspGruposInspectoresTurno
            services.AddTransient<IInspGruposInspectoresTurnosAppService, InspGruposInspectoresTurnosAppService>();
            services.AddTransient<IInspGruposInspectoresTurnosService, InspGruposInspectoresTurnosService>();
            services.AddTransient<IInspGruposInspectoresTurnosRepository, InspGruposInspectoresTurnosRepository>();

            //Novedades
            services.AddTransient<INovedadesAppService, NovedadesAppService>();
            services.AddTransient<INovedadesService, NovedadesService>();
            services.AddTransient<INovedadesRepository, NovedadesRepository>();

            //InspDiagramasInspectores
            services.AddTransient<IInspDiagramasInspectoresAppService, InspDiagramasInspectoresAppService>();
            services.AddTransient<IInspDiagramasInspectoresService, InspDiagramasInspectoresService>();
            services.AddTransient<IInspDiagramasInspectoresRepository, InspDiagramasInspectoresRepository>();

            services.AddTransient<IInspDiagramasInspectoresTurnosAppService, InspDiagramasInspectoresTurnosAppService>();
            services.AddTransient<IInspDiagramasInspectoresTurnosService, InspDiagramasInspectoresTurnosService>();
            services.AddTransient<IInspDiagramasInspectoresTurnosRepository, InspDiagramasInspectoresTurnosRepository>();

            //InspEstadosDiagramaInspectores
            services.AddTransient<IInspEstadosDiagramaInspectoresAppService, InspEstadosDiagramaInspectoresAppService>();
            services.AddTransient<IInspEstadosDiagramaInspectoresService, InspEstadosDiagramaInspectoresService>();
            services.AddTransient<IInspEstadosDiagramaInspectoresRepository, InspEstadosDiagramaInspectoresRepository>();

            //Tareas
            services.AddTransient<IInspTareaAppService, InspTareaAppService>();
            services.AddTransient<IInspTareaService, InspTareaService>();
            services.AddTransient<IInspTareaRepository, InspTareaRepository>();

            //TareasCampos
            services.AddTransient<IInspTareaCampoAppService, InspTareaCampoAppService>();
            services.AddTransient<IInspTareaCampoService, InspTareaCampoService>();
            services.AddTransient<IInspTareaCampoRepository, InspTareaCampoRepository>();
            
            //TareasCamposConfig
            services.AddTransient<IInspTareaCampoConfigAppService, InspTareaCampoConfigAppService>();
            services.AddTransient<IInspTareaCampoConfigService, InspTareaCampoConfigService>();
            services.AddTransient<IInspTareaCampoConfigRepository, InspTareaCampoConfigRepository>();

            //InspTareasRealizadas
            services.AddTransient<IInspTareasRealizadasAppService, InspTareasRealizadasAppService>();
            services.AddTransient<IInspTareasRealizadasService, InspTareasRealizadasService>();
            services.AddTransient<IInspTareasRealizadasRepository, InspTareasRealizadasRepository>();

            

            //PersTopesHorasExtras
            services.AddTransient<IPersTopesHorasExtrasAppService, PersTopesHorasExtrasAppService>();
            services.AddTransient<IPersTopesHorasExtrasService, PersTopesHorasExtrasService>();
            services.AddTransient<IPersTopesHorasExtrasRepository, PersTopesHorasExtrasRepository>();

            //PersTurnos
            services.AddTransient<IPersTurnosAppService, PersTurnosAppService>();
            services.AddTransient<IPersTurnosService, PersTurnosService>();
            services.AddTransient<IPersTurnosRepository, PersTurnosRepository>();

            //PlanillaIncognito
            services.AddTransient<IInspPlanillaIncognitosAppService, InspPlanillaIncognitosAppService>();
            services.AddTransient<IInspPlanillaIncognitosService, InspPlanillaIncognitosService>();
            services.AddTransient<IInspPlanillaIncognitosRepository, InspPlanillaIncognitosRepository>();

            //PlanillaIncongnitoDetalles
            services.AddTransient<IInspPlanillaIncognitosDetalleAppService, InspPlanillaIncognitosDetalleAppService>();
            services.AddTransient<IInspPlanillaIncognitosDetalleService, InspPlanillaIncognitosDetalleService>();
            services.AddTransient<IInspPlanillaIncognitosDetalleRepository, InspPlanillaIncognitosDetalleRepository>();


            //InspRespuestasIncognitos
            services.AddTransient<IInspRespuestasIncognitosAppService, InspRespuestasIncognitosAppService>();
            services.AddTransient<IInspRespuestasIncognitosService, InspRespuestasIncognitosService>();
            services.AddTransient<IInspRespuestasIncognitosRepository, InspRespuestasIncognitosRepository>();

            //InspPreguntasIncognitos
            services.AddTransient<IInspPreguntasIncognitosAppService, InspPreguntasIncognitosAppService>();
            services.AddTransient<IInspPreguntasIncognitosService, InspPreguntasIncognitosService>();
            services.AddTransient<IInspPreguntasIncognitosRepository, InspPreguntasIncognitosRepository>();

            //InspPreguntasIncognitosRespuestas
            services.AddTransient<IInspPreguntasIncognitosRespuestasAppService, InspPreguntasIncognitosRespuestasAppService>();
            services.AddTransient<IInspPreguntasIncognitosRespuestasService, InspPreguntasIncognitosRespuestasService>();
            services.AddTransient<IInspPreguntasIncognitosRespuestasRepository, InspPreguntasIncognitosRespuestasRepository>();


            //SysParameteres and SysDataTypes
            services.AddTransient<ISysParametersAppService, SysParametersAppService>();
            services.AddTransient<ISysParametersService, SysParametersService>();
            services.AddTransient<ISysParametersRepository, SysParametersRepository>();
            services.AddTransient<ISysDataTypesAppService, SysDataTypesAppService>();
            services.AddTransient<ISysDataTypesService, SysDataTypesService>();
            services.AddTransient<ISysDataTypesRepository, SysDataTypesRepository>();


            //PlanCamNav
            services.AddTransient<IPlanCamAppService, PlanCamAppService>();
            services.AddTransient<IPlanCamService, PlanCamService>();
            services.AddTransient<IPlanCamRepository, PlanCamRepository>();

            //SentidoBanderaSube
            services.AddTransient<ISentidoBanderaSubeAppService, SentidoBanderaSubeAppService>();
            services.AddTransient<ISentidoBanderaSubeService, SentidoBanderaSubeService>();
            services.AddTransient<ISentidoBanderaSubeRepository, SentidoBanderaSubeRepository>();

            //HDesignar
            services.AddTransient<IHDesignarAppService, HDesignarAppService>();
            services.AddTransient<IHDesignarService, HDesignarService>();
            services.AddTransient<IHDesignarRepository, HDesignarRepository>();


            //HChoxser
            services.AddTransient<IHChoxserAppService, HChoxserAppService>();
            services.AddTransient<IHChoxserService, HChoxserService>();
            services.AddTransient<IHChoxserRepository, HChoxserRepository>();

 

            //HBanderasColores
            services.AddTransient<IHBanderasColoresAppService, HBanderasColoresAppService>();
            services.AddTransient<IHBanderasColoresService, HBanderasColoresService>();
            services.AddTransient<IHBanderasColoresRepository, HBanderasColoresRepository>();

            //PlaParadas
            services.AddTransient<IPlaParadasAppService, PlaParadasAppService>();
            services.AddTransient<IPlaParadasService, PlaParadasService>();
            services.AddTransient<IPlaParadasRepository, PlaParadasRepository>();


            //PlaTipoViaje
            services.AddTransient<IPlaTipoViajeAppService, PlaTipoViajeAppService>();
            services.AddTransient<IPlaTipoViajeService, PlaTipoViajeService>();
            services.AddTransient<IPlaTipoViajeRepository, TipoViajeRepository>();


            services.AddScoped<TECSO.FWK.Domain.Interfaces.Services.IPermissionProvider, PermissionProvider>();

            services.AddTransient<TECSO.FWK.Domain.Interfaces.Services.IAuthService, AuthService>();

            services.AddSingleton<TECSO.FWK.Caching.Configuration.ICachingConfiguration, TECSO.FWK.Caching.Configuration.CachingConfiguration>();
            services.AddSingleton<ICacheManager, TECSO.FWK.Caching.Memory.MemoryCacheManager>();
            services.AddTransient<IParametersHelper, ParametersHelper>();
            services.AddTransient<ISharpKMLHelper, SharpKMLHelper>();

            ///////////LOG///////////////
            if (environment.IsDevelopment())
            {
                //services.AddTransient<ILogger, TECSO.FWK.AppService.LogServiceDebug>();
                services.AddTransient<ILogger, LogServiceDebug>();
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

            var version = string.Format("v{0}", Configuration.GetValue<string>("Version")) ;

            var enableSwagger = Configuration.GetValue<bool>("EnableSwagger");

            if (enableSwagger)
            {
                app.UseSwagger();
                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint($"{ (!string.IsNullOrEmpty(pathBase) ? pathBase : string.Empty) }/swagger/v1/swagger.json", "Ros Bus Admin API " + version);
                });
            }
            

            ServiceProviderResolver.ServiceProvider = app.ApplicationServices;

            LDapUtils.LdapConfiguration = this.Configuration.GetSection("LdapConfiguration").Get<LdapConfiguration>();
            LDapUtils.LdapConfiguration.IsDevelopment = !env.IsProduction();


            var locale = Configuration["SiteLocale"];

            if (!string.IsNullOrEmpty(locale))
            {
                RequestLocalizationOptions localizationOptions = new RequestLocalizationOptions
                {
                    SupportedCultures = new List<CultureInfo> { new CultureInfo(locale) },
                    SupportedUICultures = new List<CultureInfo> { new CultureInfo(locale) },
                    DefaultRequestCulture = new RequestCulture(locale)
                };
                app.UseRequestLocalization(localizationOptions);

                CultureInfo.DefaultThreadCurrentCulture = new CultureInfo(locale);
            }



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
