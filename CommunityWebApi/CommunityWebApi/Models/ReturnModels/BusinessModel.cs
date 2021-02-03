using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    public class BusinessModel
    {
        public string ID { get; set; }
        public DateTime? DATETIME_CREATED { get; set; }
        public string CONTENT { get; set; }
        public string USER_ID { get; set; }
        public int? TIMESTAMP_INT { get; set; }
    }
}