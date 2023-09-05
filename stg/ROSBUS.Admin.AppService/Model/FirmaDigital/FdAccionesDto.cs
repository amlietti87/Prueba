using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FdAccionesDto :EntityDto<int>
    {
        public int TipoDocumentoId { get; set; }
        public int AccionPermitidaId { get; set; }
        public int EstadoActualId { get; set; }
        public int EstadoNuevoId { get; set; }
        public bool MostrarBdempleador { get; set; }
        public bool MostrarBdempleado { get; set; }
        public bool AccionBdempleador { get; set; }
        public bool EsFin { get; set; }
        public bool GeneraNotificacion { get; set; }
        public int? NotificacionId { get; set; }

        public Boolean PermiteMarcarLote { get; set; }
        public Boolean MenorFechaPrimero { get; set; }


        public FdAccionesPermitidasDto AccionPermitida { get; set; }

        public override string Description => String.Empty;
    }


    public class AplicarAccioneDto
    {

        public AplicarAccioneDto()
        {
            this.Documentos = new List<long>();
        }

        public int? AccionId { get; set; }

        public List<long> Documentos { get; set; }

        public Boolean Empleador { get; set; }
        public string Motivo { get; set; }

        public string Correo { get; set; }

        public Boolean EsFirmador { get; set; }
        public FdFirmadorDto Firmador { get; set; }
    }


    public class AplicarAccioneResponseDto
    {

        public FileDto FileDto { get; set; }

        public long? FirmadorId { get; set; }

        public AplicarAccioneResponseDto()
        {
            this.Detalles = new List<DetalleResponse>();
        }

        public Boolean IsValid
        {
            get
            {
                if (this.Detalles.Any(e=> !e.IsValid))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }


        public List<DetalleResponse> Detalles { get; set; }

    }


    public class DetalleResponse
    {
        public string Documento { get; set; }

        public string Error { get; set; }

        public Boolean IsValid { get; set; }
    }

}
