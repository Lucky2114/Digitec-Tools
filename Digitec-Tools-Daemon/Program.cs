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
                new ChangesToPriceTask(7000)
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
