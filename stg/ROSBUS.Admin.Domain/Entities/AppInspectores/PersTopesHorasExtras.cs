using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities.AppInspectores
{
    public partial class PersTopesHorasExtras : Entity<int>
    {
        
        public int? CodGalpon { get; set; }
        public int Hs50Persona { get; set; }
        public int Hs50Taller { get; set; }
        public int FrancosTrabajadosPersona { get; set; }
        public int FrancosTrabajadosTaller { get; set; }
        public int MinutosNocturnosPersona { get; set; }
        public int MinutosNocturnosTaller { get; set; }
        public int FeriadosPersona { get; set; }
        public int FeriadosTaller { get; set; }
        public DateTime? HoraFeriado { get; set; }
        public DateTime? HoraFranco { get; set; }
        public int? CodArea { get; set; }
        public DateTime? HorasComunes { get; set; }
        public int? IdGrupoInspectores { get; set; }
        public bool? PermiteHsExtrasFeriado { get; set; }
        public bool? PermiteFrancosTrabajadosFeriado { get; set; }
    }
}
