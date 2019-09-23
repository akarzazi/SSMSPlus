namespace SSMSPlusCore.Utils
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;

    public sealed class AsyncLock
    {
        private readonly SemaphoreSlim _semaphore = new SemaphoreSlim(1, 1);
        private readonly Task<IDisposable> _releaser;

        private AsyncLocal<StatusWrapper> _owningStatus = new AsyncLocal<StatusWrapper>();

        public AsyncLock()
        {
            _releaser = Task.FromResult((IDisposable)new Releaser(this));
        }

        public Task<IDisposable> LockAsync()
        {
            lock (_semaphore)
            {
                if (_owningStatus.Value?.IsOwner == true)
                {
                    System.Diagnostics.Debug.WriteLine("owner");
                    return Task.FromResult((IDisposable)new Noop());
                }

                var wait = _semaphore.WaitAsync();
                if (wait.IsCompleted)
                {
                    _owningStatus.Value = new StatusWrapper(true);
                    return _releaser;
                }
                else
                {
                    var statusInst = new StatusWrapper(false);
                    _owningStatus.Value = statusInst;
                    System.Diagnostics.Debug.WriteLine("waiting");
                    return wait.ContinueWith(
                                    (_, state) =>
                                    {
                                        var status = (StatusWrapper)state;
                                        status.IsOwner = true;
                                        return _releaser.Result;
                                    },
                                    statusInst,
                                    CancellationToken.None,
                                    TaskContinuationOptions.ExecuteSynchronously,
                                    TaskScheduler.Default);
                }
            }
        }

        private void Release()
        {
            lock (_semaphore)
            {
                _owningStatus.Value = null;
                _semaphore.Release();
            }
        }

        private sealed class Releaser : IDisposable
        {
            private readonly AsyncLock _toRelease;
            internal Releaser(AsyncLock toRelease) { _toRelease = toRelease; }
            public void Dispose()
            {
                _toRelease.Release();
            }
        }

        private sealed class Noop : IDisposable
        {
            public void Dispose() { }
        }

        private sealed class StatusWrapper
        {
            internal bool IsOwner { get; set; }
            internal StatusWrapper(bool isOwner)
            {
                IsOwner = isOwner;
            }
        }
    }
}