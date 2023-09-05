using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaParadas : Entity<int>
    {
        public PlaParadas()
        {
            PlaPuntos = new HashSet<PlaPuntos>();
        }
        
        public string Codigo { get; set; }
        public string Calle { get; set; }
        public string Cruce { get; set; }
        public int LocalidadId { get; set; }
        public string Sentido { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
        public bool Anulada { get; set; }
        public bool PickUpType { get; set; }
        public bool DropOffType { get; set; }
        public int LocationType { get; set; }
        public int? ParentStationId { get; set; }
        public PlaParadas ParentStation { get; set; }


        public virtual ICollection<PlaPuntos> PlaPuntos { get; set; }

        public string GetDescription()
        {
            return string.Format("{0}", this.Codigo);
        }
    }
}
