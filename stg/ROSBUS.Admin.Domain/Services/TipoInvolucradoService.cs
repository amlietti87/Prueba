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
    public class TipoInvolucradoService : ServiceBase<SinTipoInvolucrado, int, ITipoInvolucradoRepository>, ITipoInvolucradoService
    { 
        public TipoInvolucradoService(ITipoInvolucradoRepository produtoRepository, IInvolucradosRepository involucradosRepository)
            : base(produtoRepository)
        {
            _involucradosRepository = involucradosRepository;
        }

        private readonly IInvolucradosRepository _involucradosRepository;

        public override Task DeleteAsync(int id)
        {

            var involucrados = _involucradosRepository.GetAllAsync(e => e.TipoInvolucradoId == id).Result;
            if (involucrados.Items.Count > 0)
            {
                throw new DomainValidationException("Existen involucrados asociados a este tipo de involucrado");
            }

            return base.DeleteAsync(id);
        }




    }
    
}
