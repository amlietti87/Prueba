using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Interface;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Services;

namespace TECSO.FWK.AppService
{
    public class AppServiceBase<TEntity, TPrimaryKey, TServiceBase> : IDisposable, IAppServiceBase<TEntity, TPrimaryKey>
         where TEntity : Entity<TPrimaryKey>
        where TServiceBase : IServiceBase<TEntity, TPrimaryKey>
    {
        protected readonly TServiceBase _serviceBase;

        public AppServiceBase(TServiceBase serviceBase)
        {
            _serviceBase = serviceBase;
        }

        public TEntity Add(TEntity entity)
        {
            return _serviceBase.Add(entity);
        }

        public Task<TEntity> AddAsync(TEntity entity)
        {
            return _serviceBase.AddAsync(entity);
        }

        public TEntity AddOrUpdate(TEntity entity)
        {
            return _serviceBase.AddOrUpdate(entity);
        }

        public Task<TEntity> AddOrUpdateAsync(TEntity entity)
        {
            return _serviceBase.AddOrUpdateAsync(entity);
        }

        public void Delete(TPrimaryKey id)
        {
            _serviceBase.Delete(id);
        }

        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            return _serviceBase.DeleteAsync(id);
        }

        public void Dispose()
        {
            _serviceBase.Dispose();
        }

        public PagedResult<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _serviceBase.GetAll(predicate);
        }

        public Task<PagedResult<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, Object>>> includeExpression = null)
        {
            return _serviceBase.GetAllAsync(predicate, includeExpression);
        }

        public TEntity GetById(TPrimaryKey id)
        {
            return _serviceBase.GetById(id);
        }

        public async Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return await _serviceBase.GetByIdAsync(id);
        }

        public async Task<TEntity> GetByIdAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>
        {
            return await _serviceBase.GetByIdAsync(filter);
        }



        public virtual async Task<List<ItemDto<TPrimaryKey>>> GetItemsAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new()
        {
            Expression<Func<TEntity, bool>> exp = e => true;

            if (filter != null)
            {
                exp = filter.GetFilterExpression();
            }
            var list = await this._serviceBase.GetAllAsync(exp);

            var r = list.Items.Select(filter.GetItmDTO()).ToList();

            //foreach (var item in r)
            //{
            //    string[] arr = item.Description.Split(' ');
            //    IEnumerable<string> filteredArr = arr.Where(e => e != "").ToList();
            //    item.Description = String.Join(" ",filteredArr);
            //}
            return r;
        }

        public virtual PagedResult<TEntity> GetPagedList<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>
        {
            return _serviceBase.GetPagedList(filter);
        }

        public virtual Task<PagedResult<TEntity>> GetPagedListAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new()
        {
            return _serviceBase.GetPagedListAsync(filter);
        }

        public TEntity Update(TEntity entity)
        {
            return _serviceBase.Update(entity);
        }

        public Task<TEntity> UpdateAsync(TEntity entity)
        {
            return _serviceBase.UpdateAsync(entity);
        }
    }

    public class AppServiceBase<TEntity, TDto, TPrimaryKey, TServiceBase> : AppServiceBase<TEntity, TPrimaryKey, TServiceBase>, IAppServiceBase<TEntity, TDto, TPrimaryKey>
             where TEntity : Entity<TPrimaryKey>
            where TDto : EntityDto<TPrimaryKey>, new()
        where TServiceBase : IServiceBase<TEntity, TPrimaryKey>
    {


        public AppServiceBase(TServiceBase serviceBase) : base(serviceBase)
        {

        }

        public virtual async Task<TDto> UpdateAsync(TDto dto)
        {
            var entity = await this.GetByIdAsync(dto.Id);
            MapObject(dto, entity);
            await this.UpdateAsync(entity);
            return MapObject<TEntity, TDto>(entity);
        }


        public virtual async Task<TDto> AddOrUpdateAsync(TDto dto)
        {
            var entity = MapObject<TDto, TEntity>(dto);
            return MapObject<TEntity, TDto>(await this.AddOrUpdateAsync(entity));

        }


        public virtual async Task<TDto> AddAsync(TDto dto)
        {

            var entity = MapObject<TDto, TEntity>(dto);
            return MapObject<TEntity, TDto>(await this.AddAsync(entity));
        }

        protected virtual IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> list)
        {
            return AutoMapper.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(list);
        }

        protected virtual IEnumerable<TDestination> MapList<TSource, TDestination>(IEnumerable<TSource> source, IEnumerable<TDestination> destinations)
        {
            return AutoMapper.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source, destinations);
        }

        protected virtual TDestination MapObject<TSource, TDestination>(TSource obj)
        {
            return AutoMapper.Mapper.Map<TSource, TDestination>(obj);
        }

        protected virtual TDestination MapObject<TSource, TDestination>(TSource obj, TDestination dest)
        {
            return AutoMapper.Mapper.Map<TSource, TDestination>(obj, dest);
        }

        public virtual async Task<TDto> GetDtoByAndBlockIdAsync(TPrimaryKey id)
        {
            TEntity entity = await this._serviceBase.GetDtoByAndBlockIdAsync(id);
            var dto = this.MapObject<TEntity, TDto>(entity);
            return dto;
        }

        public virtual async Task<TDto> GetDtoByIdAsync(TPrimaryKey id)
        {

            TEntity entity = await this._serviceBase.GetByIdAsync(id);
            var dto = this.MapObject<TEntity, TDto>(entity);
            return dto;
        }

        public virtual async Task<PagedResult<TDto>> GetDtoAllAsync(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, object>>> includeExpression = null)
        {
            var list = await this._serviceBase.GetAllAsync(predicate, includeExpression);


            var listDto = this.MapList<TEntity, TDto>(list.Items);

            PagedResult<TDto> pList = new PagedResult<TDto>(list.TotalCount, listDto.ToList());

            return pList;
        }

        public async virtual Task<PagedResult<TDto>> GetDtoAllFilterAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>
        {
            return await this.GetDtoAllAsync(filter.GetFilterExpression(), filter.GetIncludesForPageList());
        }

        public virtual async  Task<PagedResult<TDto>> GetDtoPagedListAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new()
        {
            var list = await _serviceBase.GetPagedListAsync(filter);
            var listDto = this.MapList<TEntity, TDto>(list.Items);

            PagedResult<TDto> pList = new PagedResult<TDto>(list.TotalCount, listDto.ToList());
            return pList;
        }

        public virtual Task UnBlockEntity(TPrimaryKey id)
        {
            return this._serviceBase.UnBlockEntity(id);
        }

        public Task ValidateCocurrencySave(TPrimaryKey id, DateTime lockDate)
        {
            return this._serviceBase.ValidateCocurrencySave(id, lockDate);
        }

       
    }

}
