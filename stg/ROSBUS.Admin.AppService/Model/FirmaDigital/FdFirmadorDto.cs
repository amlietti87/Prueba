using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FdFirmadorDto : EntityDto<long>
    {
        public string SessionId { get; set; }





        public string PathGetDescarga { get; set; }
        public string PathPostSubida { get; set; }
        public int AccionId { get; set; }
        public string UsuarioId { get; set; }
        public string UsuarioRol { get; set; }
        public string UsuarioUserName { get; set; }
        public string UsuarioNombre { get; set; }
        public string UsuarioApellido { get; set; }
        public string CoordenadasEmpleado { get; set; }
        public string CoordenadasEmpleador { get; set; }

        public override string Description => "";

        public virtual List<FdFirmadorDetalleDto> FdFirmadorDetalle { get; set; }
        public virtual List<FdFirmadorLogDto> FdFirmadorLog { get; set; }
        public byte[] file { get; set; }

        public string FileName { get; set; }

        public int? CreatedUserId { get; set; }
        public bool? EmpleadoConforme { get; set; }

        public MetadatosDto Metadatos { get; set; }
    }


    public class MetadatosDto
    {
        public string pathGetDescarga { get; set; }
        public string pathPostSubida { get; set; }
        public long[] recibos { get; set; }
        public MetadatosUsuarioDto usuario { get; set; }
        public int[] coordenadas_empleado { get; set; }
        public int[] coordenadas_empleador { get; set; }
    }

    public class MetadatosUsuarioDto
    {
        public string id { get; set; }
        public string rol { get; set; }
        public string username { get; set; }
        public string nombre { get; set; }
        public string apellido { get; set; }
    }

    public class FdFirmadorDetalleDto : EntityDto<long>
    {
        public override string Description => "";

        public int FirmadorId { get; set; }
        public long DocumentoProcesadoId { get; set; }
        public bool Firmado { get; set; }
        public Guid? ArchivoIdRecibido { get; set; }
        public int EstadoId { get; set; }
        public Guid ArchivoIdEnviado { get; set; }


        public bool HasChange { get; set; }
    }
}
