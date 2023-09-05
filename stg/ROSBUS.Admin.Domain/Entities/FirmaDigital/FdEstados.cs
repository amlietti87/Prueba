using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.FirmaDigital
{
    public partial class FdEstados : Entity<int>
    {

        
        public FdEstados()
        {
            FdAccionesEstadoActual = new HashSet<FdAcciones>();
            FdAccionesEstadoNuevo = new HashSet<FdAcciones>();
            FdDocumentosProcesados = new HashSet<FdDocumentosProcesados>();
            FdDocumentosProcesadosHistorico = new HashSet<FdDocumentosProcesadosHistorico>();
        }

        public string Descripcion { get; set; }
        public bool PermiteRechazo { get; set; }
        public bool ImportarDocumentoOk { get; set; }
        public Guid? ImagenGrilla { get; set; }

        public Boolean VpDBDEmpleado { get; set; }

        public ICollection<FdAcciones> FdAccionesEstadoActual { get; set; }
        public ICollection<FdAcciones> FdAccionesEstadoNuevo { get; set; }
        public ICollection<FdDocumentosProcesados> FdDocumentosProcesados { get; set; }
        public ICollection<FdDocumentosProcesadosHistorico> FdDocumentosProcesadosHistorico { get; set; }
    }
}
