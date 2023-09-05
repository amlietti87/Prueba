using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class HFechasService : ServiceBase<HFechas,int, IHFechasRepository>, IHFechasService
    { 
        public HFechasService(IHFechasRepository produtoRepository)
            : base(produtoRepository)
        {
       
        }

        public Task<HFechas> RecuperarProximaFecha(int cod_linea, int idTipoDia, DateTime fecDesde)
        {
            return this.repository.RecuperarProximaFecha(cod_linea, idTipoDia, fecDesde);
        }
    }
    
}
