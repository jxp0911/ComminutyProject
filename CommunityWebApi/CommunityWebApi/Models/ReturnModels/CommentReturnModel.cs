using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models
{
    [JsonObject(MemberSerialization.OptIn)]
    public class CommentReturnModel
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string CONTENT { get; set; }
        [JsonProperty(PropertyName = "publish_time")]
        public int PUBLISH_TIME { get; set; }
        [JsonProperty(PropertyName = "path_id")]
        public string PATH_ID { get; set; }
        [JsonProperty(PropertyName = "from_uid")]
        public string FROM_UID { get; set; }
        [JsonProperty(PropertyName = "path_class")]
        public int PATH_CLASS { get; set; }
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        [JsonProperty(PropertyName = "reply_list")]
        public List<ReplyReturnModel> ReplyList { get; set; } = new List<ReplyReturnModel>();


        private string _NICK_NAME = "";
        public string NICK_NAME
        {
            get
            {
                return _NICK_NAME;
            }
            set
            {
                if (_NICK_NAME != value)
                {
                    _NICK_NAME = value;
                    UserInfo.NiCK_NAME = value;
                }
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class ReplyReturnModel
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string CONTENT { get; set; }
        [JsonProperty(PropertyName = "publish_time")]
        public int PUBLISH_TIME { get; set; }
        [JsonProperty(PropertyName = "comment_id")]
        public string COMMENT_ID { get; set; }
        [JsonProperty(PropertyName = "reply_id")]
        public string REPLY_ID { get; set; }
        [JsonProperty(PropertyName = "from_uid")]
        public string FROM_UID { get; set; }
        [JsonProperty(PropertyName = "reply_type")]
        public string REPLY_TYPE { get; set; }

        //回复人的用户信息
        [JsonProperty(PropertyName = "from_user_info")]
        public UserInfoReturnModel FromUserInfo { get; set; } = new UserInfoReturnModel();
        //被回复人的用户信息
        [JsonProperty(PropertyName = "to_user_info")]
        public UserInfoReturnModel ToUserInfo { get; set; } = new UserInfoReturnModel();


        public string _FROM_NICK_NAME = "";
        public string FROM_NICK_NAME
        {
            get
            {
                return _FROM_NICK_NAME;
            }
            set
            {
                if (_FROM_NICK_NAME != value)
                {
                    _FROM_NICK_NAME = value;
                    FromUserInfo.NiCK_NAME = value;
                }
            }
        }
        public string _TO_NICK_NAME = "";
        public string TO_NICK_NAME
        {
            get
            {
                return _TO_NICK_NAME;
            }
            set
            {
                if (_TO_NICK_NAME != value)
                {
                    _TO_NICK_NAME = value;
                    ToUserInfo.NiCK_NAME = value;
                }
            }
        }
    }
}