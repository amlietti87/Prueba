using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.service.http;
using ROSBUS.Admin.Domain.Constants;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using ROSBUS.Admin.Domain.Interfaces.Services;
using ROSBUS.Operaciones.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory
{
    public class Accion_FirmarEmpleado : Firmador.AccionFirmadorBase, IAccion_FirmarEmpleado
    {
      

        public Accion_FirmarEmpleado(IServiceProvider _serviceProvider, IAdjuntosService adjuntosService, IFirmadorHelper firmadorHelper, IFdFirmadorAppService fdFirmadorAppService, ISignalRHttpService signalRHttpService, IUserService userService) 
            : base(_serviceProvider, firmadorHelper, fdFirmadorAppService, adjuntosService, signalRHttpService, userService)
        {


        }
        
      

        
    }

    public interface IAccion_FirmarEmpleado : IAccionBuilder
    {
    }
}