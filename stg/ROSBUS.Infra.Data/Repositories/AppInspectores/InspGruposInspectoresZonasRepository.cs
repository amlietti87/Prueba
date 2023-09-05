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
    public class InspGruposInspectoresZonasRepository : RepositoryBase<AdminContext, InspGrupoInspectoresZona, int>, IInspGruposInspectoresZonasRepository
    {

        public InspGruposInspectoresZonasRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspGrupoInspectoresZona, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<InspGrupoInspectoresZona> AddIncludeForGet(DbSet<InspGrupoInspectoresZona> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Zona);
        }

        public override async Task<PagedResult<InspGrupoInspectoresZona>> GetAllAsync(Expression<Func<InspGrupoInspectoresZona, bool>> predicate, List<Expression<Func<InspGrupoInspectoresZona, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<InspGrupoInspectoresZona> query = Context.Set<InspGrupoInspectoresZona>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.Include(e => e.Zona);

                return new PagedResult<InspGrupoInspectoresZona>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
