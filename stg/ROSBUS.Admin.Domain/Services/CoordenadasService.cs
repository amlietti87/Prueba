using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
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
    public class CoordenadasService : ServiceBase<PlaCoordenadas, int, ICoordenadasRepository>, ICoordenadasService
    {
        public CoordenadasService(ICoordenadasRepository produtoRepository)
            : base(produtoRepository)
        {

        }

        protected override Task ValidateEntity(PlaCoordenadas entity, SaveMode mode)
        {
            var otroigual = this.repository.ExistExpression(e =>
             e.Id != entity.Id
            && !e.IsDeleted
            && e.Calle1 == entity.Calle1
            && e.LocalidadId == entity.LocalidadId
            && e.Calle2 == entity.Calle2
            && e.BeforeMigration == false);


            if (otroigual)
            {
                throw new DomainValidationException("La combinacion de calle1 y calle2 ya fueron cargada para la localidad ");
            }


            var otroigualDes = this.repository.ExistExpression(e =>
             e.Id != entity.Id
            && !e.IsDeleted
            && e.DescripcionCalle1 == entity.DescripcionCalle1
            && e.LocalidadId == entity.LocalidadId
            && e.DescripcionCalle2 == entity.DescripcionCalle2
            && e.BeforeMigration == false);


            if (otroigualDes)
            {
                throw new DomainValidationException("La combinacion de Descripcion Calle 1 y Descripcion Calle 2 ya fueron cargada para la localidad ");
            }

            return base.ValidateEntity(entity, mode);
        }

        public async Task<List<PlaCoordenadas>> RecuperarCoordenadasPorFecha(CoordenadasFilter filter)
        {
            var coordenadas = await repository.RecuperarCoordenadasPorFecha(filter);

            return coordenadas;
        }



    }

}
