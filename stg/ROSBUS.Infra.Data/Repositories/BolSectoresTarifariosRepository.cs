using Microsoft.EntityFrameworkCore;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.infra.Data.Contexto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Infra.Data;
using TECSO.FWK.Infra.Data.Repositories;

namespace ROSBUS.Infra.Data.Repositories
{

    public class BolSectoresTarifariosRepository: RepositoryBase<AdminContext,BolSectoresTarifarios,int>, IBolSectoresTarifariosRepository
    {
        public BolSectoresTarifariosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<BolSectoresTarifarios, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }

        public override async Task<BolSectoresTarifarios> AddAsync(BolSectoresTarifarios entity)
        {
            try
            {
                DbSet<BolSectoresTarifarios> dbSet = Context.Set<BolSectoresTarifarios>();
                entity.Id = Context.BolSectoresTarifarios.Max(e => e.Id) + 1;
                var entry = await dbSet.AddAsync(entity);
                await SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        protected override IQueryable<BolSectoresTarifarios> AddIncludeForGet(DbSet<BolSectoresTarifarios> dbSet)
        {
            return dbSet.Include(m => m.HSectores)
                .AsQueryable();
        }
    }
}
