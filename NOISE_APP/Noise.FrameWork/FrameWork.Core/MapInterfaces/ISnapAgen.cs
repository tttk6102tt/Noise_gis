namespace FrameWork.Core.MapInterfaces
{
    using System;

    public class ISnapAgen : IDisposable
    {
        public void Dispose()
        {
            this.Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
            }
        }

        ~ISnapAgen()
        {
            this.Dispose(false);
        }
    }
}

