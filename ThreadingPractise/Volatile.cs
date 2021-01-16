using System;
using System.Threading;

namespace ThreadingPractise
{
    public class VolatileUsage : Example
    {
        public volatile string validDeclarion;
        struct AnotherValidDeclaration
        {
            public volatile byte validVolatileByte;
        }

        /*
         * Local variables cannot be declared volatile!
         * 
         */
        public VolatileUsage()
        {
            //volatile int error; will throw error if uncomment
        }

        public void ExecuteExample()
        {
            Console.WriteLine("Executing volatile example!");

            /*
             * Unsafe Example
             */

            // Create the worker thread object. This does not start the thread.
            UnsafeWorker unsafeWorker = new UnsafeWorker();
            Thread unsafeWorkerThread = new Thread( unsafeWorker.DoWork);

            // Start the worker thread.
            unsafeWorkerThread.Start();
            Console.WriteLine("Main Thread: unsafe worker thread started...");

            // Loop until the worker thread activates.
            while ( unsafeWorkerThread.IsAlive)
            {
                // Put the main thread to sleep for a second.
                Thread.Sleep(1000);

                unsafeWorker.RequestStop();

                // Use the Thread.Join method to block the current thread
                // until the object's thread terminates.
                unsafeWorkerThread.Join();
                Console.WriteLine("Main Thread: unsafe worker thread is closed...");
            }

            Console.WriteLine("\n");
            /*
             * Safe Example
             */

            SafeWorker safeWorker = new SafeWorker();
            Thread safeWorkerThread = new Thread( safeWorker.DoWork);

            // Start the worker thread.
            safeWorkerThread.Start();
            Console.WriteLine("Main Thread: safe worker thread started...");

            // Loop until the worker thread activates.
            while (safeWorkerThread.IsAlive)
            {
                // Put the main thread to sleep for a second.
                Thread.Sleep(1000);

                safeWorker.RequestStop();

                // Use the Thread.Join method to block the current thread
                // until the object's thread terminates.
                safeWorkerThread.Join();
                Console.WriteLine("Main Thread: safe worker thread is closed...");
            }

            // Compare the amount of executions after stop request!
        }

        class VolatileUsageSimpleExample
        {
            public volatile int sharedData;

            public void Test(int i)
            {
                sharedData = i;
            }
        }

        class UnsafeWorker
        {
            private bool shouldStop = false;
            int executionCountAfterRequest = 0;

            // This method is called when the thread is started.
            public void DoWork()
            {
                bool work = false;
                while ( !shouldStop)
                {
                    // simulating some work done.
                    work = !work;
                    executionCountAfterRequest++;
                }

                Console.WriteLine( $"Unsafe Worker terminating gracefully... Loop count after request stop: {executionCountAfterRequest}");
            }

            public void RequestStop()
            {
                executionCountAfterRequest = 0;
                shouldStop = true;
            }
        }

        class SafeWorker
        {
            // Keyword volatile is used as a hint to the compiler that this data
            // member is accessed by multiple threads.
            private volatile bool shouldStop = false;
            private int executionCountAfterRequest = 0;

            // This method is called when the thread is started.
            public void DoWork()
            {
                bool work = false;
                while (!shouldStop)
                {
                    // simulating some work done.
                    work = !work;
                    executionCountAfterRequest++;
                }

                Console.WriteLine($"Safe Worker terminating gracefully... Loop count after request stop: {executionCountAfterRequest}");
            }

            public void RequestStop()
            {
                executionCountAfterRequest = 0;
                shouldStop = true;
            }
        }
    }
}
