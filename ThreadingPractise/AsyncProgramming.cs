using System;
using ThreadingPractise.Breakfast;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ThreadingPractise
{
    public class AsyncProgramming : Example
    {
        public void ExecuteExample()
        {
            Console.WriteLine("Executing async programming example!");

            // Syncronized on main thread
            CookBreakFastSyncronized();

            Console.WriteLine("\n");
            // Asyncronizedly cook breakfast but in syncronized
            CookBadAsyncBreakfast().Wait();

            Console.WriteLine("\n");
            // Asyncronizedly cook breakfast while burning eggs and bacon for waiting toast to finish
            CookImprovedBreakfast().Wait();

            Console.WriteLine("\n");
            // Asyncronizedly cook breakfast
            CookMasterBreakfast().Wait();
        }

        static void CookBreakFastSyncronized()
        {
            DateTime time = DateTime.Now;

            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Egg eggs = FryEggs(2);
            Console.WriteLine("eggs are ready");

            Bacon bacon = FryBacon(3);
            Console.WriteLine("bacon is ready");

            Toast toast = ToastBread(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");

            Console.WriteLine($"It took {DateTime.Now - time} seconds");
        }

        static async Task CookBadAsyncBreakfast()
        {
            DateTime time = DateTime.Now;

            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Egg eggs = await FryEggsAsync(2);
            Console.WriteLine("eggs are ready");

            Bacon bacon = await FryBaconAsync(3);
            Console.WriteLine("bacon is ready");

            Toast toast = await ToastBreadAsync(2);
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");

            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");
            Console.WriteLine("Breakfast is ready!");

            Console.WriteLine($"It took {DateTime.Now - time} seconds");
        }

        static async Task CookImprovedBreakfast()
        {
            DateTime time = DateTime.Now;

            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Task<Egg> eggsTask = FryEggsAsync( 2);
            Task<Bacon> baconTask = FryBaconAsync(3);
            Task<Toast> toastTask = ToastBreadAsync(2);

            Toast toast = await toastTask;
            ApplyButter(toast);
            ApplyJam(toast);
            Console.WriteLine("toast is ready");
            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");

            Egg eggs = await eggsTask;
            Console.WriteLine("eggs are ready");
            Bacon bacon = await baconTask;
            Console.WriteLine("bacon is ready");

            Console.WriteLine("Breakfast is ready!");

            Console.WriteLine($"It took {DateTime.Now - time} seconds");
        }

        static async Task CookMasterBreakfast()
        {
            DateTime time = DateTime.Now;

            Coffee cup = PourCoffee();
            Console.WriteLine("coffee is ready");

            Task<Egg> eggsTask = FryEggsAsync(2);
            Task<Bacon> baconTask = FryBaconAsync(3);
            Task<Toast> toastTask = ToastBreadAsync(2);

            List<Task> tasks = new List<Task> { eggsTask, baconTask, toastTask };
            while ( tasks.Count > 0)
            {
                Task finishedTask = await Task.WhenAny(tasks);
                if ( finishedTask == eggsTask)
                {
                    Console.WriteLine("eggs are ready");
                }
                else if ( finishedTask == baconTask)
                {
                    Console.WriteLine("bacon is ready");
                }
                else if ( finishedTask == toastTask)
                {
                    Console.WriteLine("toast is ready");
                }
                tasks.Remove( finishedTask);
            }
            Juice oj = PourOJ();
            Console.WriteLine("oj is ready");

            Console.WriteLine("Breakfast is ready!");

            Console.WriteLine($"It took {DateTime.Now - time} seconds");
        }

        #region Syncronized breakfast cooking!
        static Juice PourOJ()
        {
            Console.WriteLine("Pouring orange juice");
            return new Juice();
        }

        static void ApplyJam(Toast toast) => Console.WriteLine("Applying jam to toast.");
        static void ApplyButter(Toast toast) => Console.WriteLine("Applying butter to toast.");

        static Bacon FryBacon( int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            Task.Delay(3000).Wait();
            for ( int i = 0; i < slices; i++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        static Toast ToastBread( int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start the toaster");
            Task.Delay(3000).Wait();
            Console.WriteLine("Put toast on plate");

            return new Toast();
        }

        static Egg FryEggs( int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            Task.Delay(3000).Wait();
            Console.WriteLine($"Cracking {howMany} eggs...");
            Console.WriteLine("cooking the eggs ...");
            Task.Delay(3000).Wait();
            Console.WriteLine("Putting the egss on plate...");

            return new Egg();
        }

        static Coffee PourCoffee()
        {
            Console.WriteLine("Pouring coffee");
            return new Coffee();
        }
        #endregion

        #region AsyncBreakfast  
        static async Task<Toast> MakeBreadWithJamAndButter( int slices)
        {
            Toast toast = await ToastBreadAsync(slices);
            ApplyJam(toast);
            ApplyButter(toast);
            return toast;
        }

        static async Task<Bacon> FryBaconAsync(int slices)
        {
            Console.WriteLine($"putting {slices} slices of bacon in the pan");
            Console.WriteLine("cooking first side of bacon...");
            await Task.Delay(3000);
            for (int i = 0; i < slices; i++)
            {
                Console.WriteLine("flipping a slice of bacon");
            }
            Console.WriteLine("cooking the second side of bacon...");
            await Task.Delay(3000);
            Console.WriteLine("Put bacon on plate");

            return new Bacon();
        }

        static async Task<Toast> ToastBreadAsync(int slices)
        {
            for (int slice = 0; slice < slices; slice++)
            {
                Console.WriteLine("Putting a slice of bread in the toaster");
            }
            Console.WriteLine("Start the toaster");
            await Task.Delay(3000);
            Console.WriteLine("Put toast on plate");

            return new Toast();
        }

        static async Task<Egg> FryEggsAsync(int howMany)
        {
            Console.WriteLine("Warming the egg pan...");
            await Task.Delay(3000);
            Console.WriteLine($"Cracking {howMany} eggs...");
            Console.WriteLine("cooking the eggs ...");
            await Task.Delay(3000);
            Console.WriteLine("Putting the egss on plate...");

            return new Egg();
        }
        #endregion
    }
}
