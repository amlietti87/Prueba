using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.ART.AppService.Model;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Extensions;

namespace ROSBUS.WebService.Siniestros
{
    public class MappingProfile : Profile
    {
        //  where TModel : Entity<TPrimaryKey>, new()
        //where TDto : EntityDto<TPrimaryKey>, new()
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects 
            CreateMap<SinAbogados, AbogadosDto>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email.ToLower()));

            CreateMap<AbogadosDto, SinAbogados>();

            CreateMap<Adjuntos, AdjuntosDto>();
            CreateMap<AdjuntosDto, Adjuntos>();


            CreateMap<SinBandaSiniestral, BandaSiniestralDto>();
            CreateMap<BandaSiniestralDto, SinBandaSiniestral>();

            CreateMap<SinCategorias, CategoriasDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<CategoriasDto, SinCategorias>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<CategoriasDto, SinCategorias>()
             .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<SinCategorias, CategoriasDto>()
             .EqualityComparison((odto, o) => odto.Id == o.Id);


            CreateMap<SinSubCausas, SubCausasDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<SubCausasDto, SinSubCausas>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<SinCausas, CausasDto>()
            .ForMember(dest => dest.SubCausas, opt => opt.MapFrom(src => src.SinSubCausas));
            CreateMap<CausasDto, SinCausas>()
            .ForMember(dest => dest.SinSubCausas, opt => opt.MapFrom(src => src.SubCausas));




            CreateMap<SubEstadosDto, SinSubEstados>()
             .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<SinSubEstados, SubEstadosDto>()
                .ForMember(dest => dest.EstadoNombre, opt => opt.MapFrom(src => src.Estado.Descripcion))
             .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<SinEstados, EstadosDto>()
            .ForMember(dest => dest.SubEstados, opt => opt.MapFrom(src => src.SinSubEstados));
            CreateMap<EstadosDto, SinEstados>()
            .ForMember(dest => dest.SinSubEstados, opt => opt.MapFrom(src => src.SubEstados));


            CreateMap<LocalidadesDto, Localidades>();
            CreateMap<Localidades, LocalidadesDto>()
            .ForMember(dest => dest.ProvinciaName, opt => opt.MapFrom(src => src.Provincia.DscProvincia));

            CreateMap<Provincias, ProvinciasDto>();
            CreateMap<ProvinciasDto, Provincias>();


            CreateMap<SinCiaSeguros, CiaSegurosDto>();
            CreateMap<CiaSegurosDto, SinCiaSeguros>();


            CreateMap<SinTipoColision, TipoColisionDto>();
            CreateMap<TipoColisionDto, SinTipoColision>();

            CreateMap<SinConductasNormas, ConductasNormasDto>();
            CreateMap<ConductasNormasDto, SinConductasNormas>();

            //CreateMap<CCoches, ConductoresDto>();
            //CreateMap<ConductoresDto, CCoches>();
            CreateMap<CCoches, CCochesDto>()
            .ForMember(dest => dest.Dominio, opt => opt.MapFrom(src => src.Dominio.Replace(" ", String.Empty)));
            CreateMap<CCochesDto, CCoches>();

            CreateMap<SinConsecuencias, ConsecuenciasDto>()
            .ForMember(dest => dest.Categorias, opt => opt.MapFrom(src => src.SinCategorias));
            CreateMap<ConsecuenciasDto, SinConsecuencias>()
            .ForMember(dest => dest.SinCategorias, opt => opt.MapFrom(src => src.Categorias));




            CreateMap<SinDetalleLesion, DetalleLesionDto>().EqualityComparison((odto, o) => odto.Id == o.Id); ;
            CreateMap<DetalleLesionDto, SinDetalleLesion>().EqualityComparison((odto, o) => odto.Id == o.Id); ;


            CreateMap<SinFactoresIntervinientes, FactoresIntervinientesDto>();
            CreateMap<FactoresIntervinientesDto, SinFactoresIntervinientes>();

            CreateMap<TiposReclamo, TiposReclamoDto>();
            CreateMap<TiposReclamoDto, TiposReclamo>();

            CreateMap<SinSancionSugerida, SancionSugeridaDto>();
            CreateMap<SancionSugeridaDto, SinSancionSugerida>();

            CreateMap<SinInvolucrados, InvolucradosDto>()
                    .ForMember(dest => dest.DetalleLesion, opt => opt.MapFrom(src => src.SinDetalleLesion))
                     .ForMember(dest => dest.DescripcionInv, opt => opt.MapFrom(src => src.getDescription()))
                  .ForMember(dest => dest.TipoInvolucradoNombre, opt => opt.MapFrom(src => src.TipoInvolucrado.Descripcion))
                  .ForMember(dest => dest.EstadoInsercion, opt => opt.MapFrom(src => src.GetEstadoInsercion()))
                  .ForMember(dest => dest.TipoDocNombre, opt => opt.MapFrom(src => src.TipoDoc.Descripcion))
                  .ForMember(dest => dest.VehiculoNombre, opt => opt.MapFrom(src => src.Vehiculo != null ? src.Vehiculo.GetDescription() : string.Empty))
                  .ForMember(dest => dest.ConductorNombre, opt => opt.MapFrom(src => src.Conductor != null ? src.Conductor.GetDescription() : string.Empty))
                  .ForMember(dest => dest.MuebleInmuebleLocalidadID, opt => opt.MapFrom(src => src.MuebleInmueble != null ? src.MuebleInmueble.LocalidadId : null))
                  .ForMember(dest => dest.MuebleInmuebleNombre, opt => opt.MapFrom(src => src.MuebleInmueble != null ? src.MuebleInmueble.TipoInmueble.Descripcion + " - " + src.MuebleInmueble.Lugar : null))
                  .ForMember(dest => dest.LesionadoNombre, opt => opt.MapFrom(src => src.Lesionado != null ? src.Lesionado.GetDescription() : string.Empty))
                  .ForMember(dest => dest.TieneConductor, opt => opt.MapFrom(src => src.TipoInvolucrado.Conductor))
                  .ForMember(dest => dest.TieneLesionado, opt => opt.MapFrom(src => src.TipoInvolucrado.Lesionado))
                  .ForMember(dest => dest.TieneMuebleInmueble, opt => opt.MapFrom(src => src.TipoInvolucrado.MuebleInmueble))
                  .ForMember(dest => dest.TieneVehiculo, opt => opt.MapFrom(src => src.TipoInvolucrado.Vehiculo))

                  .EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<InvolucradosDto, SinInvolucrados>()
                .ForMember(dest => dest.SinDetalleLesion, opt => opt.MapFrom(src => src.DetalleLesion))
                //.ForMember(dest => dest.SinInvolucradosAdjuntos, opt => opt.MapFrom(src => src.adj))
                //.ForMember(dest => dest.SinReclamos, opt => opt.Ignore())

                .EqualityComparison((odto, o) => odto.Id == o.Id);


            CreateMap<SinConductores, ConductorDto>();
            CreateMap<ConductorDto, SinConductores>();



            CreateMap<SinVehiculos, VehiculosDto>();
            CreateMap<VehiculosDto, SinVehiculos>();

            CreateMap<SinInvolucradosAdjuntos, InvolucradosAdjuntosDto>();
            CreateMap<InvolucradosAdjuntosDto, SinInvolucradosAdjuntos>();

            CreateMap<SinJuzgados, JuzgadosDto>();
            CreateMap<JuzgadosDto, SinJuzgados>();

            CreateMap<SinLesionados, LesionadosDto>()
                .ForMember(dest => dest.TipoLesionadoDescripcion, opt => opt.MapFrom(src => src.TipoLesionado.Descripcion));

            CreateMap<LesionadosDto, SinLesionados>();

            CreateMap<SinMuebleInmueble, MuebleInmuebleDto>();
            CreateMap<MuebleInmuebleDto, SinMuebleInmueble>();

            CreateMap<SinPracticantes, PracticantesDto>()
            .ForMember(dest => dest.ApellidoNombre, opt => opt.MapFrom(src => src.ApellidoNombre.Trim()));
            CreateMap<PracticantesDto, SinPracticantes>();

            CreateMap<SinReclamoAdjuntos, ReclamoAdjuntosDto>();
            CreateMap<ReclamoAdjuntosDto, SinReclamoAdjuntos>();

            CreateMap<SinReclamoCuotas, ReclamoCuotasDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<ReclamoCuotasDto, SinReclamoCuotas>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<SinReclamos, ReclamosDto>()
            .ForMember(dest => dest.ReclamoCuotas, opt => opt.MapFrom(src => src.ReclamoCuotas))
            .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedUser.DisplayName))
            .ForMember(dest => dest.JudicialSelected, opt => opt.MapFrom(src => src.Estado.Judicial))
            .ForMember(dest => dest.ReclamoHistorico, opt => opt.MapFrom(src => src.GetReclamoHistorico()))
            .ForMember(dest => dest.selectedSiniestro, opt => opt.MapFrom(src => (src.SiniestroId.HasValue) ? (new ItemDto<int> { Id = src.SiniestroId.Value, Description = src.Siniestro.getDescription(), IsSelected = true }) : null))
            //.ForMember(dest => dest.selectedDenuncia, opt => opt.MapFrom(src => (src.DenunciaId.HasValue) ? (new ItemDto<int> { Id = src.DenunciaId.Value, Description = src.Denuncia.getDescription(), IsSelected = true }) : null))
            ;
            CreateMap<ReclamosDto, SinReclamos>()
            .ForMember(dest => dest.ReclamoCuotas, opt => opt.MapFrom(src => src.ReclamoCuotas))
            .ForMember(dest => dest.SubEstado, opt => opt.Ignore())
            .ForMember(dest => dest.Estado, opt => opt.Ignore())
            .ForMember(dest => dest.Involucrado, opt => opt.Ignore())
            .ForMember(dest => dest.Abogado, opt => opt.Ignore());

            CreateMap<SinReclamosHistoricos, ReclamosHistoricosDto>()
                .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedUser.DisplayName))
                                            .ForMember(dest => dest.JudicialSelected, opt => opt.MapFrom(src => src.Estado.Judicial))
                .ForMember(dest => dest.selectedSiniestro, opt => opt.MapFrom(src => (src.SiniestroId.HasValue) ? (new ItemDto<int> { Id = src.SiniestroId.Value, Description = src.Siniestro.getDescription(), IsSelected = true }) : null))
            .ForMember(dest => dest.selectedDenuncia, opt => opt.MapFrom(src => (src.DenunciaId.HasValue) ? (new ItemDto<int> { Id = src.DenunciaId.Value, Description = src.Denuncia.getDescription(), IsSelected = true }) : null));
            CreateMap<ReclamosHistoricosDto, SinReclamosHistoricos>();

            CreateMap<SinSiniestroAdjuntos, SiniestroAdjuntosDto>();
            CreateMap<SiniestroAdjuntosDto, SinSiniestroAdjuntos>();

            CreateMap<SinSiniestros, SiniestrosDto>()
                 .ForMember(dest => dest.SiniestrosConsecuencias, opt => opt.MapFrom(src => src.SinSiniestrosConsecuencias))
                        .ForMember(dest => dest.Lugar, opt => opt.MapFrom(src => src.Lugar.Trim()))
                        .ForMember(dest => dest.DescCombo, opt => opt.MapFrom(src => src.getDescription()))
                        .ForMember(dest => dest.DescripcionSucursal, opt => opt.MapFrom(src => src.Sucursal.DscSucursal.Trim()))
                        .ForMember(dest => dest.DescripcionLinea, opt => opt.MapFrom(src => src.CocheLinea.DesLin.Trim()))
                        .ForMember(dest => dest.selectCoches, opt => opt.MapFrom(src => new ItemDto<String>() { Id = src.CocheId, Description = src.CocheFicha.ToString().Trim() }))
                        .ForMember(dest => dest.Latitud, opt => opt.MapFrom(src => (src.Latitud * -1)))
                        .ForMember(dest => dest.Longitud, opt => opt.MapFrom(src => (src.Longitud * -1)))
                        .ForMember(dest => dest.selectPracticantes, opt => opt.MapFrom(src => new ItemDto<int>() { Id = src.Practicante.Id, Description = src.Practicante.ApellidoNombre }))
                        .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedUser.DisplayName));

            //.ForMember(dest => dest.Involucrados, opt => opt.MapFrom(src => src.SinInvolucrados))

            //.ForMember(dest => dest.GrillaConductor,  opt => opt.MapFrom(src => new GrillaConductor() {
            //    NombreConductor = src.ConductorId.HasValue ? (src.Empleado.Apellido + ' ' + src.Empleado.Nombre) : src.Practicante.ApellidoNombre,
            //    Legajo = src.ConductorId.HasValue ? (src.ConductorLegajo) : string.Empty,
            //    NroDoc = src.ConductorId.HasValue ? (src.Empleado.Dni) : src.Practicante.NroDoc,
            //    TipoDoc = src.ConductorId.HasValue ? string.Empty : src.Practicante.TipoDoc.Descripcion,
            //}))

            ;
            CreateMap<SiniestrosDto, SinSiniestros>()
                .ForMember(dest => dest.SinSiniestrosConsecuencias, opt => opt.MapFrom(src => src.SiniestrosConsecuencias))
                        //.ForMember(dest => dest.SinInvolucrados, opt => opt.Ignore())
                        .ForMember(dest => dest.Empresa, opt => opt.Ignore())
                        .ForMember(dest => dest.ConductorEmpresa, opt => opt.Ignore())
                        .ForMember(dest => dest.Latitud, opt => opt.MapFrom(src => (src.Latitud * -1)))
                        .ForMember(dest => dest.CocheLinea, opt => opt.Ignore())
                        .ForMember(dest => dest.Longitud, opt => opt.MapFrom(src => (src.Longitud * -1)))
                        .ForMember(dest => dest.Lugar, opt => opt.MapFrom(src => src.Lugar.Trim()));


            CreateMap<SinSiniestrosConsecuencias, SiniestrosConsecuenciasDto>()
                .ForMember(dest => dest.CategoriaNombre, opt => opt.MapFrom(src => src.Categoria.Descripcion))
                .ForMember(dest => dest.ConsecuenciaNombre, opt => opt.MapFrom(src => src.Consecuencia.Descripcion))
                .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<SiniestrosConsecuenciasDto, SinSiniestrosConsecuencias>()
                  .ForMember(dest => dest.Categoria, opt => opt.Ignore())
                  .ForMember(dest => dest.Consecuencia, opt => opt.Ignore())
                  .ForMember(dest => dest.Siniestro, opt => opt.Ignore())
                  .EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<SinTipoDanio, TipoDanioDto>();
            CreateMap<TipoDanioDto, SinTipoDanio>();

            CreateMap<SinTipoInvolucrado, TipoInvolucradoDto>();
            CreateMap<TipoInvolucradoDto, SinTipoInvolucrado>();

            CreateMap<SinTipoLesionado, TipoLesionadoDto>();
            CreateMap<TipoLesionadoDto, SinTipoLesionado>();

            CreateMap<SinTipoMueble, TipoMuebleDto>();
            CreateMap<TipoMuebleDto, SinTipoMueble>();

            CreateMap<SinVehiculos, VehiculosDto>();
            CreateMap<VehiculosDto, SinVehiculos>();


            CreateMap<TipoDni, TipoDniDto>();
            CreateMap<TipoDniDto, TipoDni>();

            CreateMap<Linea, LineaDto>()
    .ForMember(
        dest => dest.SucursalId,
        opt => opt.MapFrom(src => src.SucursalesxLineas.FirstOrDefault().Id)
    )
    .ForMember(
        dest => dest.Grupolinea,
        opt => opt.MapFrom(src => src.PlaGrupoLinea.Nombre)
    )
   .ForMember(dest => dest.DesLin, opt => opt.MapFrom(src => src.DesLin.Trim()));
            CreateMap<LineaDto, Linea>();

            CreateMap<Empleados, EmpleadosDto>()
            .ForMember(dest => dest.Apellido, opt => opt.MapFrom(src => src.Apellido.Trim()))
            .ForMember(dest => dest.Nombre, opt => opt.MapFrom(src => src.Nombre.Trim()))
            .ForMember(dest => dest.cod_sucursal, opt => opt.MapFrom(src => src.UnidadNegocio.cod_sucursal));
            CreateMap<EmpleadosDto, Empleados>();

            CreateMap<LegajosEmpleado, LegajosEmpleadoDto>();
            CreateMap<LegajosEmpleadoDto, LegajosEmpleado>();

            CreateMap<Empresa, EmpresaDto>();
            CreateMap<EmpresaDto, Empresa>();

            CreateMap<Sucursales, sucursalesDto>();
            CreateMap<sucursalesDto, Sucursales>();

            CreateMap<UnidadesNegocio, UnidadesNegocioDto>()
                .ForMember(dest => dest.cod_sucursal, opt => opt.MapFrom(src => src.cod_sucursal));
            CreateMap<UnidadesNegocioDto, UnidadesNegocio>()
                .ForMember(dest => dest.cod_sucursal, opt => opt.MapFrom(src => src.cod_sucursal));




        }
    }
}
