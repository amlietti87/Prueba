using ROSBUS.Admin.Domain;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using ROSBUS.Admin.Domain.Model;
using System;
using System.Collections.Generic;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;
using System.Linq;
using TECSO.FWK.Domain.Interfaces.Repositories;
using System.Linq.Expressions;
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using ROSBUS.Infra.Data;
using System.ComponentModel.DataAnnotations;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.infra.Data.Repositories
{
    public class HMinxtipoRepository : RepositoryBase<AdminContext, HMinxtipo, int>, IHMinxtipoRepository
    {
        private readonly ILogger logger;

        public HMinxtipoRepository(IAdminDbContext _context, ILogger logger)
            : base(new DbContextProvider<AdminContext>(_context))
        {
            this.logger = logger;
        }

        public override Expression<Func<HMinxtipo, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<List<HSectores>> GetHSectores(HMinxtipoFilter filter)
        {

            //           SELECT
            //             Calle1, pc.Calle2, s.orden, pc.Id
            //           FROM  h_sectores s
            // INNER JOIN  pla_Coordenadas AS pc ON s.cod_hsector = pc.Id
            //INNER JOIN h_basec AS b ON b.cod_sec = s.cod_sec
            //and b.cod_ban = 546
            //AND b.cod_hfecha = 7040
            // order by s.orden
            //.Include(i => i.CodHsectorNavigation)

            //var query1 = from ss in Context.HSectores
            //            join bs in Context.HBasec on bs.CodSec equals s.CodSec
            //           // where b. == id
            //            select s;

            var query = Context.HSectores
                .Include(i => i.CodHsectorNavigation)
                .Include(i => i.CodSectorTarifarioNavigation)
               .Join(Context.HBasec,
                 b => b.CodSec,
                  s => s.CodSec,
                  (S, B) => new { b = B, s = S })
               .Where(w => w.b.CodBan == filter.CodBan && w.b.CodHfecha == filter.CodHfecha);

            var r = await query.Select(e => e.s).ToListAsync();
            return r;
        }



        public async Task UpdateHMinxtipo(IEnumerable<HMinxtipo> items)
        {
            try
            {

                await this.logger.LogInformation("Entro a UpdateHMinxtipo");
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    //foreach (var entity in items)
                    //{
                    //    Context.Entry(entity).State = EntityState.Modified;
                    //}
                    await this.SaveChangesAsync();

                    await this.logger.LogInformation("SaveChangesAsync UpdateHMinxtipo");
                    //var entries = this.Context.ChangeTracker.Entries().ToList<EntityEntry>();

                    //var entriesModify = entries.Where(e => e.Entity is HMinxtipo).Select(s => (s.Entity as HMinxtipo)).ToList();

                    ts.Commit();
                }

                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    try
                    {
                        await this.logger.LogInformation("Inicio RecrearSabanaPorMinutos UpdateHMinxtipo");
                        await this.RecrearSabanaPorMinutos(items);
                        await this.logger.LogInformation("Fin RecrearSabanaPorMinutos UpdateHMinxtipo");
                        ts.Commit();

                    }
                    catch (Exception ex)
                    {
                        await this.logger.LogInformation("Error RecrearSabanaPorMinutos UpdateHMinxtipo");

                        await this.logger.LogError(ex.Message);

                        throw new ValidationException("Se guardaron los datos correctamente pero no se pudieron regenrar los datos de la sabana.");
                        
                    }
                    

                }

                await this.logger.LogInformation("Fin UpdateHMinxtipo UpdateHMinxtipo");
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task RecrearSabanaPorMinutos(IEnumerable<HMinxtipo> items)
        {
            string _TipoHora = null;

            if (items.Select(e => e.TipoHora).Distinct().Count() == 1)
            {
                _TipoHora = items.Select(e => e.TipoHora).Distinct().First();
            }

            await this.RecrearSabanaPorMinutos(items.First().CodHfecha, items.First().CodTdia, _TipoHora, items.First().CodBan, items.Select(e => e.Id).ToList());
        }

        public async Task RecrearSabanaPorMinutos(int CodHfecha, int CodTdia, string TipoHora, int CodBan, List<int> codMinxtipoList = null)
        {
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = SqlObjects.sp_pla_recrearHProcMinPorMin;
                command.CommandType = System.Data.CommandType.StoredProcedure;
                command.CommandTimeout = 600;

                command.Parameters.Add(new SqlParameter("cod_hfecha ", CodHfecha));
                command.Parameters.Add(new SqlParameter("cod_tdia ", CodTdia));
                command.Parameters.Add(new SqlParameter("codtpoHora ", TipoHora));
                command.Parameters.Add(new SqlParameter("codBan ", CodBan));

                if (codMinxtipoList != null && codMinxtipoList.Any())
                {
                    command.Parameters.Add(new SqlParameter("codMinxtipoList ", string.Join(",", codMinxtipoList)));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("codMinxtipoList ", DBNull.Value));
                }

                Context.Database.OpenConnection();
                await command.ExecuteNonQueryAsync();
            }
        }

        /// <summary>
        /// elimina h_proc_min y copia de h_detaminxtipo a h_proc_min, recrando la sabana teniendo en cuenta la salida
        /// </summary>
        public async Task RecrearSabanaBandera(CopiarHMinxtipoInput input)
        {
            if (input.TipoHoraDestino == "all")
            {
                input.TipoHoraDestino = null;
            }

            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandTimeout = 120;
                command.CommandText = Infra.Data.SqlObjects.sp_pla_recrearHProcMin;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("cod_hfecha ", input.CodHfechaDestino));
                command.Parameters.Add(new SqlParameter("cod_tdia ", input.CodTdiaDestino));
                command.Parameters.Add(new SqlParameter("codtpoHora ", input.TipoHoraDestino));
                command.Parameters.Add(new SqlParameter("CodMVueltasList ", DBNull.Value));
                command.Parameters.Add(new SqlParameter("CodBanderasList ", string.Join(",", input.BanderasId)));

                Context.Database.OpenConnection();
                await command.ExecuteNonQueryAsync();
            }
        }


        public async Task<string> CopiarMinutosAsync(CopiarHMinxtipoInput input)
        {
            if (input.TipoHoraDestino == "all")
            {
                input.TipoHoraDestino = null;
            }

            if (input.TipoHoraOrigen == "all")
            {
                input.TipoHoraOrigen = null;
            }
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = SqlObjects.sp_pla_CopiarMinutos;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("CodHfechaOrigen ", input.CodHfechaOrigen));
                command.Parameters.Add(new SqlParameter("CodTdiaOrigen ", input.CodTdiaOrigen));
                command.Parameters.Add(new SqlParameter("TipoHoraOrigen ", input.TipoHoraOrigen));
                command.Parameters.Add(new SqlParameter("CodHfechaDestino ", input.CodHfechaDestino));
                command.Parameters.Add(new SqlParameter("CodTdiaDestino ", input.CodTdiaDestino));
                command.Parameters.Add(new SqlParameter("TipoHoraDestino ", input.TipoHoraDestino));

                if (input.BanderasId.Any())
                {
                    command.Parameters.Add(new SqlParameter("BanderasList ", string.Join(",", input.BanderasId)));
                }
                else
                {
                    command.Parameters.Add(new SqlParameter("BanderasList ", DBNull.Value));
                }

                Context.Database.OpenConnection();
                var result = (await command.ExecuteScalarAsync()) as string;


                await this.RecrearSabanaBandera(input);


                if (input.BanderasId.Any())
                {
                    var banderasString = this.Context.HBanderas.Where(e => input.BanderasId.Contains(e.Id)).Select(e => e.AbrBan).ToArray();
                    return string.Join(",", banderasString);
                }

                return result;
            }
        }




        public async Task ImportarMinutos(ImportadorHMinxtipoResult planilla)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    List<HDetaminxtipo> list = new List<HDetaminxtipo>();

                    foreach (var item in planilla.HMinxtipoImportados)
                    {
                        foreach (var m in item.Detalles)
                        {
                            var detaMinPorTipo = await Context.HDetaminxtipo.Include(e => e.CodMinxtipoNavigation).FirstOrDefaultAsync(e => e.CodMinxtipo == item.Id_HMinxtipo && e.CodHsector == m.CodHsector);

                            detaMinPorTipo.Minuto = m.Minuto;

                            Context.Entry(detaMinPorTipo).State = EntityState.Modified;

                            list.Add(detaMinPorTipo);
                        }
                    }

                    await this.SaveChangesAsync();

                    foreach (var item in list.Select(e => e.CodMinxtipoNavigation).Distinct())
                    {
                        var minutos = list.Select(e => e.CodMinxtipo).Distinct().ToList();
                        await this.RecrearSabanaPorMinutos(item.CodHfecha, item.CodTdia, item.TipoHora, item.CodBan, minutos);
                    }



                    ts.Commit();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task SetHSectores(IEnumerable<HSectores> entities)
        {
            try
            {
                foreach (var item in entities)
                {
                    var updateteitem = this.Context.HSectores.Where(e => e.CodSec == item.CodSec && e.CodHsector == item.CodHsector).Single();
                    updateteitem.VerEnResumen = item.VerEnResumen;
                    //this.Context.Entry(item).State = EntityState.Modified;
                }
                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

    }
}
