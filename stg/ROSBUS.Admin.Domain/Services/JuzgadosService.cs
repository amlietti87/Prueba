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
    public class JuzgadosService : ServiceBase<SinJuzgados, int, IJuzgadosRepository>, IJuzgadosService
    { 
        public JuzgadosService(IJuzgadosRepository produtoRepository,
            IReclamosRepository reclamosRepository)
            : base(produtoRepository)
        {
            _reclamosRepository = reclamosRepository;
        }

        private readonly IReclamosRepository _reclamosRepository;

        public override Task DeleteAsync(int id)
        {

            var reclamos = _reclamosRepository.GetAllAsync(e => e.JuzgadoId == id).Result;
            if (reclamos.Items.Count > 0)
            {
                throw new DomainValidationException("Existen Reclamos asociados a este Juzgado");
            }

            return base.DeleteAsync(id);
        }


    }
    
}
