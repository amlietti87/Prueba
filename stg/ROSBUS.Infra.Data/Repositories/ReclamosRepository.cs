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
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Model;
using Snickler.EFCore;
using System.Data.SqlClient;

namespace ROSBUS.infra.Data.Repositories
{
    public class ReclamosRepository : RepositoryBase<AdminContext, SinReclamos, int>, IReclamosRepository
    {

        public ReclamosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }



        public override Expression<Func<SinReclamos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public override async Task<SinReclamos> AddAsync(SinReclamos entity)
        {

            //try
            //{
            //    DbSet<PlaCoordenadas> dbSet = Context.Set<PlaCoordenadas>();
            //    EntityEntry<PlaCoordenadas> entittyentry;

            //    if (!dbSet.Any(e => e.Id == entity.Id)) {
            //        entittyentry = await dbSet.AddAsync(entity);
            //    }
            //    else{

            //        entittyentry = dbSet.Attach(entity);
            //        entittyentry.State = EntityState.Modified;
            //    }

            //    await this.SaveChangesAsync();
            //    return entittyentry.Entity;
            //}
            //catch (Exception ex)
            //{
            //    this.HandleException(ex);
            //    throw;
            //}

            return await base.AddAsync(entity);

        }

        public async Task<SinReclamos> CambioEstado(SinReclamos reclamo, SinReclamosHistoricos historico)
        {

            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    this.Context.Entry(reclamo).State = EntityState.Modified;

                    this.Context.Entry(historico).State = EntityState.Added;


                    await this.Context.SaveChangesAsync();
                    ts.Commit();

                }

