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
    public class InspRangosHorarioRepository : RepositoryBase<AdminContext, InspRangosHorarios, int>, IInspRangosHorarioRepository
    {

        public InspRangosHorarioRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspRangosHorarios, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public override async Task<InspRangosHorarios> AddAsync(InspRangosHorarios entity)
        {
            return await base.AddAsync(entity);
        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UK_insp_RangosHorarios_Descripcion", " Ya existe Rango Horario con la descripción.");
            return d;
        }
    }
}
