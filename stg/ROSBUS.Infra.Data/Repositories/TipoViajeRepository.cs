using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.Infra.Data.Repositories
{
    public class TipoViajeRepository : RepositoryBase<AdminContext, PlaTipoViaje, int>, IPlaTipoViajeRepository
    {
        public TipoViajeRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaTipoViaje, bool>> GetFilterById(int id)
        {
            throw new NotImplementedException();
        }
    }
}
