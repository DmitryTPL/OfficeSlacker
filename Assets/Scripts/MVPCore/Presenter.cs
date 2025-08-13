using System;
using System.Threading;
using Zenject;

namespace MVP
{
    public abstract class Presenter : IPresenter
    {
        private BasePresenterViewSharedData _sharedData;
        private readonly CancellationTokenSource _destroyCancellationTokenSource = new();

        public event Action InitializationCompleted;

        public Guid Guid { get; }

        protected CancellationToken DestroyCancellationToken => _destroyCancellationTokenSource.Token;
        protected bool IsEnable { get; private set; }

        protected Presenter()
        {
            Guid = Guid.NewGuid();
        }

        void IInitializable.Initialize()
        {
            InitializeData();
            InitializationCompleted?.Invoke();
        }

        public virtual void OnEnable()
        {
            IsEnable = true;
        }

        public virtual void OnDisable()
        {
            IsEnable = false;
        }

        public virtual void OnValidate()
        {
        }

        protected virtual void InitializeData()
        {
        }
        
        public void CancelDestroyCancellationToken()
        {
            _destroyCancellationTokenSource.Cancel();
            _destroyCancellationTokenSource.Dispose();
        }

        protected TData GetSharedData<TData>()
            where TData : BasePresenterViewSharedData
        {
            return (TData)_sharedData;
        }

        public void SetSharedData<TData>(TData data)
            where TData : BasePresenterViewSharedData
        {
            _sharedData = data;
        }

        public override bool Equals(object obj)
        {
            return obj is Presenter presenter && Equals(presenter);
        }

        protected bool Equals(Presenter other)
        {
            return Equals(other.Guid);
        }

        protected bool Equals(Guid guid)
        {
            return Guid.Equals(guid);
        }

        public override int GetHashCode()
        {
            return Guid.GetHashCode();
        }

        public void Dispose()
        {
            CancelDestroyCancellationToken();
            Disposed();
        }

        protected virtual void Disposed()
        {
        }
    }
}