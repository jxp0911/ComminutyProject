using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Models.ReturnModels
{
    [JsonObject(MemberSerialization.OptIn)]
    public class MsgReturnModel
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string CONTENT { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();

        public string _USER_ID = "";
        //用户ID
        public string USER_ID
        {
            get
            {
                return _USER_ID;
            }
            set
            {
                if (_USER_ID != value)
                {
                    _USER_ID = value;
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
}