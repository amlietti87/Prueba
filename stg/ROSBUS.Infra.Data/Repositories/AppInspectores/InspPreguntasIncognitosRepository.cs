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
    public class InspPreguntasIncognitosRepository : RepositoryBase<AdminContext, InspPreguntasIncognitos, int>, IInspPreguntasIncognitosRepository
    {

        public InspPreguntasIncognitosRepository(IAdminDbContext _context)
            : base(new DbContextProvider<AdminContext>(_context))
        {

        }

        public override Expression<Func<InspPreguntasIncognitos, bool>> GetFilterById(int id)
        {
            return e => e.Id == id;
        }


        protected override IQueryable<InspPreguntasIncognitos> AddIncludeForGet(DbSet<InspPreguntasIncognitos> dbSet)
        {
            return base.AddIncludeForGet(dbSet)
                .Include(e => e.InspPreguntasIncognitosRespuestas).ThenInclude(git => git.Respuesta);
        }


        public override Task DeleteAsync(InspPreguntasIncognitos entity)
        {
            var respuestas = this.Context.InspPreguntasIncognitosRespuestas.Where(r => r.PreguntaIncognitoId == entity.Id);

            if (respuestas != null )
            {
                if(respuestas.Count() != 0 )
                    throw new ValidationException("Esta pregunta no se puede eliminar, existen asociaciones");
            }

            return base.DeleteAsync(entity);
        }

        public override async Task<InspPreguntasIncognitos> AddAsync(InspPreguntasIncognitos entity)
        {
            return await base.AddAsync(entity);
        }

        

        protected override IQueryable<InspPreguntasIncognitos> GetIncludesForPageList(IQueryable<InspPreguntasIncognitos> query)
        {
            return base.GetIncludesForPageList(query).Include(e=> e.InspPreguntasIncognitosRespuestas).ThenInclude(e=> e.Respuesta);
        }

        protected override Dictionary<string, string> GetMachKeySqlException()
        {
            var d = base.GetMachKeySqlException();
            d.Add("UK_insp_PreguntasIncognitos", "Descripción está repetida");
            return d;
        }

    }
}
