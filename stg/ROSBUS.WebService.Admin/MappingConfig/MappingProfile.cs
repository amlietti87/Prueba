using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.Model.AppInspectores;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Extensions;

namespace ROSBUS.WebService.Admin
{

    public class MappingProfile : Profile
    {
        //  where TModel : Entity<TPrimaryKey>, new()
        //where TDto : EntityDto<TPrimaryKey>, new()
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<SysUsersAd, UserDto>().ForMember(
                    dest => dest.EsInspector,
                    opt => opt.MapFrom(src => src.UserRoles.Any(e => e.RoleId == SysUsersRoles.Inspector))
                ).ForMember(
                    dest => dest.TpoNroDoc,
                    opt => opt.MapFrom(src => GetTpoNroDoc(src))
                )
                .ForMember(
                    dest => dest.TurnoIdAnterior,
                    opt => opt.MapFrom(src => src.TurnoId)
                );

 
            CreateMap<UserDto, SysUsersAd>().ForMember(
                    dest => dest.TpoNroDoc,
                    opt => opt.MapFrom(src => GetTpoNroDocDto(src))
                );

            CreateMap<UserRoleDto, SysUsersRoles>().EqualityComparison((odto, o) => odto.RoleId == o.RoleId);
            CreateMap<SysUsersRoles, UserRoleDto>().EqualityComparison((odto, o) => odto.RoleId == o.RoleId);
            CreateMap<SysParameters, SysParametersDto>()
                .ForMember(d => d.Descripcion, o => o.MapFrom(s => s.Description));
            CreateMap<SysParametersDto, SysParameters>();


            CreateMap<SysRoles, RoleDto>();
            CreateMap<RoleDto, SysRoles>();

            CreateMap<Linea, LineaDto>()
                .ForMember(dest => dest.DesLin, opt => opt.MapFrom(src => src.DesLin.TrimOrNull()))
                .ForMember(
                    dest => dest.SucursalId,
                    opt => opt.MapFrom(src => src.SucursalesxLineas.FirstOrDefault().Id)
                );

            CreateMap<LineaDto, Linea>();


            CreateMap<PlaParadas, PlaParadasDto>()
                .ForMember(dest => dest.ParentStation, opt => opt.MapFrom(src => src.ParentStation != null ? new ItemDto<int>(src.ParentStation.Id, src.ParentStation.GetDescription()) : null));                       
            CreateMap<PlaParadasDto, PlaParadas>()
                .ForMember(x => x.ParentStation, opt => opt.Ignore());
             

            CreateMap<PlaCoordenadas, CoordenadasDto>()
                   .ForMember(dest => dest.DescripcionCalle1, opt => opt.MapFrom(src => src.DescripcionCalle1.TrimOrNull()))
                   .ForMember(dest => dest.DescripcionCalle2, opt => opt.MapFrom(src => src.DescripcionCalle2.TrimOrNull()));

            CreateMap<CoordenadasDto, PlaCoordenadas>();
            CreateMap<BolSectoresTarifarios, BolSectoresTarifariosDto>();
            CreateMap<BolSectoresTarifariosDto, BolSectoresTarifarios>()
                .ForMember(a => a.HSectores, options => { options.Ignore(); })
                .ForMember(a => a.PlaPuntos, options => { options.Ignore(); });

            CreateMap<HTposHoras, HTposHorasDto>()
                .ForMember(dest => dest.DscTpoHora, opt => opt.MapFrom(src => src.DscTpoHora.Trim()));
            CreateMap<HTposHorasDto, HTposHoras>()
                .ForMember(dest => dest.DscTpoHora, opt => opt.MapFrom(src => src.DscTpoHora.Trim()));
                       

            CreateMap<CroTipoElemento, CroTipoElementoDto>()
            .ForMember(dest => dest.CroElemeneto, opt => opt.MapFrom(src => src.CroElemeneto));
            CreateMap<CroTipoElementoDto, CroTipoElemento>()
            .ForMember(dest => dest.CroElemeneto, opt => opt.MapFrom(src => src.CroElemeneto));

            CreateMap<PlaLineaLineaHoraria, PlaLineaLineaHorariaDto>()
                .ForMember(dest => dest.DescripcionPlaLinea, opt => opt.MapFrom(src => src.PlaLinea.DesLin))
                .ForMember(dest => dest.DescripcionLinea, opt => opt.MapFrom(src => src.Linea.DesLin))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<PlaLineaLineaHorariaDto, PlaLineaLineaHoraria>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<CroElemeneto, CroElemenetoDto>()
              .ForMember(dest => dest.DescripcionTipoElemento, opt => opt.MapFrom(src => src.TipoElemento.Nombre));
            CreateMap<CroElemenetoDto, CroElemeneto>();

            CreateMap<CroTipo, CroTipoDto>();
            CreateMap<CroTipoDto, CroTipo>();

