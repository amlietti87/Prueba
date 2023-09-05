using AutoMapper;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;

namespace ROSBUS.WebService.FirmaDigital.MappingConfig
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<FdDocumentosProcesados, FdDocumentosProcesadosDto>()
             .ForMember(dest => dest.EmpresaDescripcion, opt => opt.MapFrom(src => src.Empresa.DesEmpr))
             .ForMember(dest => dest.SucursalDescripcion, opt => opt.MapFrom(src => src.Sucursal.DscSucursal))
             .ForMember(dest => dest.PermiteRechazo, opt => opt.MapFrom(src => src.Estado.PermiteRechazo))
             .ForMember(dest => dest.TipoDocumentoDescripcion, opt => opt.MapFrom(src => src.TipoDocumento.Descripcion));
            CreateMap<FdDocumentosProcesadosDto, FdDocumentosProcesados>();

            CreateMap<FdAccionesPermitidas, FdAccionesPermitidasDto>()
            .ForMember(dest => dest.TokenPermission, opt => opt.MapFrom(src => src.Permiso.Token));
            CreateMap<FdAccionesPermitidasDto, FdAccionesPermitidas>();

            CreateMap<FdTiposDocumentos, FdTiposDocumentosDto>()
            .ForMember(dest => dest.EsPredeterminadoOriginal, opt => opt.MapFrom(src => src.EsPredeterminado));
            CreateMap<FdTiposDocumentosDto, FdTiposDocumentos>();

            CreateMap<FdDocumentosError, FdDocumentosErrorDto>()
            .ForMember(dest => dest.TipoDocumentoDescripcion, opt => opt.MapFrom(src => src.TipoDocumento.Descripcion))
             .ForMember(dest => dest.EmpresaDescripcion, opt => opt.MapFrom(src => src.Empresa.DesEmpr))
             .ForMember(dest => dest.SucursalDescripcion, opt => opt.MapFrom(src => src.Sucursal.DscSucursal))
            ;
            CreateMap<FdDocumentosErrorDto, FdDocumentosError>();


            CreateMap<FdFirmador, FdFirmadorDto>();
            CreateMap<FdFirmadorDto, FdFirmador>()
                .ForMember(dest => dest.CreatedUserId, opt => opt.Ignore());



            CreateMap<FdFirmadorDetalle, FdFirmadorDetalleDto>();
            CreateMap<FdFirmadorDetalleDto, FdFirmadorDetalle>();

            CreateMap<FdFirmadorLog, FdFirmadorLogDto>();
            CreateMap<FdFirmadorLogDto, FdFirmadorLog>();
            
            CreateMap<FdCertificados, FdCertificadosDto>()
                .ForMember(dest => dest.UsuarioNombre, opt => opt.MapFrom(src => src.Usuario.NomUsuario));
            CreateMap<FdCertificadosDto, FdCertificados>();
        }
    }
}
