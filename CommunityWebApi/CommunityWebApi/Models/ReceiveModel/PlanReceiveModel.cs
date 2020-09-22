using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    public class PlanHeaderModel
    {
        [JsonProperty(PropertyName = "plan_id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "source_user_id")]
        public string SOURCE_USER_ID { get; set; }
        [JsonProperty(PropertyName = "first_path_id")]
        public string FIRST_PATH_ID { get; set; }
        [JsonProperty(PropertyName = "plan_dtl")]
        public List<PlanDetailModel> PLAN_DTL { get; set; } = new List<PlanDetailModel>();
    }

    public class PlanDetailModel
    {
        [JsonProperty(PropertyName = "plan_dtl_id")]
        public string PLAN_DTL_ID { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string CONTENT { get; set; }
    }
}