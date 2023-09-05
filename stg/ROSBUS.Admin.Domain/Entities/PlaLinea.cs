using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using TECSO.FWK.Domain.Auditing;

namespace ROSBUS.Admin.Domain.Entities
{
    public partial class PlaLinea : FullAuditedEntity<int>
    {

        public PlaLinea()
        {
            PlaLineaLineaHoraria = new HashSet<PlaLineaLineaHoraria>();
            RamalColor = new HashSet<PlaRamalColor>();
        }


        public string DesLin { get; set; } 
        public int SucursalId { get; set; } 
        public int PlaTipoLineaId { get; set; }

        public bool Activo { get; set; }

        /// <summary>
        /// Parent PlaTipoLinea pointed by [pla_Linea].([PlaTipoLineaId]) (FK_pla_Linea_pla_TipoLinea)
        /// </summary>

        public virtual PlaTipoLinea PlaTipoLinea { get; set; } // FK_pla_Linea_pla_TipoLinea


        /// <summary>
        /// Parent Sucursale pointed by [pla_Linea].([SucursalId]) (FK_pla_Linea_sucursales)
        /// </summary>

        public virtual Sucursales Sucursal { get; set; } // FK_pla_Linea_sucursales

        public ICollection<PlaLineaLineaHoraria> PlaLineaLineaHoraria { get; set; }


        public ICollection<PlaRamalColor> RamalColor { get; set; }
    }
}
