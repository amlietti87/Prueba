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
    public class EstadosService : ServiceBase<SinEstados, int, IEstadosRepository>, IEstadosService
    { 
        public EstadosService(IEstadosRepository produtoRepository, IReclamosRepository reclamosRepository, ISubEstadosRepository subEstadosRepository)
            : base(produtoRepository)
        {
            _reclamosRepository = reclamosRepository;
            _subestadosRepository = subEstadosRepository;
        }


        private readonly IReclamosRepository _reclamosRepository;
        private readonly ISubEstadosRepository _subestadosRepository;

        public override Task DeleteAsync(int id)
        {
            //var SubEstados = _subestadosRepository.GetAllAsync(e => e.EstadoId == id).Result;
            //if (SubEstados.Items.Count > 0)
            //{
            //    throw new DomainValidationException("Existen Sub-Estados asociadas a este Estado");
            //}

            var reclamos = _reclamosRepository.GetAllAsync(e => e.EstadoId == id).Result;
            if (reclamos.Items.Count > 0)
            {
                throw new DomainValidationException("Existen Reclamos asociados a esta Estado");
            }

            return base.DeleteAsync(id);
        }

        protected async override Task ValidateEntity(SinEstados entity, SaveMode mode)
        {
            await base.ValidateEntity(entity, mode);

            if (entity.SinSubEstados.Count == 0)
            {
                throw new DomainValidationException("Debe agregar al menos un Sub-Estado");
            }

            if (entity.Anulado)
            {

                foreach (var item in entity.SinSubEstados)
                {
                    if (!item.Anulado)
                    {
                        throw new DomainValidationException("No se puede tener anulada el Estado sin que se anulen todos sus Sub-Estados");
                    }
                }
            }
        }

    }
    
}
