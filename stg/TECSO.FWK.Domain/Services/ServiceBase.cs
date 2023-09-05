using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Domain.Interfaces.Services;

namespace TECSO.FWK.Domain.Services
{
    public class ServiceBase<TEntity, TPrimaryKey, TRepository> : IDisposable, IServiceBase<TEntity, TPrimaryKey>
         where TEntity : Entity<TPrimaryKey>
        where TRepository : IRepositoryBase<TEntity, TPrimaryKey>
    {
        private readonly TRepository _repository;

        protected TRepository repository
        {
            get
            {
                return _repository;
            }
        }

        public ServiceBase(TRepository repository)
        {
            _repository = repository;
        }

        public virtual TEntity Add(TEntity entity)
        {
            return _repository.Add(entity);
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            await this.ValidateEntity(entity, SaveMode.Add);
            return await _repository.AddAsync(entity);
        }

        public virtual TEntity AddOrUpdate(TEntity entity)
        {
            return _repository.AddOrUpdate(entity);
        }

        public virtual Task<TEntity> AddOrUpdateAsync(TEntity entity)
        {
            return _repository.AddOrUpdateAsync(entity);
        }

        public void Delete(TPrimaryKey id)
        {
            _repository.Delete(id);
        }

        public virtual Task DeleteAsync(TPrimaryKey id)
        {
            return _repository.DeleteAsync(id);
        }

        public void Dispose()
        {
            _repository.Dispose();
        }

        public PagedResult<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.GetAll(predicate);
        }

        public Task<PagedResult<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, Object>>> includeExpression = null)
        {
            return _repository.GetAllAsync(predicate, includeExpression);
        }

        public TEntity GetById(TPrimaryKey id)
        {
            return _repository.GetById(id);
        }

        public Task<TEntity> GetDtoByAndBlockIdAsync(TPrimaryKey id)
        {
            return _repository.GetDtoByAndBlockIdAsync(id);
        }

        public Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            return _repository.GetByIdAsync(id);
        }

        public TEntity Update(TEntity entity)
        {
            return _repository.Update(entity);
        }

        public async Task<TEntity> UpdateAsync(TEntity entity)
        {
            await this.ValidateEntity(entity, SaveMode.Update);
            return await _repository.UpdateAsync(entity);
        }

        protected async virtual Task ValidateEntity(TEntity entity, SaveMode mode)
        {
            await Task.FromResult(true);
        }

        public PagedResult<TEntity> GetPagedList<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>
        {
            return _repository.GetPagedList(filter);
        }

        public Task<PagedResult<TEntity>> GetPagedListAsync<TFilter>(TFilter filter)
            where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new()
        {
            return _repository.GetPagedListAsync(filter);
        }

        public Task<TEntity> GetByIdAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>
        {
            return _repository.GetByIdAsync(filter);
        }

        public bool ExistExpression(Expression<Func<TEntity, bool>> predicate)
        {
            return _repository.ExistExpression(predicate);
        }

        public Task UnBlockEntity(TPrimaryKey id)
        {
            return this.repository.UnBlockEntity(id);
        }

        public Task ValidateCocurrencySave(TPrimaryKey id, DateTime lockDate)
        {
            return this.repository.ValidateCocurrencySave(id, lockDate);
        }
    }

    public enum SaveMode
    {
        Add,
        Update,
        Delete
    }
}
