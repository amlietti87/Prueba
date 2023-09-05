using System;
using System.Threading.Tasks;

namespace TECSO.FWK.Domain.Event
{
  public interface IEventData
  {
    DateTime EventTime { get; set; }

    object EventSource { get; set; }
  }


    public interface IEventHandlerFactory
    {
        IEventHandler GetHandler();

        void ReleaseHandler(IEventHandler handler);
    }

    public interface IEventHandler
    {
    }

    public interface IEventHandler<in TEventData> : IEventHandler
    {
        void HandleEvent(TEventData eventData);
    }

    internal sealed class NullDisposable : IDisposable
    {
        public static NullDisposable Instance { get; } = new NullDisposable();

        private NullDisposable()
        {
        }

        public void Dispose()
        {
        }
    }
    public sealed class NullEventBus : IEventBus
    {
        private static readonly NullEventBus SingletonInstance = new NullEventBus();

        public static NullEventBus Instance
        {
            get
            {
                return NullEventBus.SingletonInstance;
            }
        }

        private NullEventBus()
        {
        }

        public IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
            return (IDisposable)NullDisposable.Instance;
        }

        public IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
            return (IDisposable)NullDisposable.Instance;
        }

        public IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler<TEventData>, new()
        {
            return (IDisposable)NullDisposable.Instance;
        }

        public IDisposable Register(Type eventType, IEventHandler handler)
        {
            return (IDisposable)NullDisposable.Instance;
        }

        public IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData
        {
            return (IDisposable)NullDisposable.Instance;
        }

        public IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory)
        {
            return (IDisposable)NullDisposable.Instance;
        }

        public void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData
        {
        }

        public void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData
        {
        }

        public void Unregister(Type eventType, IEventHandler handler)
        {
        }

        public void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData
        {
        }

        public void Unregister(Type eventType, IEventHandlerFactory factory)
        {
        }

        public void UnregisterAll<TEventData>() where TEventData : IEventData
        {
        }

        public void UnregisterAll(Type eventType)
        {
        }

        public void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData
        {
        }

        public void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
        }

        public void Trigger(Type eventType, IEventData eventData)
        {
        }

        public void Trigger(Type eventType, object eventSource, IEventData eventData)
        {
        }

        public Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData
        {
            return new Task((Action)(() => { }));
        }

        public Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData
        {
            return new Task((Action)(() => { }));
        }

        public Task TriggerAsync(Type eventType, IEventData eventData)
        {
            return new Task((Action)(() => { }));
        }

        public Task TriggerAsync(Type eventType, object eventSource, IEventData eventData)
        {
            return new Task((Action)(() => { }));
        }
    }

    public interface IEventBus
    {
        IDisposable Register<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        IDisposable Register<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        IDisposable Register<TEventData, THandler>() where TEventData : IEventData where THandler : IEventHandler<TEventData>, new();

        IDisposable Register(Type eventType, IEventHandler handler);

        IDisposable Register<TEventData>(IEventHandlerFactory handlerFactory) where TEventData : IEventData;

        IDisposable Register(Type eventType, IEventHandlerFactory handlerFactory);

        void Unregister<TEventData>(Action<TEventData> action) where TEventData : IEventData;

        void Unregister<TEventData>(IEventHandler<TEventData> handler) where TEventData : IEventData;

        void Unregister(Type eventType, IEventHandler handler);

        void Unregister<TEventData>(IEventHandlerFactory factory) where TEventData : IEventData;

        void Unregister(Type eventType, IEventHandlerFactory factory);

        void UnregisterAll<TEventData>() where TEventData : IEventData;

        void UnregisterAll(Type eventType);

        void Trigger<TEventData>(TEventData eventData) where TEventData : IEventData;

        void Trigger<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

        void Trigger(Type eventType, IEventData eventData);

        void Trigger(Type eventType, object eventSource, IEventData eventData);

        Task TriggerAsync<TEventData>(TEventData eventData) where TEventData : IEventData;

        Task TriggerAsync<TEventData>(object eventSource, TEventData eventData) where TEventData : IEventData;

        Task TriggerAsync(Type eventType, IEventData eventData);

        Task TriggerAsync(Type eventType, object eventSource, IEventData eventData);
    }
}
