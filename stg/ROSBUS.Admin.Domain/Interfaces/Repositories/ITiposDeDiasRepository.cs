using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Repositories;

namespace ROSBUS.Admin.Domain.Interfaces.Repositories
{
    public interface ITiposDeDiasRepository: IRepositoryBase<HTipodia,int>
    {
        Task<List<KeyValuePair<bool, string>>> DescripcionPredictivo(TiposDeDiasFilter filtro);
    }
}
