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

namespace ROSBUS.infra.Data.Repositories
{
    public class InspPlanillaIncognitosRepository : RepositoryBase<AdminContext,InspPlanillaIncognitos, int>, IInspPlanillaIncognitosRepository
    {

        public InspPlanillaIncognitosRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspPlanillaIncognitos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<InspPlanillaIncognitos> AddIncludeForGet(DbSet<InspPlanillaIncognitos> dbSet)
        {
            var query = base.AddIncludeForGet(dbSet);

            query = query.Include(e => e.InspPlanillaIncognitosDetalle);

            return query;
        }
    }
}
