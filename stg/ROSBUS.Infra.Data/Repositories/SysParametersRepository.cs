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
    public class SysParametersRepository : RepositoryBase<AdminContext, SysParameters, long>, ISysParametersRepository
    {
        public SysParametersRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }
        public override Expression<Func<SysParameters, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UC_Token", "El campo Token no permite valores repetidos.");
            return d;
        }
    }
}
