using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface IHFechasRepository: IRepositoryBase<HFechas,int>
    {
        Task<HFechas> GetHFechaAsync(HFechasFilter filter);
        Task<HFechas> RecuperarProximaFecha(int cod_linea, int idTipoDia, DateTime fecDesde);
    }
}
