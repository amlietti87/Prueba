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
    public class ConsecuenciasService : ServiceBase<SinConsecuencias, int, IConsecuenciasRepository>, IConsecuenciasService
    {
        public ConsecuenciasService(IConsecuenciasRepository produtoRepository, ISiniestrosConsecuenciasRepository siniestrosConsecuenciasRepository,
            ICategoriasRepository categoriasRepository
            )
            : base(produtoRepository)
        {
            _siniestrosConsecuenciasRepository = siniestrosConsecuenciasRepository;
            _categoriasRepository = categoriasRepository;
        }


        private readonly ISiniestrosConsecuenciasRepository _siniestrosConsecuenciasRepository;
        private readonly ICategoriasRepository _categoriasRepository;

        public override Task DeleteAsync(int id)
        {

            var siniestrosConsecuencias = _siniestrosConsecuenciasRepository.GetAllAsync(e => e.ConsecuenciaId == id).Result;
            if (siniestrosConsecuencias.Items.Count > 0)
            {
                throw new DomainValidationException("Existen siniestros asociados a esta consecuencia");
            }

            //var categoriasConsecuencia = _categoriasRepository.GetAllAsync(e => e.ConsecuenciaId == id).Result;
            //if (categoriasConsecuencia.Items.Count > 0)
            //{
            //    throw new DomainValidationException("Existen categorias asociadas a esta consecuencia");
            //}



            return base.DeleteAsync(id);
        }

        protected async override Task ValidateEntity(SinConsecuencias entity, SaveMode mode)
        {
            await base.ValidateEntity(entity, mode);

            if (entity.Anulado)
            {

                foreach (var item in entity.SinCategorias)
                {
                    if (!item.Anulado)
                    {
                        throw new DomainValidationException("No se puede tener anulada la Consecuencia sin que se anulen todas sus Categorías");
                    }
                }
            }
        }


        public async Task<List<SinConsecuencias>> GetConsecuenciasSinAnular()
        {
            return await this.repository.GetConsecuenciasSinAnular();
        }
    }

}
