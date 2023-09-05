using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{

        public class ReclamosHistoricosFilter : FilterPagedListFullAudited<SinReclamosHistoricos, int>
        {
            public int? ReclamoId { get; set; }


            public override Expression<Func<SinReclamosHistoricos, bool>> GetFilterExpression()
            {
                Expression<Func<SinReclamosHistoricos, bool>> Exp = base.GetFilterExpression();


                if (ReclamoId.HasValue)
                {
                    Exp = Exp.And(e => e.ReclamoId == this.ReclamoId);
                }

                return Exp;
            }

        public override List<Expression<Func<SinReclamosHistoricos, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<SinReclamosHistoricos, object>>>() {
                e => e.Abogado,
                e => e.AbogadoEmpresa,
                e => e.Estado,
                e => e.Involucrado,
                e => e.Juzgado,
                e => e.SubEstado,
                e => e.TipoReclamo,
                e => e.Siniestro,
                e => e.Siniestro.Sucursal,
                e => e.CreatedUser,
                e => e.Denuncia,
                e => e.Denuncia.PrestadorMedico
            };
        }

    }


    
}
