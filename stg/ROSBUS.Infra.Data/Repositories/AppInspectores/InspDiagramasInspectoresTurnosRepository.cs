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
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Entities.Filters;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Admin.Domain.ParametersHelper;
using TECSO.FWK.Domain.Interfaces.Services;
using ROSBUS.Admin.Domain.Model;

namespace ROSBUS.infra.Data.Repositories
{
    public class InspDiagramasInspectoresTurnosRepository : RepositoryBase<AdminContext, InspDiagramasInspectoresTurnos, int>, IInspDiagramasInspectoresTurnosRepository
    {

        public InspDiagramasInspectoresTurnosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {
          
        }

        public override Expression<Func<InspDiagramasInspectoresTurnos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

     
    }
}
