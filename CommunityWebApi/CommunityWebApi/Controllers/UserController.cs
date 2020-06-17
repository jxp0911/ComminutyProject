using CommunityWebApi.Common;
using CommunityWebApi.Domains;
using CommunityWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommunityWebApi.Controllers
{
    public class UserController : ApiController
    {
        /// <summary>
        /// 关注职业路径接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/user/path")]
        [HttpPost]
        public IHttpActionResult PostCareerPath([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string FirstId = Convert.ToString(value.first_id);
                UserDomain UD = new UserDomain();
                result = UD.FollowCareerPath(UserId, FirstId);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("User", "PostCareerPath", "api/user/path", "关注职业路径接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "关注失败,请重试";
                return Json(result);
            }
        }


        [Route("api/user/getfocus")]
        [HttpGet]
        public IHttpActionResult GetFocusInfo(string user_id,int cursor,int count)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                UserDomain UD = new UserDomain();
                result = UD.GetUserFocus(user_id, cursor, count);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("User", "GetFocusInfo", "api/user/getfocus", "获取当前用户所有关注的职业路径", "用户ID：" + user_id, ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "失败,请重试";
                return Json(result);
            }
        }


        [Route("api/user/p/comment")]
        [HttpPost]
        public IHttpActionResult PostComment([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string pathId = Convert.ToString(value.path_id);
                string content = Convert.ToString(value.content);
                int pathClass = Convert.ToInt32(value.path_class);

                UserDomain UD = new UserDomain();
                result = UD.PublishComment(UserId, pathId, content, pathClass);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("User", "PostComment", "api/user/p/comment", "发表对职业路线的评论", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "评论失败,请重试";
                return Json(result);
            }
        }


        [Route("api/user/p/reply")]
        [HttpPost]
        public IHttpActionResult PostReply([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string commentId = Convert.ToString(value.comment_id);
                string replyId = Convert.ToString(value.reply_id);
                string content = Convert.ToString(value.content);
                string replyType = Convert.ToString(value.reply_type);
                string toUid = Convert.ToString(value.to_uid);

                UserDomain UD = new UserDomain();
                result = UD.PublishReply(UserId, commentId, replyId, replyType, content, toUid);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("User", "PostReply", "api/user/p/reply", "发表对于评论下面的回复", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "回复失败,请重试";
                return Json(result);
            }
        }

        [Route("api/user/p/favour")]
        [HttpPost]
        public IHttpActionResult PostFavour([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string typeId = Convert.ToString(value.type_id);
                int type = Convert.ToInt32(value.type);

                UserDomain UD = new UserDomain();
                result = UD.Favour(UserId, typeId, type);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("User", "PostFavour", "api/user/p/favour", "点赞接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "失败,请重试";
                return Json(result);
            }
        }
    }
}
