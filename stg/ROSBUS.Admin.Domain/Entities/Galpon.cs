using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class Galpon : TECSO.FWK.Domain.Auditing.FullAuditedEntity<Decimal>
    {

        public Galpon()
        {
            Configu = new HashSet<Configu>();
            PlaTalleresIvu = new HashSet<PlaTalleresIvu>();
        }
        public string DesGal { get; set; }
        public string DomGal { get; set; }
        public string PosGal { get; set; }
        public string TelGal { get; set; }
        public string EncGal { get; set; }

        public DateTime? HoraCorte { get; set; }
        public float? Latitud { get; set; }
        public float? Longitud { get; set; }
        public int? Radio { get; set; }
        public decimal? PtoPedido { get; set; }
        public string IdSap { get; set; }
        public string IdSapRepuestos { get; set; }
        public Guid? IdentificadorMapa { get; set; }
        public ICollection<Configu> Configu { get; set; }
        public ICollection<PlaTalleresIvu> PlaTalleresIvu { get; set; }
    }
}
