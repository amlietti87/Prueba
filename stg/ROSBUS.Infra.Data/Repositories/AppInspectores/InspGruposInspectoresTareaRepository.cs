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
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System.ComponentModel.DataAnnotations;

namespace ROSBUS.infra.Data.Repositories
{
    public class InspGruposInspectoresTareaRepository : RepositoryBase<AdminContext, InspGrupoInspectoresTarea, int>, IInspGruposInspectoresTareaRepository
    {

        public InspGruposInspectoresTareaRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspGrupoInspectoresTarea, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<InspGrupoInspectoresTarea> AddIncludeForGet(DbSet<InspGrupoInspectoresTarea> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Tarea);
            
        }

        public override async Task<PagedResult<InspGrupoInspectoresTarea>> GetAllAsync(Expression<Func<InspGrupoInspectoresTarea, bool>> predicate, List<Expression<Func<InspGrupoInspectoresTarea, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<InspGrupoInspectoresTarea> query = Context.Set<InspGrupoInspectoresTarea>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.Include(e => e.Tarea);

                return new PagedResult<InspGrupoInspectoresTarea>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
