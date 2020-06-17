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
    [JsonObject(MemberSerialization.OptIn)]
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
        [JsonProperty(PropertyName = "is_focus")]
        public bool IS_FOCUS { get; set; }
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        [JsonProperty(PropertyName = "second_list")]
        public List<FeedSecondReturnModel> SecondList { get; set; } = new List<FeedSecondReturnModel>();

        //评论信息
        [JsonProperty(PropertyName = "comment_list")]
        public List<CommentReturnModel> CommentList { get; set; } = new List<CommentReturnModel>();

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
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        [JsonProperty(PropertyName = "third_list")]
        public List<FeedThirdReturnModel> ThirdList { get; set; } = new List<FeedThirdReturnModel>();

        //评论信息
        [JsonProperty(PropertyName = "comment_list")]
        public List<CommentReturnModel> CommentList { get; set; } = new List<CommentReturnModel>();

        public string ID { get; set; }
        public string FIRST_ID { get; set; }
        public string _NICK_NAME = "";
        public string NICK_NAME
        {
            get
            {
                return _NICK_NAME;
            }
            set
            {
                if(_NICK_NAME != value)
                {
                    _NICK_NAME = value;
                    UserInfo.NiCK_NAME = value;
                }
            }
        }
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
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();

        //评论信息
        [JsonProperty(PropertyName = "comment_list")]
        public List<CommentReturnModel> CommentList { get; set; } = new List<CommentReturnModel>();

        public string FIRST_ID { get; set; }
        public string SECOND_ID { get; set; }
        public string ID { get; set; }
        public string _NICK_NAME = "";
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
    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoReturnModel
    {
        [JsonProperty(PropertyName = "nick_name")]
        public string NiCK_NAME { get; set; }
    }
}