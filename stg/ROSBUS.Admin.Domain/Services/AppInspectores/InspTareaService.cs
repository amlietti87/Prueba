using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class InspTareaService : ServiceBase<InspTarea, int, IInspTareaRepository>, IInspTareaService
    {
        public InspTareaService(IInspTareaRepository repository)
            : base(repository)
        {

        }

        protected override Task ValidateEntity(InspTarea entity, SaveMode mode)
        {
            if(entity.TareasCampos == null || entity.TareasCampos.Count == 0)
            {
                throw new DomainValidationException("La tarea debe contener al menos un campo");
            }

            foreach (var item in entity.TareasCampos)
            {
                if (string.IsNullOrEmpty(item.Etiqueta))
                {
                    throw new DomainValidationException("Falta completar la Etiqueta en el campo de la tarea");
                }
                if (item.TareaCampoConfigId == 0)
                {
                    throw new DomainValidationException("Falta seleccionar la Descripcion en el campo de la tarea");
                }
            }

            return base.ValidateEntity(entity, mode);
        }

        public override Task DeleteAsync(int id)
        {
            //TODO: Validar el borrado
            return base.DeleteAsync(id);
        }
    }

}
