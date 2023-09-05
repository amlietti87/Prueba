using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Partials;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IInfInformesRepository : IRepositoryBase<InfInformes, string>
    {
        Task<List<Destinatario>> GetNotificacionesMail(string token);

        Task<List<InformeConsulta>> ConsultaInformesUserDia();
    }
}
