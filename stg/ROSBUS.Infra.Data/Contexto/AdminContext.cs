using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.EntityConfig;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ROSBUS.Admin.Domain;
using TECSO.FWK.Infra.Data.Transaction;
using Microsoft.EntityFrameworkCore.Storage;
using TECSO.FWK.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using TECSO.FWK.Infra.Data;

namespace ROSBUS.infra.Data.Contexto
{
    public class AdminContext : BaseContext, IAdminDbContext
    { 
        //private IAdminDBResilientTransaction resilientTransaction;

        //private IAdminDBResilientTransaction ResilientTransaction
        //{
        //    get
        //    {
        //        return this.resilientTransaction ?? (this.resilientTransaction = ServiceProviderResolver.ServiceProvider.GetService<IAdminDBResilientTransaction>());
        //    }

        //}


        public AdminContext(DbContextOptions<AdminContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Para regenerar todo el modelo nuevamente correr
            //En package Manager Console lo siguiente
            //scaffold-dbcontext 'Server=172.16.17.59;Database=ROSBUS;User Id=sa; Password=Pa$$w0rd'  Microsoft.EntityFrameworkCore.SqlServer -OutputDir Models -Force 
            //En la carpeta Modelss se van a generar todas las clases.

            modelBuilder.ApplyConfigurations();
            

        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            //if (ResilientTransaction.IsResilientTransaction())
            //{
            //    return Task.FromResult(0);
            //}
            return base.SaveChangesAsync(cancellationToken);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default(CancellationToken))
        {
            //if (ResilientTransaction.IsResilientTransaction())
            //{
            //    return Task.FromResult(0);
            //} 
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
        public virtual DbSet<Configu> Configu { get; set; }
        public virtual DbSet<CroElemeneto> CroElemeneto { get; set; }
        public virtual DbSet<CroTipo> CroTipo { get; set; }
        public virtual DbSet<CroTipoElemento> CroTipoElemento { get; set; }
        public virtual DbSet<CroCroquis> CroCroquis { get; set; }


        public virtual DbSet<HServicios> HServicios { get; set; }
        public virtual DbSet<PlaDistribucionDeCochesPorTipoDeDia> PlaDistribucionDeCochesPorTipoDeDia { get; set; }

        public virtual DbSet<HMediasVueltas> HMediasVueltas { get; set; }
        public virtual DbSet<SubGalpon> SubGalpon { get; set; }

        public virtual DbSet<HMinxtipo> HMinxtipo { get; set; }
        public virtual DbSet<HDetaminxtipo> HDetaminxtipo { get; set; }

        public virtual DbSet<HProcMin> HProcMin { get; set; }
        public virtual DbSet<HHorariosConfi> HHorariosConfi { get; set; }
        

        public virtual DbSet<PlaEstadoHorarioFecha> PlaEstadoHorarioFecha { get; set; }
        public virtual DbSet<HSectores> HSectores { get; set; }

        public virtual DbSet<BolBanderasCartelDetalle> BolBanderasCartelDetalle { get; set; }
        public virtual DbSet<BolBanderasCartel> BolBanderasCartel { get; set; }
        public virtual DbSet<BolSectoresTarifarios> BolSectoresTarifarios { get; set; }


        public virtual DbSet<Empresa> Empresa { get; set; }

        public virtual DbSet<FdAcciones> FdAcciones { get; set; }
        public virtual DbSet<FdFirmador> FdFirmadores { get; set; }

        public virtual DbSet<FdFirmadorDetalle> FdFirmadorDetalle { get; set; }
        public virtual DbSet<FdAccionesPermitidas> FdAccionesPermitidas { get; set; }
        public virtual DbSet<FdEstados> FdEstados { get; set; }
        public virtual DbSet<FdTiposDocumentos> FdTiposDocumentos { get; set; }
        public virtual DbSet<FdDocumentosProcesados> FdDocumentosProcesados { get; set; }
        public virtual DbSet<FdDocumentosError> FdDocumentosError { get; set; }
        public virtual DbSet<FdCertificados> FdCertificados { get; set; }
        public virtual DbSet<Galpon> Galpones { get; set; }
        public virtual DbSet<Grupos> Grupos { get; set; }
        public virtual DbSet<GpsEstadosActualesRepli> GpsEstadosActualesRepli { get; set; }
        public virtual DbSet<GpsMensajesCoches> GpsMensajesCoches { get; set; }
        public virtual DbSet<GpsDetaReco> GpsDetaReco { get; set; }
        public virtual DbSet<PlaPuntos> PlaPuntos { get; set; } 
        public virtual DbSet<GpsRecorridos> GpsRecorridos { get; set; }
        public virtual DbSet<HBanderas> HBanderas { get; set; }
        public virtual DbSet<HBanderasEspeciales> HBanderasEspeciales { get; set; }    
        public virtual DbSet<HBanderasTup> HBanderasTup { get; set; }
        public virtual DbSet<HBasec> HBasec { get; set; }
        public virtual DbSet<HKilometros> HKilometros { get; set; }
        public virtual DbSet<HBanderasColores> HBanderasColores { get; set; }

        public virtual DbSet<HFechas> HFechas { get; set; }
        public virtual DbSet<HFechasConfi> HFechasConfi { get; set; }
        public virtual DbSet<HDesignar> HDesignar { get; set; }
        public virtual DbSet<HTipodia> HTipodia { get; set; }
        public virtual DbSet<InfInformes> InfInformes { get; set; }
        public virtual DbSet<InsDesvios> InsDesvios { get; set; }
        public virtual DbSet<Linea> Linea { get; set; }
        public virtual DbSet<InfResponsables> InfResponsables { get; set; }
        public virtual DbSet<MotivoInfra> MotivoInfra { get; set; }
        public virtual DbSet<SysUltimosNumeros> SysUltimosNumeros { get; set; }

        public virtual DbSet<PlaCodigoSubeBandera> PlaCodigoSubeBandera { get; set; }
        public virtual DbSet<PlaCoordenadas> PlaCoordenadas { get; set; }
        public virtual DbSet<PlaEstadoRuta> PlaEstadoRuta { get; set; } 
        public virtual DbSet<PlaGrupoLineas> PlaGrupoLineas { get; set; }
        public virtual DbSet<PlaSentidoBanderaSube> PlaSentidosBanderaSube { get; set; }



        public virtual DbSet<PlaLinea> PlaLinea { get; set; }

        public virtual DbSet<PlanCam> PlanCam { get; set; }
        public virtual DbSet<PlaLineaLineaHoraria> PlaLineaLineaHoraria { get; set; }

        public virtual DbSet<PlaLineasUsuarios> PlaLineasUsuarios { get; set; }
        public virtual DbSet<PlaRamalColor> PlaRamalColor { get; set; }
        public virtual DbSet<PlaSector> PlaSector { get; set; }
        public virtual DbSet<PlaTalleresIvu> PlaTalleresIvu { get; set; }
        public virtual DbSet<PlaTiempoEsperadoDeCarga> PlaTiempoEsperadoDeCarga { get; set; }
        public virtual DbSet<PlaTipoBandera> PlaTipoBandera { get; set; }
        public virtual DbSet<PlaTipoLinea> PlaTipoLinea { get; set; }
        public virtual DbSet<PlaTipoParada> PlaTipoParada { get; set; }

        public virtual DbSet<PlaParadas> PlaParadas { get; set; }

        public virtual DbSet<SinAbogados> SinAbogados { get; set; }
        public virtual DbSet<SinBandaSiniestral> SinBandaSiniestral { get; set; }
        public virtual DbSet<SinCategorias> SinCategorias { get; set; }
        public virtual DbSet<SinCausas> SinCausas { get; set; }
        public virtual DbSet<SinCiaSeguros> SinCiaSeguros { get; set; }
        public virtual DbSet<SinConductasNormas> SinConductasNormas { get; set; }
        public virtual DbSet<CCoches> CCoches { get; set; }
        public virtual DbSet<SinConductores> SinConductores { get; set; }
        public virtual DbSet<SinConsecuencias> SinConsecuencias { get; set; }
        public virtual DbSet<SinDetalleLesion> SinDetalleLesion { get; set; }
        public virtual DbSet<SinEstados> SinEstados { get; set; }
        public virtual DbSet<SinFactoresIntervinientes> SinFactoresIntervinientes { get; set; }
        public virtual DbSet<SinSancionSugerida> SinSancionSugerida { get; set; }
        public virtual DbSet<SinInvolucrados> SinInvolucrados { get; set; }
        public virtual DbSet<SinInvolucradosAdjuntos> SinInvolucradosAdjuntos { get; set; }
        public virtual DbSet<SinJuzgados> SinJuzgados { get; set; }
        public virtual DbSet<SinLesionados> SinLesionados { get; set; }
        public virtual DbSet<SinMuebleInmueble> SinMuebleInmueble { get; set; }
        public virtual DbSet<SinPracticantes> SinPracticantes { get; set; }
        public virtual DbSet<SinReclamoAdjuntos> SinReclamoAdjuntos { get; set; }
        public virtual DbSet<SinReclamoCuotas> SinReclamoCuotas { get; set; }
        public virtual DbSet<SinReclamos> SinReclamos { get; set; }
        public virtual DbSet<SinReclamosHistoricos> SinReclamosHistoricos { get; set; }
        public virtual DbSet<SinSiniestroAdjuntos> SinSiniestroAdjuntos { get; set; }
        public virtual DbSet<SinSiniestros> SinSiniestros { get; set; }
        public virtual DbSet<SinSiniestrosConsecuencias> SinSiniestrosConsecuencias { get; set; }
        public virtual DbSet<SinSubCausas> SinSubCausas { get; set; }
        public virtual DbSet<SinSubEstados> SinSubEstados { get; set; }
        public virtual DbSet<SinTipoDanio> SinTipoDanio { get; set; }
        public virtual DbSet<SinTipoInvolucrado> SinTipoInvolucrado { get; set; }
        public virtual DbSet<SinTipoLesionado> SinTipoLesionado { get; set; }
        public virtual DbSet<SinTipoMueble> SinTipoMueble { get; set; }
        public virtual DbSet<SinVehiculos> SinVehiculos { get; set; }
        public virtual DbSet<Sucursales> Sucursales { get; set; }
        public virtual DbSet<SucursalesxLineas> SucursalesxLineas { get; set; }
        public virtual DbSet<SucursalesxEmpresas> SucursalesxEmpresas { get; set; }
        public virtual DbSet<SysAreas> SysAreas { get; set; }
        public virtual DbSet<SysPages> SysPages { get; set; }
        public virtual DbSet<SysPermissions> SysPermissions { get; set; }
        public virtual DbSet<SysPermissionsRoles> SysPermissionsRoles { get; set; }
        public virtual DbSet<SysPermissionsUsers> SysPermissionsUsers { get; set; }
        public virtual DbSet<SysRoles> SysRoles { get; set; }
        public virtual DbSet<SysUsersAd> SysUsersAd { get; set; }
        public virtual DbSet<SysUsersRoles> SysUsersRoles { get; set; }
        public virtual DbSet<SysParameters> SysParameters { get; set; }
        public virtual DbSet<Notification> Notification { get; set; }
        public virtual DbSet<SysDataTypes> SysDataTypes { get; set; }
        public virtual DbSet<TipoDni> TipoDni { get; set; }

        public virtual DbSet<HChoxser> HChoxser { get; set; }

        public virtual DbSet<DshUsuarioDashboardItem> DshUsuarioDashboardItems { get; set; }


        public virtual DbSet<ArtContingencias> ArtContingencias { get; set; }
        public virtual DbSet<ArtDenunciaAdjuntos> ArtDenunciaAdjuntos { get; set; }
        public virtual DbSet<ArtDenuncias> ArtDenuncias { get; set; }
        public virtual DbSet<ArtDenunciasNotificaciones> ArtDenunciasNotificaciones { get; set; }
        public virtual DbSet<ArtEstados> ArtEstados { get; set; }
        public virtual DbSet<ArtMotivosAudiencias> ArtMotivosAudiencias { get; set; }
        public virtual DbSet<ArtMotivosNotificaciones> ArtMotivosNotificaciones { get; set; }
        public virtual DbSet<ArtPatologias> ArtPatologias { get; set; }
        public virtual DbSet<ArtPrestadoresMedicos> ArtPrestadoresMedicos { get; set; }
        public virtual DbSet<ArtTratamientos> ArtTratamientos { get; set; }
        public virtual DbSet<CausasReclamo> CausasReclamo { get; set; }
        public virtual DbSet<TiposReclamo> TiposReclamo { get; set; }
        public virtual DbSet<TiposAcuerdo> TiposAcuerdo { get; set; }
        public virtual DbSet<RubrosSalariales> RubrosSalariales { get; set; }
        public virtual DbSet<InspGeo> InspGeo { get; set; }
        public virtual DbSet<InspGruposInspectores> InspGruposInspectores { get; set; }
        public virtual DbSet<InspTopesFrancosGruposInspectores> InspTopesFrancosGruposInspectores { get; set; }
        public virtual DbSet<InspTopesGruposInspectores> InspTopesGruposInspectores { get; set; }
        public virtual DbSet<InspZonas> InspZonas { get; set; }
        public virtual DbSet<InspDiagramasInspectores> InspDiagramasInspectores { get; set; }
        public virtual DbSet<InspEstadosDiagramaInspectores> InspEstadosDiagrama { get; set; }
        public virtual DbSet<InspGrupoInspectoresRangosHorarios> InspGrupoInspectoresRangosHorarios { get; set; }
        public virtual DbSet<InspGrupoInspectoresZona> InspGrupoInspectoresZona { get; set; }
        public virtual DbSet<InspGrupoInspectoresTarea> InspGrupoInspectoresTareas { get; set; }
        public virtual DbSet<InspTarea> InspTareas { get; set; }
        public virtual DbSet<InspTareaCampo> InspTareasCampos { get; set; }
        public virtual DbSet<InspTareaCampoConfig> InspTareasCamposConfig { get; set; }
        public virtual DbSet<InspTiposTarea> InspTiposTarea { get; set; }
        public virtual DbSet<InspRangosHorarios> InspRangosHorario { get; set; }
        public virtual DbSet<InspDiagramasInspectoresTurnos> InspDiagramaInspectoresTurnos { get; set; }
        public virtual DbSet<PersTurnos> PersTurnos { get; set; }
        public virtual DbSet<PersTopesHorasExtras> PersTopesHorasExtras { get; set; }
        public virtual DbSet<InspGruposInspectoresTurnos> InspGruposInspectoresTurnos { get; set; }

        public virtual DbSet<PersJornadasTrabajadas> PersJornadasTrabajadas { get; set; }
        public virtual DbSet<HFrancos> HFrancos { get; set; }
        public virtual DbSet<Novedades> Novedades { get; set; }
        public virtual DbSet<HNovxchof> HNovxchofs { get; set; }
        public virtual DbSet<InspGeo_Hist> InspGeoHist { get; set; }
        public virtual DbSet<InspRespuestasIncognitos> InspRespuestasIncognitos { get; set; }
        public virtual DbSet<InspPreguntasIncognitos> InspPreguntasIncognitos { get; set; }
        public virtual DbSet<InspPreguntasIncognitosRespuestas> InspPreguntasIncognitosRespuestas { get; set; }
        public virtual DbSet<PlaTipoViaje> PlaTipoViaje { get; set; }
        
    }


    public class AdminDBResilientTransaction : IAdminDBResilientTransaction
    {
        private readonly ResilientTransaction<AdminContext> resilientTransaction;

        public AdminDBResilientTransaction(AdminContext context)
        {
            this.resilientTransaction = ResilientTransaction<AdminContext>.New(context);
        }

        public Task ExecuteAsync(Func<Task> action)
        {
            return this.resilientTransaction.ExecuteAsync(action);
        }

        public bool IsResilientTransaction()
        {
            return this.resilientTransaction.IsResilientTransaction();
        }
    }
}
