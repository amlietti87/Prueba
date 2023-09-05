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
    public class FdEstadosDto :EntityDto<int>
    {
        public string Descripcion { get; set; }
        public bool PermiteRechazo { get; set; }
        public bool ImportarDocumentoOk { get; set; }
        public Guid? ImagenGrilla { get; set; }
        public Boolean VpDBDEmpleado { get; set; }
        
        public override string Description => Descripcion;
    }
}
