using System;

namespace NetDevPL
{
    // cr:mmisztal1980
    public abstract class Disposable : IDisposable
    {
        protected bool IsDisposed;

        protected Disposable()
        {
        }

        ~Disposable()
        {
            Dispose(false);
        }

        public void Dispose()
        {
            if (IsDisposed) return;

            try
            {
                Dispose(true);
            }
            catch
            {
                // Dispose() must not throw
            }
            finally
            {
                GC.SuppressFinalize(this);
                IsDisposed = true;
            }
        }

        protected virtual void Dispose(bool disposing)
        {
        }
    }
}