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
using ROSBUS.Admin.Domain.Entities.Partials;

namespace ROSBUS.infra.Data.Repositories
{
    public class InvolucradosRepository : RepositoryBase<AdminContext, SinInvolucrados, int>, IInvolucradosRepository
    {

        public InvolucradosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SinInvolucrados, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<HistorialInvolucrados> HistorialSiniestros(int TipoDocId, string NroDoc)
        {

            HistorialInvolucrados hist = new HistorialInvolucrados();
            hist.Conductor = await this.Context.SinConductores.Where(e => e.TipoDocId == TipoDocId && e.NroDoc == NroDoc).CountAsync();
            hist.Tercero = await this.Context.SinInvolucrados.Where(e => e.TipoDocId == TipoDocId && e.NroDoc == NroDoc && (e.TipoInvolucradoId == 1) && !e.IsDeleted).CountAsync();
            hist.Testigo = await this.Context.SinInvolucrados.Where(e => e.TipoDocId == TipoDocId && e.NroDoc == NroDoc && (e.TipoInvolucradoId == 2) && !e.IsDeleted).CountAsync();
            hist.Titular = await this.Context.SinInvolucrados.Where(e => e.TipoDocId == TipoDocId && e.NroDoc == NroDoc && (e.TipoInvolucradoId == 3) && !e.IsDeleted).CountAsync();
            hist.Lesionado = await this.Context.SinInvolucrados.Where(e => e.TipoDocId == TipoDocId && e.NroDoc == NroDoc && (e.TipoInvolucradoId == 4) && !e.IsDeleted).CountAsync();
            return hist;
        }
        public override async Task<SinInvolucrados> AddAsync(SinInvolucrados entity)
        {
            if(entity.Id < 0)
            {
                entity.Id = 0;
            }
            return await base.AddAsync(entity);

        }

        protected override IQueryable<SinInvolucrados> AddIncludeForGet(DbSet<SinInvolucrados> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Lesionado)
                .Include(e => e.MuebleInmueble)
                .Include(e => e.Lesionado.TipoLesionado)
                .Include(e => e.MuebleInmueble.TipoInmueble)
                .Include(e => e.TipoDoc)
                .Include(e => e.TipoInvolucrado)
                .Include(e => e.Vehiculo)
                .Include(e => e.Conductor)
                .Include(e => e.SinDetalleLesion)
                .Include(e => e.Conductor.TipoDoc);
        }

        public async Task DeleteFileById(Guid id)
        {
            var adj = await this.Context.SinInvolucradosAdjuntos.FirstOrDefaultAsync(e => e.AdjuntoId == id);

            this.Context.SinInvolucradosAdjuntos.Remove(adj);

            await this.Context.SaveChangesAsync();
        }

        public async Task<List<SinInvolucradosAdjuntos>> GetAdjuntos(int involucradoId)
        {
            return await Context.SinInvolucradosAdjuntos.Where(e => e.InvolucradoId == involucradoId).ToListAsync();
        }

        public async Task<List<SinInvolucrados>> GetBySiniestro(int SiniestroId)
        {
            return await Context.SinInvolucrados
                .Include(e => e.Lesionado)
                .Include(e => e.MuebleInmueble)
                .Include(e => e.Lesionado.TipoLesionado)
                .Include(e => e.MuebleInmueble.TipoInmueble)
                .Include(e => e.TipoDoc)
                .Include(e => e.TipoInvolucrado)
                .Include(e => e.Vehiculo)
                .Include("Vehiculo.Seguro")
                .Include(e => e.Conductor)
                .Include(e => e.SinDetalleLesion)
                .Include(e => e.Conductor.TipoDoc)
                .Where(e => e.SiniestroId == SiniestroId && !e.IsDeleted).ToListAsync();
        }

        public override async Task<SinInvolucrados> UpdateAsync(SinInvolucrados entity)
        {
            try
            {
                //if(entity.LesionadoId > 0)
                //{
                //    if (entity.Lesionado != null)
                //    {
                //        entity.Lesionado.Id = (int)entity.LesionadoId;
                //    }
                //}
                //entity.Lesionado = null;
                Context.Entry(entity).State = EntityState.Modified;
                await this.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

    }
}
