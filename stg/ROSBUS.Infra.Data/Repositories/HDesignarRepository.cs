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
using Snickler.EFCore;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ROSBUS.infra.Data.Repositories
{
    public class HDesignarRepository : RepositoryBase<AdminContext, HDesignar, int>, IHDesignarRepository
    {

        public HDesignarRepository(IAdminDbContext _context)
            :base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<HDesignar, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        public async Task<List<HDesignarSabanaSector>> RecuperarSabanaPorSector(HDesignarFilter Filtro)
        {
            List<HDesignarSabanaSector> itemDtos = new List<HDesignarSabanaSector>();

            var sp = this.Context.LoadStoredProc("dbo.sp_h_designar_RecuperarAsignacion")

                .WithSqlParam("fecha", new SqlParameter("fecha", Filtro.Fecha))
                .WithSqlParam("coordenadaId", new SqlParameter("coordenadaId", (Object)Filtro.Sector))
                .WithSqlParam("sentidoBanderaId", new SqlParameter("sentidoBanderaId", (Object)Filtro.Sentido))
                .WithSqlParam("TipoLineaId", new SqlParameter("TipoLineaId", (Object)Filtro.TipoLinea));

            await sp.ExecuteStoredProcAsync((handler) =>
            {
                itemDtos = handler.ReadToList<HDesignarSabanaSector>().ToList();
            });

            return itemDtos;
        }

    }
}
