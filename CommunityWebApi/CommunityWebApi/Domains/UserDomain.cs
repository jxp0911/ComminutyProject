using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityWebApi.Common;
using CommunityWebApi.Models;
using Entitys;
using Newtonsoft.Json;

namespace CommunityWebApi.Domains
{
    public class UserDomain
    {
        /// <summary>
        /// 关注职业路径
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="firstId">一级职业路径ID</param>
        /// <returns></returns>
        public RetJsonModel FollowCareerPath(string userId,string firstId)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                #region 数据验证
                Tuple<bool, string> verify = FunctionHelper.Verify(userId, firstId);
                if (verify.Item1 == false)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = verify.Item2;
                    return jsonModel;
                }
                #endregion

                var ucData = db.Queryable<MAP_USER_CARREERPATH>()
                    .Where(x => x.USER_ID == userId && x.CP_FIRST_ID == firstId && x.STATE == "A")
                    .First();
                if (ucData == null)
                {
                    MAP_USER_CARREERPATH mapModel = new MAP_USER_CARREERPATH();
                    mapModel.ID = Guid.NewGuid().ToString();
                    mapModel.DATETIME_CREATED = now;
                    mapModel.STATE = "A";
                    mapModel.USER_ID = userId;
                    mapModel.CP_FIRST_ID = firstId;
                    mapModel.TIMESTAMP_INT = timestamp;
                    db.Insertable(mapModel).ExecuteCommand();

                    jsonModel.status = 1;
                    jsonModel.msg = "关注成功";
                }
                else
                {
                    db.Deleteable<MAP_USER_CARREERPATH>().Where(x => x.ID == ucData.ID).ExecuteCommand();

                    jsonModel.status = 1;
                    jsonModel.msg = "取关成功";
                }

                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取用户关注的所有职业路径信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="cursor">已经返回的数据条数</param>
        /// <param name="count">前台需要的数据条数</param>
        /// <returns></returns>
        public RetJsonModel GetUserFocus(string userId,int cursor, int count)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                #region 数据验证
                Tuple<bool, string> verify = FunctionHelper.Verify(userId);
                if (verify.Item1 == false)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = verify.Item2;
                    return jsonModel;
                }
                #endregion

                var fIdList = db.Queryable<MAP_USER_CARREERPATH>()
                        .Where(x => x.USER_ID == userId && x.STATE == "A")
                        .Select(x => x.CP_FIRST_ID).ToList();
                FeedDomain FD = new FeedDomain();
                List<FeedFirstReturnModel> data = FD.GetFeedInfo(db, userId, cursor, count, 1, fIdList);

                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = data;

                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 发表对职业路线的评论
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pathId">职业路线ID</param>
        /// <param name="content">评论内容</param>
        /// <returns></returns>
        public RetJsonModel PublishComment(string userId, string pathId, string content, int pathClass)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                #region 数据验证
                Tuple<bool, string> verify = FunctionHelper.Verify(userId, "", pathId);
                if (verify.Item1 == false)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = verify.Item2;
                    return jsonModel;
                }
                #endregion

                BUS_CAREERPATH_COMMENT commentModel = new BUS_CAREERPATH_COMMENT();
                commentModel.ID = Guid.NewGuid().ToString();
                commentModel.DATETIME_CREATED = now;
                commentModel.STATE = "A";
                commentModel.TIMESTAMP_INT = timestamp;
                commentModel.PATH_ID = pathId;
                commentModel.CONTENT = content;
                commentModel.FROM_UID = userId;
                commentModel.PATH_CLASS = pathClass;
                db.Insertable(commentModel).ExecuteCommand();

                jsonModel.status = 1;
                jsonModel.msg = "评论成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 发表对评论的回复或用户间的互动
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="commentId">评论表ID</param>
        /// <param name="replyId">评论表ID</param>
        /// <param name="replyType">评论表类型（COMMENT：评论；REPLY：回复）</param>
        /// <param name="content">评论内容</param>
        /// <param name="toUid">被评论人ID</param>
        /// <returns></returns>
        public RetJsonModel PublishReply(string userId, string commentId, string replyId,string replyType,string content,string toUid)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                #region 数据验证
                Tuple<bool, string> verify = FunctionHelper.Verify(userId, "", "", commentId, replyId);
                if (verify.Item1 == false)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = verify.Item2;
                    return jsonModel;
                }
                #endregion

                BUS_COMMENT_REPLY replyModel = new BUS_COMMENT_REPLY();
                replyModel.ID = Guid.NewGuid().ToString();
                replyModel.DATETIME_CREATED = now;
                replyModel.STATE = "A";
                replyModel.TIMESTAMP_INT = timestamp;
                replyModel.COMMENT_ID = commentId;
                replyModel.REPLY_ID = replyId;
                replyModel.REPLY_TYPE = replyType;
                replyModel.CONTENT = content;
                replyModel.FROM_UID = userId;
                replyModel.TO_UID = toUid;

                db.Insertable(replyModel).ExecuteCommand();

                jsonModel.status = 1;
                jsonModel.msg = "回复成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public RetJsonModel Favour(string userId,string typeId,int type)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                #region 数据验证
                Tuple<bool, string> verify = FunctionHelper.Verify(userId);
                if (verify.Item1 == false)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = verify.Item2;
                    return jsonModel;
                }
                #endregion

                var count = db.Queryable<BUS_USER_FAVOUR>()
                    .Where(x => x.USER_ID == userId && x.TYPE_ID == typeId && x.TYPE == type && x.STATE == "A")
                    .Count();
                if (count == 1)
                {
                    db.Deleteable<BUS_USER_FAVOUR>().Where(x => x.USER_ID == userId && x.TYPE_ID == typeId && x.STATE == "A" && x.TYPE == type).ExecuteCommand();
                    jsonModel.msg = "取消成功";
                }
                else
                {
                    BUS_USER_FAVOUR model = new BUS_USER_FAVOUR();
                    model.ID = Guid.NewGuid().ToString();
                    model.DATETIME_CREATED = now;
                    model.STATE = "A";
                    model.TIMESTAMP_INT = timestamp;
                    model.USER_ID = userId;
                    model.TYPE_ID = typeId;
                    model.TYPE = type;
                    db.Insertable(model).ExecuteCommand();

                    jsonModel.msg = "点赞成功";
                }

                jsonModel.status = 1;
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}