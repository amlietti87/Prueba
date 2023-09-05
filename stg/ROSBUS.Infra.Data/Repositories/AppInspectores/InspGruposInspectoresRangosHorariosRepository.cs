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
    public class InspGruposInspectoresRangosHorariosRepository : RepositoryBase<AdminContext, InspGrupoInspectoresRangosHorarios, int>, IInspGruposInspectoresRangosHorariosRepository
    {

        public InspGruposInspectoresRangosHorariosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspGrupoInspectoresRangosHorarios, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<InspGrupoInspectoresRangosHorarios> AddIncludeForGet(DbSet<InspGrupoInspectoresRangosHorarios> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.RangoHorario);
            
        }
        public override async Task<PagedResult<InspGrupoInspectoresRangosHorarios>> GetAllAsync(Expression<Func<InspGrupoInspectoresRangosHorarios, bool>> predicate, List<Expression<Func<InspGrupoInspectoresRangosHorarios, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<InspGrupoInspectoresRangosHorarios> query = Context.Set<InspGrupoInspectoresRangosHorarios>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.Include(e => e.RangoHorario);

                return new PagedResult<InspGrupoInspectoresRangosHorarios>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
