using System;

namespace PKS.Base
{
    /// <summary>释放资源对象</summary>
    [Serializable]
    public abstract class DisposableObject : IDisposable
    {
        #region 实现IDisposable接口
        /// <summary>是否已释放</summary>
        private bool disposed = false;
        /// <summary>释放资源</summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        /// <summary>释放资源</summary>
        private void Dispose(bool disposing)
        {
            if (!disposed)
            {
                DisposeInternal(disposing);
                disposed = true;
            }
        }
        /// <summary>释放资源内部方法</summary>
        protected abstract void DisposeInternal(bool disposing);
        #endregion
    }
}
