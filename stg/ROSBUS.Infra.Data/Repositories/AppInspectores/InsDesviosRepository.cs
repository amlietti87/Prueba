using ROSBUS.Admin.Domain.Entities.AppInspectores;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Repositories.AppInspectores;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Interface;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.Infra.Data.Repositories.AppInspectores
{
    public class InsDesviosRepository : RepositoryBase<AdminContext, InsDesvios, long>, IInsDesviosRepository
    {
        public InsDesviosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {
        }

        public override Expression<Func<InsDesvios, bool>> GetFilterById(long id)
        {
            return e => e.Id == id;
        }


    }
}
