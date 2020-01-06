using Digitec_Tools.Source.Tasks;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Digitec_Tools_Daemon
{
    public class Program
    {
        static void Main(string[] args)
        {
            //This Daemon has two tasks:
            //1. Update the database with all registered products at any given intervall
            //2. Notify the registered Users whenever there is a change to the registered products.

            List<ITask> tasks = new List<ITask>()
            {
                new ChangesToPriceTask(7000)
            };

            foreach (ITask task in tasks)
            {
                task.StartTask();
            }

            Console.ReadLine();
        }
    }
}
