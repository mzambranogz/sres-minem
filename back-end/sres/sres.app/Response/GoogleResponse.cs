using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace sres.app.Response
{
    public class GoogleResponse
    {
        public bool success { get; set; }
        public double score { get; set; }
        public string action { get; set; }
        public DateTime challenge_ts { get; set; }
        public string hostname { get; set; }
    }
}