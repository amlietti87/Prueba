using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Repositories.ART;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Interface;
using TECSO.FWK.Infra.Data.Repositories;
using ROSBUS.Admin.Domain.Entities.ART;

namespace ROSBUS.Infra.Data.Repositories.ART
{
    public class DenunciasNotificacionesRepository : RepositoryBase<AdminContext, ArtDenunciasNotificaciones, int>, IDenunciasNotificacionesRepository

    {
        public DenunciasNotificacionesRepository(IAdminDbContext _context)
            : base(_dbContextProvider: new DbContextProvider<AdminContext>(_context))
        {

        }


        public override Expression<Func<ArtDenunciasNotificaciones, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }
    }
}
