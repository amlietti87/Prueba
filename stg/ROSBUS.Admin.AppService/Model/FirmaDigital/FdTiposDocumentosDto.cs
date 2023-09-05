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
    public class FdTiposDocumentosDto :AuditedEntityDto<int>
    {

        public FdTiposDocumentosDto()
        {
            FdAcciones = new HashSet<FdAccionesDto>();
        }
        public string Descripcion { get; set; }
        public string Prefijo { get; set; }
        public bool RequiereLider { get; set; }
        public bool EsPredeterminado { get; set; }
        public bool EsPredeterminadoOriginal { get; set; }
        public bool Anulado { get; set; }

        public ICollection<FdAccionesDto> FdAcciones { get; set; }
        public override string Description => Descripcion;
    }
}
