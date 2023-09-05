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
using System.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Snickler.EFCore;
using Microsoft.EntityFrameworkCore.Storage;
using ROSBUS.Admin.Domain.Entities.Filters;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.infra.Data.Repositories
{
    public class PlaDistribucionDeCochesPorTipoDeDiaRepository : RepositoryBase<AdminContext,PlaDistribucionDeCochesPorTipoDeDia, int>, IPlaDistribucionDeCochesPorTipoDeDiaRepository
    {
        private readonly IHServiciosRepository _hServiciosRepository;
        private readonly ILogger logger;

        public PlaDistribucionDeCochesPorTipoDeDiaRepository(IAdminDbContext _context, IHServiciosRepository hServiciosRepository, ILogger logger)
            :base(new DbContextProvider<AdminContext>(_context))
        {
            _hServiciosRepository = hServiciosRepository;
            this.logger = logger;
        }

        public override Expression<Func<PlaDistribucionDeCochesPorTipoDeDia, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<PlaDistribucionDeCochesPorTipoDeDia> AddIncludeForGet(DbSet<PlaDistribucionDeCochesPorTipoDeDia> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(h => h.CodHfechaNavigation);
        }


        public async Task<PlaDistribucionEstadoView> ExistenMediasVueltasIncompletas(PlaDistribucionDeCochesPorTipoDeDia input) {

            if (input.Banderas==null)
            {
                input.Banderas = new List<int>();
            }

            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = Infra.Data.SqlObjects.sp_pla_ExistenMediasVueltasIncompletas  ;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("cod_hfecha", input.CodHfecha));
                command.Parameters.Add(new SqlParameter("cod_tdia", input.CodTdia));
                command.Parameters.Add(new SqlParameter("banderaList", string.Join(',', input.Banderas)));


                Context.Database.OpenConnection();
                var value = await command.ExecuteScalarAsync();

                PlaDistribucionEstadoView result = new PlaDistribucionEstadoView();

                result.Estado = (PlaDistribucionEstadoEnum)(value as int?).GetValueOrDefault();

                return result;
            }

        }

        public async Task<PlaDuracionesEstadoView> ExistenDuracionesIncompletas(PlaDistribucionDeCochesPorTipoDeDia item)
        {
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = Infra.Data.SqlObjects.sp_pla_ExistenDuracionesIncompletas;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("CodHfecha", item.CodHfecha));
                command.Parameters.Add(new SqlParameter("CodTdia", item.CodTdia));


                Context.Database.OpenConnection();
                var value = await command.ExecuteScalarAsync();

                PlaDuracionesEstadoView result = new PlaDuracionesEstadoView();

                result.Estado = (PlaDuracionesEstadoEnum)Enum.ToObject(typeof(PlaDuracionesEstadoEnum), value);

                return result;
            }
        }

        /// <summary>
        /// elimina h_proc_min y copia de h_detaminxtipo a h_proc_min, recrando la sabana teniendo en cuenta la salida
        /// </summary>
        public async Task RecrearSabanaSector(PlaDistribucionDeCochesPorTipoDeDia input)
        {
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                await this.logger.LogInformation("Inicio RecrearSabanaSector ");

                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                

                command.CommandTimeout = 600;
                command.CommandText = Infra.Data.SqlObjects.sp_pla_recrearHProcMin;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("cod_hfecha ", input.CodHfecha));
                command.Parameters.Add(new SqlParameter("cod_tdia ", input.CodTdia));
                command.Parameters.Add(new SqlParameter("codtpoHora ", DBNull.Value));
                command.Parameters.Add(new SqlParameter("CodMVueltasList ", DBNull.Value));
                command.Parameters.Add(new SqlParameter("CodBanderasList ", DBNull.Value));

                Context.Database.OpenConnection();
                await command.ExecuteNonQueryAsync();
                await this.logger.LogInformation("FIN RecrearSabanaSector ");
            }
        }






        public async Task ImportarServiciosAsync(ImportarServiciosInput input)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {




                    //var commandText = "INSERT Categories (CategoryName) VALUES (@CategoryName)";
                    //var name = new SqlParameter("@CategoryName", "Test");
                    //Context.Database.ExecuteSqlCommand(commandText, name);


                    using (var command = Context.Database.GetDbConnection().CreateCommand())
                    {
                        if (Context.Database.CurrentTransaction != null)
                            command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                        command.CommandText = Infra.Data.SqlObjects.pla_BorrarServiciosFueraDelRango ;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("CodHfecha ", (Object)input.CodHfecha ?? DBNull.Value));
                        command.Parameters.Add(new SqlParameter("CodTipoDia ", (Object)input.CodTdia ?? DBNull.Value));
                        command.Parameters.Add(new SqlParameter("desde", (Object)input.desde ?? DBNull.Value));
                        command.Parameters.Add(new SqlParameter("hasta", (Object)input.hasta ?? DBNull.Value));


                        Context.Database.OpenConnection();
                        command.ExecuteNonQuery();
                    }

                    //await Context.LoadStoredProc("dbo.sp_pla_BorrarServiciosFueraDelRango")
                    //          .WithSqlParam("CodHfecha ", new SqlParameter("CodHfecha ", (Object)input.CodHfecha ?? DBNull.Value))
                    //          .WithSqlParam("CodTipoDia ", new SqlParameter("CodTipoDia ", (Object)input.CodTdia ?? DBNull.Value))
                    //          .WithSqlParam("desde", new SqlParameter("desde", (Object)input.desde ?? DBNull.Value))
                    //          .WithSqlParam("hasta", new SqlParameter("hasta", (Object)input.hasta ?? DBNull.Value))
                    //          .ExecuteNonQueryAsync();
                    //         //.ExecuteStoredProcAsync()



                    var subgalpones = input.MediasVueltas.GroupBy(w => w.CodSubGalpon);
                    var maxIDHHorariosConfi = this.Context.HHorariosConfi.Max(e => e.Id) + 1;


                    //BUG FIX 1177
                    var maxIdServ = this.Context.HServicios.Max(r => r.Id) + 1;
                    var CodServicio = this.Context.HMediasVueltas.Max(r => r.CodServicio) + 1;

                    if (CodServicio > maxIdServ)
                    {
                        maxIdServ = CodServicio;
                    }

                    foreach (var subgalpon in subgalpones)
                    {
                        var horariosconfig = this.Context.HHorariosConfi.Where(e =>
                        e.CodTdia == input.CodTdia
                         && e.CodHfecha == input.CodHfecha
                        && e.CodSubg == subgalpon.Key
                        ).FirstOrDefault();

                        if (horariosconfig == null)
                        {

                            horariosconfig = new HHorariosConfi();
                            horariosconfig.Id = maxIDHHorariosConfi;
                            maxIDHHorariosConfi++;
                            horariosconfig.CodSubg = subgalpon.Key;
                            horariosconfig.CodHfecha = input.CodHfecha.Value;
                            horariosconfig.CodTdia = input.CodTdia.Value;
                            
                            this.Context.HHorariosConfi.Add(horariosconfig);

                            await this.SaveChangesAsync();
                        }

                        var servicios = subgalpon.GroupBy(w =>  w.NumServicio ); 

                        var maxIdMediavuelta = this.Context.HMediasVueltas.Max(r => r.Id) + 1;

                        foreach (var servicio in servicios)
                        {
                            var hs = new HServicios();
                            
                            hs.Id = maxIdServ;
                            maxIdServ++;
                            hs.CodHconfiNavigation = horariosconfig;
                            hs.Duracion = 0;
                            hs.NroInterno = null;
                            hs.NumSer = servicio.Key.ToString().PadLeft(4, '0');


                            this.Context.HServicios.Add(hs);

                            await this.SaveChangesAsync();

                            foreach (var mediavuelta in servicio)
                            {
                                var mv = new HMediasVueltas();
                                mv.Id = maxIdMediavuelta;
                                maxIdMediavuelta++;
                                mv.CodServicioNavigation = hs;
                                mv.CodTpoHora = mediavuelta.CodTpoHora;
                                mv.CodBan = mediavuelta.CodBan;
                                mv.Sale = mediavuelta.Sale;
                                mv.Llega = mediavuelta.Llega;
                                mv.DifMin = Convert.ToDecimal((mv.Llega - mv.Sale).TotalMinutes);
                                mv.Orden = mediavuelta.OrdenImportado;
                                hs.HMediasVueltas.Add(mv);
                            }

                            await this.SaveChangesAsync();

                        }
                    }




                    //Guardamos los Carteles

                    if (input.BolBanderasCartel!=null)
                    {
                        var entity = await this.Context.BolBanderasCartel.FirstOrDefaultAsync(e => e.Id == input.BolBanderasCartel.Id);
                        EntityState state = EntityState.Modified;
                        if (entity == null)
                        {

                            entity = new BolBanderasCartel() { Id = this.Context.BolBanderasCartel.Max(e => e.Id) + 1, CodHfecha = input.BolBanderasCartel.CodHfecha, CodLinea = input.BolBanderasCartel.CodLinea };
                            //Busco la row en la cual modifico en ultimo numero en sys_ultimosNumeros
                            var ultimoNumero = this.Context.SysUltimosNumeros.Where(w => w.Tabla == "bol_banderascartel").FirstOrDefault();
                            //Realizo el insert en BolBanderasCartel y al id le asigno el ultimo numero de la tabla sys_ultimosNumeros + 1
                            entity = new BolBanderasCartel() { Id = (ultimoNumero.UltNumero + 1), CodHfecha = input.BolBanderasCartel.CodHfecha, CodLinea = input.BolBanderasCartel.CodLinea };
                            //Actualizo el row de ultimo numero en la tabla sys_ultimosNumeros
                            ultimoNumero.UltNumero = (ultimoNumero.UltNumero + 1);
                            //Cambio el estado de ultimoNumero a modificado.
                            this.Context.Entry(ultimoNumero).State = EntityState.Modified;
                            state = EntityState.Added;
                        }
                        entity.CodLinea = input.CodLinea.GetValueOrDefault();
                        this.Context.Entry(entity).State = state;

                        foreach (var detalle in input.BolBanderasCartel.BolBanderasCartelDetalle)
                        {
                            var detalleEntity = this.Context.BolBanderasCartelDetalle.FirstOrDefault(e => e.CodBanderaCartel == entity.Id && e.CodBan == detalle.CodBan);
                            state = EntityState.Modified;
                            if (detalleEntity==null)
                            {
                                state = EntityState.Added;
                                detalleEntity = new BolBanderasCartelDetalle();
                                entity.BolBanderasCartelDetalle.Add(detalleEntity);
                            }

                            detalleEntity.CodBan = detalle.CodBan;
                            detalleEntity.CodBanderaCartel = entity.Id;
                            detalleEntity.Movible = detalle.Movible;
                            detalleEntity.NroSecuencia = detalle.NroSecuencia;
                            detalleEntity.ObsBandera = detalle.ObsBandera;
                            detalleEntity.TextoBandera = detalle.TextoBandera;
                            this.Context.Entry(detalleEntity).State = state;
                        }

                    }

                    

                    await this.SaveChangesAsync();


                    await _hServiciosRepository.CrearSectores(input.CodHfecha.Value, input.MediasVueltas.Select(e => e.CodBan).Distinct().ToList());

                    await _hServiciosRepository.RecrearMinutosPorSectorTemplete(input.CodHfecha.Value, input.CodTdia.Value);

                    //TODO:Podriamos pasar los ids de las medias vueltas que fueron generadas
                    await _hServiciosRepository.RecrearSabanaSector(input.CodHfecha.Value, input.CodTdia.Value);




                    ts.Commit();
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public async Task<bool> TieneMinutosAsignados(PlaDistribucionDeCochesPorTipoDeDiaFilter filter)
        {
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = Infra.Data.SqlObjects.sp_pla_DistribucionDeCochesPorTipoDeDia_TieneHorarioAsignado;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("Id_DistribucionDeCochesPorTipoDeDia", filter.Id));


                Context.Database.OpenConnection();
                var retult = await command.ExecuteScalarAsync();
                return (retult as Boolean?).GetValueOrDefault();
            }
        }


    }
}
