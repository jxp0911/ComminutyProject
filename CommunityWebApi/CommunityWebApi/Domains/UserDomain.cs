using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityWebApi.Common;
using CommunityWebApi.Interface;
using CommunityWebApi.Models;
using CommunityWebApi.RealizeInterface;
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
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, firstId, "FIRST_PATH");

                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

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
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");

                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

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
        public RetJsonModel PublishComment(string userId, string pathId, string content,int pathType)
        {
            var db = DBContext.GetInstance;
            try
            {
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, pathId, FunctionHelper.GetDescByCode(pathType));

                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                BUS_CAREERPATH_COMMENT commentModel = new BUS_CAREERPATH_COMMENT();
                commentModel.ID = Guid.NewGuid().ToString();
                commentModel.DATETIME_CREATED = now;
                commentModel.STATE = "A";
                commentModel.TIMESTAMP_INT = timestamp;
                commentModel.PATH_ID = pathId;
                commentModel.CONTENT = content;
                commentModel.FROM_UID = userId;
                commentModel.PATH_TYPE = pathType;
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
        public RetJsonModel PublishReply(string userId, string commentId, string replyId,string content,string toUid)
        {
            var db = DBContext.GetInstance;
            try
            {
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, toUid, "USER_ID");
                FunctionHelper.VerifyInfo(db, commentId, "COMMENT");
                if(commentId != replyId)
                {
                    FunctionHelper.VerifyInfo(db, replyId, "REPLY");
                }

                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                BUS_COMMENT_REPLY replyModel = new BUS_COMMENT_REPLY();
                replyModel.ID = Guid.NewGuid().ToString();
                replyModel.DATETIME_CREATED = now;
                replyModel.STATE = "A";
                replyModel.TIMESTAMP_INT = timestamp;
                replyModel.COMMENT_ID = commentId;
                replyModel.REPLY_ID = replyId;
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


        /// <summary>
        /// 点赞/取消点赞
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="typeId">被点赞的ID</param>
        /// <param name="favourType">点赞类型(1:一级职业路径；2:二级职业路径；3:三级职业路径；4:一级评论；5:二级评论)</param>
        /// <param name="pathType">职业路径等级</param>
        /// <returns></returns>
        public RetJsonModel Favour(string userId,string typeId,int favourType)
        {
            var db = DBContext.GetInstance;
            try
            {
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, typeId, FunctionHelper.GetDescByCode(favourType));

                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                db.Ado.BeginTran();

                RunFunction RF = new RunFunction();
                IGiveFavour GFClass = InterfaceArray.DicGF[favourType];

                RF.RunFavour(db, userId, typeId, favourType, now, timestamp, GFClass);

                db.Ado.CommitTran();

                jsonModel.status = 1;
                jsonModel.msg = "点赞成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <summary>
        /// 普通用户投票
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="modifyId">修改职业规划ID</param>
        /// <param name="isSupport">是否赞同(1:赞同；2:反对)</param>
        /// <returns></returns>
        public RetJsonModel NormalVote(string userId, string modifyId,int isSupport)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, modifyId, "MODIFY");
                int count = db.Queryable<BUS_MODIFIED_VOTE>()
                    .Where(x => x.USER_ID == userId && x.MODIFY_PATH_ID == modifyId && x.STATE == "A")
                    .Count();
                if (count != 0)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = "您已经参与过此投票";
                    return jsonModel;
                }

                db.Ado.BeginTran();
                //在投票记录表插入投票人的投票情况
                BUS_MODIFIED_VOTE model = new BUS_MODIFIED_VOTE();
                model.ID = Guid.NewGuid().ToString();
                model.DATETIME_CREATED = now;
                model.STATE = "A";
                model.TIMESTAMP_INT = timestamp;
                model.USER_ID = userId;
                model.MODIFY_PATH_ID = modifyId;
                model.IS_SUPPORT = isSupport;
                db.Insertable(model).ExecuteCommand();

                //根据赞成还是反对，更新修改职业规划表的投票情况
                db.Updateable<BUS_MODIFY_PATH>()
                    .SetColumnsIF(isSupport == 1, x => new BUS_MODIFY_PATH()
                    {
                        DATETIME_MODIFIED = now,
                        SUPPORT = x.SUPPORT + 1
                    })
                    .SetColumnsIF(isSupport == 0, x => new BUS_MODIFY_PATH()
                    {
                        DATETIME_MODIFIED = now,
                        OPPOSE = x.OPPOSE + 1
                    }).Where(x => x.ID == modifyId && x.STATE == "A").ExecuteCommand();

                db.Ado.CommitTran();

                jsonModel.status = 1;
                jsonModel.msg = "投票成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw ex;
            }
        }


        /// <summary>
        /// Reviewer表决
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="modifyId">修改职业规划ID</param>
        /// <param name="isSupport">是否赞同(1:赞同；2:反对)</param>
        /// <returns></returns>
        public RetJsonModel ReviewerVote(string userId, string modifyId, int isSupport)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, modifyId, "MODIFY");
                int isReviewer = db.Queryable<BUS_PATH_REVIEWER>()
                    .Where(x => x.USER_ID == userId && x.MODIFY_PATH_ID == modifyId && x.STATE == "A")
                    .Count();
                if (isReviewer == 0)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = "您不是当前修改内容的Reviewer";
                    return jsonModel;
                }

                db.Ado.BeginTran();
                //职业规划修改审核人表 更新Reviewer的是否同意字段
                db.Updateable<BUS_PATH_REVIEWER>().SetColumns(x => new BUS_PATH_REVIEWER()
                {
                    DATETIME_MODIFIED = now,
                    IS_AGREE = isSupport
                }).Where(x => x.USER_ID == userId && x.MODIFY_PATH_ID == modifyId && x.STATE == "A")
                .ExecuteCommand();

                int isAllAgree = db.Queryable<BUS_PATH_REVIEWER>()
                    .Where(x => x.MODIFY_PATH_ID == modifyId && x.IS_AGREE == 0 && x.STATE == "A")
                    .Count();
                //判断如果所有的Reviewer都表决通过就将修改后的内容合并到主路径上
                if (isAllAgree == 0)
                {
                    var modifyInfo = db.Queryable<BUS_MODIFY_PATH>()
                        .Where(x => x.ID == modifyId && x.STATE == "A")
                        .First();
                    //如果修改的是二级路径
                    if (modifyInfo.PATH_CLASS == 2)
                    {
                        var secondInfo = db.Queryable<BUS_CAREERPATH_SECOND>()
                            .Where(x => x.ID == modifyInfo.PATH_ID && x.STATE == "A")
                            .First();

                        BUS_CAREERPATH_SECOND secondModel = new BUS_CAREERPATH_SECOND();
                        secondModel.ID = Guid.NewGuid().ToString();
                        secondModel.DATETIME_CREATED = now;
                        secondModel.STATE = "A";
                        secondModel.TIMESTAMP_INT = timestamp;
                        secondModel.USER_ID = modifyInfo.USER_ID;
                        secondModel.TITLE = modifyInfo.CONTENT;
                        secondModel.FIRST_ID = secondInfo.FIRST_ID;
                        secondModel.STATUS = 1;
                        secondModel.FAVOUR_COUNT = 0;
                        secondModel.VERSION_NO = secondInfo.VERSION_NO + 1;
                        db.Insertable(secondModel).ExecuteCommand();

                        //将原来的职业规划状态更新为已过期
                        db.Updateable<BUS_CAREERPATH_SECOND>().SetColumns(x => new BUS_CAREERPATH_SECOND()
                        {
                            DATETIME_MODIFIED = now,
                            STATE = "D"
                        }).Where(x => x.ID == secondInfo.ID).ExecuteCommand();
                    }
                    //如果修改的是三级路径
                    if (modifyInfo.PATH_CLASS == 3)
                    {
                        var thirdInfo = db.Queryable<BUS_CAREERPATH_THIRD>()
                            .Where(x => x.ID == modifyInfo.PATH_ID && x.STATE == "A")
                            .First();

                        BUS_CAREERPATH_THIRD thirdModel = new BUS_CAREERPATH_THIRD();
                        thirdModel.ID = Guid.NewGuid().ToString();
                        thirdModel.DATETIME_CREATED = now;
                        thirdModel.STATE = "A";
                        thirdModel.TIMESTAMP_INT = timestamp;
                        thirdModel.USER_ID = modifyInfo.USER_ID;
                        thirdModel.TITLE = modifyInfo.CONTENT;
                        thirdModel.FIRST_ID = thirdInfo.FIRST_ID;
                        thirdModel.SECOND_ID = thirdInfo.SECOND_ID;
                        thirdModel.STATUS = 1;
                        thirdModel.FAVOUR_COUNT = 0;
                        thirdModel.VERSION_NO = thirdInfo.VERSION_NO + 1;
                        db.Insertable(thirdModel).ExecuteCommand();

                        //将原来的职业规划状态更新为已过期
                        db.Updateable<BUS_CAREERPATH_THIRD>().SetColumns(x => new BUS_CAREERPATH_THIRD()
                        {
                            DATETIME_MODIFIED = now,
                            STATE = "D"
                        }).Where(x => x.ID == thirdInfo.ID).ExecuteCommand();
                    }
                    //更新修改职业规划表的是否合并字段为Y
                    db.Updateable<BUS_MODIFY_PATH>().SetColumns(x => new BUS_MODIFY_PATH()
                    {
                        DATETIME_MODIFIED = now,
                        IS_MERGE = "Y"
                    }).Where(x => x.ID == modifyInfo.ID && x.STATE == "A").ExecuteCommand();
                }

                db.Ado.CommitTran();

                jsonModel.status = 1;
                jsonModel.msg = "投票成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw ex;
            }
        }
    }
}