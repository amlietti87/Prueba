using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Repositories.ART;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Interface;
using TECSO.FWK.Infra.Data.Repositories;
using ROSBUS.Admin.Domain.Entities.ART;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ROSBUS.Infra.Data.Repositories.ART
{
    public class TratamientosRepository : RepositoryBase<AdminContext, ArtTratamientos, int>, ITratamientosRepository
    {
        public TratamientosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }


        public override Expression<Func<ArtTratamientos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async override Task<PagedResult<ArtTratamientos>> GetAllAsync(Expression<Func<ArtTratamientos, bool>> predicate, List<Expression<Func<ArtTratamientos, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<ArtTratamientos> query = Context.Set<ArtTratamientos>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.OrderBy(e => e.Descripcion);
                return new PagedResult<ArtTratamientos>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
    }
}
