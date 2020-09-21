using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    public class FeedPathFirstModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
        [JsonProperty(PropertyName = "topic_id")]
        public string TOPIC_ID { get; set; }
        [JsonProperty(PropertyName = "faq_id")]
        public string FAQ_ID { get; set; }

        [JsonProperty(PropertyName = "second_list")]
        public List<FeedPathSecondModel> SecondList { get; set; }
    }
    public class FeedPathSecondModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }

        [JsonProperty(PropertyName = "third_list")]
        public List<FeedPathThirdModel> ThirdList { get; set; }
    }
    public class FeedPathThirdModel
    {
        [JsonProperty(PropertyName = "title")]
        public string Title { get; set; }
    }
}