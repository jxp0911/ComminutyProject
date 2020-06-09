using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    /// <summary>
    /// first级信息
    /// </summary>
    public class FeedFirstReturnModel
    {
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string UserId { get; set; }
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        [JsonProperty(PropertyName = "first_id")]
        public string FIRST_ID { get; set; }
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        [JsonProperty(PropertyName = "second_list")]
        public List<FeedSecondReturnModel> SecondList { get; set; } = new List<FeedSecondReturnModel>();
    }
    /// <summary>
    /// second级信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class FeedSecondReturnModel
    {
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        [JsonProperty(PropertyName = "third_list")]
        public List<FeedThirdReturnModel> ThirdList { get; set; } = new List<FeedThirdReturnModel>();

        public string ID { get; set; }
        public string FIRST_ID { get; set; }
    }
    /// <summary>
    /// third级信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class FeedThirdReturnModel
    {
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }

        public string FIRST_ID { get; set; }
        public string SECOND_ID { get; set; }
    }
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoReturnModel
    {
        [JsonProperty(PropertyName = "nick_name")]
        public string NiCK_NAME { get; set; }
    }
}