            CreateMap<SinSiniestros, SiniestrosDto>();
            CreateMap<SiniestrosDto, SinSiniestros>();

            CreateMap<SinSubCausas, SubCausasDto>();
            CreateMap<SubCausasDto, SinSubCausas>();

            CreateMap<Sucursales, sucursalesDto>();
            CreateMap<sucursalesDto, Sucursales>();

            CreateMap<CroCroquis, CroCroquisDto>();
            CreateMap<CroCroquisDto, CroCroquis>();

            CreateMap<BolBanderasCartel, BolBanderasCartelDto>();
            CreateMap<BolBanderasCartelDto, BolBanderasCartel>();

            CreateMap<BolBanderasCartelDetalle, BolBanderasCartelDetalleDto>()
                 .ForMember(dest => dest.AbrBan, opt => opt.MapFrom(src => src.CodBanNavigation.AbrBan))
                  .ForMember(dest => dest.ObsBandera, opt => opt.MapFrom(src => src.ObsBandera.Trim()))
                  .ForMember(dest => dest.TextoBandera, opt => opt.MapFrom(src => src.TextoBandera.TrimEnd()))
                .EqualityComparison((odto, o) => odto.CodBanderaCartel == o.CodBanderaCartel && odto.CodBan == o.CodBan);
            CreateMap<BolBanderasCartelDetalleDto, BolBanderasCartelDetalle>().EqualityComparison((odto, o) => odto.CodBanderaCartel == o.CodBanderaCartel && odto.CodBan == o.CodBan);


            CreateMap<LocalidadesDto, Localidades>();
            CreateMap<Localidades, LocalidadesDto>()
            .ForMember(dest => dest.ProvinciaName, opt => opt.MapFrom(src => src.Provincia.DscProvincia));


            CreateMap<ProvinciasDto, Provincias>();
            CreateMap<Provincias, ProvinciasDto>();

            CreateMap<PlaLinea, PlaLineaDto>()
                .ForMember(dest => dest.SucursalId, opt => opt.MapFrom(src => src.SucursalId))
                .ForMember(dest => dest.TipoLinea, opt => opt.MapFrom(src => src.PlaTipoLinea.Nombre))
               .ForMember(dest => dest.DesLin, opt => opt.MapFrom(src => src.DesLin.Trim()));
            CreateMap<PlaLineaDto, PlaLinea>();

            CreateMap<UnidadesNegocio, UnidadesNegocioDto>();
            CreateMap<UnidadesNegocioDto, UnidadesNegocio>();

            CreateMap<InspGruposInspectores, InspGruposInspectoresDto>()
                .ForMember(dest => dest.InspGrupoInspectoresZona, opt => opt.MapFrom(src => src.InspGrupoInspectoresZona))
                .ForMember(dest => dest.InspGrupoInspectoresRangosHorarios, opt => opt.MapFrom(src => src.InspGrupoInspectoresRangosHorarios))
                .ForMember(dest => dest.InspGrupoInspectoresTareas, opt => opt.MapFrom(src => src.InspGrupoInspectoresTareas))
                .ForMember(dest => dest.InspGrupoInspectoresTurnos, opt => opt.MapFrom(src => src.InspGruposInspectoresTurnos))
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspGruposInspectoresDto, InspGruposInspectores>()
                .ForMember(dest => dest.InspGrupoInspectoresZona, opt => opt.MapFrom(src => src.InspGrupoInspectoresZona))
                .ForMember(dest => dest.InspGrupoInspectoresRangosHorarios, opt => opt.MapFrom(src => src.InspGrupoInspectoresRangosHorarios))
                .ForMember(dest => dest.InspGrupoInspectoresTareas, opt => opt.MapFrom(src => src.InspGrupoInspectoresTareas))
                .ForMember(dest => dest.InspGruposInspectoresTurnos, opt => opt.MapFrom(src => src.InspGrupoInspectoresTurnos))
                .ForMember(dest => dest.Linea, opt => opt.Ignore())

                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspTopesFrancosGruposInspectores, InspTopesFrancosGruposInspectoresDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspTopesFrancosGruposInspectoresDto, InspTopesFrancosGruposInspectores>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspTopesGruposInspectores, InspTopesGruposInspectoresDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspTopesGruposInspectoresDto, InspTopesGruposInspectores>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspZonas, InspZonasDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspZonasDto, InspZonas>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspGrupoInspectoresRangosHorarios, InspGrupoInspectoresRangosHorariosDto>()
                .ForMember(d => d.NombreRangoHorario, o => o.MapFrom(s => s.RangoHorario.Descripcion))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspGrupoInspectoresRangosHorariosDto, InspGrupoInspectoresRangosHorarios>()
                 .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspGrupoInspectoresZona, InspGrupoInspectoresZonasDto>()
                .ForMember(d => d.ZonaNombre, o => o.MapFrom(s => s.Zona.Descripcion))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspGrupoInspectoresZonasDto, InspGrupoInspectoresZona>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspGrupoInspectoresTarea, InspGrupoInspectoresTareaDto>()
                 .ForMember(d => d.TareaNombre, o => o.MapFrom(s => s.Tarea.Descripcion))
                 .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspGrupoInspectoresTareaDto, InspGrupoInspectoresTarea>()
                 .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspGruposInspectoresTurnos, InspGruposInspectoresTurnoDto>()
                .ForMember(d => d.TurnoNombre, o => o.MapFrom(s => s.Turno.DscTurno))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspGruposInspectoresTurnoDto, InspGruposInspectoresTurnos>()
                 .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<Novedades, NovedadesDto>();
            CreateMap<NovedadesDto, Novedades>();

