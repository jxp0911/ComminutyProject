using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    public class LoginReturnModel
    {
        public RetModel user_info { get; set; } = new RetModel();
    }
    public class RetModel
    {
        public string uid { get; set; }
        public string user_name { get; set; }
    }
}