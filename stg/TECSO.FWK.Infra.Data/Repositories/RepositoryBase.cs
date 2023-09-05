using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using TECSO.FWK.Domain.Interfaces.Entities;
using TECSO.FWK.Domain.Interfaces.Repositories;
using TECSO.FWK.Infra.Data.Interface;
using System.Linq;
using TECSO.FWK.Domain.Entities;
using TECSO.FWK.Domain;
using System.Data.SqlClient;
using System.ComponentModel.DataAnnotations;
using TECSO.FWK.Domain.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using TECSO.FWK.Domain.Interfaces.Services;
using Snickler.EFCore;

namespace TECSO.FWK.Infra.Data.Repositories
{
    public abstract class RepositoryBase<TContext, TEntity, TPrimaryKey> : IDisposable, IRepositoryBase<TEntity, TPrimaryKey>
        where TContext : DbContext, IDbContext
         where TEntity : Entity<TPrimaryKey>
    {
        public virtual TContext Context { get; }

        //private readonly IResilientTransaction<TContext> resilientTransaction;
        private IDbContextProvider<TContext> dbContextProvider;
        private IAuthService authService;

        public RepositoryBase(IDbContextProvider<TContext> _dbContextProvider)
        {
            dbContextProvider = _dbContextProvider;
            Context = dbContextProvider.GetDbContext();
            authService = ServiceProviderResolver.ServiceProvider.GetService<IAuthService>();
            //this.resilientTransaction = ServiceProviderResolver.ServiceProvider.GetService<IResilientTransaction<TContext>>(); 
        }

        public void Dispose()
        {
            Context.Dispose();
        }


        public virtual int SaveChanges()
        {
            //if (!this.resilientTransaction.IsResilientTransaction())
            //{
            return Context.SaveChanges();
            //}
            //return 0;
        }

        public virtual Task<int> SaveChangesAsync()
        {
            ////if (!this.resilientTransaction.IsResilientTransaction())
            //{
            return Context.SaveChangesAsync();
            //}
            //return Task.FromResult(0);
        }


        public TEntity Add(TEntity entity)
        {
            try
            {
                DbSet<TEntity> dbSet = Context.Set<TEntity>();
                var entry = dbSet.Add(entity);
                this.SaveChanges();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }
        }

        public virtual async Task<TEntity> AddAsync(TEntity entity)
        {
            try
            {
                DbSet<TEntity> dbSet = Context.Set<TEntity>();
                var entry = await dbSet.AddAsync(entity);
                await this.SaveChangesAsync();
                return entry.Entity;
            }
            catch (Exception ex)
            {
                this.HandleException(ex);
                throw;
            }

        }




        public TEntity AddOrUpdate(TEntity entity)
        {


            try
            {
                DbSet<TEntity> dbSet = Context.Set<TEntity>();
                TEntity obj = dbSet.Find(entity.Id);
                if (obj != null)
                {
                    Context.Entry(obj).State = EntityState.Detached;
                    return this.Update(entity);
                }
                else
                {
                    return this.Add(entity);
                }
                
            }
            catch (DbUpdateException ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public async Task<TEntity> AddOrUpdateAsync(TEntity entity)
        {
            try
            {
                DbSet<TEntity> dbSet = Context.Set<TEntity>();
                TEntity obj = await dbSet.FindAsync(entity.Id);
                if (obj != null)
                {
                    Context.Entry(obj).State = EntityState.Detached;

                    return await this.UpdateAsync(entity);
                }
                else
                {
                    return await this.AddAsync(entity);
                }

            }
            catch (DbUpdateException ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public TEntity Update(TEntity entity)
        {
            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                this.SaveChanges();
                Context.UnBlockEntity(entity, this.authService.GetCurretUserId());
                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public virtual async Task<TEntity> UpdateAsync(TEntity entity)
        {

            try
            {
                Context.Entry(entity).State = EntityState.Modified;
                await this.SaveChangesAsync();
                Context.UnBlockEntity(entity, this.authService.GetCurretUserId());
                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        public void Delete(TPrimaryKey id)
        {
            try
            {
                var entity= GetById(id);
                Context.Set<TEntity>().Remove(entity);
                this.SaveChanges();
                Context.UnBlockEntity(entity, this.authService.GetCurretUserId());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;

            }

        }

        public virtual async Task DeleteAsync(TPrimaryKey id)
        {
            try
            {
                var entity = await GetByIdAsync(id);
                await DeleteAsync(entity);
                await this.SaveChangesAsync();
                Context.UnBlockEntity(entity, this.authService.GetCurretUserId());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }


        public virtual async Task DeleteAsync(TEntity entity)
        {
            try
            {
                await Task.FromResult(Context.Set<TEntity>().Remove(entity));

                Context.UnBlockEntity(entity, this.authService.GetCurretUserId());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public TEntity GetById(TPrimaryKey id)
        {
            try
            {
                DbSet<TEntity> dbSet = Context.Set<TEntity>();
                TEntity entity = dbSet.Find(id);
                if ((object)entity == null)
                    throw new EntityNotFoundException(typeof(TEntity), (object)id);
                this.Context.ValidateBlockEntity(entity, this.authService.GetCurretUserId());
                this.Context.BlockEntity(entity, this.authService.GetCurretUserId());

                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
        public async virtual Task<TEntity> GetDtoByAndBlockIdAsync(TPrimaryKey id)
        {
            var entity = await this.GetByIdAsync(id);
            this.Context.BlockEntity(entity, this.authService.GetCurretUserId());
            return entity;
        }



        public async virtual Task<TEntity> GetByIdAsync(TPrimaryKey id)
        {
            try
            {
                DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<TEntity> query = this.AddIncludeForGet(dbSet);

                TEntity entity = await query.SingleAsync(GetFilterById(id));

                if ((object)entity == null)
                    throw new EntityNotFoundException(typeof(TEntity), (object)id);

                this.CompleteEntityAfterRead(entity);
                this.Context.ValidateBlockEntity(entity, this.authService.GetCurretUserId());

                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        protected virtual void CompleteEntityAfterRead(TEntity entity)
        {

        }

        public virtual async Task<TEntity> GetByIdAsync<TFilter>(TFilter filter) where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>
        {
            try
            {
                IQueryable<TEntity> query = Context.Set<TEntity>().AsQueryable();

                foreach (var include in filter.GetIncludesForGetById())
                {
                    var includeString = new FixVisitor(include).GetInclude();

                    query = query.Include(includeString);
                }
                TEntity entity = await query.SingleAsync(GetFilterById(filter.Id));

                if ((object)entity == null)
                    throw new EntityNotFoundException(typeof(TEntity), (object)filter.Id);

                this.Context.ValidateBlockEntity(entity, this.authService.GetCurretUserId());
                this.Context.BlockEntity(entity, this.authService.GetCurretUserId());

                return entity;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }



        //protected static Expression<Func<TEntity, bool>> CreateEqualityExpressionForId(TPrimaryKey id)
        //{
        //    var selectorParameter = Expression.Parameter(typeof(TEntity), "Id");

        //    Expression current = selectorParameter;

        //    return (Expression<Func<TEntity, bool>>)(entity => Expression.PropertyOrField(current,  "Id"));
        //}


        protected virtual IQueryable<TEntity> AddIncludeForGet(DbSet<TEntity> dbSet)
        {
            return dbSet.AsQueryable();
        }

        public abstract Expression<Func<TEntity, bool>> GetFilterById(TPrimaryKey id);
        
        public PagedResult<TEntity> GetPagedList<TFilter>(TFilter filter)
            where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>
        {
            try
            {
                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<TEntity> query = Context.Set<TEntity>().Where(filter.GetFilterExpression()).AsQueryable();

                if (!String.IsNullOrEmpty(filter.Sort))
                {
                    query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, filter.Sort);
                }

                var total = query.Count();

                var Page = filter.Page.GetValueOrDefault();
                var PageSize = filter.PageSize.GetValueOrDefault();

                var list = query.Skip((Page - 1) * PageSize).Take(PageSize);

                return new PagedResult<TEntity>(total, list.ToList());

            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
        
        public virtual async Task<PagedResult<TEntity>> GetPagedListAsync<TFilter>(TFilter filter)
            where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new()
        {
            try
            {
                filter = CompleteFilterPageList(filter);

                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<TEntity> query = Context.Set<TEntity>().Where(filter.GetFilterExpression()).AsQueryable();

                var total = await query.CountAsync();

                
                query = this.GetIncludesForPageList(query);

                foreach (var include in filter.GetIncludesForPageList())
                {
                    query = query.Include(include);
                }

                if (!String.IsNullOrEmpty(filter.Sort))
                {
                    query = System.Linq.Dynamic.Core.DynamicQueryableExtensions.OrderBy(query, filter.Sort);
                }

                //var total = query.Count();


                var Page = filter.Page.GetValueOrDefault();
                var PageSize = filter.PageSize.GetValueOrDefault();

                var list = query.Skip((Page - 1) * PageSize).Take(PageSize);

                return new PagedResult<TEntity>(total, await list.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }

        protected virtual IQueryable<TEntity> GetIncludesForPageList(IQueryable<TEntity> query)
        {
            return query;
        }
        
        protected virtual TFilter CompleteFilterPageList<TFilter>(TFilter filter)
            where TFilter : FilterPagedListBase<TEntity, TPrimaryKey>, new()
        {
            if (filter == null)
            {
                filter = new TFilter();
            }

            if (!filter.Page.HasValue || filter.Page == 0)
            {
                filter.Page = 1;
            }

            if (!filter.PageSize.HasValue || filter.PageSize == 0)
            {
                filter.PageSize = 100;
            }

            return filter;
        }

        public virtual PagedResult<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                //DbSet<TEntity> dbSet = Context.Set<TEntity>();
                IQueryable<TEntity> query = Context.Set<TEntity>().Where(predicate).AsQueryable();

                var total = query.Count();

                return new PagedResult<TEntity>(total, query.ToList());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }
        
        public virtual async Task<PagedResult<result>> FindAllAsync<result>(
            Expression<Func<TEntity, bool>> predicate, Expression<Func<TEntity, result>> select,
            List<Expression<Func<TEntity, Object>>> includeExpression = null)
               where result : class, new()
        {
            try
            {
                IQueryable<TEntity> query = Context.Set<TEntity>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }
                return new PagedResult<result>(total, await query.Select(select).ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public virtual async Task<PagedResult<TEntity>> GetAllAsync(Expression<Func<TEntity, bool>> predicate, List<Expression<Func<TEntity, Object>>> includeExpression = null)
        {
            try
            {
                IQueryable<TEntity> query = Context.Set<TEntity>().Where(predicate).AsQueryable();
                var total = await query.CountAsync();

                if (includeExpression != null)
                {
                    foreach (var include in includeExpression)
                    {
                        query = query.Include(include);
                    }

                }

                return new PagedResult<TEntity>(total, await query.ToListAsync());
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }

        }

        public virtual bool ExistExpression(Expression<Func<TEntity, bool>> predicate)
        {
            try
            {
                var any = Context.Set<TEntity>().Any(predicate);
                return any;
            }
            catch (Exception ex)
            {
                HandleException(ex);
                throw;
            }
        }
        
        protected virtual void HandleException(Exception ex)
        {
            if (ex is DbUpdateException)
            {

                if (ex.InnerException != null)
                {
                    var sqlex = ex.InnerException as SqlException;
                    if (sqlex != null)
                    {
                        switch (sqlex.Number)
                        {
                            case 547:
                                {
                                    MachSqlException(ex.InnerException.Message);//FK exception

                                    if (ex.InnerException.Message.Contains("FOREIGN") && ex.InnerException.Message.Contains("INSERT"))
                                    {
                                        throw new ValidationException("Error al insertar la entidad, tiene errores de relacion con otra entidad");
                                    }
                                    if (ex.InnerException.Message.Contains("FOREIGN") && ex.InnerException.Message.Contains("UPDATE"))
                                    {
                                        throw new ValidationException("Error al actualizar la entidad, tiene errores de relacion con otra entidad");
                                    }
                                    else {
                                        throw new ValidationException("No se puede eliminar la Entidad por que está siendo utilizada");
                                    }
                                   

                                }
                            case 2627:
                                {
                                    MachSqlException(ex.InnerException.Message);//UK exception
                                    //throw new ValidationException("No se puede generar la operación, porque existe una restricción de clave unica");
                                    throw new ValidationException("Existen campos repetidos");
                                }
                            case 2601:
                                throw new ValidationException("La entidad ya existe"); //primary key exception

                            default:
                                throw new Exception(ex.InnerException.Message, ex);
                        }
                    }

                    throw new Exception(ex.InnerException.Message, ex);
                }
            }

            throw new Exception(ex.Message, ex);
        }

        protected virtual void MachSqlException(string ExceptionMessage)
        {
            var MachKeySqlException = GetMachKeySqlException();

            var Value = MachKeySqlException.Where(e => ExceptionMessage.Contains(e.Key)).Select(e => e.Value).FirstOrDefault();
            if (Value != null)
            {
                throw new ValidationException(Value);
            }
        }

        protected virtual Dictionary<String, string> GetMachKeySqlException()
        {
            return new Dictionary<string, string>();
        }

        public async Task UnBlockEntity(TPrimaryKey id)
        {
            await Task.Run(()=> this.Context.UnBlockEntityFull(id.ToString(), typeof(TEntity).FullName, this.authService.GetCurretUserId()));
        }

        public async Task ValidateCocurrencySave(TPrimaryKey id, DateTime lockDate)
        {
            if (typeof(TEntity).GetInterface("IConcurrencyEntity") != null )
            {
                await Task.Run(() => this.Context.ValidateCocurrencySave(id.ToString(), typeof(TEntity).FullName, this.authService.GetCurretUserId(), lockDate));
            }
            
        }
    }



    public class FixVisitor : ExpressionVisitor
    {
        public FixVisitor(Expression node)
            :base()
        {
            this.Visit(node);
        }

        bool IsMemeberAccessOfAConstant(Expression exp)
        {
            if (exp.NodeType == ExpressionType.MemberAccess)
            {
                var memberAccess = (MemberExpression)exp;
                if (memberAccess.Expression.NodeType == ExpressionType.Constant)
                    return true;
                return IsMemeberAccessOfAConstant(memberAccess.Expression);
            }

            return false;
        }

        public List<string> propertyName { get; set; }


        public string GetInclude() {

            return String.Join(".", propertyName);
        }

        protected override Expression VisitMember(MemberExpression node)
        {

            if (propertyName == null)
            {
                propertyName = new List<string>();
            }

            propertyName.Add(node.Member.Name);

            if (IsMemeberAccessOfAConstant(node) && node.Type == typeof(string))
            {
                var item = Expression.Lambda<Func<string>>(node);
                var value = item.Compile()();
                return Expression.Constant(value, typeof(string));
            }

            return base.VisitMember(node);
        }
    }
}
