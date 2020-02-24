using Shopping_Tools.Source;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Shopping_Tools_Daemon.Tasks
{
    public class ChangesToPriceTask : ITask
    {
        private bool shouldAbort;
        public Task Task { get; set; }

        private readonly double _interval;

        public ChangesToPriceTask(double intervalInMinutes)
        {
            _interval = intervalInMinutes;
        }

        public void StartTask()
        {
            Console.WriteLine("Creating new task");

            Task = new Task(Worker);
            Console.WriteLine("Starting new task");
            Task.Start();
        }

        public void RestartTask()
        {
            Console.WriteLine("Restarting Task..");
            Task.Dispose();
            StartTask();
        }

        private void Worker()
        {
            Console.WriteLine("Task started");
            var storage = new Storage();
            while (!shouldAbort)
            {
                Random rand = new Random();
                var randSleep = rand.Next(25000, 60000);
                var timer = new TimerPlus()
                {
                    AutoReset = false,
                    Interval = TimeSpan.FromMinutes(_interval).TotalMilliseconds + randSleep
                };
                timer.Start();

                var result = storage.GetAllProducts().Result;
                Console.WriteLine($"Received {result.Count} products.");

                List<Task> pendingTasks = new List<Task>();

                foreach (var item in result)
                {
                    Task task = new Task(delegate
                    {
                        //TODO If no users => continue;
                        var productFromDB = Helpers.DictionaryToProduct(item);
                        var productCurrent = storage.UpdateProduct(productFromDB).Result;

                        double priceDB = productFromDB.PriceCurrent.ParseToDouble();
                        double priceCurrent = productCurrent.PriceCurrent.ParseToDouble();

                        if (priceCurrent != priceDB)
                        {
                            var message =
                                $"{productCurrent.Brand} {productCurrent.Name} now costs {productCurrent.PriceCurrent} {productCurrent.Currency} instead of {productFromDB.PriceCurrent} {productFromDB.Currency}! \n" +
                                "\n" +
                                $"Here's the link: {productCurrent.Url}" +
                                $"\n\n" +
                                $"Edit Account Settings: https://www.shoppingtools.online/Identity/Account/Manage";

                            Console.WriteLine(message);
                            Console.WriteLine("Notifying Users...");
                            var notifiedUsers = UserNotifier.NotifyUsersForProduct(productFromDB.ProductIdSimple, message).Result;
                            Console.WriteLine($"Notified {notifiedUsers} users.");
                        }
                        else
                        {
                            Console.WriteLine("Nothing Happened..");
                        }
                    });
                    pendingTasks.Add(task);
                    task.Start();
                }

                Console.WriteLine($"Waiting for {pendingTasks.Count} requests to finish..");
                foreach (var item in pendingTasks)
                {
                    item.Wait();
                }
                Console.WriteLine("All requests finished!");

                if (timer.TimeLeft > 0)
                {
                    //sleep the remaining time of the interval
                    Console.WriteLine($"Sleeping for {TimeSpan.FromMilliseconds(timer.TimeLeft).TotalSeconds} seconds.");
                    Thread.Sleep(Convert.ToInt32(timer.TimeLeft));
                }
                else
                {
                    Console.WriteLine($"Updating the database took to long! Now {TimeSpan.FromMilliseconds(timer.TimeLeft).TotalSeconds} seconds behind!");
                }

                //Send Log AFTER Sleep. If the products only take one sec to update, logs will be spammed every second.
                if (Convert.ToInt32(DateTime.UtcNow.TimeOfDay.TotalHours) == 14)
                {
                    EmailSender.Send("kevin.mueller1@outlook.com", $"Latest updating routine took: {TimeSpan.FromMilliseconds(timer.Interval).TotalMinutes - TimeSpan.FromMilliseconds(timer.TimeLeft).TotalMinutes} minutes", "Daily Updating Routine Log").RunSynchronously();
                }
            }

            Task = null;
        }

        public void Abort()
        {
            shouldAbort = true;
        }
    }
}