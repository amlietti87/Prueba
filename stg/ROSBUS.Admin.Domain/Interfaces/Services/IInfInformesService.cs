using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.Domain.Interfaces.Services
{
    public interface IInfInformesService : IServiceBase<InfInformes, string>
    {
        Task<List<Destinatario>> GetNotificacionesMail(string token);

        Task<List<InformeConsulta>> ConsultaInformesUserDia();
    }
}
