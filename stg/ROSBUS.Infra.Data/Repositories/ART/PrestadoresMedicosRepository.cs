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
using System.Linq;
using Microsoft.EntityFrameworkCore;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.Infra.Data.Repositories.ART
{
    public class PrestadoresMedicosRepository : RepositoryBase<AdminContext, ArtPrestadoresMedicos, int>, IPrestadoresMedicosRepository
    {
        public PrestadoresMedicosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }


        public override Expression<Func<ArtPrestadoresMedicos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public async override Task<PagedResult<ArtPrestadoresMedicos>> GetAllAsync(Expression<Func<ArtPrestadoresMedicos, bool>> predicate, List<Expression<Func<ArtPrestadoresMedicos, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<ArtPrestadoresMedicos> query = Context.Set<ArtPrestadoresMedicos>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.OrderBy(e => e.Descripcion);
                return new PagedResult<ArtPrestadoresMedicos>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
    }
}
