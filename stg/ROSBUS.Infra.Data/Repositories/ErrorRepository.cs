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

namespace ROSBUS.infra.Data.Repositories
{
    public class ErrorRepository : RepositoryBase<ErrorDBContext,Error, Int64>, IErrorRepository
    {

        public ErrorRepository(IErrorDbContext _context)
            :base(new DbContextProvider<ErrorDBContext>(_context))
        {

        }

        public override Expression<Func<Error, bool>> GetFilterById(Int64 id)
        {
            return e => e.Id == id;
        }
    }
}
