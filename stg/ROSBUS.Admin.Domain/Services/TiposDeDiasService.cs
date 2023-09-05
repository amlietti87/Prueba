using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Repositories;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain;
using TECSO.FWK.Domain.Services;

namespace ROSBUS.Admin.Domain.Services
{
    public class TiposDeDiasService : ServiceBase<HTipodia, int, ITiposDeDiasRepository>, ITiposDeDiasService
    {
        public TiposDeDiasService(ITiposDeDiasRepository produtoRepository)
            : base(produtoRepository)
        {

        }


        protected async override Task ValidateEntity(HTipodia entity, SaveMode mode)
        {
            var exist = this.repository.ExistExpression(e => e.Id != entity.Id && e.DesTdia.Trim() == entity.DesTdia.Trim());

            if (exist)
                throw new DomainValidationException("El Tipo de día ya existe");

            var existOrden = this.repository.ExistExpression(e => !e.IsDeleted && e.Id != entity.Id && e.Orden == entity.Orden); 
            if (existOrden)
                throw new DomainValidationException("El Orden ya existe"); 


            await base.ValidateEntity(entity, mode);
        }

        public async Task<List<KeyValuePair<bool, string>>> DescripcionPredictivo(TiposDeDiasFilter filtro)
        {
            return await this.repository.DescripcionPredictivo(filtro);
        }

        public override Task DeleteAsync(int id)
        {
            var existHHorariosConfi = this.repository.ExistExpression(e => e.Id == id && e.HHorariosConfi.Any());
            if (existHHorariosConfi)
            {
                throw new DomainValidationException("El tipo de dia esta siendo utilizado en un horario.");
            }
            var existe = this.repository.ExistExpression(e => e.Id == id && e.PlaDistribucionDeCochesPorTipoDeDia.Any());
            if (existe)
            {
                throw new DomainValidationException("El tipo de dia esta siendo utilizado en una estimacion de un horario.");
            }

            existe = this.repository.ExistExpression(e => e.Id == id && e.HMinxtipo.Any());
            if (existe)
            {
                throw new DomainValidationException("El tipo de dia esta siendo utilizado en minutos por sector");
            }

            return base.DeleteAsync(id);
        }

    }

}
