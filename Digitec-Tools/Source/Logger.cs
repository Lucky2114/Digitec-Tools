using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Digitec_Tools.Source
{
    public class Logger
    {
        private static Logger instance;
        public string LatestLog { get; set; }

        public static Logger GetInstance()
        {
            return instance ?? (instance = new Logger());
        }

        public void Log(string message)
        {
            LatestLog = message;
        }
    }
}
