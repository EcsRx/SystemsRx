using System;

namespace SystemsRx.MicroRx.Disposables
{
    public class SingleAssignmentDisposable : IDisposable, ICancelable
    {
        private readonly object _gate = new object();
        private IDisposable _current;
        private bool _disposed;

        public bool IsDisposed
        {
            get { lock (_gate) { return _disposed; } }
        }

        public IDisposable Disposable
        {
            get => _current;
            set
            {
                var old = default(IDisposable);
                bool alreadyDisposed;
                lock (_gate)
                {
                    alreadyDisposed = _disposed;
                    old = _current;
                    if (!alreadyDisposed)
                    {
                        if (value == null) { return; }
                        _current = value;
                    }
                }

                if (alreadyDisposed && value != null)
                {
                    value.Dispose();
                    return;
                }

                if (old != null) 
                { throw new InvalidOperationException("Disposable is already set"); }
            }
        }

        public void Dispose()
        {
            IDisposable old = null;

            lock (_gate)
            {
                if (!_disposed)
                {
                    _disposed = true;
                    old = _current;
                    _current = null;
                }
            }
            old?.Dispose();
        }
    }
}