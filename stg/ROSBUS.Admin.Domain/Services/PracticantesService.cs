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
    public class PracticantesService : ServiceBase<SinPracticantes, int, IPracticantesRepository>, IPracticantesService
    { 
        public PracticantesService(IPracticantesRepository produtoRepository,
            ISiniestrosRepository siniestrosRepository)
            : base(produtoRepository)
        {
            _siniestrosRepository = siniestrosRepository;
        }


        private readonly ISiniestrosRepository _siniestrosRepository;

        public override Task DeleteAsync(int id)
        {

            var siniestros = _siniestrosRepository.GetAllAsync(e => e.PracticanteId == id).Result;
            if (siniestros.Items.Count > 0)
            {
                throw new DomainValidationException("Existen Siniestros asociados a este Practicante");
            }

            return base.DeleteAsync(id);
        }



    }
    
}
