using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Extensions;

namespace ROSBUS.Admin.Domain.Entities.Filters
{
    public class RutasFilter : FilterPagedListFullAudited<GpsRecorridos, int>
    {
        public int? SucursalId { get; set; }

        public long? BanderaId { get; set; }
        public int? EstadoRutaId { get; set; }


        public int? EsOriginal { get; set; }

        public string Abreviacion { get; set; }



        public Boolean AllIncludes { get; set; }


        public override List<Expression<Func<GpsRecorridos, object>>> GetIncludesForPageList()
        {
            if (AllIncludes)
            {
                return new List<Expression<Func<GpsRecorridos, object>>>
                {
                    e=> e.Puntos,
                    e=> e.Sectores,
                     e=> e.EstadoRuta,
                     e=> e.Bandera
                };

            }
            else
            {
                return new List<Expression<Func<GpsRecorridos, object>>>
                {
                    e=> e.EstadoRuta
                };
            }

        }

        public override List<Expression<Func<GpsRecorridos, object>>> GetIncludesForGetById()
        {
            return new List<Expression<Func<GpsRecorridos, object>>>
            {
                e=> e.Puntos,
                e=> e.Sectores
            };
        }

        public override Expression<Func<GpsRecorridos, bool>> GetFilterExpression()
        {
            var basequery = base.GetFilterExpression();


            if (Id != 0)
            {
                basequery = basequery.And(e => e.Id == Id);
            }



            if (EsOriginal.HasValue)
            {
                var _EsOriginal = EsOriginal.Value;
                basequery = basequery.And(e => e.EsOriginal == _EsOriginal);
            }

            if (EstadoRutaId.HasValue)
            {
                var _estadoRutaId = EstadoRutaId.Value;
                basequery = basequery.And(e => e.EstadoRutaId == _estadoRutaId);
            }

            if (BanderaId.HasValue)
            {
                var banderaId = BanderaId.Value;
                basequery = basequery.And(e => e.CodBan == banderaId);
            }

            return basequery;
        }

        public Expression<Func<GpsRecorridos, bool>> GetFilterRutasPosicionamiento()
        {

            // var IdEstadoRutaAprobado = EstadoRuta.APROBADO;

            var IdTipoBandera = PlaTipoBandera.Posicionamiento;
            Expression<Func<GpsRecorridos, bool>> Exp = e => (
               e.Bandera.TipoBanderaId == IdTipoBandera
               //TODO: RB
            && e.Bandera.SucursalId == this.SucursalId
            && !e.Bandera.IsDeleted
            && !e.IsDeleted
            && e.Puntos.Any(a => (a.EsPuntoInicio || a.EsPuntoTermino) && a.Abreviacion == Abreviacion)
            );

            return Exp;

        }





    }

    public class RutasViewFilter : FilterPagedListFullAudited<GpsRecorridos, int>
    {
        public bool Original { get; set; }

        public bool Vigente { get; set; }

        public List<long?> BanderasId { get; set; }
        public DateTime? FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int EstadoRecorrido { get; set; }


        public override List<Expression<Func<GpsRecorridos, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<GpsRecorridos, object>>>
                {
                    e=> e.Puntos,
                    e=> e.Sectores
                };
        }

        public override Expression<Func<GpsRecorridos, bool>> GetFilterExpression()
        {
            var basequery = base.GetFilterExpression();

            if (BanderasId != null)
            {
                var hoy = DateTime.Now.Date;

                basequery = basequery.And(e => BanderasId.Contains(e.CodBan));

                if (Original == Vigente)
                {
                    var _esOriginal = Original ? (int)GpsRecorridos.EnumOriginal.Original : (int)GpsRecorridos.EnumOriginal.No;
                    basequery = basequery.And(e => e.EsOriginal == _esOriginal ||
                    (e.EstadoRutaId == PlaEstadoRuta.APROBADO 
                    && e.Fecha <= hoy && (e.FechaVigenciaHasta == null || e.FechaVigenciaHasta >= hoy))
                    );
                }
                else
                {
                    if (Original)
                    {
                        var _esOriginal = (int)GpsRecorridos.EnumOriginal.Original;
                        basequery = basequery.And(e => e.EsOriginal == _esOriginal);
                    }
                    if (Vigente)
                    {
                        basequery = basequery.And(e =>
                        e.EstadoRutaId == PlaEstadoRuta.APROBADO
                        && e.Fecha <= hoy && (e.FechaVigenciaHasta == null || e.FechaVigenciaHasta >= hoy));
                    }
                }

            }
            return basequery;
        }
    }

    public class RutasFilteredFilter : FilterPagedListFullAudited<GpsRecorridos, int>
    {

        public int BanderaId { get; set; }
        public int CodHFecha { get; set; }
        public DateTime FechaDesde { get; set; }
        public DateTime? FechaHasta { get; set; }
        public int CodRec { get; set; }


        public override List<Expression<Func<GpsRecorridos, object>>> GetIncludesForPageList()
        {
            return new List<Expression<Func<GpsRecorridos, object>>>
                {
                    e=> e.Puntos,
                    e=> e.Sectores
                };
        }

        public override Expression<Func<GpsRecorridos, bool>> GetFilterExpression()
        {
            var basequery = base.GetFilterExpression();

            if (BanderaId != 0)
            {
                
                basequery = basequery.And(e => e.CodBan == BanderaId  && e.EstadoRutaId == PlaEstadoRuta.APROBADO);
                basequery = basequery.And(e => (e.FechaVigenciaHasta >= FechaDesde || !e.FechaVigenciaHasta.HasValue));
                if (FechaHasta.HasValue)
                {
                    basequery = basequery.And(e => e.Fecha <= FechaHasta);
                }

            }
            return basequery;
        }
    }


    public class MinutosPorSectorFilter : FilterPagedListFullAudited<GpsRecorridos, int>
    {
        public List<long> SectoresIds { get; set; }
        public int TipoDiaId { get; set; }
        public int RutaId { get; set; }


        public override Expression<Func<GpsRecorridos, bool>> GetFilterExpression()
        {
            var basequery = base.GetFilterExpression();

            if (Id != 0)
            {
                basequery = basequery.And(e => e.Id == Id);
            }

            return basequery;
        }

    }

}
