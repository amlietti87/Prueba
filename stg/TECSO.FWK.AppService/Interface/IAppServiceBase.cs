using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.AppService.Model;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;

namespace TECSO.FWK.AppService.Interface
{
    public interface IAppServiceBase
    {

    }


    public interface IAppServiceBase<TEntity, TPrimaryKey> : IAppServiceBase
        where TEntity : Entity<TPrimaryKey>
    {
        TEntity Add(TEntity obj);
        Task<TEntity> AddAsync(TEntity entity);


        TEntity AddOrUpdate(TEntity entity);
        Task<TEntity> AddOrUpdateAsync(TEntity entity);


        TEntity Update(TEntity entity);
        Task<TEntity> UpdateAsync(TEntity entity);


        void Delete(TPrimaryKey id);
        Task DeleteAsync(TPrimaryKey id);



        TEntity GetById(TPrimaryKey id);
        Task<TEntity> GetByIdAsync(TPrimaryKey id);


        PagedResult<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);
        Task<PagedResult<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, Object>>> includeExpression = null);

        Task<List<ItemDto<TPrimaryKey>>> GetItemsAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new();

        PagedResult<TEntity> GetPagedList<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>;
        Task<PagedResult<TEntity>> GetPagedListAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new();

        void Dispose();

    }


    public interface IAppServiceBase<TEntity, TDto,  TPrimaryKey> : IAppServiceBase<TEntity, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TDto : EntityDto<TPrimaryKey>
    {
        Task<TDto> UpdateAsync(TDto dto);
        Task<TDto> AddAsync(TDto dto);
        Task<TDto> AddOrUpdateAsync(TDto dto);

        Task<TDto> GetDtoByAndBlockIdAsync(TPrimaryKey id);
        Task<TDto> GetDtoByIdAsync(TPrimaryKey id); 
        Task<PagedResult<TDto>> GetDtoAllAsync(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, Object>>> includeExpression = null); 
        Task<PagedResult<TDto>> GetDtoAllFilterAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>; 
        Task<PagedResult<TDto>> GetDtoPagedListAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new();
        Task UnBlockEntity(TPrimaryKey id);
        Task ValidateCocurrencySave(TPrimaryKey id, DateTime lockDate);
    }

    public interface IHttpAppServiceBase<TEntity, TDto, TPrimaryKey>: IAppServiceBase<TEntity, TDto, TPrimaryKey>
        where TEntity : Entity<TPrimaryKey>
        where TDto : EntityDto<TPrimaryKey>
    {
        string EndPoint { get; }
    }
}
