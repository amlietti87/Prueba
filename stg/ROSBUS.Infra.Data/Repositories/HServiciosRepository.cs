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
using TECSO.FWK.Domain.Interfaces.Entities;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.Entities.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ROSBUS.Infra.Data;

namespace ROSBUS.infra.Data.Repositories
{
    public class HServiciosRepository : RepositoryBase<AdminContext, HServicios, int>, IHServiciosRepository
    {

        public HServiciosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<HServicios, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        protected override IQueryable<HServicios> AddIncludeForGet(DbSet<HServicios> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e => e.HMediasVueltas);
        }


        public async Task<List<ItemDto>> RecuperarServiciosPorLinea(HServiciosFilter Filtro)
        {
            List<ItemDto> itemDtos = new List<ItemDto>();


            if (!Filtro.LineaId.HasValue)
            {
                throw new ArgumentException("Falta Linea ");
            }

            if (!(Filtro.Fecha.HasValue || (Filtro.CodTdia.HasValue && Filtro.CodHfecha.HasValue)))
            {
                throw new ArgumentException("Falta Fecha o el tipo de dia junto con el codHFecha ");
            }


            var sp = this.Context.LoadStoredProc("dbo.sp_HServicios_RecuperarServiciosPorLinea")
                .WithSqlParam("cod_lin", new SqlParameter("cod_lin", Filtro.LineaId))                
                .WithSqlParam("fecha", new SqlParameter("fecha", Filtro.Fecha.GetValueOrDefault(DateTime.Today).Date))
                .WithSqlParam("conductorId", new SqlParameter("conductorId", (object)Filtro.ConductorId ?? DBNull.Value))
                .WithSqlParam("CodHfecha", new SqlParameter("CodHfecha", (object)Filtro.CodHfecha ?? DBNull.Value))
                .WithSqlParam("codTipoDia", new SqlParameter("codTipoDia", (object)Filtro.CodTdia ?? DBNull.Value));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<ItemDto>().ToList();
            });

            return itemDtos;
        }

        public async Task<List<ItemDto<string>>> RecuperarConductoresPorServicio(HServiciosFilter Filtro)
        {
                List<ItemDto<string>> itemDtos = new List<ItemDto<string>>();


            if (!Filtro.LineaId.HasValue)
            {
                throw new ArgumentException("Falta Linea ");
            }

            if (!Filtro.Fecha.HasValue)
            {
                throw new ArgumentException("Falta Linea ");
            }

            var sp = this.Context.LoadStoredProc("dbo.sp_HServicios_RecuperarConductoresPorServicio")
                .WithSqlParam("cod_lin", new SqlParameter("cod_lin", Filtro.LineaId))
                .WithSqlParam("fecha", new SqlParameter("fecha", Filtro.Fecha.GetValueOrDefault(DateTime.Today).Date))
                .WithSqlParam("cod_servicio", new SqlParameter("cod_servicio", (object)Filtro.ServicioId ?? DBNull.Value));
            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<ItemDto<string>>().ToList();
            });

            return itemDtos;
        }

        public async Task<List<ConductoresLegajoDto>> RecuperarConductores(HServiciosFilter Filtro)
        {
           var itemDtos = new List<ConductoresLegajoDto>();


            var sp = this.Context.LoadStoredProc("dbo.sp_Insp_RecuperarConductores")
                .WithSqlParam("NombreConductor", new SqlParameter("NombreConductor", (object)Filtro.Nombre ??DBNull.Value))
                .WithSqlParam("fecha", new SqlParameter("fecha", (object)Filtro.Fecha ?? DBNull.Value))
                .WithSqlParam("UserIdInspector", new SqlParameter("UserIdInspector", (object)Filtro.UserIdInspector ?? DBNull.Value));
            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<ConductoresLegajoDto>().ToList();
            });

            return itemDtos;
        }


        /// <summary>
        /// Crea los h_minxtipo y h_detaminxtipo  faltantes en minutos Media vuelta (min en null) y elimina los mismos que ya no existan por minutos Media vuelta   
        /// </summary>        
        public async Task RecrearMinutosPorSectorTemplete(HHorariosConfi entity)
        {
            await RecrearMinutosPorSectorTemplete(entity.CodHfecha, entity.CodTdia);
        }


        public async Task RecrearMinutosPorSectorTemplete(int CodHfecha, int CodTdia)
        {
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = SqlObjects.sp_pla_RecrearMinutosPorSector;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("cod_hfecha ", CodHfecha));
                command.Parameters.Add(new SqlParameter("cod_tdia ", CodTdia));

                Context.Database.OpenConnection();
                await command.ExecuteNonQueryAsync();
            }
        }







        /// <summary>
        /// elimina h_proc_min y copia de h_minxtipo a h_proc_min, recrando la sabana teniendo en cuenta la salida
        /// </summary>
        public async Task RecrearSabanaSector(HHorariosConfi entity, IEnumerable<int> MediasVueltasAActualizar)
        {
            await this.RecrearSabanaSector(entity.CodHfecha, entity.CodTdia, MediasVueltasAActualizar);
        }

        public async Task RecrearSabanaSector(int CodHfecha, int CodTdia, IEnumerable<int> MediasVueltasAActualizar= null)
        {
            if (MediasVueltasAActualizar==null)
            {
                MediasVueltasAActualizar = new List<int>();
            }

            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandTimeout = 120;

                command.CommandText = SqlObjects.sp_pla_recrearHProcMin;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("cod_hfecha ", CodHfecha));
                command.Parameters.Add(new SqlParameter("cod_tdia", CodTdia));
                command.Parameters.Add(new SqlParameter("codtpoHora", DBNull.Value));
                command.Parameters.Add(new SqlParameter("CodMVueltasList", MediasVueltasAActualizar.Any() ? string.Join(",", MediasVueltasAActualizar) : DBNull.Value as object));
                command.Parameters.Add(new SqlParameter("CodBanderasList", DBNull.Value));

                Context.Database.OpenConnection();
                await command.ExecuteNonQueryAsync();
            }
        }




        public override async Task<HServicios> UpdateAsync(HServicios entity)
        {
            try
            {
                //using (var ts = await this.Context.Database.BeginTransactionAsync())
                //{
                Context.Entry(entity).State = EntityState.Modified;
                return entity;
                //}                    
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        /// <summary>
        /// Elimina los registros de h_choxser 
        /// </summary>
        /// <param name="enumerable"></param>
        /// <returns></returns>
        private async Task EliminarDuracionServicio(HServicios servicio)
        {
            try
            {
                if (servicio.Id > 0)
                {
                    Context.HChoxser.RemoveRange(Context.HChoxser.Where(a => a.Id == servicio.Id));
                    await Context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }
        }


        /// <summary>
        /// elimina los detalles de una mediavuelta que tiene que ser actualizada
        /// </summary>
        /// <param name="IdMediaVueltas">CodMvuelta</param>
        public async Task EliminarMinutosPorSectorMediaVueltaAsync(IEnumerable<int> IdMediaVueltas)
        {
            try
            {
                if (IdMediaVueltas.Any())
                {
                    Context.HProcMin.RemoveRange(Context.HProcMin.Where(a => IdMediaVueltas.Contains(a.CodMvuelta)));
                    await Context.SaveChangesAsync();
                }

            }
            catch (Exception)
            {

                throw;
            }

        }


        public async Task CrearSectores(HHorariosConfi entity, List<int> CodBan)
        {
            await this.CrearSectores(entity.CodHfecha, CodBan);
        }

        public async Task CrearSectores(int CodHfecha, List<int> CodBan)
        {
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {
                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = SqlObjects.sp_pla_CrearSectores;
                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("cod_hfecha ", CodHfecha));
                command.Parameters.Add(new SqlParameter("CodBanderasList ", string.Join(",", CodBan)));


                Context.Database.OpenConnection();
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task RecrearMinutosPorSector(HHorariosConfi entity, HServicios servicioEntity, IEnumerable<int> MediasVueltasAActualizar, IEnumerable<int> MediasVueltasAEliminar, HChoxserExtendedDto Duracion = null)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    if (entity.Id <=0 )
                    {

                        entity.Id = Context.HHorariosConfi.Max(e => e.Id) + 1;
                        Context.Entry(entity).State = EntityState.Added;
                        await Context.SaveChangesAsync();
                        servicioEntity.CodHconfi = entity.Id;
                        
                    }
                    else
                    {
                        Context.Entry(entity).State = EntityState.Modified;
                    }
                    

                    var mxaMV = Context.HMediasVueltas.Max(a => a.Id);
                    foreach (var mvNueva in servicioEntity.HMediasVueltas.Where(w => w.Id <= 0))
                    {
                        mvNueva.Id = ++mxaMV;
                        if (servicioEntity.Id <= 0)
                        {
                            mvNueva.CodServicioNavigation = servicioEntity;
                        }

                    }

                    //await Context.SaveChangesAsync();

                    await this.EliminarMinutosPorSectorMediaVueltaAsync(MediasVueltasAActualizar.Union(MediasVueltasAEliminar));

                    //await this.EliminarDuracionServicio(servicioEntity);

                    if (servicioEntity.Id <= 0)
                    {
                        var mxaHS = Context.HServicios.Max(a => a.Id);

                        servicioEntity.Id = ++mxaHS;
                        await Context.HServicios.AddAsync(servicioEntity);
                    }
                    else
                    {
                        Context.Entry(servicioEntity).State = EntityState.Modified;
                    }


                    await Context.SaveChangesAsync();

                    await this.CrearSectores(entity, servicioEntity.HMediasVueltas.Select(e => e.CodBan).Distinct().ToList());
                    //llamar para crear nuevas banderas en h_basec

                    await this.RecrearMinutosPorSectorTemplete(entity);
                    await this.RecrearSabanaSector(entity, MediasVueltasAActualizar);

                    if (Duracion != null && servicioEntity != null)
                    {
                        await this.AddOrUpdateDuracion(Duracion, servicioEntity);
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

        

        public override async Task DeleteAsync(int id)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    HServicios entity = Context.HServicios.Include(e => e.CodHconfiNavigation).Include(e => e.HMediasVueltas).ThenInclude(t => t.HProcMin).Single(e => e.Id == id);

                    foreach (var mv in entity.HMediasVueltas.Where(e => e.CodBan == entity.Id))
                    {

                        foreach (var hproc in mv.HProcMin)
                        {
                            Context.Set<HProcMin>().Remove(hproc);
                        }
                        Context.Set<HMediasVueltas>().Remove(mv);
                    }

                    Context.Set<HServicios>().Remove(entity);


                    await this.EliminarDuracionServicio(entity);

                    await this.SaveChangesAsync(); 
                    await this.RecrearMinutosPorSectorTemplete(entity.CodHconfiNavigation);
                    ts.Commit();
                } 
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        private async Task AddOrUpdateDuracion(HChoxserExtendedDto duracion, HServicios hServicio)
        {
            try
            {
                //using (var ts = await this.Context.Database.BeginTransactionAsync())
                //{
                    if (duracion.Sale != null)
                    {
                        var duracionTitular = this.Context.HChoxser.Where(e => e.Id == hServicio.Id && e.CodUni == 1).FirstOrDefault();
                        if (duracionTitular != null)
                        {
                            duracionTitular.Sale = duracion.Sale.GetValueOrDefault();
                            duracionTitular.SalePlanificado = duracion.Sale.GetValueOrDefault();
                            duracionTitular.Llega = duracion.Llega.GetValueOrDefault();
                            duracionTitular.LlegaPlanificado = duracion.Llega.GetValueOrDefault();
                            duracionTitular.DuracionPlanificada = Convert.ToInt32((duracion.Llega.GetValueOrDefault() - duracion.Sale.GetValueOrDefault()).TotalMinutes);
                            this.Context.Entry(duracionTitular).State = EntityState.Modified;
                        }
                        else
                        {
                            HChoxser Titular = new HChoxser();
                            Titular.Id = hServicio.Id;
                            Titular.CodUni = 1;
                            Titular.CodEmp = "";
                            Titular.Sale = duracion.Sale.GetValueOrDefault();
                            Titular.Llega = duracion.Llega.GetValueOrDefault();
                            Titular.SalePlanificado = Titular.Sale;
                            Titular.LlegaPlanificado = Titular.Llega;
                            Titular.DuracionPlanificada = Convert.ToInt32((Titular.Llega - Titular.Sale).TotalMinutes);
                            Context.HChoxser.Add(Titular);
                        }
                        
                    }

                    if (duracion.SaleRelevo != null)
                    {
                        var duracionRelevo = this.Context.HChoxser.Where(e => e.Id == hServicio.Id && e.CodUni == 2).FirstOrDefault();
                        if (duracionRelevo != null)
                        {
                            duracionRelevo.Sale = duracion.SaleRelevo.GetValueOrDefault();
                            duracionRelevo.SalePlanificado = duracion.SaleRelevo.GetValueOrDefault();
                            duracionRelevo.Llega = duracion.LlegaRelevo.GetValueOrDefault();
                            duracionRelevo.LlegaPlanificado = duracion.LlegaRelevo.GetValueOrDefault();
                            duracionRelevo.DuracionPlanificada = Convert.ToInt32((duracion.LlegaRelevo.GetValueOrDefault() - duracion.SaleRelevo.GetValueOrDefault()).TotalMinutes);
                            this.Context.Entry(duracionRelevo).State = EntityState.Modified;
                        }
                        else
                        {
                            HChoxser Relevo = new HChoxser();
                            Relevo.Id = hServicio.Id;
                            Relevo.CodUni = 2;
                            Relevo.CodEmp = "";
                            Relevo.Sale = duracion.SaleRelevo.GetValueOrDefault();
                            Relevo.Llega = duracion.LlegaRelevo.GetValueOrDefault();
                            Relevo.SalePlanificado = Relevo.Sale;
                            Relevo.LlegaPlanificado = Relevo.Llega;
                            Relevo.DuracionPlanificada = Convert.ToInt32((Relevo.Llega - Relevo.Sale).TotalMinutes);
                            Context.HChoxser.Add(Relevo);
                        }

                    }

                    if (duracion.SaleAuxiliar != null)
                    {
                        var duracionAuxiliar = this.Context.HChoxser.Where(e => e.Id == hServicio.Id && e.CodUni == 3).FirstOrDefault();
                        if (duracionAuxiliar != null)
                        {
                            duracionAuxiliar.Sale = duracion.SaleAuxiliar.GetValueOrDefault();
                            duracionAuxiliar.SalePlanificado = duracion.SaleAuxiliar.GetValueOrDefault();
                            duracionAuxiliar.Llega = duracion.LlegaAuxiliar.GetValueOrDefault();
                            duracionAuxiliar.LlegaPlanificado = duracion.LlegaAuxiliar.GetValueOrDefault();
                            duracionAuxiliar.DuracionPlanificada = Convert.ToInt32((duracion.LlegaAuxiliar.GetValueOrDefault() - duracion.SaleAuxiliar.GetValueOrDefault()).TotalMinutes);
                            this.Context.Entry(duracionAuxiliar).State = EntityState.Modified;
                        }
                        else
                        {
                            HChoxser Auxiliar = new HChoxser();
                            Auxiliar.Id = hServicio.Id;
                            Auxiliar.CodUni = 3;
                            Auxiliar.CodEmp = "";
                            Auxiliar.Sale = duracion.SaleAuxiliar.GetValueOrDefault();
                            Auxiliar.Llega = duracion.LlegaAuxiliar.GetValueOrDefault();
                            Auxiliar.SalePlanificado = Auxiliar.Sale;
                            Auxiliar.LlegaPlanificado = Auxiliar.Llega;
                            Auxiliar.DuracionPlanificada = Convert.ToInt32((Auxiliar.Llega - Auxiliar.Sale).TotalMinutes);
                            Context.HChoxser.Add(Auxiliar);
                        }
                        
                    }

                    await this.SaveChangesAsync();

                    //ts.Commit();

                //}
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public async Task<List<ItemDto<Decimal>>> RecuperarLineasPorConductor(HServiciosFilter Filtro)
        {
            
            List<ItemDto<Decimal>> itemDtos = new List<ItemDto<Decimal>>();

            var fecha = Filtro.Fecha?.Date;

            var sp = this.Context.LoadStoredProc("dbo.sp_Insp_RecuperarLineasPorConductor")
                .WithSqlParam("conductorId", new SqlParameter("conductorId", Filtro.ConductorId ))
                .WithSqlParam("Fecha", new SqlParameter("Fecha", Filtro.Fecha.GetValueOrDefault(DateTime.Today).Date))
                .WithSqlParam("UserIdInspector", new SqlParameter("UserIdInspector", (object)Filtro.UserIdInspector ?? DBNull.Value));
            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<ItemDto<Decimal>>().ToList();
            });

            return itemDtos;
        }
    }
}
