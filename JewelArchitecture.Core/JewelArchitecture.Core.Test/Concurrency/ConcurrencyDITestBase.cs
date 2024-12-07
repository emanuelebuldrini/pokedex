﻿namespace JewelArchitecture.Core.Test.Concurrency
{
    public abstract class ConcurrencyDITestBase : DiTestBase
    {
        protected int SlowerWriteMsDelay = 300;

        private readonly ConcurrencySynchronizer _synchronizer;

        protected ConcurrencySynchronizer Synchronizer
        {
            get => _synchronizer;
        }

        protected ConcurrencyDITestBase()
        {
            _synchronizer = new ConcurrencySynchronizer();
        }

        protected async Task SimulateConcurrency()
        {
            // Wait a moment to make sure that all threads reached the critical section.
            await ShortWaitAsync();

            // Release all the waiting threads in once.
            _synchronizer?.ReleaseAll();
        }

        /// <summary>
        /// Await a short delay to simulate concurrency between requests
        /// </summary>
        /// <returns></returns>
        protected async Task ShortWaitAsync(int delay = 150) => await Task.Delay(delay);

        public override void Dispose()
        {
            base.Dispose();

            _synchronizer.Dispose();
        }
    }
}
