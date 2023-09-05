using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class SinConductores : Entity<int>
    {
        public SinConductores()
        {
            SinInvolucrados = new HashSet<SinInvolucrados>();
        }

        public string ApellidoNombre { get; set; }
        public int? TipoDocId { get; set; }
        public string NroDoc { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public int? LocalidadId { get; set; }
        public string Domicilio { get; set; }
        public string Telefono { get; set; }
        public string Celular { get; set; }
        public string NroLicencia { get; set; }

        public TipoDni TipoDoc { get; set; }
        public ICollection<SinInvolucrados> SinInvolucrados { get; set; }

        public string GetDescription()
        {
            return string.Format("{0} {1} {2}", this.ApellidoNombre, this.TipoDoc?.Descripcion, this.NroDoc);
        }
    }
}
