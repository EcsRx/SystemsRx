using System;
using System.Collections.Generic;
using SystemsRx.Extensions;
using SystemsRx.MicroRx.Extensions;
using SystemsRx.MicroRx.Subjects;

namespace SystemsRx.Computeds.Data
{
   public abstract class ComputedFromObservable<TOutput, TInput> : IComputed<TOutput>, IDisposable
    {
        public TOutput CachedData;
        protected readonly List<IDisposable> Subscriptions;
        
        private readonly Subject<TOutput> _onDataChanged;
        private readonly object _lock = new object();
        
        public IObservable<TInput> DataSource { get; }

        public ComputedFromObservable(IObservable<TInput> dataSource, TOutput initialValue = default(TOutput))
        {
            DataSource = dataSource;
            CachedData = initialValue;
            Subscriptions = new List<IDisposable>();
            _onDataChanged = new Subject<TOutput>();

            MonitorChanges();
        }
                
        public IDisposable Subscribe(IObserver<TOutput> observer)
        { return _onDataChanged.Subscribe(observer); }

        public TOutput Value => GetData();

        public void MonitorChanges()
        { DataSource.Subscribe(RefreshData).AddTo(Subscriptions); }


        public void RefreshData(TInput data)
        {
            lock(_lock)
            { CachedData = Transform(data); }
         
            _onDataChanged.OnNext(CachedData);
        }

        /// <summary>
        /// The method to generate given data from the data source
        /// </summary>
        /// <param name="dataSource">The source of data to work off</param>
        /// <returns>The transformed data</returns>
        public abstract TOutput Transform(TInput dataSource);

        public TOutput GetData()
        { return CachedData; }

        public virtual void Dispose()
        {
            Subscriptions.DisposeAll();
            _onDataChanged.Dispose();
        }
    }
}