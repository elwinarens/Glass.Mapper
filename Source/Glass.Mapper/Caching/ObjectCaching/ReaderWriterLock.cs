using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace Glass.Mapper.Caching.ObjectCaching
{
    public class ReaderWriterLock : IDisposable
    {
        readonly ReaderWriterLockSlim readerWriterLock;

        public bool HasTakenLock { get; private set; }
        public ReaderWriterLockType ReaderWriterLockType { get; private set; }

        public ReaderWriterLock(ReaderWriterLockSlim readerWriterLock, ReaderWriterLockType readerWriterLockType, int timeout)
        {
            try
            {
                this.readerWriterLock = readerWriterLock;
                this.ReaderWriterLockType = readerWriterLockType;

                if (readerWriterLockType == ReaderWriterLockType.Reader)
                {
                    HasTakenLock = readerWriterLock.TryEnterReadLock(timeout);
                }
                else if (readerWriterLockType == ReaderWriterLockType.Writer)
                {
                    HasTakenLock = readerWriterLock.TryEnterWriteLock(timeout);
                }
                else
                {
                    HasTakenLock = false;
                }

            }
            catch
            {
                HasTakenLock = false;
            }
        }

        public void Dispose()
        {
            if (readerWriterLock.IsReadLockHeld)
            {
                readerWriterLock.ExitReadLock();
            }

            if (readerWriterLock.IsWriteLockHeld)
            {
                readerWriterLock.ExitWriteLock();
            }
        }
    }
    public enum ReaderWriterLockType
    {
        Reader,
        Writer
    }
}
