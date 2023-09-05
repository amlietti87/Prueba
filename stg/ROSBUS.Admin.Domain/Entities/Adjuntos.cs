using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Adjuntos : Entity<Guid>
    {

        public string Nombre { get; set; }
        public byte[] Archivo { get; set; }

        //public ICollection<SinInvolucradosAdjuntos> SinInvolucradosAdjuntos { get; set; }
        //public ICollection<SinReclamoAdjuntos> SinReclamoAdjuntos { get; set; }
       // public ICollection<SinSiniestroAdjuntos> SinSiniestroAdjuntos { get; set; }
    }
}
