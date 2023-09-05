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
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Threading.Tasks;
using Snickler.EFCore;
using TECSO.FWK.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ROSBUS.infra.Data.Repositories
{
    public class PlaParadasRepository : RepositoryBase<AdminContext,PlaParadas, int>, IPlaParadasRepository
    {

        public PlaParadasRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaParadas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async Task<List<PlaParadas>> ParadasBuscarCercanos(PlaParadasFilter filter)
        {
            IList<PlaParadas> Results = new List<PlaParadas>();
            filter = CompleteFilterPageList(filter);
            var sp = this.Context.LoadStoredProc("dbo.[sp_pla_paradasfind]")
                       .WithSqlParam("FilterText", filter.FilterText)
                       .WithSqlParam("Anulada", filter.Anulada)
                       .WithSqlParam("Calle", filter.Calle)
                       .WithSqlParam("Sentido", filter.Sentido)
                       .WithSqlParam("Cruce", filter.Cruce)
                       .WithSqlParam("Codigo", filter.Codigo)
                       .WithSqlParam("LocalidadId", filter.LocalidadId)
                       .WithSqlParam("orig_lat", filter.Lat.Value)
                       .WithSqlParam("orig_lng", filter.Long.Value)
                       .WithSqlParam("PageNumber", filter.Page)
                       .WithSqlParam("PageSize", filter.PageSize);

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                Results = handler.ReadToList<PlaParadas>();
            });
            
            return Results.ToList();
        }


        public async override Task<PagedResult<PlaParadas>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            var f = filter as PlaParadasFilter;
            if (f != null && f.Lat.HasValue && f.Lat.HasValue && string.IsNullOrEmpty(filter.Sort))
            {
                var list = await this.ParadasBuscarCercanos(f);
                IQueryable<PlaParadas> query = Context.Set<PlaParadas>().Where(filter.GetFilterExpression()).AsQueryable();
                var total = await query.CountAsync();  
                return new PagedResult<PlaParadas>(total, list.ToList());                
            }

            return await base.GetPagedListAsync(filter);
        }

        protected override IQueryable<PlaParadas> GetIncludesForPageList(IQueryable<PlaParadas> query)
        {
            query = query.Include(e => e.ParentStation);
            return query;
        }

        protected override IQueryable<PlaParadas> AddIncludeForGet(DbSet<PlaParadas> dbSet)
        {
            IQueryable<PlaParadas> q = base.AddIncludeForGet(dbSet).AsQueryable();

            q = q.Include(e => e.ParentStation).AsQueryable();
            return q;
        }

    }
}
