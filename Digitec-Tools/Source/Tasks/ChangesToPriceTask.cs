using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace Digitec_Tools.Source.Tasks
{
    public class ChangesToPriceTask : ITask
    {
        private bool shouldAbort;
        private Thread thread;

        private readonly int intervall;

        public ChangesToPriceTask(int intervallInMinutes)
        {
            intervall = intervallInMinutes;
        }

        public void StartTask()
        {
            thread = new Thread(Worker)
            {
                Name = GetType().Name
            };
            thread.Start();
        }

        private async void Worker()
        {
            var storage = Storage.GetInstance(null);
            List<Dictionary<string, object>> lastResult = null;
            while (!shouldAbort)
            {
                var result = await storage.GetAllProducts();

                if (lastResult != null)
                {
                    foreach (var currentProduct in result)
                    {
                        //find this in the old product list by searching for the first matching ProductIdSimple. (It's unique so that's fine)
                        var oldProduct = lastResult.Find(x =>
                            x.First(y => y.Key.Equals("ProductIdSimple")).Value
                                .Equals(currentProduct["ProductIdSimple"].ToString()));

                        double priceCurrent = currentProduct["PriceCurrent"].ToString().ParseToDouble();
                        double priceOld = oldProduct["PriceCurrent"].ToString().ParseToDouble();

                        if (priceOld != priceCurrent)
                        {
                            string message =
                                $"{currentProduct["Brand"]} {currentProduct["Name"]} now costs {currentProduct["PriceCurrent"]} instead of {oldProduct["PriceCurrent"]} ! \n" +
                                "\n" +
                                $"Here's the link: {currentProduct["Url"]}";

                            Console.WriteLine(message);
                            Console.WriteLine("Notifiying Users...");
                            await UserNotifier.NotifyUsersForProduct(currentProduct["ProductIdSimple"].ToString(),
                                message);
                        }
                        else
                        {
                            Console.WriteLine("Nothing Happened..");
                        }
                    }
                }
                
                lastResult = result;

                //Now update the database
                Console.WriteLine("Updating the product by fetching Digitec");
                await storage.UpdateAllProducts(result);


                Thread.Sleep(intervall);
            }

            thread = null;
        }

        public void Abort()
        {
            shouldAbort = true;
        }
    }
}