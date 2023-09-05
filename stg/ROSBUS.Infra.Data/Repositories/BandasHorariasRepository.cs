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
using System.Transactions;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data.SqlClient;

namespace ROSBUS.infra.Data.Repositories
{
    public class BandasHorariasRepository : RepositoryBase<AdminContext, PlaBandasHorarias, int>, IBandasHorariasRepository
    {
        public BandasHorariasRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaBandasHorarias, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }
    }

}