                var x = await this.Context.SinReclamosHistoricos.Where(e => e.Id == historico.Id).FirstOrDefaultAsync();
                return reclamo;

            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }


        }

        public async Task<SinReclamos> Anular(SinReclamos reclamo)
        {

            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    this.Context.Entry(reclamo).State = EntityState.Modified;

                    await this.Context.SaveChangesAsync();
                    ts.Commit();

                }

                return reclamo;

            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }


        }
        public async Task DeleteFileById(Guid id)
        {
            var adj = await this.Context.SinReclamoAdjuntos.FirstOrDefaultAsync(e => e.AdjuntoId == id);

            this.Context.SinReclamoAdjuntos.Remove(adj);

            await this.Context.SaveChangesAsync();
        }

        public async Task<List<SinReclamoAdjuntos>> GetAdjuntos(int reclamoId)
        {
            return await Context.SinReclamoAdjuntos.Where(e => e.ReclamoId == reclamoId).ToListAsync();
        }


        protected override IQueryable<SinReclamos> AddIncludeForGet(DbSet<SinReclamos> dbSet)
        {

            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Abogado)
                .Include(e => e.AbogadoEmpresa)
                .Include(e => e.Estado)
                .Include(e => e.Involucrado)
                .Include(e => e.Involucrado.TipoDoc)
                .Include(e => e.Involucrado.TipoInvolucrado)
                .Include(e => e.Juzgado)
                .Include(e => e.SubEstado)
                .Include(e => e.ReclamoCuotas)
                .Include(e => e.TipoReclamo)
                .Include(e => e.Siniestro)
                .Include(e => e.Denuncia)
                .Include("Siniestro.Sucursal")
                .Include(e => e.CreatedUser)
                .Include(e => e.Sucursal)
                .Include(e => e.Empresa)
                .Include(e => e.Denuncia.PrestadorMedico)
                .Include(e => e.ReclamosHistoricos)
                ;
        }

        protected override IQueryable<SinReclamos> GetIncludesForPageList(IQueryable<SinReclamos> query)
        {
            return base.GetIncludesForPageList(query)
                .Include(e => e.Abogado)
                .Include(e => e.AbogadoEmpresa)
                .Include(e => e.Estado)
                .Include(e => e.Involucrado)
                .Include(e => e.Involucrado.TipoDoc)
                .Include(e => e.Involucrado.TipoInvolucrado)
                .Include(e => e.Juzgado)
                .Include(e => e.SubEstado)
                .Include(e => e.ReclamoCuotas)
                .Include(e => e.TipoReclamo)
                .Include(e => e.Siniestro)
                .Include(e => e.Denuncia)
                .Include("Siniestro.Sucursal")
                .Include(e => e.Sucursal)
                .Include(e => e.Empresa)
                .Include(e => e.ReclamosHistoricos).ThenInclude(navigationPropertyPath: hist => hist.Estado)
                .Include(e => e.ReclamosHistoricos).ThenInclude(navigationPropertyPath: hist => hist.SubEstado)
                ;
        }

        public override Task DeleteAsync(SinReclamos entity)
        {
            return base.DeleteAsync(entity);
        }

        public async Task<List<SinReclamos>> GetByInvolucrado(int InvolucradoId)
        {
            return await Context.SinReclamos
                .Where(e => e.InvolucradoId == InvolucradoId).ToListAsync();
        }

        public async Task<List<ReporteReclamosExcel>> GetReporteExcel(ExcelReclamosFilter filter)
        {
            List<ReporteReclamosExcel> items = new List<ReporteReclamosExcel>();

            var sp = Context.LoadStoredProc("dbo.sp_Reclamos_ReporteReclamosExcel")
                .WithSqlParam("ids", new SqlParameter("ids", filter.Ids));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                items = handler.ReadToList<ReporteReclamosExcel>().ToList();
            });

            return items;
        }

        public async Task ImportarReclamos(List<ImportadorExcelReclamos> planilla)
        {
            try
            {
                using (var ts = await this.Context.Database.BeginTransactionAsync())
                {
                    foreach (var item in planilla)
                    {
                        SinReclamos reclamos = new SinReclamos
                        {
                            TipoReclamoId = item.TiposReclamoId,
                            EstadoId = item.EstadoId,
                            SubEstadoId = item.SubEstadoId,
                            SucursalId = item.SucursalId,
                            EmpresaId = item.EmpresaId,
                            Fecha = (DateTime)item.FechaReclamo,
                            DenunciaId = item.DenunciaId,
                            EmpleadoId = item.EmpleadoId,
                            EmpleadoFechaIngreso = item.EmpleadoFechaIngreso,
                            EmpleadoLegajo = item.EmpleadoLegajo,
                            EmpleadoEmpresaId = item.EmpleadoEmpresaId,
                            EmpleadoAntiguedad = item.EmpleadoAntiguedad,
                            EmpleadoArea = item.EmpleadoArea,
                            MontoDemandado = item.MontoDemandado,
                            FechaOfrecimiento = item.FechaOfrecimiento,
                            MontoOfrecido = item.MontoOfrecido,
                            MontoReconsideracion = item.MontoReconsideracion,
                            CausaId = item.CausaId,
                            Hechos = item.Hechos,
                            FechaPago = item.FechaPago,
                            MontoPagado = item.MontoPagado,
                            MontoFranquicia = item.MontoFranquicia,
                            AbogadoId = item.AbogadoId,
                            MontoHonorariosAbogado = item.HonorariosAbogadoActor,
                            MontoHonorariosMediador = item.HonorariosMediador,
                            MontoHonorariosPerito = item.HonorariosPerito,
                            JuntaMedica = item.JuntaMedica,
                            PorcentajeIncapacidad = item.PorcentajeIncapacidad,
                            TipoAcuerdoId = item.TipoAcuerdoId,
                            RubroSalarialId = item.RubroSalarialId,
                            MontoTasasJudiciales = item.MontoTasasJudiciales,
                            Observaciones = item.Observaciones,
                            ObsConvenioExtrajudicial = item.ObsConvExtrajudicial,
                            Autos = item.Autos,
                            NroExpediente = item.NroExpediente,
                            JuzgadoId = item.JuzgadoId,
                            AbogadoEmpresaId = item.AbogadoEmpresaId
                        };
                        await AddAsync(reclamos);
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
    }
}
