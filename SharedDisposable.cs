using System;

namespace NvTimingsEd
{
    public class SharedDisposable<T> : IDisposable where T : class, IDisposable
    {
        private class Target
        {
            public T value;
            public uint count;
        }

        // Lambda constructor, because reflection is too strict.
        // This will also help with classes that use factories.
        public SharedDisposable(Func<T> create)
        {
            target = new Target()
            {
                value = create(),
                count = 1,
            };
        }

        public SharedDisposable(SharedDisposable<T> share)
        {
            lock (share.target)
            {
                // Check that Dispose() did not get a lock and dispose the
                //   object before we are able to obtain a reference to it.
                if (share.target.count == 0)
                    throw new ObjectDisposedException(nameof(share), "Target object has been disposed before being shared.");
                target = share.target;
                target.count++;
            }
        }

        // To be used as a temporary lock, such as in `using()`.
        // Can also be used as a synonym for copy c'tor.
        public SharedDisposable<T> Share()
        {
            return new SharedDisposable<T>(this);
        }

        public void Dispose()
        {
            lock (target)
            {
                if (--target.count == 0)
                {
                    //We don't care if Dispose takes a while as we're the only lock
                    //  holder of `target` so the lock isn't blocking any other threads
                    //  Unless there's some time related overhead of `lock()` ?
                    Value.Dispose();
                }
            }
        }

        private Target target = null;
        public T Value { get => target.value; }
    }
}
