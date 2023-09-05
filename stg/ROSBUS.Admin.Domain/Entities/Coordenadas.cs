using System;
using System.Collections.Generic;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaCoordenadas : FullAuditedEntity<int>
    {
        public PlaCoordenadas()
        {
            HDetaminxtipo = new HashSet<HDetaminxtipo>();
            HProcMin = new HashSet<HProcMin>();
            HSectores = new HashSet<HSectores>();
            PlaPuntos = new HashSet<PlaPuntos>();
        }

        public string Abreviacion { get; set; }
        public string CodigoNombre { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }

        public string Calle1 { get; set; }
        public string Calle2 { get; set; }

        public string DescripcionCalle1 { get; set; }
        public string DescripcionCalle2 { get; set; }
        public bool? BeforeMigration { get;set; }
        public string Descripcion { get; set; }

        public bool Anulado { get; set; }
        public string GetDescription()
        {
            return string.Format("{0}-{1}", Calle1.Trim(), Calle2.Trim());
        }

        public int? LocalidadId { get; set; }
        public string NumeroExternoIVU { get; set; }

        public ICollection<HDetaminxtipo> HDetaminxtipo { get; set; }
        public ICollection<HProcMin> HProcMin { get; set; }
        public ICollection<HSectores> HSectores { get; set; }
        public ICollection<PlaPuntos> PlaPuntos { get; set; }
    }
}
