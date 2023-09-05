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
    public class TipoMuebleService : ServiceBase<SinTipoMueble, int, ITipoMuebleRepository>, ITipoMuebleService
    { 
        public TipoMuebleService(ITipoMuebleRepository produtoRepository,
            IMuebleInmuebleRepository muebleInmuebleRepository)
            : base(produtoRepository)
        {
            _muebleInmuebleRepository = muebleInmuebleRepository;
        }

        private readonly IMuebleInmuebleRepository _muebleInmuebleRepository;

        public override Task DeleteAsync(int id)
        {

            var muebles = _muebleInmuebleRepository.GetAllAsync(e => e.TipoInmuebleId == id).Result;
            if (muebles.Items.Count > 0)
            {
                throw new DomainValidationException("Existen Muebles / Inmuebles asociados a este Tipo de Mueble / Inmueble");
            }

            return base.DeleteAsync(id);
        }



    }

}
