using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    public class RetJsonModel
    {
        public string msg { get; set; }
        public string status { get; set; }
        public long? time { get; set; }
        //public List<dynamic> data { get; set; } = new List<dynamic>();
        public dynamic data { get; set; }
    }
}