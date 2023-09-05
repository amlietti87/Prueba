using AutoMapper;
using AutoMapper.EquivalencyExpression;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using ROSBUS.Admin.Domain.Entities.ART;
using ROSBUS.ART.Domain.Entities.ART;
using ROSBUS.ART.AppService.Model;
using ROSBUS.Admin.Domain.Model;

namespace ROSBUS.WebService.ART
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ArtContingencias, ArtContingenciasDto>();
            CreateMap<ArtContingenciasDto, ArtContingencias>();


            CreateMap<ArtDenunciaAdjuntos, ArtDenunciaAdjuntosDto>();
            CreateMap<ArtDenunciaAdjuntosDto, ArtDenunciaAdjuntos>();

            CreateMap<ArtDenuncias, ArtDenunciasDto>()
                .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedUser.DisplayName))
                .ForMember(dest => dest.SucursalGrilla, opt => opt.MapFrom(src => src.Sucursal.DscSucursal))
                .ForMember(dest => dest.EmpresaGrilla, opt => opt.MapFrom(src => src.Empresa.DesEmpr))
                .ForMember(dest => dest.DenunciaNotificaciones, opt => opt.MapFrom(src => src.ArtDenunciasNotificaciones))
                .ForMember(dest => dest.selectedSiniestro, opt => opt.MapFrom(src => (src.SiniestroId.HasValue) ? (new ItemDto<int> { Id = src.SiniestroId.Value, Description = src.Siniestro.getDescription(), IsSelected = true }) : null));
            CreateMap<ArtDenunciasDto, ArtDenuncias>()
                .ForMember(dest => dest.ArtDenunciasNotificaciones, opt => opt.MapFrom(src => src.DenunciaNotificaciones));


            CreateMap<ArtDenunciasNotificaciones, ArtDenunciasNotificacionesDto>().EqualityComparison((odto, o) => odto.Id == o.Id);
            CreateMap<ArtDenunciasNotificacionesDto, ArtDenunciasNotificaciones>().EqualityComparison((odto, o) => odto.Id == o.Id);

            CreateMap<ArtEstados, ArtEstadosDto>();
            CreateMap<ArtEstadosDto, ArtEstados>();

            CreateMap<ArtMotivosAudiencias, ArtMotivosAudienciasDto>();
            CreateMap<ArtMotivosAudienciasDto, ArtMotivosAudiencias>();

            CreateMap<ArtMotivosNotificaciones, ArtMotivosNotificacionesDto>();
            CreateMap<ArtMotivosNotificacionesDto, ArtMotivosNotificaciones>();

            CreateMap<ArtPatologias, ArtPatologiasDto>();
            CreateMap<ArtPatologiasDto, ArtPatologias>();

            CreateMap<ArtPrestadoresMedicos, ArtPrestadoresMedicosDto>();
            CreateMap<ArtPrestadoresMedicosDto, ArtPrestadoresMedicos>();

            CreateMap<ArtTratamientos, ArtTratamientosDto>();
            CreateMap<ArtTratamientosDto, ArtTratamientos>();

            CreateMap<Empresa, EmpresaDto>();
            CreateMap<EmpresaDto, Empresa>();

            CreateMap<RubrosSalariales, RubrosSalarialesDto>();
            CreateMap<RubrosSalarialesDto, RubrosSalariales>();

            CreateMap<TiposAcuerdo, TiposAcuerdoDto>();
            CreateMap<TiposAcuerdoDto, TiposAcuerdo>();

            CreateMap<TiposReclamo, TiposReclamoDto>();
            CreateMap<TiposReclamoDto, TiposReclamo>();

            CreateMap<CausasReclamo, CausasReclamoDto>();
            CreateMap<CausasReclamoDto, CausasReclamo>();

            CreateMap<SinReclamosHistoricos, ReclamosHistoricosDto>()
                .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedUser.DisplayName))
                            .ForMember(dest => dest.JudicialSelected, opt => opt.MapFrom(src => src.Estado.Judicial))
                .ForMember(dest => dest.selectedSiniestro, opt => opt.MapFrom(src => (src.SiniestroId.HasValue) ? (new ItemDto<int> { Id = src.SiniestroId.Value, Description = src.Siniestro.getDescription(), IsSelected = true }) : null))
            .ForMember(dest => dest.selectedDenuncia, opt => opt.MapFrom(src => (src.DenunciaId.HasValue) ? (new ItemDto<int> { Id = src.DenunciaId.Value, Description = src.Denuncia.getDescription(), IsSelected = true }) : null));
            CreateMap<ReclamosHistoricosDto, SinReclamosHistoricos>();


            CreateMap<SinReclamos, ReclamosDto>()
            .ForMember(dest => dest.ReclamoCuotas, opt => opt.MapFrom(src => src.ReclamoCuotas))
            .ForMember(dest => dest.CreatedUserName, opt => opt.MapFrom(src => src.CreatedUser.DisplayName))
             .ForMember(dest => dest.ReclamoHistorico, opt => opt.MapFrom(src => src.GetReclamoHistorico()))
            .ForMember(dest => dest.JudicialSelected, opt => opt.MapFrom(src => src.Estado.Judicial))
            .ForMember(dest => dest.selectedSiniestro, opt => opt.MapFrom(src => (src.SiniestroId.HasValue) ? (new ItemDto<int> { Id = src.SiniestroId.Value, Description = src.Siniestro.getDescription(), IsSelected = true }) : null))
            //.ForMember(dest => dest.selectedDenuncia, opt => opt.MapFrom(src => (src.DenunciaId.HasValue) ? (new ItemDto<int> { Id = src.DenunciaId.Value, Description = src.Denuncia.getDescription(), IsSelected = true }) : null))
            ;
            CreateMap<ReclamosDto, SinReclamos>()
            .ForMember(dest => dest.ReclamoCuotas, opt => opt.MapFrom(src => src.ReclamoCuotas))
            .ForMember(dest => dest.SubEstado, opt => opt.Ignore())
            .ForMember(dest => dest.Estado, opt => opt.Ignore())
            .ForMember(dest => dest.Involucrado, opt => opt.Ignore())
            .ForMember(dest => dest.Abogado, opt => opt.Ignore());


            CreateMap<SinSiniestros, SiniestrosDto>()
                .ForMember(dest => dest.SiniestrosConsecuencias, opt => opt.MapFrom(src => src.SinSiniestrosConsecuencias))
                       .ForMember(dest => dest.Lugar, opt => opt.MapFrom(src => src.Lugar.Trim()))
                       .ForMember(dest => dest.DescCombo, opt => opt.MapFrom(src => src.getDescription()))
                       .ForMember(dest => dest.selectCoches, opt => opt.MapFrom(src => new ItemDto<String>() { Id = src.CocheId, Description = src.CocheDominio.Trim() }))
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


            CreateMap<Sucursales, sucursalesDto>();
            CreateMap<sucursalesDto, Sucursales>();


        }
    }
}
