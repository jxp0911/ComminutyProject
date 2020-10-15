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
        //feed类型(1:所有三级职业规划；2:一、二级职业规划；3:一级职业规划；4:问答)
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }
        //一级标题
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        //创建时间
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        //一级职业路径ID
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        //是否关注
        [JsonProperty(PropertyName = "is_focus")]
        public bool IS_FOCUS { get; set; }
        //是否点赞
        [JsonProperty(PropertyName = "is_favour")]
        public bool IS_FAVOUR { get; set; }
        //点赞数
        [JsonProperty(PropertyName = "favour_count")]
        public int FAVOUR_COUNT { get; set; }
        //是否还有更多
        [JsonProperty(PropertyName = "has_more")]
        public int has_more { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        //评论信息
        [JsonProperty(PropertyName = "comment_info")]
        public dynamic CommentInfo { get; set; }
        //二级职业路径实体
        [JsonProperty(PropertyName = "second_list")]
        public List<FeedSecondReturnModel> SecondList { get; set; } = new List<FeedSecondReturnModel>();
        //话题集合
        [JsonProperty(PropertyName = "topic_list")]
        public List<TopicsRetornModel> TopicList { get; set; } = new List<TopicsRetornModel>();
        //所属的问题集合
        [JsonProperty(PropertyName = "faq_list")]
        public List<FaqsRetornModel> FaqList { get; set; } = new List<FaqsRetornModel>();
        //分享人集合
        [JsonProperty(PropertyName = "share_list")]
        public List<ShareRetornModel> ShareList { get; set; } = new List<ShareRetornModel>();
        //分享人集合
        [JsonProperty(PropertyName = "plan_list")]
        public List<PlanHeaderReturnModel> PlanList { get; set; } = new List<PlanHeaderReturnModel>();

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

    /// <summary>
    /// second级信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class FeedSecondReturnModel
    {
        //二级职业路径ID
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        //二级标题内容
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        //创建时间
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        //是否点赞
        [JsonProperty(PropertyName = "is_favour")]
        public bool IS_FAVOUR { get; set; }
        //点赞数
        [JsonProperty(PropertyName = "favour_count")]
        public int FAVOUR_COUNT { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        //评论信息
        [JsonProperty(PropertyName = "comment_info")]
        public dynamic CommentInfo { get; set; }
        //三级职业路径实体
        [JsonProperty(PropertyName = "third_list")]
        public List<FeedThirdReturnModel> ThirdList { get; set; } = new List<FeedThirdReturnModel>();
        //修改的职业规划实体
        [JsonProperty(PropertyName = "modify_info")]
        public dynamic ModifyInfo { get; set; }

        //一级职业路径ID
        public string FIRST_ID { get; set; }

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
        public string _NICK_NAME = "";
        //用户昵称
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
        //三级职业路径ID
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        //二级标题
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        //创建时间
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        //是否点赞
        [JsonProperty(PropertyName = "is_favour")]
        public bool IS_FAVOUR { get; set; }
        //点赞数
        [JsonProperty(PropertyName = "favour_count")]
        public int FAVOUR_COUNT { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        //评论信息
        [JsonProperty(PropertyName = "comment_info")]
        public dynamic CommentInfo { get; set; }
        //修改的职业规划实体
        [JsonProperty(PropertyName = "modify_info")]
        public dynamic ModifyInfo { get; set; }


        //一级职业路径ID
        public string FIRST_ID { get; set; }
        //二级职业路径ID
        public string SECOND_ID { get; set; }
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
        public string _NICK_NAME = "";
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

    /// <summary>
    /// 修改职业规划信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class ModifyPathModel
    {
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        [JsonProperty(PropertyName = "path_id")]
        public string PATH_ID { get; set; }
        [JsonProperty(PropertyName = "path_class")]
        public int PATH_CLASS { get; set; }
        [JsonProperty(PropertyName = "content")]
        public string CONTENT { get; set; }
        [JsonProperty(PropertyName = "support")]
        public int SUPPORT { get; set; }
        [JsonProperty(PropertyName = "oppose")]
        public int OPPOSE { get; set; }
        [JsonProperty(PropertyName = "is_vote")]
        public bool IS_VOTE { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        //评论信息
        [JsonProperty(PropertyName = "comment_info")]
        public dynamic CommentInfo { get; set; }
        //reviewer表决情况
        [JsonProperty(PropertyName = "reviewer_list")]
        public List<ReviewerVote> ReviewerList { get; set; } = new List<ReviewerVote>();
        //修改前的原内容
        [JsonProperty(PropertyName = "old_path")]
        public string OLD_PATH { get; set; }

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

    public class ReviewerVote
    {
        [JsonProperty(PropertyName = "is_agree")]
        public int IS_AGREE { get; set; }
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

    /// <summary>
    /// 用户信息
    /// </summary>
    public class UserInfoReturnModel
    {
        [JsonProperty(PropertyName = "nick_name")]
        public string NiCK_NAME { get; set; }
        [JsonProperty(PropertyName = "user_id")]
        public string USER_ID { get; set; }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PlanHeaderReturnModel
    {
        //计划头ID
        [JsonProperty(PropertyName = "plan_id")]
        public string PLAN_ID { get; set; }
        //创建时间
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        //是否点赞
        [JsonProperty(PropertyName = "is_favour")]
        public bool IS_FAVOUR { get; set; }
        //点赞数
        [JsonProperty(PropertyName = "favour_count")]
        public int FAVOUR_COUNT { get; set; }
        //计划来源(1：原创；2：借鉴)
        [JsonProperty(PropertyName = "type")]
        public int SOURCE_TYPE { get; set; }
        //是否被分享
        [JsonProperty(PropertyName = "is_shared")]
        public int IS_SHARED { get; set; }
        //被分享的次数
        [JsonProperty(PropertyName = "share_count")]
        public int SHARED_COUNT { get; set; }
        //版本号
        [JsonProperty(PropertyName = "version")]
        public int SHARE_VERSION { get; set; }
        //发布人
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        //原作者
        [JsonProperty(PropertyName = "source_user_info")]
        public UserInfoReturnModel SourceUserInfo { get; set; } = new UserInfoReturnModel();
        //计划明细
        [JsonProperty(PropertyName = "plan_dtl")]
        public List<PlanDetailReturnModel> PLAN_DTL { get; set; } = new List<PlanDetailReturnModel>();

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
        public string _SOURCE_USER_ID = "";
        //用户ID
        public string SOURCE_USER_ID
        {
            get
            {
                return _SOURCE_USER_ID;
            }
            set
            {
                if (_SOURCE_USER_ID != value)
                {
                    _SOURCE_USER_ID = value;
                    SourceUserInfo.USER_ID = value;
                }
            }
        }
        private string _SOURCE_NICK_NAME = "";
        //用户昵称
        public string SOURCE_NICK_NAME
        {
            get
            {
                return _SOURCE_NICK_NAME;
            }
            set
            {
                if (_SOURCE_NICK_NAME != value)
                {
                    _SOURCE_NICK_NAME = value;
                    SourceUserInfo.NiCK_NAME = value;
                }
            }
        }
    }

    [JsonObject(MemberSerialization.OptIn)]
    public class PlanDetailReturnModel
    {
        //计划头ID
        [JsonProperty(PropertyName = "plan_id")]
        public string PLAN_ID { get; set; }
        //计划明细ID
        [JsonProperty(PropertyName = "plan_dtl_id")]
        public string PLAN_DTL_ID { get; set; }
        //创建时间
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        //内容
        [JsonProperty(PropertyName = "content")]
        public string CONTENT { get; set; }
        //完成状态
        [JsonProperty(PropertyName = "status")]
        public int STATUS { get; set; }
        //完成时间
        [JsonProperty(PropertyName = "complete_time")]
        public int? COMPLETE_TIME { get; set; }
        //1:未分享，尽自己可见；2:已分享，全员可见
        [JsonProperty(PropertyName = "visible_type")]
        public int VISIBLE_TYPE { get; set; }
    }


    //-------------新的feed结构-------------
    [JsonObject(MemberSerialization.OptIn)]
    public class NewFeedFirstReturnModel
    {
        //feed类型(1:所有三级职业规划；2:一、二级职业规划；3:一级职业规划；4:问答)
        [JsonProperty(PropertyName = "type")]
        public int Type { get; set; }
        //一级标题
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        //创建时间
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        //一级职业路径ID
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        //二级职业路径实体
        [JsonProperty(PropertyName = "second_list")]
        public List<NewFeedSecondReturnModel> SecondList { get; set; } = new List<NewFeedSecondReturnModel>();

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

        public string PLAN_ID { get; set; }
    }

    /// <summary>
    /// second级信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class NewFeedSecondReturnModel
    {
        //二级职业路径ID
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        //二级标题内容
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        //创建时间
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();
        //三级职业路径实体
        [JsonProperty(PropertyName = "third_list")]
        public List<NewFeedThirdReturnModel> ThirdList { get; set; } = new List<NewFeedThirdReturnModel>();

        //一级职业路径ID
        public string FIRST_ID { get; set; }

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
        public string _NICK_NAME = "";
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

    /// <summary>
    /// third级信息
    /// </summary>
    [JsonObject(MemberSerialization.OptIn)]
    public class NewFeedThirdReturnModel
    {
        //三级职业路径ID
        [JsonProperty(PropertyName = "id")]
        public string ID { get; set; }
        //三级标题
        [JsonProperty(PropertyName = "title")]
        public string TITLE { get; set; }
        //创建时间
        [JsonProperty(PropertyName = "datetime_created")]
        public int TIMESTAMP { get; set; }
        //用户信息
        [JsonProperty(PropertyName = "user_info")]
        public UserInfoReturnModel UserInfo { get; set; } = new UserInfoReturnModel();

        //一级职业路径ID
        public string FIRST_ID { get; set; }
        //二级职业路径ID
        public string SECOND_ID { get; set; }
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
        public string _NICK_NAME = "";
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