            CreateMap<PersTurnos, PersTurnosDto>();
            CreateMap<PersTurnosDto, PersTurnos>();

            CreateMap<InspRangosHorarios, InspRangosHorarioDto>()
                .ForMember(d => d.HoraDesde, o => o.MapFrom(s => new HoraDto(s.HoraDesde)))
                .ForMember(d => d.HoraHasta, o => o.MapFrom(s => new HoraDto(s.HoraHasta)))
                .EqualityComparison((odto, o) => odto.Id == o.Id); 

            CreateMap<InspRangosHorarioDto, InspRangosHorarios>()
                .ForMember(d => d.HoraDesde, o => o.MapFrom(s => s.HoraDesde.getFechaCompleta()))
                .ForMember(d => d.HoraHasta, o => o.MapFrom(s => s.HoraHasta.getFechaCompleta()))
                .EqualityComparison((odto, o) => odto.Id == o.Id); 

            CreateMap<InspTiposTarea, InspTiposTareaDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspTiposTareaDto, InspTiposTarea>().EqualityComparison((odto, o) => odto.Id == o.Id);


            CreateMap<InspDiagramasInspectoresTurnos, InspDiagramasInspectoresTurnosDto>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspDiagramasInspectoresTurnosDto, InspDiagramasInspectoresTurnos>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);


            CreateMap<InspTareasRealizadas, InspTareasRealizadasDto>()
                .ForMember(d => d.TareasRealizadasDetalle, o => o.MapFrom(s => s.InspTareasRealizadasDetalle))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspTareasRealizadasDto, InspTareasRealizadas>()
                .ForMember(d => d.InspTareasRealizadasDetalle, o => o.MapFrom(s => s.TareasRealizadasDetalle))
                .EqualityComparison((odto, o) => odto.Id == o.Id);



