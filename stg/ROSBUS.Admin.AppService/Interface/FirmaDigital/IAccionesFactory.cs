using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.AppService.service.FirmaDigital.AccionesFactory;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.FirmaDigital;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;

namespace ROSBUS.Admin.AppService.Interface
{
    public interface IAccionesFactory
    {
        void Inicialize();
        IAccionBuilder GetAccionFromId(int id);
    }
}
