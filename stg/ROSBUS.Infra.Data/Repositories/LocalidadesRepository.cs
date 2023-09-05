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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class LocalidadesRepository : RepositoryBase<OperacionesRBContext, Localidades, int>, ILocalidadesRepository
    {

        public LocalidadesRepository(IOperacionesRBDbContext _context)
            :base(new DbContextProvider<OperacionesRBContext>(_context))
        {

        }

        public override Expression<Func<Localidades, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override IQueryable<Localidades> AddIncludeForGet(DbSet<Localidades> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.Provincia);

        }

        protected override IQueryable<Localidades> GetIncludesForPageList(IQueryable<Localidades> query)
        {
            return query
                .Include(a => a.Provincia);
        }

       
    }
}
