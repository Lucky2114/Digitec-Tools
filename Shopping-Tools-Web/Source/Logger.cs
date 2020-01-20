using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shopping_Tools_Web.Source
{
    public class Logger
    {
        private static Logger _instance;
        public string LatestLog { get; set; }

        public static Logger GetInstance()
        {
            return _instance ??= new Logger();
        }

        public void Log(string message)
        {
            LatestLog = message;
        }
    }
}
