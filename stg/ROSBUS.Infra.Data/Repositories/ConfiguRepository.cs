//using ROSBUS.Admin.Domain;
//using ROSBUS.Admin.Domain.Entities;
//using ROSBUS.Admin.Domain.Interfaces.Repositories;
//using ROSBUS.infra.Data.Contexto;
//using System;
//using System.Collections.Generic;
//using System.Text;
//using TECSO.FWK.Infra.Data;
//using TECSO.FWK.Infra.Data.Repositories;
//using System.Linq;
//using TECSO.FWK.Domain.Interfaces.Repositories;
//using System.Linq.Expressions;
//using Microsoft.EntityFrameworkCore;

//namespace ROSBUS.infra.Data.Repositories
//{
//    public class ConfiguRepository : RepositoryBase<AdminContext,Configu, decimal>, IConfiguRepository
//    {

//        public ConfiguRepository(IAdminDbContext _context)
//            :base(new DbContextProvider<AdminContext>(_context))
//        {

//        }

//        public override Expression<Func<Configu, bool>> GetFilterById(decimal id)
//        {
//            return e => e.Id == id;
//        }
//        protected override IQueryable<Configu> AddIncludeForGet(DbSet<Configu> dbSet)
//        {


//            return base.AddIncludeForGet(dbSet)
//    .Include(e => e.Grupo)
//                .Include(e => e.Empresa)
//                .Include(e => e.Sucursal)
//                .Include(e => e.Linea)
//                .Include(e => e.Galpon)
//                .Include(e => e.PlanCamNav);
//        }
//    }
//}