            CreateMap<InspTareasRealizadasDetalle, InspTareasRealizadasDetalleDto>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspTareasRealizadasDetalleDto, InspTareasRealizadasDetalle>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);



            CreateMap<PersTurnos, PersTurnosDto>();
            CreateMap<PersTurnosDto, PersTurnos>();

            CreateMap<PersTopesHorasExtras, PersTopesHorasExtrasDto>();
            CreateMap<PersTopesHorasExtrasDto, PersTopesHorasExtras>();

            CreateMap<InspDiagramasInspectoresDto, InspDiagramasInspectores>()
                .ForMember(d => d.Anio, o => o.MapFrom(s => s.Anio))
                 .ForMember(d => d.GrupoInspectores, o => o.MapFrom(s => s.GrupoInspectores))
                 .ForMember(d => d.EstadoDiagrama, o => o.MapFrom(s => s.EstadoDiagrama))
                 .ForMember(d => d.InspDiagramaInspectoresTurnos, o => o.MapFrom(s => s.InspDiagramaInspectoresTurnos))
            .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspDiagramasInspectores, InspDiagramasInspectoresDto>()
                 .ForMember(d => d.Anio, o => o.MapFrom(s => s.Anio))
                 .ForMember(d => d.GrupoInspectores, o => o.MapFrom(s => s.GrupoInspectores))
                 .ForMember(d => d.EstadoDiagrama, o => o.MapFrom(s => s.EstadoDiagrama))
                 .ForMember(d => d.InspDiagramaInspectoresTurnos, o => o.MapFrom(s => s.InspDiagramaInspectoresTurnos))
                 .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<PersJornadasTrabajadasDto, PersJornadasTrabajadas>();

            CreateMap<PersJornadasTrabajadas, PersJornadasTrabajadasDto>();

            CreateMap<HFrancosDto, HFrancos>();

            CreateMap<HFrancos, HFrancosDto>();


            CreateMap<InspEstadosDiagramaInspectoresDto, InspEstadosDiagramaInspectores>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspEstadosDiagramaInspectores, InspEstadosDiagramaInspectoresDto>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspPlanillaIncognitos, InspPlanillaIncognitosDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspPlanillaIncognitosDto, InspPlanillaIncognitos>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspPlanillaIncognitosDetalle, InspPlanillaIncognitosDetalleDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspPlanillaIncognitosDetalleDto, InspPlanillaIncognitosDetalle>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspRespuestasIncognitos, InspRespuestasIncognitosDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspRespuestasIncognitosDto, InspRespuestasIncognitos>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspPreguntasIncognitosRespuestas, InspPreguntasIncognitosRespuestasDto>()
                .ForMember(d => d.RespuestaNombre, o => o.MapFrom(s => s.Respuesta.Descripcion))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspPreguntasIncognitosRespuestasDto, InspPreguntasIncognitosRespuestas>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspPreguntasIncognitos, InspPreguntasIncognitosDto>()
                .ForMember(dest => dest.InspPreguntasIncognitosRespuestas, opt => opt.MapFrom(src => src.InspPreguntasIncognitosRespuestas))
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspPreguntasIncognitosDto, InspPreguntasIncognitos>()
                .ForMember(dest => dest.InspPreguntasIncognitosRespuestas, opt => opt.MapFrom(src => src.InspPreguntasIncognitosRespuestas))
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspTarea, InspTareaDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspTareaDto, InspTarea>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<InspTareaCampo, InspTareaCampoDto>()
                .ForMember(dto => dto.NombreTareaCampo, e => e.MapFrom(entity => entity.TareaCampoConfig.Descripcion))
                .ForMember(dto => dto.Campo, e => e.MapFrom(entity => entity.TareaCampoConfig.Campo))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InspTareaCampoDto, InspTareaCampo>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<Error, LogDto>()
                .ForMember(d => d.Description, o => o.MapFrom(s => s.ErrorMessage))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.LogDate, o => o.MapFrom(s => s.ErrorDate))
                .ForMember(d => d.LogMessage, o => o.MapFrom(s => s.ErrorMessage))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.SessionId, o => o.MapFrom(s => s.SessionId))
                .ForMember(d => d.StackTrace, o => o.MapFrom(s => s.StackTrace))
                ;

            CreateMap<LogDto, Error>()
                .ForMember(d => d.ErrorDate, o => o.MapFrom(s => s.LogDate))
                .ForMember(d => d.ErrorMessage, o => o.MapFrom(s => s.LogMessage))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.Id))
                .ForMember(d => d.UserName, o => o.MapFrom(s => s.UserName))
                .ForMember(d => d.SessionId, o => o.MapFrom(s => s.SessionId))
                .ForMember(d => d.StackTrace, o => o.MapFrom(s => s.StackTrace));


            CreateMap<Logs, LogDto>();
            CreateMap<LogDto, Logs>();

            CreateMap<PlaGrupoLineas, GrupoLineasDto>()
                .ForMember(
                    dest => dest.Sucursal, opt => opt.MapFrom(src => src.Sucursal.DscSucursal)
                )
                .ForMember(
                    dest => dest.LineasTotales, opt => opt.MapFrom(src => src.Linea.Count)
                )
                .ForMember(
                    dest => dest.Lineas, opt => opt.MapFrom(src => src.Linea.Select(s => new ItemDecimalDto(s.Id, s.DesLin, false)))
                );

            CreateMap<GrupoLineasDto, PlaGrupoLineas>();

            CreateMap<InfResponsablesDto, InfResponsables>();
            CreateMap<InfResponsables, InfResponsablesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.TrimOrNull()))
                .ForMember(dest => dest.DscResponsable, opt => opt.MapFrom(src => src.DscResponsable.TrimOrNull()));

            CreateMap<TipoLineaDto, PlaTipoLinea>();
            CreateMap<PlaTipoLinea, TipoLineaDto>();

            CreateMap<HDesignarDto, HDesignar>();
            CreateMap<HDesignar, HDesignarDto>();

            CreateMap<InfInformesDto, InfInformes>();
            CreateMap<InfInformes, InfInformesDto>();

            CreateMap<InsDesviosDto, InsDesvios>();
            CreateMap<InsDesvios, InsDesviosDto>();

            CreateMap<MotivoInfraDto, MotivoInfra>();
            CreateMap<MotivoInfra, MotivoInfraDto>();

            CreateMap<InfResponsablesDto, InfResponsables>();
            CreateMap<InfResponsables, InfResponsablesDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id.TrimOrNull()))
                .ForMember(dest => dest.DscResponsable, opt => opt.MapFrom(src => src.DscResponsable.TrimOrNull()));

            CreateMap<EmpresaDto, Empresa>();
            CreateMap<Empresa, EmpresaDto>();

            CreateMap<TiposDeDiasDto, HTipodia>();
            CreateMap<HTipodia, TiposDeDiasDto>().ForMember(dest => dest.DesTdia, opt => opt.MapFrom(src => src.DesTdia.Trim()));


            CreateMap<HBanderas, BanderaDto>()
                .ForMember(dest => dest.RamalColorNombre, opt => opt.MapFrom(src => src.RamalColor.Nombre))
                .ForMember(dest => dest.LineaId, opt => opt.MapFrom(src => src.RamalColor.LineaId))
                .ForMember(dest => dest.LineaNombre, opt => opt.MapFrom(src => src.RamalColor.PlaLinea.DesLin))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.AbrBan.Trim()))
                .ForMember(dest => dest.Descripcion, opt => opt.MapFrom(src => src.DesBan.Trim()))
                .ForMember(dest => dest.Sentido, opt => opt.MapFrom(src => src.SentidoBandera.Descripcion))

                .ForMember(x => x.Rutas, opt => opt.Ignore());
            CreateMap<BanderaDto, HBanderas>()
                //.ForMember(x => x.Rutas, opt => opt.Ignore())
                .ForMember(dest => dest.AbrBan, opt => opt.MapFrom(src => src.Nombre))
                .ForMember(dest => dest.DesBan, opt => opt.MapFrom(src => src.Descripcion))
                .ForMember(dest => dest.SenBan, opt => opt.MapFrom(src => src.Sentido))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo))
                .ForMember(dest => dest.CodigoVarianteLinea, opt => opt.MapFrom(src => src.CodigoVarianteLinea))
                .ForMember(dest => dest.TipoBanderaId, opt => opt.MapFrom(src => src.TipoBanderaId))

                ;

            CreateMap<RamalColorDto, PlaRamalColor>();
            CreateMap<PlaRamalColor, RamalColorDto>()
                .ForMember(
                    dest => dest.NombreLinea, opt => opt.MapFrom(src => src.PlaLinea.DesLin)
                )
                //.ForMember(
                //    dest => dest.NombreUN, opt => opt.MapFrom(src => src.Linea.UnidadDeNegocio.Nombre)
                //) 
                ;

            //CreateMap<RamalSube, RamalSubeDto>()
            //   .ForMember(
            //       dest => dest.EmpresaNombre, opt => opt.MapFrom(src => src.Empresa.DesEmpr)
            //   ).EqualityComparison((odto, o) => odto.Id == o.Id);
            //CreateMap<RamalSubeDto, RamalSube>().EqualityComparison((odto, o) => odto.Id == o.Id);



            CreateMap<PlaTipoParada, TipoParadaDto>()
                .ForMember(dest => dest.TiempoEsperadoDeCarga, opt => opt.MapFrom(src => src.PlaTiempoEsperadoDeCarga));
            CreateMap<TipoParadaDto, PlaTipoParada>()
                .ForMember(dest => dest.PlaTiempoEsperadoDeCarga, opt => opt.MapFrom(src => src.TiempoEsperadoDeCarga));



            CreateMap<TiempoEsperadoDeCargaDto, PlaTiempoEsperadoDeCarga>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<PlaTiempoEsperadoDeCarga, TiempoEsperadoDeCargaDto>().ForMember(
                    dest => dest.TipoDiaNombre, opt => opt.MapFrom(src => src.TipodeDia.DesTdia)
                ).EqualityComparison((odto, o) => odto.Id == o.Id);





            CreateMap<GpsRecorridos, RutasDto>()
                .ForMember(dest => dest.EstadoRutaNombre, opt => opt.MapFrom(src => src.EstadoRuta.Nombre))
                .ForMember(dest => dest.TipoBanderaId, opt => opt.MapFrom(src => src.Bandera.TipoBanderaId))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo == "1"))
                .ForMember(dest => dest.BanderaId, opt => opt.MapFrom(src => src.CodBan))
                .ForMember(dest => dest.CodLin, opt => opt.MapFrom(src => src.CodLin))
                .ForMember(dest => dest.FechaVigenciaDesde, opt => opt.MapFrom(src => src.Fecha))
                .ForMember(dest => dest.BanderaNombre, opt => opt.MapFrom(src => src.Bandera.DesBan))
                .ForMember(dest => dest.AbrBan, opt => opt.MapFrom(src => src.Bandera.AbrBan))
                .ForMember(dest => dest.CodigoVarianteLinea, opt => opt.MapFrom(src => src.Bandera.CodigoVarianteLinea))
                ;


            CreateMap<RutasDto, GpsRecorridos>()
                .ForMember(dest => dest.CodBan, opt => opt.MapFrom(src => src.BanderaId))
                .ForMember(dest => dest.CodLin, opt => opt.MapFrom(src => src.CodLin))
                .ForMember(dest => dest.Fecha, opt => opt.MapFrom(src => src.FechaVigenciaDesde))
                .ForMember(dest => dest.Activo, opt => opt.MapFrom(src => src.Activo ? "1" : "0"));


            CreateMap<PlaEstadoRuta, EstadoRutaDto>();
            CreateMap<EstadoRutaDto, PlaEstadoRuta>();



            CreateMap<PlaPuntos, PuntosDto>()
                      .ForMember(dest => dest.RutaId, opt => opt.MapFrom(src => src.CodRec))
                      .ForMember(dest => dest.PlaCoordenadaItem, opt => opt.MapFrom(src => src.PlaCoordenada != null ? CoordenadasFilter.GetItmDTOFunc()(src.PlaCoordenada) : null))
                      .ForMember(dest => dest.PlaCoordenadaAnulada, opt => opt.MapFrom(src => src.PlaCoordenada.Anulado))
                      .ForMember(dest => dest.PlaCoordenadaCalle1, opt => opt.MapFrom(src => src.PlaCoordenada.DescripcionCalle1.Trim()))
                      .ForMember(dest => dest.PlaCoordenadaCalle2, opt => opt.MapFrom(src => src.PlaCoordenada.DescripcionCalle2.Trim()))
                      .ForMember(dest => dest.PlaParadaItem, opt => opt.MapFrom(src => src.PlaParada != null ? new ItemDto<int>(src.PlaParada.Id, src.PlaParada.GetDescription()) : null))
                      .ForMember(dest => dest.PlaParadaCruceCalle, opt => opt.MapFrom(src => src.PlaParada != null ? string.Format("{0} y {1}", src.PlaParada.Calle.TrimOrNull(), src.PlaParada.Cruce.TrimOrNull()) : null))
                      .ForMember(dest => dest.SectoresTarifariosItem, opt => opt.MapFrom(src => src.BolSectoresTarifarios != null ? BolSectoresTarifariosFilter.GetItmDTOFunc()(src.BolSectoresTarifarios) : null))
                      .ForMember(dest => dest.NumeroExterno, opt => opt.MapFrom(src => src.PlaCoordenada.NumeroExternoIVU))
                      .ForMember(dest => dest.PickUpType, opt => opt.MapFrom(src => src.PlaParada.PickUpType))
                      .ForMember(dest => dest.DropOffType, opt => opt.MapFrom(src => src.PlaParada.DropOffType))
                      .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<PuntosDto, PlaPuntos>()
                 .ForMember(dest => dest.CodRec, opt => opt.MapFrom(src => src.RutaId))
                 .ForMember(x => x.PlaCoordenada, opt => opt.Ignore())
                 .ForMember(x => x.PlaParada, opt => opt.Ignore())
                 .ForMember(x => x.BolSectoresTarifarios, opt => opt.Ignore())
                 .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<PlaSector, SectorDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<SectorDto, PlaSector>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<Galpon, GalponDto>()
                .ForMember(dest => dest.Lat, opt => opt.MapFrom(src => src.Latitud * -1))
                .ForMember(dest => dest.Long, opt => opt.MapFrom(src => src.Longitud * -1))
                .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.DesGal))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<GalponDto, Galpon>()
                .ForMember(dest => dest.Latitud, opt => opt.MapFrom(src => src.Lat * -1))
                .ForMember(dest => dest.Longitud, opt => opt.MapFrom(src => src.Long * -1))
                .ForMember(dest => dest.DesGal, opt => opt.MapFrom(src => src.Nombre))
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<PlaTalleresIvu, PlaTalleresIvuDto>();
            CreateMap<PlaTalleresIvuDto, PlaTalleresIvu>();


            CreateMap<HBanderasTup, HBanderasTupDto>();
            CreateMap<HBanderasTupDto, HBanderasTup>();


            CreateMap<PlaSentidoBandera, PlaSentidoBanderaDto>();
            CreateMap<PlaSentidoBanderaDto, PlaSentidoBandera>();

            CreateMap<Empleados, EmpleadosDto>()
                .ForMember(dest => dest.cod_sucursal, opt => opt.MapFrom(src => src.UnidadNegocio.cod_sucursal));
            CreateMap<EmpleadosDto, Empleados>();

            CreateMap<HFechasDto, HFechas>();
            CreateMap<HFechas, HFechasDto>();

            CreateMap<DshDashboard, DshDashboardDto>();
            CreateMap<DshDashboardDto, DshDashboard>();


            //CreateMap<PlaHorarioFecha, PlaHorarioFechaDto>()
            //    .ForMember(dest => dest.Linea, opt => opt.MapFrom(src => new ItemDto<Decimal>(src.CodLinea, src.CodLineaNavigation.DesLin)));
            //CreateMap<PlaHorarioFechaDto, PlaHorarioFecha>();


            //CreateMap<PlaHorarioFecha, PlaHorarioFechaDto>()
            //    .ForMember(dest => dest.Linea, opt => opt.MapFrom(src => new ItemDto<Decimal>(src.CodLinea, src.CodLineaNavigation.DesLin)))
            //    .ForMember(dest => dest.DescripcionEstado, opt => opt.MapFrom(src => src.EstadoHorarioFecha.Descripcion))
            //    .ForMember(dest => dest.DescripcionLinea, opt => opt.MapFrom(src => src.CodLineaNavigation.DesLin))

            //    ;
            //CreateMap<PlaHorarioFechaDto, PlaHorarioFecha>();


            CreateMap<PlaEstadoHorarioFecha, PlaEstadoHorarioFechaDto>();
            CreateMap<PlaEstadoHorarioFechaDto, PlaEstadoHorarioFecha>();

            CreateMap<UsuarioDashboardItemDto, DshUsuarioDashboardItem>();
            CreateMap<DshUsuarioDashboardItem, UsuarioDashboardItemDto>()
                .ForMember(dest => dest.TipoDashboardId, opt => opt.MapFrom(src => src.Dashboard.TipoDashboardId));


            CreateMap<PlaCodigoSubeBandera, PlaCodigoSubeBanderaDto>()
                .ForMember(dest => dest.EmpresaNombre, opt => opt.MapFrom(src => src.CodEmpresaNavigation.DesEmpr))
                .ForMember(dest => dest.SentidoBanderaSubeNombre, opt => opt.MapFrom(src => src.PlaSentidoBanderaSubeNavigation.Descripcion))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<PlaCodigoSubeBanderaDto, PlaCodigoSubeBandera>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<PlaSentidoBanderaSube, SentidoBanderaSubeDto>()
               .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<PlaCodigoSubeBanderaDto, PlaCodigoSubeBandera>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<PlaDistribucionDeCochesPorTipoDeDia, PlaDistribucionDeCochesPorTipoDeDiaDto>()
                .ForMember(dest => dest.TipoDediaDescripcion, opt => opt.MapFrom(src => src.CodTdiaNavigation.DesTdia))

                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<PlaDistribucionDeCochesPorTipoDeDiaDto, PlaDistribucionDeCochesPorTipoDeDia>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);



            CreateMap<HFechasConfiDto, HFechasConfi>()
                .ForMember(dest => dest.FecDesde, opt => opt.MapFrom(src => src.FechaDesde))
                .ForMember(dest => dest.FecHasta, opt => opt.MapFrom(src => src.FechaHasta))
                .ForMember(dest => dest.CodLin, opt => opt.MapFrom(src => src.CodLinea))
                //.ForMember(dest => dest.PlaEstadoHorarioFechaId, opt => opt.MapFrom(src => src.EstadoHorarioId))
                .ForMember(dest => dest.PlaDistribucionDeCochesPorTipoDeDia, opt => opt.MapFrom(src => src.DistribucionDeCochesPorTipoDeDia))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<HFechasConfi, HFechasConfiDto>()
                .ForMember(dest => dest.FechaDesde, opt => opt.MapFrom(src => src.FecDesde))
                .ForMember(dest => dest.FechaHasta, opt => opt.MapFrom(src => src.FecHasta))
                .ForMember(dest => dest.DescripcionLinea, opt => opt.MapFrom(src => src.CodLinNavigation.DesLin))
                .ForMember(dest => dest.DescripcionEstado, opt => opt.MapFrom(src => src.PlaEstadoHorarioFecha.Descripcion))
                .ForMember(dest => dest.CodLinea, opt => opt.MapFrom(src => src.CodLin))
                .ForMember(dest => dest.Linea, opt => opt.MapFrom(src => new ItemDto<decimal>(src.CodLin, src.CodLinNavigation.DesLin)))
                .ForMember(dest => dest.DistribucionDeCochesPorTipoDeDia, opt => opt.MapFrom(src => src.PlaDistribucionDeCochesPorTipoDeDia))
                //.ForMember(dest => dest.EstadoHorarioId, opt => opt.MapFrom(src => src.PlaEstadoHorarioFechaId))
                .ForMember(dest => dest.TiposDeDias, opt => opt.MapFrom(src => string.Join(',', src.HHorariosConfi.Select(hc => hc.CodTdiaNavigation.DesTdia).Distinct())))
                .EqualityComparison((odto, o) => odto.Id == o.Id);




            CreateMap<HHorariosConfiDto, HHorariosConfi>()
           .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<HHorariosConfi, HHorariosConfiDto>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<HServiciosDto, HServicios>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<HServicios, HServiciosDto>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);


            CreateMap<HMediasVueltas, HMediasVueltasDto>()
                .ForMember(dest => dest.DescripcionBandera, opt => opt.MapFrom(src => src.CodBanNavigation.AbrBan))
                .ForMember(dest => dest.DescripcionTpoHora, opt => opt.MapFrom(src => src.CodTpoHoraNavigation.DscTpoHora))
                .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<HMediasVueltasDto, HMediasVueltas>()
                .EqualityComparison((odto, o) => odto.Id == o.Id);


            CreateMap<HMinxtipoDto, HMinxtipo>()
            .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<HMinxtipo, HMinxtipoDto>()
                .EqualityComparison((odto, o) => odto.Id == o.Id)
                .ForMember(dest => dest.TipoHoraDesc, opt => opt.MapFrom(src => src.TipoHoraNavigation.DscTpoHora));


            CreateMap<HDetaminxtipo, HDetaminxtipoDto>()
                .EqualityComparison((odto, o) => odto.CodHsector == o.CodHsector && odto.CodMinxtipo == o.CodMinxtipo);

            CreateMap<HDetaminxtipoDto, HDetaminxtipo>()
                .EqualityComparison((odto, o) => odto.CodHsector == o.CodHsector && odto.CodMinxtipo == o.CodMinxtipo);


            CreateMap<HSectores, HSectoresDto>()
                 .ForMember(dest => dest.Calle1, opt => opt.MapFrom(src => src.CodHsectorNavigation.Calle1))
                  .ForMember(dest => dest.Calle2, opt => opt.MapFrom(src => src.CodHsectorNavigation.Calle2)) 
                  .ForMember(dest => dest.VerEnResumen, opt =>  opt.MapFrom(src => MapVerEnResumen(src.VerEnResumen)))



                  .ForMember(dest => dest.DescripcionSectorTarifario, opt => opt.MapFrom(src => src.CodSectorTarifarioNavigation.DscSectorTarifario))
               .EqualityComparison((odto, o) => odto.CodHsector == o.CodHsector && odto.CodSec == o.CodSec);


            CreateMap<HSectoresDto, HSectores>()
                    .ForMember(dest => dest.VerEnResumen, opt => opt.MapFrom(src => MapVerEnResumen(src.VerEnResumen)));


 

            CreateMap<HBasec, HBasecDto>()
                .ForMember(dest => dest.DescripcionAbreviatura, opt => opt.MapFrom(src => src.CodBanNavigation.AbrBan))
                   .ForMember(dest => dest.Recorido, opt => opt.MapFrom(src => src.CodRecNavigation != null ?
                  string.Format("{0} FD:{1}", src.CodRecNavigation.Nombre , src.CodRecNavigation.Fecha.ToStringDefaultFormat())
                   : string.Empty))
                .ForMember(dest => dest.DescripcionBandera, opt => opt.MapFrom(src => src.CodBanNavigation.DesBan))
                .ForMember(dest => dest.DescripcionSentido, opt => opt.MapFrom(src => src.CodBanNavigation.SentidoBandera.Descripcion))
                .EqualityComparison((odto, o) => odto.CodHfecha == o.CodHfecha && odto.CodBan == o.CodBan);

            CreateMap<HBasecDto, HBasec>().EqualityComparison((odto, o) => odto.CodHfecha == o.CodHfecha && odto.CodBan == o.CodBan); ;



            CreateMap<Configu, ConfiguDto>()
                 .ForMember(dest => dest.GrupoGrilla, opt => opt.MapFrom(src => src.Grupo.DesGru))
                 .ForMember(dest => dest.EmpresaGrilla, opt => opt.MapFrom(src => src.Empresa.DesEmpr))
                 .ForMember(dest => dest.SucursalGrilla, opt => opt.MapFrom(src => src.Sucursal.DscSucursal))
                 .ForMember(dest => dest.GalponGrilla, opt => opt.MapFrom(src => src.Galpon.DesGal));
            CreateMap<ConfiguDto, Configu>();

            CreateMap<SubGalpon, SubGalponDto>()
            .ForMember(dest => dest.Configu, opt => opt.MapFrom(src => src.Configu));
            CreateMap<SubGalponDto, SubGalpon>()
            .ForMember(dest => dest.Configu, opt => opt.MapFrom(src => src.Configu));


            CreateMap<PlanCam, PlanCamDto>();
            CreateMap<PlanCamDto, PlanCam>();

            CreateMap<Grupos, GruposDto>();
            CreateMap<GruposDto, Grupos>();

        }

        private string MapVerEnResumen(bool verEnResumen)
        {
            return verEnResumen ? "S" : "N";
        }

        private Boolean MapVerEnResumen(string verEnResumen)
        {
            return !string.IsNullOrEmpty(verEnResumen) && (verEnResumen == "S" || verEnResumen == "1");
        }

        private object GetTpoNroDocDto(UserDto src)
        {
            if (string.IsNullOrWhiteSpace(src.TpoDoc))
            {
                return src.NroDoc;
            }
            return string.Format("({0}) {1}", src.TpoDoc.Trim(), src.NroDoc.Trim());
        }

        private static string GetTpoNroDoc(SysUsersAd src)
        {
            return string.IsNullOrEmpty(src.TpoNroDoc) ? FormatTpoNroDoc(src) : src.TpoNroDoc;
        }

        private static string FormatTpoNroDoc(SysUsersAd src)
        {
            if (string.IsNullOrWhiteSpace(src.TpoDoc))
            {
                return src.NroDoc;
            }
            return string.Format("({0}) {1}", src.TpoDoc.Trim(), src.NroDoc.Trim());
        }

        

    } 
   

}
