using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.ApiServices.Filters;
using TECSO.FWK.ApiServices.Model;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.ApiServices
{
    public abstract class ManagerController<TModel, TPrimaryKey, TDto, TFilter, TService> : ManagerControllerBase<TModel, TPrimaryKey, TFilter>
        where TModel : Entity<TPrimaryKey>, new()
        where TDto : EntityDto<TPrimaryKey>, new()
        where TFilter : FilterPagedListBase<TModel, TPrimaryKey>, IFilterPagedListBase<TModel, TPrimaryKey>, new()
        where TService : IAppServiceBase<TModel, TDto, TPrimaryKey>

    {

        protected ManagerController(TService service)
            : base()
        {
            _service = service;
        }

        TService _service;


        protected TService Service
        {
            get
            {
                return _service;
            }
        }

        protected virtual IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> list)
        {
            return AutoMapper.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(list);
        }

        protected virtual TDestination MapObject<TSource, TDestination>(TSource obj)
        {
            return AutoMapper.Mapper.Map<TSource, TDestination>(obj);
        }




        [HttpGet]
        public virtual async Task<IActionResult> GetByIdAsync(TPrimaryKey Id, Boolean blockEntity = true)
        {
            try
            {
                IActionResult returnData;
                if (blockEntity && typeof(TModel).GetInterfaces().Any(e=> e.IsGenericType && e.GetGenericTypeDefinition()== typeof(IConcurrencyEntity)))
                {
                    returnData = ReturnData<TDto>(await this.Service.GetDtoByAndBlockIdAsync(Id));
                }
                else
                {
                    returnData = ReturnData<TDto>(await this.Service.GetDtoByIdAsync(Id));
                }

                return returnData;
            }
            catch (Exception ex)
            {
                return ReturnError<TDto>(ex);
            }
        }




        [HttpPost]
        public virtual async Task<IActionResult> SaveNewEntity([FromBody] TDto dto)
        {
            try
            {
                if (this.ModelState.IsValid)
                {
                    TPrimaryKey id = (await this.Service.AddAsync(dto)).Id;
                    return ReturnData<TPrimaryKey>(id);
                }
                else
                {
                    return ReturnError<TDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<TDto>(ex);
            }
        }


        [HttpPost]
        public virtual async Task<IActionResult> UpdateEntity([FromBody] TDto dto)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    TPrimaryKey id = (await this.Service.UpdateAsync(dto)).Id;
                    return ReturnData<TPrimaryKey>(id);
                }
                else
                {
                    return ReturnError<TDto>(this.ModelState);
                }

            }
            catch (Exception ex)
            {
                return ReturnError<TDto>(ex);
            }
        }

        [HttpPost]
        public virtual async Task<IActionResult> DeleteEntity([FromBody] TFilter filter)
        {
            try
            {
                await this.Service.DeleteAsync(filter.Id);

                return ReturnData<string>("Deleted");
            }
            catch (Exception ex)
            {
                return ReturnError<TDto>(ex);
            }
        }


        [HttpPost]
        public virtual async Task<IActionResult> DeleteById(TPrimaryKey Id)
        {
            try
            {
                await this.Service.DeleteAsync(Id);

                return ReturnData<string>("Deleted");
            }
            catch (Exception ex)
            {
                return ReturnError<TDto>(ex);
            }
        }


        [HttpGet]
        public async virtual Task<IActionResult> GetAllAsync(TFilter filter)
        {
            try
            {
                Expression<Func<TModel, bool>> exp = e => true;


                if (filter != null)
                {
                    exp = filter.GetFilterExpression();
                }

                var pList = await this.Service.GetDtoAllAsync(exp);

                return ReturnData<PagedResult<TDto>>(pList);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<TDto>>(ex);
            }
        }



        [HttpGet]
        public virtual async Task<IActionResult> GetItemsAsync(TFilter filter)
        {
            try
            {
                var r = await this.Service.GetItemsAsync(filter);
                return ReturnData<List<ItemDto<TPrimaryKey>>>(r);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<TDto>>(ex);
            }
        }


        [HttpPost]
        public async Task<IActionResult> FindItemsAsync([FromBody]TFilter filter)
        {
            try
            {
                var r = await this.Service.GetItemsAsync(filter);
                return ReturnData<List<ItemDto<TPrimaryKey>>>(r);

            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<TDto>>(ex);
            }
        }



        [HttpPost]
        public virtual async Task<IActionResult> GetPagedList([FromBody] TFilter filter)
        {
            try
            {
                var pList = await this.Service.GetDtoPagedListAsync<TFilter>(filter);
                return ReturnData<PagedResult<TDto>>(pList);
            }
            catch (Exception ex)
            {
                return ReturnError<PagedResult<TDto>>(ex);
            }
        }


        [HttpPost]
        
        public virtual async Task<IActionResult> UnBlockEntity(TPrimaryKey Id)
        {
            try
            {
                await this.Service.UnBlockEntity(Id);

                return ReturnData<string>("UnBlocked");
            }
            catch (Exception ex)
            {
                return ReturnError<TDto>(ex);
            }
        }


    }

    

    

}
