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
using ROSBUS.Admin.Domain.Entities.Filters;
using Snickler.EFCore;
using System.Data.SqlClient;

namespace ROSBUS.infra.Data.Repositories
{
    public class SectorRepository : RepositoryBase<AdminContext,PlaSector, Int64>, ISectorRepository
    {

        public SectorRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<PlaSector, bool>> GetFilterById(Int64 id)
        {
            return e => e.Id == id;
        }


        public async Task<List<PlaSentidoPorSector>> RecuperarSentidoPorSector(HDesignarFilter Filtro)
        {
            List<PlaSentidoPorSector> itemDtos = new List<PlaSentidoPorSector>();

            var sp = this.Context.LoadStoredProc("dbo.sp_Insp_RecuperarSentidoPorSector")

                .WithSqlParam("fecha", new SqlParameter("fecha", Filtro.Fecha))
                .WithSqlParam("CoordenadaId", new SqlParameter("CoordenadaId", (Object)Filtro.Sector));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<PlaSentidoPorSector>().ToList();
            });

            return itemDtos;
        }

    }
}
