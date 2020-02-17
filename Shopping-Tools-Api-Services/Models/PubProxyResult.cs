using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping_Tools_Api_Services.Models
{
    public class PubProxy
    {
        public Data[] data { get; set; }
        public int count { get; set; }
    }

    public class Data
    {
        public string ipPort { get; set; }
        public string ip { get; set; }
        public string port { get; set; }
        public string country { get; set; }
        public string last_checked { get; set; }
        public string proxy_level { get; set; }
        public string type { get; set; }
        public string speed { get; set; }
        public Support support { get; set; }
    }

    public class Support
    {
        public int https { get; set; }
        public int get { get; set; }
        public int post { get; set; }
        public int cookies { get; set; }
        public int referer { get; set; }
        public int user_agent { get; set; }
        public int google { get; set; }
    }

}
