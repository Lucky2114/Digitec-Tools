using Shopping_Tools.Source.Tasks;
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
            //This Daemon just runs the tasks from the class library

            List<ITask> tasks = new List<ITask>()
            {
                //Update every 10 minutes.
                new ChangesToPriceTask(10)
            };

            foreach (ITask task in tasks)
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