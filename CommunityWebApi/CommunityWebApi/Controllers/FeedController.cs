using CommunityWebApi.Common;
using CommunityWebApi.Domains;
using CommunityWebApi.Models;
using CommunityWebApi.RealizeInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommunityWebApi.Controllers
{
    public class FeedController : ApiController
    {
        [Route("bus/feed/save")]
        [HttpPost]
        public IHttpActionResult PostFeed([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());

                FeedPathFirstModel feedModel = JsonConvert.DeserializeObject<FeedPathFirstModel>(Convert.ToString(value.feed_info));
                if (!string.IsNullOrEmpty(feedModel.TOPIC_ID))
                    VD.Run(feedModel.TOPIC_ID, new VerifyTopic());
                if (!string.IsNullOrEmpty(feedModel.FAQ_ID))
                    VD.Run(feedModel.FAQ_ID, new VerifyFaq());

                FeedDomain FD = new FeedDomain();
                result = FD.PostPath(UserId, feedModel);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "PostFeed", "bus/feed/save", "发文接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "提交失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/feed/getfeed")]
        [HttpGet]
        public IHttpActionResult Get(string uid,int cursor, int count,int status,string topic_id, string faq_id, bool is_own)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                if (!string.IsNullOrEmpty(uid))
                    VD.Run(uid, new VerifyUser());
                if (!string.IsNullOrEmpty(topic_id))
                    VD.Run(uid, new VerifyTopic());
                if (!string.IsNullOrEmpty(faq_id))
                    VD.Run(uid, new VerifyFaq());

                FeedDomain FD = new FeedDomain();
                result = FD.GetSimpleFeed(uid, cursor, count, status, topic_id, faq_id, is_own);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "Get", "bus/feed/getfeed", "获取Feed信息", "用户ID：" + uid + "已经获取的卡片数量:" + cursor + ";本次请求的卡片数量:" + count + "请求的feed状态" + status+"话题ID"+topic_id, ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }
        }


        [Route("bus/feed/aduit")]
        [HttpPost]
        public IHttpActionResult PostAduit([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                int isPass = Convert.ToInt32(value.is_pass);
                string UserId = Convert.ToString(value.user_id);
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());

                List<string> firstId = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(value.first_id));
                FeedDomain FD = new FeedDomain();
                result = FD.AuditPath(firstId, isPass, UserId);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "PostAduit", "bus/feed/aduit", "审批职业路径接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "审核失败，请重试";
                return Json(result);
            }

        }


        [Route("bus/feed/detailed")]
        [HttpGet]
        public IHttpActionResult GetDetailedPage(string user_id, string path_id)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                if (!string.IsNullOrEmpty(user_id))
                    VD.Run(user_id, new VerifyUser());
                VD.Run(path_id, new VerifyFirstPath());

                FeedDomain FD = new FeedDomain();
                result = FD.GetPathDetailedInfo(user_id, path_id);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "GetDetailedPage", "bus/feed/detailed", "获取职业路径详情页接口", "用户ID："+user_id+"；职业路径ID：" + path_id, ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }

        }


        [Route("bus/feed/modify")]
        [HttpPost]
        public IHttpActionResult PostModify([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string userId = Convert.ToString(value.user_id);
                string pathId = Convert.ToString(value.path_id);
                string content = Convert.ToString(value.content);
                int pathClass = Convert.ToInt32(value.path_class);
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(userId, new VerifyUser());
                VD.Run(pathId, InterfaceArray.DicVD[pathClass]);

                FeedDomain FD = new FeedDomain();
                result = FD.ModifyPath(userId, pathId, pathClass, content);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "PostModify", "bus/feed/modify", "发起对某一级职业规划修改的接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }

        }

        [Route("bus/feed/hmodify")]
        [HttpGet]
        public IHttpActionResult GetHistortModify(string user_id, string path_id, int path_class, int cursor, int count)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(user_id, new VerifyUser());
                VD.Run(path_id, InterfaceArray.DicVD[path_class]);

                FeedDomain FD = new FeedDomain();
                result = FD.GetHistortModify(user_id, path_id, path_class, cursor, count);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                string message = "用户ID：" + user_id + "；职业规划ID：" + path_id + "；职业规划等级：" + path_class + "；已下发数：" + cursor + "；本次请求数：" + count;
                FunctionHelper.SaveFailLog("Feed", "GetHistortModify", "bus/feed/hmodify", "查询某一职业规划的历史修改记录", message, ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }

        }

        [Route("bus/feed/modify/detailed")]
        [HttpGet]
        public IHttpActionResult GetModifyDetailedInfo(string user_id, string modify_id)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(user_id, new VerifyUser());
                VD.Run(modify_id, new VerifyModifyPath());

                FeedDomain FD = new FeedDomain();
                result = FD.GetModifyDetailedInfo(user_id, modify_id);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                string message = "用户ID：" + user_id + "；修改职业规划表ID：" + modify_id;
                FunctionHelper.SaveFailLog("Feed", "GetModifyDetailedInfo", "bus/feed/hmodif/detailed", "查询修改职业规划的详情页", message, ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }

        }

        [Route("bus/feed/gettopics")]
        [HttpGet]
        public IHttpActionResult GetTopics(int cursor, int count,bool is_need_path)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                FeedDomain FD = new FeedDomain();
                result = FD.GetTopics(cursor, count, is_need_path);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "GetTopics", "bus/feed/gettopics", "获取所有的话题", "已下发数：" + cursor + "；本次请求数：" + count+";是否需要带一条职业规划"+is_need_path, ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }
        }

        [Route("bus/feed/publishques")]
        [HttpPost]
        public IHttpActionResult PublishQues([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string Desc = Convert.ToString(value.desc);
                string TopicName = Convert.ToString(value.topic_name);
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());

                FeedDomain FD = new FeedDomain();
                result = FD.PublishQues(UserId, Desc, TopicName);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "PublishQues", "bus/feed/publishques", "问答-发起提问接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "提交失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/feed/gettopicinfo")]
        [HttpGet]
        public IHttpActionResult GetTopicDtlById(string user_id, string topic_id)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                if(!string.IsNullOrEmpty(user_id))
                    VD.Run(user_id, new VerifyUser());
                VD.Run(topic_id, new VerifyTopic());

                FeedDomain FD = new FeedDomain();
                result = FD.GetTopicDtlById(user_id, topic_id);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "GetTopicDtlById", "bus/feed/gettopicinfo", "根据话题ID获取话题的详细信息", $"用户ID：{user_id}；话题ID：{topic_id};", ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }
        }
        [Route("bus/feed/getfaqinfo")]
        [HttpGet]
        public IHttpActionResult GetFaqDtlById(string user_id, string faq_id)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(user_id, new VerifyUser());
                VD.Run(faq_id, new VerifyFaq());

                FeedDomain FD = new FeedDomain();
                result = FD.GetFaqDtlById(user_id, faq_id);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "GetTopicDtlById", "bus/feed/gettopicinfo", "根据话题ID获取话题的详细信息", $"用户ID：{user_id}；问答ID：{faq_id};", ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }
        }
        [Route("bus/feed/share")]
        [HttpPost]
        public IHttpActionResult ShareContent([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string ContentId = Convert.ToString(value.content_id);
                string ToSharedId = Convert.ToString(value.to_shared_id);
                int ShareType = Convert.ToInt32(value.share_type);
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());
                if (ShareType == 1)
                {
                    VD.Run(ContentId, new VerifyFirstPath());
                    VD.Run(ToSharedId, new VerifyFaq());
                }

                FeedDomain FD = new FeedDomain();
                result = FD.ShareContent(UserId, ContentId, ToSharedId, ShareType);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "ShareContent", "bus/feed/share", "分享接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "提交失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/feed/gettabs")]
        [HttpGet]
        public IHttpActionResult GetTabsInfo(string user_id)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(user_id, new VerifyUser());

                FeedDomain FD = new FeedDomain();
                result = FD.GetTabsInfo(user_id);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "GetTabsInfo", "bus/feed/gettabs", "下发用户个人页tab信息", $"用户ID：{user_id}", ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }
        }
    }
}
