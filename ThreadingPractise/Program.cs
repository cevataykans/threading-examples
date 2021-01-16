using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ThreadingPractise
{
    public enum ExampleType
    {
        AsyncProgramming,
        Threading,
        Volatile,
        Lock
    }

    class MainClass
    {
        static Dictionary<int, Action> example = new Dictionary<int, Action>
        {
            { (int)ExampleType.AsyncProgramming, new AsyncProgramming().ExecuteExample },
            { (int)ExampleType.Threading, new Threading().ExecuteExample },
            { (int)ExampleType.Volatile, new VolatileUsage().ExecuteExample },
            { (int)ExampleType.Lock, new Lock().ExecuteExample }
        };

        public static async Task Main(string[] args)
        {
            // Change this to execute an example!
            ExampleType typeToExecute = ExampleType.AsyncProgramming;

            example[ (int)typeToExecute]();
        }
    }
}
