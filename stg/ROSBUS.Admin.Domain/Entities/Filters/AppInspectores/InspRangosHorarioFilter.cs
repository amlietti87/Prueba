using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters.AppInspectores
{
    public class InspRangosHorarioFilter : FilterPagedListAudited<InspRangosHorarios, int>
    {
        public int? Anulado { get; set; }
        public int? GrupoInspectoresId { get; set; }
        public Boolean? FrancoTrabajado { get; set; }
        public Boolean? EsFranco { get; set; }



        public override Expression<Func<InspRangosHorarios, bool>> GetFilterExpression()
        {
            Expression<Func<InspRangosHorarios, bool>> baseFE = base.GetFilterExpression();

            if (!String.IsNullOrEmpty(this.FilterText))
            {
                Expression<Func<InspRangosHorarios, bool>> filterTextExp = e => e.Descripcion.Contains(this.FilterText);
                baseFE = baseFE.And(filterTextExp);
            }

            if (Anulado.HasValue)
            {
                if (Anulado.Value == 2)
                {
                    baseFE = baseFE.And(e => e.Anulado == false);
                }

                if (Anulado.Value == 1)
                {
                    baseFE = baseFE.And(e => e.Anulado == true);
                }
            }

            if(GrupoInspectoresId.HasValue)
            {
                baseFE = baseFE.And(e => e.InspGrupoInspectoresRangosHorarios.Any(z => z.GrupoInspectoresId == GrupoInspectoresId));
            }

            if (FrancoTrabajado.HasValue)
            {
                baseFE = baseFE.And(e => e.EsFrancoTrabajado == this.FrancoTrabajado);
            }

            if (EsFranco.HasValue)
            {
                baseFE = baseFE.And(e => e.EsFranco == this.EsFranco);
            }
            return baseFE;

        }
    }
}
