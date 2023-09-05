using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class InspTareasRealizadasService : ServiceBase<InspTareasRealizadas,int, IInspTareasRealizadasRepository>, IInspTareasRealizadasService
    {
        private readonly IInspTareaService inspTareaService;
        private readonly IAuthService authService;
        private readonly IUserService userService;

        public InspTareasRealizadasService(IInspTareasRealizadasRepository repo,IInspTareaService _inspTareaService, IAuthService _authService, IUserService _userService )
            : base(repo)
        {
            this.inspTareaService = _inspTareaService;
            this.authService = _authService;
            this.userService = _userService;
        }


        protected override async Task ValidateEntity(InspTareasRealizadas entity, SaveMode mode)
        {
            await base.ValidateEntity(entity, mode);

            var tarea = (await this.inspTareaService.GetAllAsync(e => e.Id == entity.TareaId, new List<System.Linq.Expressions.Expression<Func<InspTarea, object>>>() { e=> e.TareasCampos })).Items.FirstOrDefault();

            var user = await this.userService.GetByIdAsync(this.authService.GetCurretUserId());

            if (mode == SaveMode.Add && (user==null  || user?.EmpleadoId != entity.EmpleadoId))
            {
                throw new ValidationException($"El Id de Empleado es incorrecto.");
            }

            if (tarea==null)
            {
                throw new ValidationException($"No se puede determinar la tarea.");
            }

            if (tarea.TareasCampos.Count!=entity.InspTareasRealizadasDetalle.Count)
            {
                throw new ValidationException($"El total de campos a completar debe ser {tarea.TareasCampos.Count}");
            }

            foreach (var item in tarea.TareasCampos)
            {
                if (!entity.InspTareasRealizadasDetalle.Any(e=> e.TareaCampoId==item.Id))
                {
                    throw new ValidationException($"No se especifico valor para el campo {item.Etiqueta}");
                }

                if (item.Requerido && !entity.InspTareasRealizadasDetalle.Any(e => e.TareaCampoId == item.Id && !string.IsNullOrEmpty(e.Valor)))
                {
                    throw new ValidationException($"No se especifico valor para el campo {item.Etiqueta}");
                }
            }

            

        }

    }
    
}
