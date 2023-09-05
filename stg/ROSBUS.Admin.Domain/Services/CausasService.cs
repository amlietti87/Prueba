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
    public class CausasService : ServiceBase<SinCausas, int, ICausasRepository>, ICausasService
    {
        public CausasService(ICausasRepository produtoRepository, ISubCausasRepository SubCausasRepository, ISiniestrosRepository SiniestrosRepository)
            : base(produtoRepository)
        {
            _siniestrosRepository = SiniestrosRepository;
            _SubCausasRepository = SubCausasRepository;
        }



        private readonly ISiniestrosRepository _siniestrosRepository;
        private readonly ISubCausasRepository _SubCausasRepository;

        public override Task DeleteAsync(int id)
        {

            var siniestros = _siniestrosRepository.GetAllAsync(e => e.CausaId == id).Result;
            if (siniestros.Items.Count > 0)
            {
                throw new DomainValidationException("Existen Siniestros asociados a esta Causa");
            }

            //var subcausas = _SubCausasRepository.GetAllAsync(e => e.CausaId == id).Result;
            //if (subcausas.Items.Count > 0)
            //{
            //    throw new DomainValidationException("Existen SubCausas asociadas a esta Causa");
            //}



            return base.DeleteAsync(id);
        }

        protected async override Task ValidateEntity(SinCausas entity, SaveMode mode)
        {
            await base.ValidateEntity(entity, mode);

            if (entity.SinSubCausas.Count == 0)
            {
                throw new DomainValidationException("Debe agregar al menos una Sub-Causa");
            }

            if (entity.Anulado)
            {

                foreach (var item in entity.SinSubCausas)
                {
                    if (!item.Anulado)
                    {
                        throw new DomainValidationException("No se puede tener anulada la Causa sin que se anulen todas sus Sub-Causas");
                    }
                }
            }
        }
    }
}


