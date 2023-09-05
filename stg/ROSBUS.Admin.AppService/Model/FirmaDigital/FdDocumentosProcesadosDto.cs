using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks; 
using TECSO.FWK.AppService.Model;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.AppService.Model
{
    public class FdDocumentosProcesadosDto :EntityDto<long>
    {
        public FdDocumentosProcesadosDto()
        {
            FdDocumentosProcesadosHistorico = new HashSet<FdDocumentosProcesadosHistoricoDto>();
            AccionesDisponibles = new HashSet<FdAccionesDto>();
        }
        public int TipoDocumentoId { get; set; }
        public int EmpleadoId { get; set; }
        public int SucursalEmpleadoId { get; set; }
        public decimal EmpresaEmpleadoId { get; set; }
        public string LegajoEmpleado { get; set; }
        public string Cuilempleado { get; set; }
        public DateTime Fecha { get; set; }
        public DateTime FechaProcesado { get; set; }
        public Guid ArchivoId { get; set; }
        public int EstadoId { get; set; }
        //public string CodMinisterio { get; set; }
        public bool Rechazado { get; set; }
        public string MotivoRechazo { get; set; }

        public string NombreEmpleado { get; set; }
        public bool Cerrado { get; set; }
        public bool? EmpleadoConforme { get; set; }

        public string Anio { get { return Fecha.Year.ToString(); } }
        public string Mes
        {
            get
            {
                int number = Fecha.Month;

                if (number == 1)
                {
                    return "Enero";
                }
                else if (number == 2)
                {
                    return "Febrero";
                }
                else if (number == 3)
                {
                    return "Marzo";
                }
                else if (number == 4)
                {
                    return "Abril";
                }
                else if (number == 5)
                {
                    return "Mayo";
                }
                else if (number == 6)
                {
                    return "Junio";
                }
                else if (number == 7)
                {
                    return "Julio";
                }
                else if (number == 8)
                {
                    return "Agosto";
                }
                else if (number == 9)
                {
                    return "Septiembre";
                }
                else if (number == 10)
                {
                    return "Octubre";
                }
                else if (number == 11)
                {
                    return "Noviembre";
                }
                else if (number == 12)
                {
                    return "Diciembre";
                }
                else
                {
                    return "";
                }
            }

        }

        //Navigations
        public ICollection<FdDocumentosProcesadosHistoricoDto> FdDocumentosProcesadosHistorico { get; set; }
        public FdTiposDocumentosDto TipoDocumento { get; set; }
        public ICollection<FdAccionesDto> AccionesDisponibles { get; set; }
        public ICollection<FdEstadosDto> EstadosConHistorico { get; set; }
        public FdEstadosDto Estado { get; set; }

        //Custom props
        public string TipoDocumentoDescripcion { get; set; }
        public string EmpresaDescripcion { get; set; }
        public string SucursalDescripcion { get; set; }
        public bool PermiteRechazo { get; set; }
        public override string Description => String.Empty;
    }



}
