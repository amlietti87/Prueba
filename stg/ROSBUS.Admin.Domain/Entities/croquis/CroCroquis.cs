using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class CroCroquis : TECSO.FWK.Domain.Entities.Entity<int>
    {

        

        // public int Id { get; set; }
        public string Svg { get; set; }
        public int? TipoId { get; set; }


        [NotMapped]
        public int? idSiniestro;
    }
}
