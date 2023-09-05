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
using TECSO.FWK.Domain.Entities;

namespace ROSBUS.infra.Data.Repositories
{
    public class GrupoLineasRepository : RepositoryBase<AdminContext, PlaGrupoLineas, int>, IGrupoLineasRepository
    {

        public GrupoLineasRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }


        protected override IQueryable<PlaGrupoLineas> AddIncludeForGet(DbSet<PlaGrupoLineas> dbSet)
        {
            return base.AddIncludeForGet(dbSet).Include("Linea");

        }

        public override Expression<Func<PlaGrupoLineas, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("FK_linea_pla_GrupoLineas", "No se puede eliminar el grupo de linea por que esta usado en al menos una linea");
            return d;
        }
    }
}
