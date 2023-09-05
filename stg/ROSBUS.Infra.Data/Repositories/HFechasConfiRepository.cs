using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Model;
using ROSBUS.Admin.Domain.Model.Reportes;
using ROSBUS.infra.Data.Contexto;
using Snickler.EFCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Extensions;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.infra.Data.Repositories
{
    public class HFechasConfiRepository : RepositoryBase<AdminContext, HFechasConfi, int>, IHFechasConfiRepository
    {
        private readonly IHServiciosRepository _hServiciosRepository;

        public HFechasConfiRepository(IAdminDbContext _context, IHServiciosRepository hServiciosRepository)
            : base(new DbContextProvider<AdminContext>(_context))
        {
            _hServiciosRepository = hServiciosRepository;
        }


        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var dic = base.GetMachKeySqlException();
            dic.Add("FK_pla_DistribucionDeCochesPorTipoDeDia_h_fechas_confi", "No se puede Eliminar porque tiene relacion a la tabla de Estimaciones por Tipo de dia");
            return dic;
        }

        public override Expression<Func<HFechasConfi, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<HFechasConfi> GetIncludesForPageList(IQueryable<HFechasConfi> query)
        {
            return query
                .Include(a => a.HHorariosConfi)
                .ThenInclude(p => p.CodTdiaNavigation)
                .Include(b => b.PlaEstadoHorarioFecha);
        }

        protected override IQueryable<HFechasConfi> AddIncludeForGet(DbSet<HFechasConfi> dbSet)
        {
            var query = base.AddIncludeForGet(dbSet)
                .Include(e => e.PlaDistribucionDeCochesPorTipoDeDia).ThenInclude(e => e.CodTdiaNavigation)
                .Include(e => e.HBasec).ThenInclude(e => e.CodBanNavigation).ThenInclude(e => e.SentidoBandera)
                .Include(e => e.HBasec).ThenInclude(e => e.CodRecNavigation);

            return query;
        }


        public override async Task<HFechasConfi> UpdateAsync(HFechasConfi entity)
        {

            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    foreach (var item in entity.PlaDistribucionDeCochesPorTipoDeDia)
                    {
                        if (item.Id < 0)
                        {
                            item.Id = 0;
                        }
                    }

                    foreach (PlaDistribucionDeCochesPorTipoDeDia item in Context.ChangeTracker.Entries().Where(e => e.Entity is PlaDistribucionDeCochesPorTipoDeDia).Select(e => e.Entity))
                    {
                        if (item.Id > 0 && item.CodHfechaNavigation == null)
                        {
                            item.CodHfecha = 0;
                            Context.Entry(item).State = EntityState.Deleted;

                        }
                    }


                    var original = await Context.HFechasConfi.AsNoTracking().FirstOrDefaultAsync(e => e.Id == entity.Id);

                    if (original.FecDesde != entity.FecDesde)
                    {
                        var anterior = Context.HFechasConfi.Where(e => e.CodLin == original.CodLin && e.FecHasta < original.FecDesde).OrderByDescending(e => e.FecDesde).Take(1).FirstOrDefault();

                        if (anterior.FecDesde >= entity.FecDesde)
                        {
                            throw new DomainValidationException("No se permite modificar la fecha desde menor a la Fecha desde del horario anterior");
                        }

                        if (anterior != null)
                        {
                            anterior.FecHasta = entity.FecDesde.AddDays(-1);
                        }

                        if (original.FecDesde  > entity.FecDesde)
                        {

                            await this.ValidarDesignar(entity.FecDesde, original.FecDesde,  entity.CodLin);
                        }
                        if (original.FecDesde < entity.FecDesde)
                        {

                            await this.ValidarDesignar(original.FecDesde, entity.FecDesde, entity.CodLin);
                        }

                    }



                    Context.Entry(entity).State = EntityState.Modified;

                    await this.SaveChangesAsync();

                    await  UpdateKilometrosPorhbasec(entity);

                     await UpdateCartelesPorhbasec(entity);

                    await this.ValidarTimeLineHorario(entity.CodLin);





                    ts.Commit();
                    return entity;
                }
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }




        }


        private async Task UpdateCartelesPorhbasec(HFechasConfi entity)
        {
            var carteles = await this.Context.BolBanderasCartel.Include(e => e.BolBanderasCartelDetalle).Where(e => e.CodHfecha == entity.Id).FirstOrDefaultAsync();
           
            if (carteles!=null)
            {
                foreach (var deteallecartel in carteles.BolBanderasCartelDetalle)
                {
                    var hbasec = entity.HBasec.FirstOrDefault(e => e.CodBan == deteallecartel.CodBan);
                    if (hbasec != null)
                    {
                        Boolean hasChanges = false;

                        if (deteallecartel.NroSecuencia != hbasec.NroSecuencia)
                        {
                            deteallecartel.NroSecuencia = hbasec.NroSecuencia.Value;
                            hasChanges = true;
                        }

                        if (deteallecartel.TextoBandera != hbasec.TextoBandera)
                        {
                            deteallecartel.TextoBandera = hbasec.TextoBandera;
                            hasChanges = true;
                        }

                        if (deteallecartel.Movible != hbasec.Movible)
                        {
                            deteallecartel.Movible = hbasec.Movible;
                            hasChanges = true;
                        }

                        if (deteallecartel.ObsBandera != hbasec.ObsBandera)
                        {
                            deteallecartel.ObsBandera = hbasec.ObsBandera;
                            hasChanges = true;
                        }

                        
                        if (hasChanges)
                        {
                            await this.SaveChangesAsync();
                        }
                    }
                }
            }
            else
            {
                var sysultimosnumerosbolban = (await Context.SysUltimosNumeros.FirstAsync(e => e.Tabla == "bol_banderasCartel"));
                var ultimoNumero = sysultimosnumerosbolban?.UltNumero;
  
                ultimoNumero = ultimoNumero ?? 0;
               

                carteles = new BolBanderasCartel()
                {
                    CodHfecha = entity.Id,
                    Id = ultimoNumero.Value + 1,
                    CodLinea = Convert.ToInt32(entity.CodLin),
                    BolBanderasCartelDetalle = new List<BolBanderasCartelDetalle>()
                };

                entity.HBasec.ToList().ForEach(e =>
                {
                    if (e.NroSecuencia.HasValue || !string.IsNullOrEmpty(e.TextoBandera) || !string.IsNullOrEmpty(e.Movible) || !string.IsNullOrEmpty(e.ObsBandera)){

                        BolBanderasCartelDetalle detalleCartel = new BolBanderasCartelDetalle()
                        {
                            Movible = e.Movible,
                            NroSecuencia = e.NroSecuencia.GetValueOrDefault(0),
                            ObsBandera = e.ObsBandera,
                            TextoBandera = e.TextoBandera,                            
                            CodBan = e.CodBan,
                        };
                        carteles.BolBanderasCartelDetalle.Add(detalleCartel);
                    }
                });

                sysultimosnumerosbolban.UltNumero = carteles.Id;
                this.Context.BolBanderasCartel.Add(carteles);
                this.Context.Entry(sysultimosnumerosbolban).State = EntityState.Modified;

                await this.SaveChangesAsync();
            }


        }

        private async Task UpdateKilometrosPorhbasec(HFechasConfi entity)
        { 
            var kmlist =  await this.GetKilometrosAsync
                (entity.HBasec.Select(e => e.CodBan).Distinct().ToList(), 
                entity.HBasec.Select(e => e.CodSec).Distinct().ToList());

            foreach (var kilometro in kmlist)
            {
                var hbasec = entity.HBasec.FirstOrDefault(e => e.CodBan == kilometro.CodBan && e.CodSec == kilometro.CodSec);
                if (hbasec != null)
                {
                    if (kilometro.Kmr != hbasec.Kmr ||
                    kilometro.Km != hbasec.Km ||
                    kilometro.CodBanderaColor != hbasec.CodBanderaColor ||
                    kilometro.CodBanderaTup != hbasec.CodBanderaTup)
                    {
                        Context.Entry(kilometro).State = EntityState.Deleted;
                        await this.SaveChangesAsync();

                        var newk = new HKilometros
                        {
                            CodBan = kilometro.CodBan,
                            CodSec = kilometro.CodSec,
                            Kmr = hbasec.Kmr,
                            Km = hbasec.Km,
                            CodBanderaColor = hbasec.CodBanderaColor,
                            CodBanderaTup = hbasec.CodBanderaTup
                        };


                        //if (kilometros.Kmr != hbasec.Kmr)
                        //    kilometros.Kmr = hbasec.Kmr;
                        //if (kilometros.Km != hbasec.Km)
                        //    kilometros.Km = hbasec.Km;
                        //if (kilometros.CodBanderaColor != hbasec.CodBanderaColor)
                        //    kilometros.CodBanderaColor = hbasec.CodBanderaColor;
                        //if (kilometros.CodBanderaTup != hbasec.CodBanderaTup)
                        //    kilometros.CodBanderaTup = hbasec.CodBanderaTup;

                        //this.SaveChangesAsync();

                        Context.Entry(newk).State = EntityState.Added;
                        await this.SaveChangesAsync();

                    }








                }
            }
        }

        public async Task ValidarTimeLineHorario(decimal CodLin)
        {


            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {

                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = Infra.Data.SqlObjects.sp_pla_validarTimeLineHorario;

                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("CodLin", CodLin));

                Context.Database.OpenConnection();
                var retult = await command.ExecuteScalarAsync();

                if ((retult as Boolean?).GetValueOrDefault())
                {
                    throw new DomainValidationException("No se puede procesar la solicitud, se generan inconsistencias en el horario");
                }

            }
        }



        public async Task<List<string>> ObtenerDestinatarios(decimal CodLin)
        {

            List<DestinatarioMail> destinatarios = new List<DestinatarioMail>();

            var sp = this.Context.LoadStoredProc("dbo.sp_GetDestinatariosHFechasConfi")
                .WithSqlParam("CodLin", new SqlParameter("CodLin", CodLin));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                destinatarios = handler.ReadToList<DestinatarioMail>()?.ToList();
            });

            return destinatarios.Select(e => e.destinatariosMail).ToList();
        }

        public async Task<List<PlaHorarioFechaLineaListView>> GetLineasHorarias()
        {
            //TODO: pasar sp;
            List<PlaHorarioFechaLineaListView> listViews = new List<PlaHorarioFechaLineaListView>();

            var lineas = await this.Context.Linea.Where(e => !e.IsDeleted && e.Activo.GetValueOrDefault()).OrderByDescending(e => e.LastUpdatedDate).ToListAsync();
            var fechaactual = DateTime.Now;

            foreach (var item in lineas)
            {
                PlaHorarioFechaLineaListView listView = new PlaHorarioFechaLineaListView();
                listView.CodLinea = item.Id;
                listView.DescripcionLinea = item.DesLin;
                //TODO: buscar esta informacion
                var hfechaconfig = this.Context.HFechasConfi.Where(e => e.CodLin == item.Id && (e.FecHasta == null || e.FecHasta >= fechaactual)).OrderByDescending(e => e.FecDesde).Take(1).FirstOrDefault();
                if (hfechaconfig != null)
                {
                    listView.FechaUltimaModificacion = hfechaconfig.FecDesde;
                }

                listView.Activo = listView.FechaUltimaModificacion.HasValue;

                listViews.Add(listView);
            }

            return listViews.OrderByDescending(e => e.FechaUltimaModificacion).ToList();
        }

        public async Task<ItemDto> CopiarHorario(int cod_hfecha, DateTime fec_desde, bool CopyConductores)
        {
            IList<ItemDto> items = new List<ItemDto>();


            try
            {
                int? cod_hfechaNew;
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    using (var command = Context.Database.GetDbConnection().CreateCommand())
                    {
                        if (Context.Database.CurrentTransaction != null)
                            command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                        command.CommandText = Infra.Data.SqlObjects.pla_CopiarHorario;
                        command.CommandType = System.Data.CommandType.StoredProcedure;

                        command.Parameters.Add(new SqlParameter("cod_hfecha", cod_hfecha));
                        command.Parameters.Add(new SqlParameter("fec_desde", fec_desde));
                        command.Parameters.Add(new SqlParameter("CopyConductores", CopyConductores));

                        Context.Database.OpenConnection();
                        var retult = await command.ExecuteScalarAsync();
                        cod_hfechaNew = (retult as int?);

                    }

                    if (!cod_hfechaNew.HasValue)
                    {
                        throw new ValidationException("No se pudo realizar el coopiado.");
                    }

                    var list = this.Context.PlaDistribucionDeCochesPorTipoDeDia.Where(e => e.CodHfecha == cod_hfechaNew);


                    var entity = Context.HFechasConfi.Where(e => e.Id == cod_hfechaNew).FirstOrDefault();


                    var anterior = Context.HFechasConfi.Where(e => e.CodLin == entity.CodLin && (e.FecDesde < entity.FecDesde)).OrderByDescending(e => e.FecDesde).Take(1).FirstOrDefault();

                    if (anterior != null)
                    {
                        entity.FecHasta = anterior.FecHasta;
                        anterior.FecHasta = entity.FecDesde.AddDays(-1);

                        await this.ValidarDesignar(entity.FecDesde, entity.FecHasta, entity.CodLin);
                    }
                    else
                    {
                        var posterior = Context.HFechasConfi.Where(e => e.CodLin == entity.CodLin && e.FecDesde > entity.FecDesde).OrderBy(e => e.FecDesde).Take(1).FirstOrDefault();
                        if (posterior != null)
                        {
                            entity.FecHasta = posterior.FecDesde.AddDays(-1);
                            
                        }
                        await this.ValidarDesignar(entity.FecDesde, entity.FecHasta, entity.CodLin);
                    }


                    await Context.SaveChangesAsync();

                    foreach (var item in list)
                    {
                        await this._hServiciosRepository.RecrearMinutosPorSectorTemplete(item.CodHfecha, item.CodTdia);
                    }

                    await this.ValidarTimeLineHorario(entity.CodLin);

                    ts.Commit();

                    return new ItemDto(cod_hfechaNew.GetValueOrDefault(), "");
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
            finally
            {

            }

        }

        private async Task ValidarDesignar(DateTime fechaDesde, DateTime? fechaHasta, decimal codLinea)
        {
            using (var command = Context.Database.GetDbConnection().CreateCommand())
            {

                if (Context.Database.CurrentTransaction != null)
                    command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                command.CommandText = Infra.Data.SqlObjects.sp_pla_FechaHorarioDiagramado;

                command.CommandType = System.Data.CommandType.StoredProcedure;

                command.Parameters.Add(new SqlParameter("fecha_d ", fechaDesde));
                command.Parameters.Add(new SqlParameter("fecha_h ", (object)fechaHasta?? DBNull.Value));
                command.Parameters.Add(new SqlParameter("cod_lin", codLinea));

                Context.Database.OpenConnection();
                var retult = await command.ExecuteScalarAsync();

                if ((retult as Boolean?).GetValueOrDefault())
                {
                    throw new DomainValidationException("No se puede procesar la solicitud, la linea esta diagramada.");
                }

            }

        }

        public async override Task<HFechasConfi> AddAsync(HFechasConfi entity)
        {

            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    entity.Id = (await this.Context.HFechasConfi.MaxAsync(m => m.Id)) + 1;
                    DbSet<HFechasConfi> dbSet = Context.Set<HFechasConfi>();

                    entity.BeforeMigration = false;

                    var entry = await dbSet.AddAsync(entity);


                    var anterior = Context.HFechasConfi.Where(e => e.CodLin == entity.CodLin && (e.FecDesde < entity.FecDesde)).OrderByDescending(e => e.FecDesde).Take(1).FirstOrDefault();

                    if (anterior != null)
                    {
                        entity.FecHasta = anterior.FecHasta;
                        anterior.FecHasta = entity.FecDesde.AddDays(-1);
                        await this.ValidarDesignar(entity.FecDesde, entity.FecHasta, entity.CodLin);
                    }
                    else
                    {
                        var posterior = Context.HFechasConfi.Where(e => e.CodLin == entity.CodLin && e.FecDesde > entity.FecDesde).OrderBy(e => e.FecDesde).Take(1).FirstOrDefault();
                        if (posterior != null)
                        {
                            entity.FecHasta = posterior.FecDesde.AddDays(-1);
                        }
                        await this.ValidarDesignar(entity.FecDesde, entity.FecHasta, entity.CodLin);
                    }

                    await this.SaveChangesAsync();


                    await this.ValidarTimeLineHorario(entity.CodLin);

                    ts.Commit();

                    return entry.Entity;
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }

        /// <summary>
        /// verifica si un horario h_fechas_confi , ya fue diagramado  
        /// </summary>  
        public async Task<Boolean> HorarioDiagramado(int CodHfecha, int? CodTdia, List<int> CodServicio)
        {
            try
            {
                using (var command = Context.Database.GetDbConnection().CreateCommand())
                {
                    if (Context.Database.CurrentTransaction != null)
                        command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                    command.CommandText = Infra.Data.SqlObjects.sp_pla_HorarioDiagramado;
                    command.CommandType = System.Data.CommandType.StoredProcedure;
                    command.Parameters.Add(new SqlParameter("cod_hfecha", CodHfecha));
                    command.Parameters.Add(new SqlParameter("cod_tdia", (object)CodTdia ?? DBNull.Value));
                    command.Parameters.Add(new SqlParameter("cod_servicio", (CodServicio != null && CodServicio.Any()) ? string.Join(",", CodServicio) : DBNull.Value as object));

                    Context.Database.OpenConnection();
                    var retult = await command.ExecuteScalarAsync();
                    return (retult as Boolean?).GetValueOrDefault();
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }

        public async Task<int> RemapearRecoridoBandera(HFechasConfiFilter filter)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    using (var command = Context.Database.GetDbConnection().CreateCommand())
                    {
                        if (Context.Database.CurrentTransaction != null)
                            command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                        command.CommandText = Infra.Data.SqlObjects.sp_pla_RemapearRecoridoBandera;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("cod_hfecha", filter.cod_hfecha));
                        command.Parameters.Add(new SqlParameter("CodBandera", filter.cod_ban));

                        Context.Database.OpenConnection();
                        var retult = await command.ExecuteNonQueryAsync();
                        ts.Commit();
                        return retult;
                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }

        public override async Task DeleteAsync(int id)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {

                    using (var command = Context.Database.GetDbConnection().CreateCommand())
                    {
                        var entity = Context.HFechasConfi.AsNoTracking().Where(e => e.Id == id).FirstOrDefault();

                        var anterior = Context.HFechasConfi.Where(e => e.CodLin == entity.CodLin && e.FecHasta < entity.FecDesde).OrderByDescending(e => e.FecDesde).Take(1).FirstOrDefault();
                        //var posterior = Context.HFechasConfi.Where(e => e.CodLin == entity.CodLin && e.FecDesde > entity.FecHasta).OrderBy(e => e.FecDesde).Take(1).FirstOrDefault();

                        if (Context.Database.CurrentTransaction != null)
                            command.Transaction = Context.Database.CurrentTransaction.GetDbTransaction();

                        command.CommandText = Infra.Data.SqlObjects.sp_pla_EliminarHorario;
                        command.CommandType = System.Data.CommandType.StoredProcedure;
                        command.Parameters.Add(new SqlParameter("cod_hfecha", id));

                        Context.Database.OpenConnection();
                        var retult = await command.ExecuteNonQueryAsync();


                        if (anterior != null)
                        {
                            anterior.FecHasta = entity.FecHasta;
                        }

                        await this.SaveChangesAsync();

                        await this.ValidarTimeLineHorario(entity.CodLin);

                        ts.Commit();

                    }
                }
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }

        public async Task<List<HKilometros>> GetKilometrosAsync(List<int> CodBanList, List<int> CodSecList)
        {
            try
            {
                Expression<Func<HKilometros, bool>> Exp = e => CodSecList.Contains(e.CodSec);
                Exp = Exp.And(e => CodBanList.Contains(e.CodBan));
                var query = Context.Set<HKilometros>().Where(Exp).AsQueryable();
                return await query.ToListAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public async Task<List<ReporteHorarioExcelModel>> GenerarExcelHorarios(ExportarExcelFilter filter)
        {
            List<ReporteHorarioExcelModel> model = new List<ReporteHorarioExcelModel>();

            var sp = this.Context.LoadStoredProc("dbo.sp_HFechasConfi_GenerarExcelHorarios")
                .WithSqlParam("cod_hfecha", new SqlParameter("cod_hfecha", filter.CodHfecha))
                .WithSqlParam("cod_subg", new SqlParameter("cod_subg", (Object)filter.CodSubg?? DBNull.Value))
                .WithSqlParam("cod_tdia", new SqlParameter("cod_tdia", (Object)filter.CodTdia ?? DBNull.Value))
                ;

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                model = handler.ReadToList<ReporteHorarioExcelModel>().ToList();
            });

            return model;
        }

        public async Task<List<RelevoModel>> GetDatosReporteRelevos(ExportarExcelFilter filtro)
        {
            List<RelevoModel> items = new List<RelevoModel>();

            var sp = this.Context.LoadStoredProc("dbo.sp_HFechasConfi_GetDatosReporteRelevos",commandTimeout:6000)
                .WithSqlParam("CodHfecha", new SqlParameter("CodHfecha", filtro.CodHfecha))
                .WithSqlParam("CodTdia", new SqlParameter("CodTdia", (object)filtro.CodTdia ?? DBNull.Value))
                .WithSqlParam("CodSubg", new SqlParameter("CodSubg", (object)filtro.CodSubg ?? DBNull.Value))
                ;


            await sp.ExecuteStoredProcAsync((handler) =>
            {
                items = handler.ReadToList<RelevoModel>().ToList();
            });

            return items;
        }

        public async Task<string> GetTitulo (ExportarExcelFilter filtro)
        {
            var descdia = await this.Context.HTipodia.Where(e => e.Id == filtro.CodTdia).FirstOrDefaultAsync();
            var codlin = await this.Context.HFechasConfi.Where(e => e.Id == filtro.CodHfecha).FirstOrDefaultAsync();
            var desclinea = await this.Context.PlaLinea.Where(e => e.Id == codlin.CodLin).FirstOrDefaultAsync();
            return "Relevo " + desclinea.DesLin.Trim() + " " + descdia.DesTdia.Trim() + " " + codlin.FecDesde.ToString("yyyy.MM.dd").Trim() + ".pdf";
        }

        public async Task<HBasec> UpdateHBasec(int CodBan, int CodHFecha, int? CodRec)
        {
            HBasec hbasecnew = new HBasec();
            try
            {
                hbasecnew = await this.Context.HBasec.Where(e => e.CodBan == CodBan && e.CodHfecha == CodHFecha).FirstOrDefaultAsync();
                hbasecnew.CodRec = CodRec;
                this.Context.Entry(hbasecnew).State = EntityState.Modified;
                await this.Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }


            return hbasecnew;
        }

        public async Task GuardarBanderaPorSector(HFechasConfi data)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    await UpdateKilometrosPorhbasec(data);
                    await UpdateCartelesPorhbasec(data);                    
                    ts.Commit();
                }
            }
            catch (Exception ex)
            {

                throw ex; 
            }
        }
    }
}
