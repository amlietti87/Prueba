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
    public class TipoLesionadoService : ServiceBase<SinTipoLesionado, int, ITipoLesionadoRepository>, ITipoLesionadoService
    { 
        public TipoLesionadoService(ITipoLesionadoRepository produtoRepository, ILesionadosRepository lesionadosRepository)
            : base(produtoRepository)
        {
            _lesionadosRepository = lesionadosRepository;
        }



        private readonly ILesionadosRepository _lesionadosRepository;

        public override Task DeleteAsync(int id)
        {

            var lesionados = _lesionadosRepository.GetAllAsync(e => e.TipoLesionadoId == id).Result;
            if (lesionados.Items.Count > 0)
            {
                throw new DomainValidationException("Existen Lesionados asociados a este Tipo de Lesionado");
            }

            return base.DeleteAsync(id);
        }

    }
    
}
