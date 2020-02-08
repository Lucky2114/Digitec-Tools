using System;
using System.Collections.Generic;
using System.Text;

namespace Shopping_Tools_Api_Services.Models
{

    public class RestProxyResult
    {
        public _Links _links { get; set; }
        public string ip { get; set; }
        public int port { get; set; }
        public string protocol { get; set; }
        public string anonymity { get; set; }
        public string lastTested { get; set; }
        public bool allowsRefererHeader { get; set; }
        public bool allowsUserAgentHeader { get; set; }
        public bool allowsCustomHeaders { get; set; }
        public bool allowsCookies { get; set; }
        public bool allowsPost { get; set; }
        public bool allowsHttps { get; set; }
        public string country { get; set; }
        public string connectTime { get; set; }
        public string downloadSpeed { get; set; }
        public string secondsToFirstByte { get; set; }
        public string uptime { get; set; }
    }

    public class _Links
    {
        public string _self { get; set; }
        public string _parent { get; set; }
    }

}
