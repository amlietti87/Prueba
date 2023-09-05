using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace TECSO.FWK.Domain.UOW
{

    [Serializable]
    public class TecsoConcurrencyException : Exception
    {
        public TecsoConcurrencyException()
        {
        }

        public TecsoConcurrencyException(SerializationInfo serializationInfo, StreamingContext context)
          : base(serializationInfo, context)
        {
        }

        public TecsoConcurrencyException(string message)
          : base(message)
        {
        }

        public TecsoConcurrencyException(string message, Exception innerException)
          : base(message, innerException)
        {
        }
    }

    public interface IUnitOfWorkManager
    {
        IActiveUnitOfWork Current { get; }

        IUnitOfWorkCompleteHandle Begin();

        IUnitOfWorkCompleteHandle Begin(TransactionScopeOption scope);

        IUnitOfWorkCompleteHandle Begin(UnitOfWorkOptions options);
    }

    public interface IUnitOfWorkCompleteHandle : IDisposable
    {
        void Complete();

        Task CompleteAsync();
    }

    public interface IActiveUnitOfWork
    {
        event EventHandler Completed;

        

        //event EventHandler<UnitOfWorkFailedEventArgs> Failed;

        //event EventHandler Disposed;

        //UnitOfWorkOptions Options { get; }

        //IReadOnlyList<DataFilterConfiguration> Filters { get; }

        //bool IsDisposed { get; }

        void SaveChanges();

        Task SaveChangesAsync();

        //IDisposable DisableFilter(params string[] filterNames);

        //IDisposable EnableFilter(params string[] filterNames);

        //bool IsFilterEnabled(string filterName);

        //IDisposable SetFilterParameter(string filterName, string parameterName, object value);

        //IDisposable SetTenantId(int? tenantId);

        //IDisposable SetTenantId(int? tenantId, bool switchMustHaveTenantEnableDisable);

        //int? GetTenantId();
    }

    public class UnitOfWorkOptions
    {
        public TransactionScopeOption? Scope { get; set; }

        public bool? IsTransactional { get; set; }

        public TimeSpan? Timeout { get; set; }

        public System.Transactions.IsolationLevel? IsolationLevel { get; set; }

        public TransactionScopeAsyncFlowOption? AsyncFlowOption { get; set; }

        //public List<DataFilterConfiguration> FilterOverrides { get; }

        public UnitOfWorkOptions()
        {
            //this.FilterOverrides = new List<DataFilterConfiguration>();
        }

        //internal void FillDefaultsForNonProvidedOptions(IUnitOfWorkDefaultOptions defaultOptions)
        //{
        //    if (!this.IsTransactional.HasValue)
        //        this.IsTransactional = new bool?(defaultOptions.IsTransactional);
        //    if (!this.Scope.HasValue)
        //        this.Scope = new TransactionScopeOption?(defaultOptions.Scope);
        //    if (!this.Timeout.HasValue && defaultOptions.Timeout.HasValue)
        //        this.Timeout = new TimeSpan?(defaultOptions.Timeout.Value);
        //    if (this.IsolationLevel.HasValue || !defaultOptions.IsolationLevel.HasValue)
        //        return;
        //    this.IsolationLevel = new System.Transactions.IsolationLevel?(defaultOptions.IsolationLevel.Value);
        //}

        //internal void FillOuterUowFiltersForNonProvidedOptions(List<DataFilterConfiguration> filterOverrides)
        //{
        //    foreach (DataFilterConfiguration filterOverride1 in filterOverrides)
        //    {
        //        DataFilterConfiguration filterOverride = filterOverride1;
        //        if (!this.FilterOverrides.Any<DataFilterConfiguration>((Func<DataFilterConfiguration, bool>)(fo => fo.FilterName == filterOverride.FilterName)))
        //            this.FilterOverrides.Add(filterOverride);
        //    }
        //}
    }
}
