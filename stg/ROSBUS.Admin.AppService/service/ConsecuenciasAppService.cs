using ROSBUS.Admin.AppService.Interface;
using ROSBUS.Admin.AppService.Model;
using ROSBUS.Admin.Domain.Entities;
using ROSBUS.Admin.Domain.Interfaces.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace ROSBUS.Admin.AppService
{

    public class ConsecuenciasAppService : AppServiceBase<SinConsecuencias, ConsecuenciasDto, int, IConsecuenciasService>, IConsecuenciasAppService
    {
        public ConsecuenciasAppService(IConsecuenciasService serviceBase)
            : base(serviceBase)
        {

        }

        public override async Task<ConsecuenciasDto> AddAsync(ConsecuenciasDto dto)
        {

            var entity = MapObject<ConsecuenciasDto, SinConsecuencias>(dto);
            foreach (var item in entity.SinCategorias.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            var result = await this.AddAsync(entity);
            return MapObject<SinConsecuencias, ConsecuenciasDto>(entity);
        }





        public async override Task<ConsecuenciasDto> UpdateAsync(ConsecuenciasDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);



            foreach (var item in entity.SinCategorias.Where(w => w.Id < 0).ToList())
            {
                item.Id = 0;
            }

            await this.UpdateAsync(entity);


            return MapObject<SinConsecuencias, ConsecuenciasDto>(entity);
        }

        public async Task<List<SinConsecuencias>> GetConsecuenciasSinAnular()
        {
            return await _serviceBase.GetConsecuenciasSinAnular();
        }


        public async override Task<PagedResult<ConsecuenciasDto>> GetDtoPagedListAsync<TFilter>(TFilter filter)
        {
            var list = await _serviceBase.GetPagedListAsync(filter);
            var listDto = this.MapList<SinConsecuencias, ConsecuenciasDto>(list.Items);

            PagedResult<ConsecuenciasDto> pList = new PagedResult<ConsecuenciasDto>(list.TotalCount, listDto.ToList());
            return pList;
        }

        public async override Task<List<ItemDto<int>>> GetItemsAsync<TFilter>(TFilter filter)
        {
            Expression<Func<SinConsecuencias, bool>> exp = e => true;

            if (filter != null)
            {
                exp = filter.GetFilterExpression();
            }
            var list = await this._serviceBase.GetAllAsync(exp);
            var r = list.Items.Select(filter.GetItmDTO()).ToList();
            return r;
        }

        public async override Task<PagedResult<ConsecuenciasDto>> GetDtoAllAsync(Expression<Func<SinConsecuencias, bool>> predicate, List<Expression<Func<SinConsecuencias, object>>> includeExpression = null)
        {
            var list = await this._serviceBase.GetAllAsync(predicate, includeExpression);


            var listDto = this.MapList<SinConsecuencias, ConsecuenciasDto>(list.Items);

            PagedResult<ConsecuenciasDto> pList = new PagedResult<ConsecuenciasDto>(list.TotalCount, listDto.ToList());

            return pList;
        }
    }
}
