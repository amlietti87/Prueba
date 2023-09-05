using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.service.http;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_FirmarEmpleador : Firmador.AccionFirmadorBase, IAccion_FirmarEmpleador
    {
        

        public Accion_FirmarEmpleador(IServiceProvider _serviceProvider, IFirmadorHelper firmadorHelper, IFdFirmadorAppService fdFirmadorAppService, IAdjuntosService adjuntosService, ISignalRHttpService signalRHttpService, IUserService userService) 
            : base(_serviceProvider, firmadorHelper, fdFirmadorAppService, adjuntosService, signalRHttpService, userService)
        {
            
        }




       



    }

    public interface IAccion_FirmarEmpleador : IAccionBuilder
    {
    }
}