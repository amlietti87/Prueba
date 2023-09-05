using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;
using System.Linq;
using TECSO.FWK.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.Model;
using System.Threading.Tasks;

namespace ROSBUS.infra.Data.Repositories
{
    public class HHorariosConfiRepository : RepositoryBase<AdminContext, HHorariosConfi, int>, IHHorariosConfiRepository
    {

        public HHorariosConfiRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<HHorariosConfi, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<List<DetalleSalidaRelevos>> ReporteDetalleSalidasYRelevos(DetalleSalidaRelevosFilter filter)
        {

           

            var sp = this.Context.LoadStoredProc("sp_pla_DetalleSalidaRelevos", commandTimeout:600)
              .WithSqlParam("cod_lin", new SqlParameter("cod_lin", filter.cod_lin))
              .WithSqlParam("cod_hfecha", new SqlParameter("cod_hfecha", filter.cod_hfecha))
              .WithSqlParam("CodTdia", new SqlParameter("CodTdia", (Object)filter.codTdia ?? DBNull.Value));


            List<DetalleSalidaRelevos> result = new List<DetalleSalidaRelevos>();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result = handler.ReadToList<DetalleSalidaRelevos>().ToList();

            });

            return result;
        }

        public async virtual Task<ReporteDistribucionCoches> ReporteDistribucionCoches(ReporteDistribucionCochesFilter filter)
        {

            var sp = this.Context.LoadStoredProc("sp_pla_ReporteDistribucionCoches")
            .WithSqlParam("lineList", new SqlParameter("lineList", string.Join(",", filter.lineList)))
            .WithSqlParam("fecha", new SqlParameter("fecha", (Object)filter.fecha ?? DBNull.Value))
            .WithSqlParam("CodTdia", new SqlParameter("CodTdia", (Object)filter.codTdia ?? DBNull.Value));


            List<ReporteDistribucionCochesSubgalpones> galponeslist = new List<ReporteDistribucionCochesSubgalpones>();

            ReporteDistribucionCoches result = new ReporteDistribucionCoches();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result.Horarios = handler.ReadToList<ReporteDistribucionHoraios>().ToList();
                if (handler.NextResult()) result.Galpones = handler.ReadToList<ReporteDistribucionCochesSubgalpones>().ToList();

            });

            return result;

        }


        public async virtual Task<ReportePasajeros> ReporteParadasPasajeros(ReportePasajerosFilter filter)
        {

            var sp = this.Context.LoadStoredProc("sp_pla_ReporteParadasPasajeros")
            .WithSqlParam("LineaId", new SqlParameter("LineaId", filter.LineaId))
            .WithSqlParam("Banderas", new SqlParameter("Banderas", string.Join(",", filter.Banderas)));

            ReportePasajeros result = new ReportePasajeros();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result.Detalle = handler.ReadToList<DetalleReportePasajeros>().ToList();

            });

            return result;

        }


        public async virtual Task<ReporteHorarioPasajeros> ReporteHorarioPasajeros(ReporteHorarioPasajerosFilter filter)
        {

            var sp = this.Context.LoadStoredProc("sp_pla_ReporteHorarioPasajeros")
            .WithSqlParam("cod_lin", new SqlParameter("cod_lin", (Object)filter.cod_lin ?? DBNull.Value))
            .WithSqlParam("CodHfecha", new SqlParameter("CodHfecha", (Object)filter.codHfecha ?? DBNull.Value))
            .WithSqlParam("banderasList ", new SqlParameter("banderasList", string.Join(",", filter.BanderasIda)))
            .WithSqlParam("CodTdia ", new SqlParameter("CodTdia", (Object)filter.codTdia ?? DBNull.Value));



            ReporteHorarioPasajeros result = new ReporteHorarioPasajeros();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result.MediasVueltasIda = handler.ReadToList<ReporteHorarioPasajerosMV>().ToList();
                if (handler.NextResult()) result.MinutosIda = handler.ReadToList<ReporteHorarioPasajerosMinutos>().ToList();

            });



            var spvuelta = this.Context.LoadStoredProc("sp_pla_ReporteHorarioPasajeros")
            .WithSqlParam("cod_lin", new SqlParameter("cod_lin", (Object)filter.cod_lin ?? DBNull.Value))
            .WithSqlParam("CodHfecha", new SqlParameter("CodHfecha", (Object)filter.codHfecha ?? DBNull.Value))
            .WithSqlParam("banderasList ", new SqlParameter("banderasList", string.Join(",", filter.BanderasVueltas)))
            .WithSqlParam("CodTdia ", new SqlParameter("CodTdia", (Object)filter.codTdia ?? DBNull.Value));

             

            await spvuelta.ExecuteStoredProcAsync((handler) =>
            {
                result.MediasVueltasVueltas = handler.ReadToList<ReporteHorarioPasajerosMV>().ToList();
                if (handler.NextResult()) result.MinutosVueltas = handler.ReadToList<ReporteHorarioPasajerosMinutos>().ToList();

            });


            return result;

        }


    }
}
