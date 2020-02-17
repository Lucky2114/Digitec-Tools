using Shopping_Tools.Source;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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

        private async void Worker()
        {
            Console.WriteLine("Task started");
            var storage = new Storage();
            List<Dictionary<string, object>> lastResult = null;
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

                var result = await storage.GetAllProducts();
                Console.WriteLine($"Received {result.Count} products.");

                if (lastResult != null)
                {
                    foreach (var currentProduct in result)
                    {
                        //find this in the old product list by searching for the first matching ProductIdSimple. (It's unique so that's fine)
                        var oldProduct = lastResult.Find(x =>
                            x.First(y => y.Key.Equals("ProductIdSimple")).Value
                                .Equals(currentProduct["ProductIdSimple"].ToString()));
                        if (oldProduct == null)
                            continue;

                        double priceCurrent = currentProduct["PriceCurrent"].ToString().ParseToDouble();
                        double priceOld = oldProduct["PriceCurrent"].ToString().ParseToDouble();

                        if (priceOld != priceCurrent)
                        {
                            var message =
                                $"{currentProduct["Brand"]} {currentProduct["Name"]} now costs {currentProduct["PriceCurrent"]} instead of {oldProduct["PriceCurrent"]} ! \n" +
                                "\n" +
                                $"Here's the link: {currentProduct["Url"]}" +
                                $"\n\n" +
                                $"Edit Account Settings: https://www.shoppingtools.online/Identity/Account/Manage";

                            Console.WriteLine(message);
                            Console.WriteLine("Notifying Users...");
                            var notifiedUsers = await UserNotifier.NotifyUsersForProduct(currentProduct["ProductIdSimple"].ToString(),
                                message);
                            Console.WriteLine($"Notified {notifiedUsers} users.");
                        }
                        else
                        {
                            Console.WriteLine("Nothing Happened..");
                        }
                    }
                }

                lastResult = result;

                //Now update the database
                Console.WriteLine("Timer is now at: " + TimeSpan.FromMilliseconds(timer.TimeLeft).TotalSeconds.ToString() + " Seconds");
                Console.WriteLine("Updating the product by fetching the online shops");
                await storage.UpdateAllProducts(result);
                Console.WriteLine("Timer is now at: " + TimeSpan.FromMilliseconds(timer.TimeLeft).TotalSeconds.ToString() + " Seconds");

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

                if (Convert.ToInt32(DateTime.UtcNow.TimeOfDay.TotalHours) == 14)
                {
                    await EmailSender.Send("kevin.mueller1@outlook.com", $"Latest updating routine took: {TimeSpan.FromMilliseconds(timer.Interval).TotalMinutes - TimeSpan.FromMilliseconds(timer.TimeLeft).TotalMinutes} minutes", "Daily Updating Routine Log");
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