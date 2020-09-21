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

        /// <summary>
        /// 发表一级评论接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
                int pathType = Convert.ToInt32(value.path_type);

                UserDomain UD = new UserDomain();
                result = UD.PublishComment(UserId, pathId, content, pathType);
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

        /// <summary>
        /// 发表二级评论接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
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
                string toUid = Convert.ToString(value.to_uid);

                UserDomain UD = new UserDomain();
                result = UD.PublishReply(UserId, commentId, replyId, content, toUid);
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

        /// <summary>
        /// 点赞接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/user/p/favour")]
        [HttpPost]
        public IHttpActionResult PostFavour([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string typeId = Convert.ToString(value.type_id);
                int favourType = Convert.ToInt32(value.favour_type);

                UserDomain UD = new UserDomain();
                result = UD.Favour(UserId, typeId, favourType);
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

        /// <summary>
        /// 投票接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/user/p/nvote")]
        [HttpPost]
        public IHttpActionResult PostNormalVote([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string modifyId = Convert.ToString(value.modify_id);
                int isSupport = Convert.ToInt32(value.is_support);

                UserDomain UD = new UserDomain();
                result = UD.NormalVote(UserId, modifyId, isSupport);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("User", "PostVote", "api/user/p/nvote", "投票接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "失败,请重试";
                return Json(result);
            }
        }

        /// <summary>
        /// Reviewer表决接口
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/user/p/rvote")]
        [HttpPost]
        public IHttpActionResult PostReviewerVote([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string modifyId = Convert.ToString(value.modify_id);
                int isSupport = Convert.ToInt32(value.is_support);

                UserDomain UD = new UserDomain();
                result = UD.ReviewerVote(UserId, modifyId, isSupport);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("User", "PostReviewerVote", "api/user/p/rvote", "Reviewer表决接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "失败,请重试";
                return Json(result);
            }
        }
    }
}
