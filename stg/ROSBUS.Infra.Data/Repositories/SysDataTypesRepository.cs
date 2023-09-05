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
    public class SysDataTypesRepository : RepositoryBase<AdminContext, SysDataTypes, int>, ISysDataTypesRepository
    {
        public SysDataTypesRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<SysDataTypes, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }
    }
}
