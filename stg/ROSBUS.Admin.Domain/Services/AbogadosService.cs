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
    public class AbogadosService : ServiceBase<SinAbogados, int, IAbogadosRepository>, IAbogadosService
    { 
        public AbogadosService(IAbogadosRepository produtoRepository, IReclamosRepository reclamosRepository)
            : base(produtoRepository)
        {
            _reclamosRepository = reclamosRepository;
        }

        private readonly IReclamosRepository _reclamosRepository;



        public override Task DeleteAsync(int id)
        {

            var reclamosasociados = _reclamosRepository.GetAllAsync(e => e.AbogadoId == id || e.AbogadoEmpresaId == id).Result;
            if (reclamosasociados.Items.Count > 0)
            {
                throw new DomainValidationException("Existen reclamos asociados a este abogado");
            }



            return base.DeleteAsync(id);
        }


    }
    
}
