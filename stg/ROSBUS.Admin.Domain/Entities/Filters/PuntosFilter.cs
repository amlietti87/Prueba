using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class PuntosFilter : FilterPagedListBase<PlaPuntos, Guid>
    {

        public int? CodRec { get; set; }

        public List<int> CodRecs { get; set; }

        public bool? CambioSectoresMapa { get; set; }

        public bool? ParadasMapa { get; set; }

        public long? RutaId { get; set; }
        public int? UnidadDeNegocioId { get; set; }


        public override List<Expression<Func<PlaPuntos, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<PlaPuntos, object>>>
            {
                e=> e.PlaCoordenada, 
                e=>e.BolSectoresTarifarios,
                e=>e.PlaParada
            };
        }

        public override Expression<Func<PlaPuntos, bool>> GetFilterExpression()
        {
            Expression<Func<PlaPuntos, bool>> Exp = base.GetFilterExpression();

            if (RutaId.HasValue)
            {
                var rutaId = RutaId.Value;
                Exp = Exp.And(e => e.CodRec == rutaId);
            }

            if (CodRecs != null &&  CodRecs.Count > 0)
            {
                Exp = Exp.And(e => CodRecs.Any(f => f == e.CodRec));
            }

            return Exp;
        }

        public PuntosFilter() {
            CodRecs = new List<int>();
        }

        public Expression<Func<PlaPuntos, bool>> GetPuntosReporteExpression()
        {
            Expression<Func<PlaPuntos, bool>> Exp = e => e.CodRec == RutaId && (e.EsCambioSector || e.EsParada ||
                e.EsPuntoInicio || e.EsPuntoTermino);

            return Exp;
        }



        public Expression<Func<PlaPuntos, bool>> GetFilterPuntosInicioFin()
        {

            var IdEstadoRutaAprobado = PlaEstadoRuta.APROBADO;

            Expression<Func<PlaPuntos, bool>> Exp =
                e => (e.EsPuntoInicio || e.EsPuntoTermino)
                        && e.Ruta.EstadoRutaId == IdEstadoRutaAprobado
                        && e.Ruta.Bandera.TipoBanderaId == 1
                        //TODO Rb: 
                        //&& e.Ruta.EsOriginal
                        ;


            if (this.UnidadDeNegocioId.HasValue)
            {
                Exp = Exp.And(e => e.Ruta.Bandera.RamalColor.PlaLinea.SucursalId == this.UnidadDeNegocioId);
            }

            return Exp;

        }

    }
}
