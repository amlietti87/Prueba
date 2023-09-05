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
using TECSO.FWK.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces;
using ROSBUS.Admin.Domain.Entities.AppInspectores;
using System.ComponentModel.DataAnnotations;

namespace ROSBUS.infra.Data.Repositories
{
    public class NovedadesRepository : RepositoryBase<AdminContext, Novedades, int>, INovedadesRepository
    {

        public NovedadesRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<Novedades, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }        
    }
}
