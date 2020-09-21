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
        //评论ID
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        //评论内容
        [JsonProperty(PropertyName = "content")]
        public string CONTENT { get; set; }
        //发布评论时间
        [JsonProperty(PropertyName = "publish_time")]
        public int PUBLISH_TIME { get; set; }
        //被评论信息的ID
        [JsonProperty(PropertyName = "path_id")]
        public string PATH_ID { get; set; }
        //职业路径级别
        [JsonProperty(PropertyName = "path_class")]
        public int PATH_CLASS { get; set; }
        //是否点赞
        [JsonProperty(PropertyName = "is_favour")]
        public bool IS_FAVOUR { get; set; }
        //点赞数
        [JsonProperty(PropertyName = "favour_count")]
        public int FAVOUR_COUNT { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        //二级评论实体
        [JsonProperty(PropertyName = "reply_info")]
        public dynamic ReplyInfo { get; set; }


        public string _FROM_UID = "";
        //评论人ID
        public string FROM_UID
        {
            get
            {
                return _FROM_UID;
            }
            set
            {
                if (_FROM_UID != value)
                {
                    _FROM_UID = value;
                    UserInfo.USER_ID = value;
                }
            }
        }
        private string _NICK_NAME = "";
        //用户昵称
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
        //二级评论ID
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        //回复内容
        [JsonProperty(PropertyName = "content")]
        public string CONTENT { get; set; }
        //发布回复时间
        [JsonProperty(PropertyName = "publish_time")]
        public int PUBLISH_TIME { get; set; }
        //评论表ID
        [JsonProperty(PropertyName = "comment_id")]
        public string COMMENT_ID { get; set; }
        //回复表ID
        [JsonProperty(PropertyName = "reply_id")]
        public string REPLY_ID { get; set; }
        //是否点赞
        [JsonProperty(PropertyName = "is_favour")]
        public bool IS_FAVOUR { get; set; }
        //点赞数
        [JsonProperty(PropertyName = "favour_count")]
        public int FAVOUR_COUNT { get; set; }
        //回复人的用户信息
        [JsonProperty(PropertyName = "from_user_info")]
        public UserInfoReturnModel FromUserInfo { get; set; } = new UserInfoReturnModel();
        //被回复人的用户信息
        [JsonProperty(PropertyName = "to_user_info")]
        public UserInfoReturnModel ToUserInfo { get; set; } = new UserInfoReturnModel();


        public string _TO_USER_ID = "";
        //目标用户ID
        public string TO_USER_ID
        {
            get
            {
                return _TO_USER_ID;
            }
            set
            {
                if (_TO_USER_ID != value)
                {
                    _TO_USER_ID = value;
                    ToUserInfo.USER_ID = value;
                }
            }
        }
        public string _FROM_USER_ID = "";
        //评论用户ID
        public string FROM_USER_ID
        {
            get
            {
                return _FROM_USER_ID;
            }
            set
            {
                if (_FROM_USER_ID != value)
                {
                    _FROM_USER_ID = value;
                    FromUserInfo.USER_ID = value;
                }
            }
        }
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