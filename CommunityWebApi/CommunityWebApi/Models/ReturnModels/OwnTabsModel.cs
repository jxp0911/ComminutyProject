using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    public class TabsModel
    {
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        [JsonProperty(PropertyName = "tab_list")]
        public List<TabDtlModel> TabDtlList { get; set; } = new List<TabDtlModel>();
    }
    public class TabDtlModel
    {
        [JsonProperty(PropertyName = "code")]
        public string CODE { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string NAME { get; set; }
        [JsonProperty(PropertyName = "seq")]
        public int SEQ { get; set; }
    }
}