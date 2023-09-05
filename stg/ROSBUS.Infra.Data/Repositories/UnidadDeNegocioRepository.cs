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
    //public class UnidadDeNegocioRepository : RepositoryBase<AdminContext,UnidadDeNegocio, int>, IUnidadDeNegocioRepository
    //{

    //    public UnidadDeNegocioRepository(IAdminDbContext _context)
    //        :base(new DbContextProvider<AdminContext>(_context))
    //    {

    //    }

    //    public override Expression<Func<UnidadDeNegocio, bool>> GetFilterById(int id)
    //    {
    //        return e => e.Id == id;
    //    }
    //}
}
