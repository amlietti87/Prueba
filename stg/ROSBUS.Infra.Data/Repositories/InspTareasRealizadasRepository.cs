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
    public class InspTareasRealizadasRepository : RepositoryBase<AdminContext,InspTareasRealizadas, int>, IInspTareasRealizadasRepository
    {

        public InspTareasRealizadasRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspTareasRealizadas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        protected override IQueryable<InspTareasRealizadas> AddIncludeForGet(DbSet<InspTareasRealizadas> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include(e=> e.InspTareasRealizadasDetalle);
        }
    }
}
