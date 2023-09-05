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
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace ROSBUS.infra.Data.Repositories
{
    public class TipoParadaRepository : RepositoryBase<AdminContext,PlaTipoParada, int>, ITipoParadaRepository
    {

        public TipoParadaRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        protected override IQueryable<PlaTipoParada> AddIncludeForGet(DbSet<PlaTipoParada> dbSet)
        {
            return base.AddIncludeForGet(dbSet)              
                .Include(e => e.PlaTiempoEsperadoDeCarga)
                .Include("PlaTiempoEsperadoDeCarga.TipodeDia");
        }


        public override Expression<Func<PlaTipoParada, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public override async Task DeleteAsync(PlaTipoParada entity)
        {

            await Task.FromResult(Context.Set<PlaTipoParada>().Remove(entity));


            foreach (var TiempoEsperado in entity.PlaTiempoEsperadoDeCarga.Reverse())
            {
                Context.Set<PlaTiempoEsperadoDeCarga>().Remove(TiempoEsperado);

            }
        }


        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("FK_Puntos_TipoParada", "No se puede eliminar la Entidad por que está siendo utilizada");
            return d;
        }

        public async override Task DeleteAsync(int id)
        {
            try
            {
                await DeleteAsync(await GetByIdAsync(id));
                await Context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
