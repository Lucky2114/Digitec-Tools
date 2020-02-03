using Shopping_Tools_Daemon.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Shopping_Tools_Daemon
{
    public class Program
    {
        static void Main(string[] args)
        {
            /// This Daemon runs the tasks, implementing the ITask interface.
            /// The tasks are running as background System.Threading.Tasks.

            List<ITask> tasks = new List<ITask>()
            {
                //Update every 10 minutes.
                new ChangesToPriceTask(10)
            };

            foreach (var task in tasks)
            {
                Console.WriteLine("Starting Task..");
                task.StartTask();
            }


            while (true)
            {
                Console.WriteLine("Daemon Loop Logger: ");
                Console.WriteLine($"Currently {tasks.Count} Tasks are running.");
                Thread.Sleep(5000);
            }
        }
    }
}