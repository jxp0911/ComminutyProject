﻿using CommunityWebApi.Common;
using CommunityWebApi.Domains;
using CommunityWebApi.Interface;
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
                
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());
                VD.Run(FirstId, new VerifyFirstPath());

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
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());
                VD.Run(pathId, InterfaceArray.DicVD[pathType]);

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

                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());
                VD.Run(toUid, new VerifyUser());
                VD.Run(commentId, new VerifyComment());
                if (commentId != replyId)
                    VD.Run(replyId, new VerifyReply());

                //执行业务
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
                string TypeId = Convert.ToString(value.type_id);
                int favourType = Convert.ToInt32(value.favour_type);
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());
                VD.Run(TypeId, InterfaceArray.DicVD[favourType]);

                UserDomain UD = new UserDomain();
                result = UD.Favour(UserId, TypeId, favourType);
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
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());
                VD.Run(modifyId, new VerifyModifyPath());

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
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());
                VD.Run(modifyId, new VerifyModifyPath());

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

        /// <summary>
        /// 修改用户昵称
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/user/changename")]
        [HttpPost]
        public IHttpActionResult ChangeNickName([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string NickName = Convert.ToString(value.nick_name);
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());

                UserDomain UD = new UserDomain();
                result = UD.ChangeNickName(UserId, NickName);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("User", "ChangeNickName", "api/user/changename", "修改用户名", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "失败,请重试";
                return Json(result);
            }
        }
    }
}
