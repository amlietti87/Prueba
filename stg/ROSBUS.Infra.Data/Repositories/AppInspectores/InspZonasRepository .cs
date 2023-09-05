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
    public class InspZonasRepository : RepositoryBase<AdminContext, InspZonas, int>, IInspZonasRepository
    {

        public InspZonasRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspZonas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }
        
        public override async Task<InspZonas> AddAsync(InspZonas entity)
        {
            DbSet<InspZonas> dbSet = Context.Set<InspZonas>();
            entity.Descripcion = entity.Descripcion.ToUpper();

            var entry = await dbSet.AddAsync(entity);

            var igiDescripcion = dbSet.Where(t => t.Descripcion == entity.Descripcion);

            if (igiDescripcion.Count() == 0)
            {
                return await base.AddAsync(entity);
            }
            else
            {
                throw new ValidationException(" Ya existe la zona con la descripción: " + entity.Descripcion);
            }


        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UK_insp_Zonas_Descripcion", "Descripción está repetida.");
            return d;
        }
    }
}
