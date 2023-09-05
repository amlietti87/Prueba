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
using ROSBUS.Admin.Domain.Entities.Filters;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class InspTareaRepository : RepositoryBase<AdminContext, InspTarea, int>, IInspTareaRepository
    {

        public InspTareaRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspTarea, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<InspTarea> AddIncludeForGet(DbSet<InspTarea> dbSet)
        {
            var query = base.AddIncludeForGet(dbSet);

            query = query.Include(e => e.TareasCampos);
            query = query.Include("TareasCampos.TareaCampoConfig");

            return query;
        }

        public override async Task<PagedResult<InspTarea>> GetPagedListAsync<TFilter>(TFilter filter)
        {
            try
            {
                filter = CompleteFilterPageList(filter);
                IQueryable<InspTarea> query = Context.Set<InspTarea>().Where(filter.GetFilterExpression()).AsQueryable();
                var total = await query.CountAsync();
                foreach (var include in filter.GetIncludesForPageList())
                {
                    query = query.Include(include);
                }
                query = query.Include(e => e.TareasCampos);
                query = query.Include("TareasCampos.TareaCampoConfig");


                if (!String.IsNullOrEmpty(filter.Sort))
                {
                    query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, filter.Sort);
                }
                var Page = filter.Page.GetValueOrDefault();
                var PageSize = filter.PageSize.GetValueOrDefault();

                var list = query.Skip((Page - 1) * PageSize).Take(PageSize);

                return new PagedResult<InspTarea>(total, await list.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }


        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UK_insp_Tareas_Descripcion", "Descripción está repetida.");
            return d;
        }
    }
}
