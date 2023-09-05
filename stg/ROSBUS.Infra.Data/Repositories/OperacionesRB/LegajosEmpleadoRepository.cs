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
using ROSBUS.Admin.Domain.Entities.Filters;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using Snickler.EFCore;
using System.Data.SqlClient;
using ROSBUS.Operaciones.Domain.Entities;
using ROSBUS.Operaciones.Domain.Interfaces.Repositories;
using TECSO.FWK.Domain;
using Microsoft.EntityFrameworkCore;

namespace ROSBUS.infra.Data.Operaciones.Repositories
{
    public class LegajosEmpleadoRepository : RepositoryBase<OperacionesRBContext, LegajosEmpleado, int>, ILegajosEmpleadoRepository
    {

        public LegajosEmpleadoRepository(IOperacionesRBDbContext _context)
            :base(new DbContextProvider<OperacionesRBContext>(_context))
        {

        }

        public override Expression<Func<LegajosEmpleado, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }
        
        public override async Task<LegajosEmpleado> AddAsync(LegajosEmpleado entity)
        {

            return await base.AddAsync(entity);

        }


        public async Task<LegajosEmpleado> GetMaxById(int id)
        {
            var empleado = await this.Context.Empleados.Include(e=> e.LegajosEmpleado).Where(e => e.Id == id).FirstOrDefaultAsync();

            var legajofecbaja = empleado.LegajosEmpleado.Where(f => f.FecBaja == null).FirstOrDefault();
            if (!String.IsNullOrEmpty(legajofecbaja?.LegajoSap))
            {
                return legajofecbaja;
            }
            else
            {

                return empleado.LegajosEmpleado.Where(f => f.FecIngreso == empleado.LegajosEmpleado.Max(u => u.FecIngreso)).FirstOrDefault();
            }

        }
    }
}
