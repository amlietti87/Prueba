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
    public class InspPreguntasIncognitosRespuestasRepository : RepositoryBase<AdminContext, InspPreguntasIncognitosRespuestas, int>, IInspPreguntasIncognitosRespuestasRepository
    {

        public InspPreguntasIncognitosRespuestasRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspPreguntasIncognitosRespuestas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<InspPreguntasIncognitosRespuestas> AddIncludeForGet(DbSet<InspPreguntasIncognitosRespuestas> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Respuesta);
        }

        public override async Task<PagedResult<InspPreguntasIncognitosRespuestas>> GetAllAsync(Expression<Func<InspPreguntasIncognitosRespuestas, bool>> predicate, List<Expression<Func<InspPreguntasIncognitosRespuestas, object>>> includeExpression = null)
        {
            try
            {
                IQueryable<InspPreguntasIncognitosRespuestas> query = Context.Set<InspPreguntasIncognitosRespuestas>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                query = query.Include(e => e.Respuesta);

                return new PagedResult<InspPreguntasIncognitosRespuestas>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
    }
}
