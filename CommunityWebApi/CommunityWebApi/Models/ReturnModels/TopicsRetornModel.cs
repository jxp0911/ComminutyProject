using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    public class TopicsRetornModel
    {
        [JsonProperty(PropertyName = "topic_id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "topic_name")]
        public string TOPIC_NAME { get; set; }
        [JsonProperty(PropertyName = "path")]
        public FeedFirstReturnModel PATH_LIST { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class FaqsRetornModel
    {
        [JsonProperty(PropertyName = "faq_id")]
        public string FAQ_ID { get; set; }
        [JsonProperty(PropertyName = "faq_desc")]
        public string FAQ_DESC { get; set; }
    }

    public class ShareRetornModel
    {
        [JsonProperty(PropertyName = "user_id")]
        public string USER_ID { get; set; }
        [JsonProperty(PropertyName = "nick_name")]
        public string NICK_NAME { get; set; }
    }
}