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
using System.Threading.Tasks;

namespace ROSBUS.infra.Data.Repositories
{
    public class GalponRepository : RepositoryBase<AdminContext,Galpon, Decimal>, IGalponRepository
    {

        public GalponRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<Galpon, bool>> GetFilterById(Decimal id)
        {
            return e => e.Id == id;
        }



        public override Task<Galpon> AddAsync(Galpon entity)
        {
            entity.Id= Context.Galpones.Max(e => e.Id) + 1;
            return base.AddAsync(entity);
        }


        public async Task UpdateList(IEnumerable<Galpon> talleres)
        {

            var ultimoid = Context.Galpones.Max(e => e.Id) + 1; 

            foreach (var item in talleres)
            {
                if (Context.Galpones.Any(e => e.Id == item.Id))
                {
                    Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                }
                else
                {
                    item.Id = ultimoid++;
                    Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Added;
                }
            }

            var notIn = Context.Galpones.Where(e => !talleres.Any(l => l.Id == e.Id)).ToList();

            foreach (var item in notIn)
            {
                Context.Entry(item).State = Microsoft.EntityFrameworkCore.EntityState.Deleted;
            }

            await Context.SaveChangesAsync();
        }
    }
}
