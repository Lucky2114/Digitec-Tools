using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using Digitec_Tools.Source;

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
                Name = this.GetType().Name
            };
            thread.Start();
        }

        private async void Worker()
        {
            List<Dictionary<string, object>> lastResult = null;
            while (!shouldAbort)
            {
                // No Digitec-Tools User needed
                var result = await Storage.GetInstance(null).GetAllProducts();

                if (lastResult != null)
                {
                    foreach (var currentProduct in result)
                    {
                        //find this in the old product list by searching for the first matching ProductIdSimple. (It's unique so that's fine)
                        var oldProduct = lastResult.Find(x => x.First(x => x.Key.Equals("ProductIdSimple")).Value.Equals(currentProduct["ProductIdSimple"].ToString()));

                        double priceCurrent = currentProduct["PriceCurrent"].ToString().ParseToDouble();
                        double priceOld = oldProduct["PriceCurrent"].ToString().ParseToDouble();

                        if (priceOld != priceCurrent)
                        {
                            Console.WriteLine($"{currentProduct["Name"]} now costs {currentProduct["PriceCurrent"]} instead of {oldProduct["PriceCurrent"]} !");
                            Console.WriteLine("Notifiying Users...");

                        }
                        else
                        {
                            Console.WriteLine("Nothing Happened..");
                        }

                    }
                }
                lastResult = result;

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