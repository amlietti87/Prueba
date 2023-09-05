using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class LineaService : ServiceBase<Linea, decimal, ILineaRepository>, ILineaService
    { 
        public LineaService(ILineaRepository repository)
            : base(repository)
        {
       
        }

        protected async override  Task ValidateEntity(Linea entity, SaveMode mode)
        {
            await base.ValidateEntity(entity, mode);


            if (entity.SucursalesxLineas.Any())
            {
                var idsuc = entity.SucursalesxLineas.FirstOrDefault().Id;

                if (repository.ExistExpression(e => e.Id != entity.Id && e.DesLin == entity.DesLin && e.SucursalesxLineas.Any(a => a.Id == idsuc)))
                {
                    throw new DomainValidationException("El nombre de la linea ya fue cargada para la unidad de negocio");
                }
            }
            
        }

    }
    
}
