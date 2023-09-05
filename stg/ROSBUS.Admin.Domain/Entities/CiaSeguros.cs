using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Auditing;
using Microsoft.Extensions.DependencyInjection;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinCiaSeguros : FullAuditedEntity<int>
    {

        public SinCiaSeguros()
        {
            SinSiniestros = new HashSet<SinSiniestros>();
            SinVehiculos = new HashSet<SinVehiculos>();
        }




        public string Descripcion { get; set; }
        public string Domicilio { get; set; }
        public int? LocalidadId { get; set; }
        public string Telefono { get; set; }
        public string Encargado { get; set; }

        public string Email { get; set; }
        public bool Anulado { get; set; }

        public ICollection<SinSiniestros> SinSiniestros { get; set; }
        public ICollection<SinVehiculos> SinVehiculos { get; set; }
        
    }
}
