using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaRamalColor : FullAuditedEntity<long>
    {
        public PlaRamalColor()
        {
            HBanderas = new HashSet<HBanderas>();
        }

        public string Nombre { get; set; }
        public int LineaId { get; set; }
        public bool Activo { get; set; }
        public int? ColorTupid { get; set; }
        public string RouteLongName { get; set; }
        public string RouteShortName { get; set; }

        public HBanderasTup BanderaTup { get; set; }

        [NotMapped]
        public PlaLinea PlaLinea { get; set; }
        public ICollection<HBanderas> HBanderas { get; set; }


    }





}
