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
            //This Daemon just runns the tasks from the class library

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

            Console.ReadLine();
        }
    }
}
