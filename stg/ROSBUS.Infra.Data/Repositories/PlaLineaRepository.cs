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
using System.Threading.Tasks;
using Snickler.EFCore;
using ROSBUS.Admin.Domain.Model;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.Entities.Filters.ComoLlegoBus;

namespace ROSBUS.infra.Data.Repositories
{
    public class PlaLineaRepository : RepositoryBase<AdminContext,PlaLinea, int>, IPlaLineaRepository
    {

        public PlaLineaRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaLinea, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<ReporteHorarioRuta> GetHorarioRuta(string routelongname)
        {
            var sp = this.Context.LoadStoredProc("sp_CL_GetHorarioRuta")
                        .WithSqlParam("NombreLinea ", routelongname);



            ReporteHorarioRuta result = new ReporteHorarioRuta();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result.MediasVueltas = handler.ReadToList<ReporteHorarioRutaMV>().ToList();
                if (handler.NextResult()) result.Minutos = handler.ReadToList<ReporteHorarioRutaMinutos>().ToList();

            });

            return result;
        }

        public async Task<List<PlaLinea>> GetLineasValidas(string nlRosBusValidas, string tiposdias)
        {
            IList<PlaLinea> LineasValidas = new List<PlaLinea>();

            var sp = this.Context.LoadStoredProc("dbo.[sp_cl_lineasvalidastarifas]")
                                  .WithSqlParam("@NombresLineas", nlRosBusValidas)
                                  .WithSqlParam("@TiposLineaId", tiposdias);

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                LineasValidas = handler.ReadToList<PlaLinea>();
            });

            return LineasValidas.ToList();
        }

        public async Task<List<int>> GetMedioPagosRuta(string name)
        {
   
            IList<int> mediospagotarifas = new List<int>();
            IList<int> mediospagotriangulotarifario = new List<int>();
   
            var sp = this.Context.LoadStoredProc("dbo.[sp_cl_mediospagotarifas]")
                      .WithSqlParam("NombresLineas", name);

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                mediospagotarifas.Add(handler.ReadToValue<int>().GetValueOrDefault());

                while (handler.NextResult())
                {
                    mediospagotarifas.Add(handler.ReadToValue<int>().GetValueOrDefault());
                }
            });

           sp = this.Context.LoadStoredProc("dbo.[sp_cl_mediospagotriangulostarifarios]")
                    .WithSqlParam("@NombresLineas", name);

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                mediospagotriangulotarifario.Add(handler.ReadToValue<int>().GetValueOrDefault());

                while (handler.NextResult())
                {
                    mediospagotriangulotarifario.Add(handler.ReadToValue<int>().GetValueOrDefault());
                }
            });

            mediospagotarifas = mediospagotarifas.Union(mediospagotriangulotarifario).ToList();
            List<int> mediospagosfiltrado = new List<int>(mediospagotarifas.Where(e => e != 0).Distinct());
            return mediospagosfiltrado;
        }

        public async Task<ReporteParadasRuta> GetParadasRuta(int? codBan, string Lineas)
        {

            var sp = this.Context.LoadStoredProc("sp_pla_CLReporteParadasRutas")
                                    .WithSqlParam("@CodBan", new SqlParameter("@CodBan", (Object)codBan ?? DBNull.Value))
                                    .WithSqlParam("@RoutesName", new SqlParameter("@RoutesName", (Object)Lineas));

            ReporteParadasRuta result = new ReporteParadasRuta();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                result.DetalleParadas = handler.ReadToList<DetalleReporteParadasRutas>().ToList();

            });

            return result;
        }

        public async Task <List<PuntosRuta>> GetPuntosRutaBanderaAsync(int? codBan)
        {
            var sp = this.Context.LoadStoredProc("sp_CL_GetRecorridoBandera")
                      .WithSqlParam("CodBan", codBan);


            List<PuntosRuta> puntos = new List<PuntosRuta>();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                puntos = handler.ReadToList<PuntosRuta>().ToList();

            });

            return puntos;
        }

        public async Task<ReporteTarifasRuta> GetTarifasRutaAsync(int? codBan, string NombresLineas)
        {
            var sp = this.Context.LoadStoredProc("sp_CL_GetALLTarifasRespaldo")
                                  .WithSqlParam("Lineas", NombresLineas)
                                  .WithSqlParam("CodBan", new SqlParameter("CodBan", (Object)codBan ?? DBNull.Value));



            ReporteTarifasRuta tarifas = new ReporteTarifasRuta();

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                tarifas.TarifasPlanasRespaldo = handler.ReadToList<TarifasPlanasRespaldo>().ToList();
                if (handler.NextResult()) tarifas.TarifasTrianguloRespaldo = handler.ReadToList<TarifasTrianguloRespaldo>().ToList();

            });

            return tarifas;
        }

        public async Task<decimal> GetTarifaUrbana(string linename)
        {
            decimal? TarifaUrbana = 0;

            var sp = this.Context.LoadStoredProc("dbo.[sp_cl_gettarifalinea]")
                                  .WithSqlParam("@NombreLinea", linename);

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                TarifaUrbana = handler.ReadToValue<decimal>();
            });

            return Convert.ToDecimal(TarifaUrbana);
        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UI_deslin", "El nombre de la linea no puede repetirse.");
            return d;
        }
    }
}
