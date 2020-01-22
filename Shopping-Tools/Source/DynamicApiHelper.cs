using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Shopping_Tools_Api_Services.Core.Brack;
using Shopping_Tools_Api_Services.Core.Digitec;

namespace Shopping_Tools.Source
{
    public static class DynamicApiHelper
    { 
        public static List<IApi> GetAllImplementingClasses()
        {
            try
            {
                var baseAssembly = AppDomain.CurrentDomain.GetAssemblies().ToList().Find(x => x.Equals(typeof(IApi).Assembly));
                var implementingTypes = baseAssembly.GetTypes().Where(x => typeof(IApi).IsAssignableFrom(x) && !x.IsInterface && !x.IsAbstract).ToList();
                return implementingTypes.Select(type => (IApi) Activator.CreateInstance(type)).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return null;
        }

        public static IApi GetApiInstanceFromName(string onlineShopName)
        {
            var allImplementingClasses = GetAllImplementingClasses();
            return allImplementingClasses?.First(x => x.OnlineShopName.Equals(onlineShopName));
        }
    }
}