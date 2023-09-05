using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Entities.Filters;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class RamalColorAppService : AppServiceBase<PlaRamalColor, RamalColorDto, Int64, IRamalColorService>, IRamalColorAppService
    {
        public readonly IBanderaService banderaService;

        public RamalColorAppService(IRamalColorService serviceBase, IBanderaService banderaService) 
            :base(serviceBase)
        {
            this.banderaService = banderaService;
        }

        public async override Task<RamalColorDto> UpdateAsync(RamalColorDto dto)
        { 
            var entity = await this.GetByIdAsync(dto.Id);
            


            if (dto.Activo && !entity.Activo)
            {
                var any = this.banderaService.ExistExpression(e => e.RamalColorId == dto.Id && e.Activo == true);
                if (!any)
                {
                    throw new ValidationException("No puede activar un Ramal/Color sin banderas activa");
                }
            }

            MapObject(dto, entity);

            await this.UpdateAsync(entity);

            return MapObject<PlaRamalColor, RamalColorDto>(entity);
        }


        public async override Task<RamalColorDto> AddAsync(RamalColorDto dto)
        {
            dto.Activo = false;
            var entity = MapObject<RamalColorDto, PlaRamalColor>(dto);
             
    

            return MapObject<PlaRamalColor, RamalColorDto>(await this.AddAsync(entity));
        }

        public async Task<List<TECSO.FWK.Domain.Interfaces.Entities.ItemDto<long>>> GetItemsAsyncSinSentidos(RamalColorFilter filter)
        {

            System.Linq.Expressions.Expression<Func<PlaRamalColor, bool>> exp = e => true;
            if (filter != null)
            {
                exp = filter.GetFilterExpression();
            }
            var include = new List<System.Linq.Expressions.Expression<Func<PlaRamalColor, object>>>
            {
                e=> e.HBanderas

            };
            var list = await this._serviceBase.GetAllSinSentidos(exp, include);
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;
        }

        public override async Task<List<TECSO.FWK.Domain.Interfaces.Entities.ItemDto<long>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            System.Linq.Expressions.Expression<Func<PlaRamalColor, bool>> exp = e => true;
            if (filter != null)
            {
                exp = filter.GetFilterExpression();
            }
            var include = new List<System.Linq.Expressions.Expression<Func<PlaRamalColor, object>>>
            {
                e=> e.HBanderas

            };
            var list = await this._serviceBase.GetAllAsync(exp,include);
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;
        }

        public override Task DeleteAsync(long id)
        {

            if (this._serviceBase.ExistExpression(e => e.Id == id && e.HBanderas.Any(b => b.Rutas.Any(rut => rut.EstadoRutaId == PlaEstadoRuta.APROBADO))))
            {
                throw new ValidationException("No se permite Eliminar el Ramal ya que tiene rutas aprobadas.");
            }
            return base.DeleteAsync(id);
        }

        public bool TieneMapasEnBorrador(int id)
        {
            return this._serviceBase.ExistExpression(e => e.Id == id && e.HBanderas.Any(b => b.Rutas.Any(rut => rut.EstadoRutaId == PlaEstadoRuta.BORRADOR)));
        }
    }
}
