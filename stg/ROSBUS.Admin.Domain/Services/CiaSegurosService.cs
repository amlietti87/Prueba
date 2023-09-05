using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class CiaSegurosService : ServiceBase<SinCiaSeguros, int, ICiaSegurosRepository>, ICiaSegurosService
    {

        private readonly IVehiculosRepository _vehiculosRepository;
        private readonly ISiniestrosRepository _siniestrosRepository;
        public CiaSegurosService(ICiaSegurosRepository produtoRepository, IVehiculosRepository vehiculosRepository, ISiniestrosRepository siniestrosRepository)
            : base(produtoRepository)
        {
            _vehiculosRepository = vehiculosRepository;
            _siniestrosRepository = siniestrosRepository;
        }


        public override Task DeleteAsync(int id)
        {

            var vehiculosasociados = _vehiculosRepository.GetAllAsync(e => e.SeguroId == id).Result;
            if (vehiculosasociados.Items.Count > 0)
            {
                throw new DomainValidationException("Existen vehiculos asociados a esta compañia de seguros");
            }

            var siniestrosasociados = _siniestrosRepository.GetAllAsync(e => e.SeguroId == id).Result;
            if (siniestrosasociados.Items.Count > 0)
            {
                throw new DomainValidationException("Existen siniestros asociados a esta compañia de seguros");
            }


            return base.DeleteAsync(id);
        }


    }
    
}
