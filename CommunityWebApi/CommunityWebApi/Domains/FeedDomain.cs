using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityWebApi.Common;
using CommunityWebApi.Models;
using Entitys;
using Newtonsoft.Json;
using SqlSugar;

namespace CommunityWebApi.Domains
{
    public class FeedDomain
    {
        /// <summary>
        /// 发文
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="feedModel">职业路径三级信息</param>
        /// <returns></returns>
        public RetJsonModel FeedPath(string userId, FeedPathFirstModel feedModel)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();

                RetJsonModel jsonModel = new RetJsonModel();
                int timestamp = FunctionHelper.GetTimestamp();
                jsonModel.time = timestamp;
                //判断当前用户是否拥有发文的权限
                int isRelease = db.Queryable<SYS_USER_PERMISSION>()
                    .Where(x => x.USER_ID == userId && x.STATE == "A" && x.PERMISSION_CODE == "RELEASE")
                    .Count();
                if (isRelease == 0)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = "您没有发文权限";
                    return jsonModel;
                }

                db.Ado.BeginTran();
                BUS_CAREERPATH_FIRST first = new BUS_CAREERPATH_FIRST();
                first.ID = Guid.NewGuid().ToString();
                first.DATETIME_CREATED = now;
                first.STATE = "A";
                first.TITLE = feedModel.Title;
                first.USER_ID = userId;
                first.TIMESTAMP_INT = timestamp;
                first.STATUS = 2;
                db.Insertable(first).ExecuteCommand();

                foreach (var item in feedModel.SecondList)
                {
                    BUS_CAREERPATH_SECOND second = new BUS_CAREERPATH_SECOND();
                    second.ID = Guid.NewGuid().ToString();
                    second.DATETIME_CREATED = now;
                    second.STATE = "A";
                    second.TITLE = item.Title;
                    second.FIRST_ID = first.ID;
                    second.USER_ID = userId;
                    second.TIMESTAMP_INT = timestamp;
                    second.STATUS = 2;
                    db.Insertable(second).ExecuteCommand();

                    foreach(var item2 in item.ThirdList)
                    {
                        BUS_CAREERPATH_THIRD third = new BUS_CAREERPATH_THIRD();
                        third.ID = Guid.NewGuid().ToString();
                        third.DATETIME_CREATED = now;
                        third.STATE = "A";
                        third.TITLE = item2.Title;
                        third.FIRST_ID = first.ID;
                        third.SECOND_ID = second.ID;
                        third.USER_ID = userId;
                        third.TIMESTAMP_INT = timestamp;
                        third.STATUS = 2;
                        db.Insertable(third).ExecuteCommand();
                    }
                }
                db.Ado.CommitTran();

                jsonModel.status = 1;
                jsonModel.msg = "提交成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <summary>
        /// 获取feed流信息
        /// </summary>
        /// <param name="cursor">已经返回的数据条数</param>
        /// <param name="count">前台需要的数据条数</param>
        /// <returns></returns>
        public RetJsonModel GetFeedPath(string userId,int cursor,int count,int status)
        {
            var db = DBContext.GetInstance;
            try
            {
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();

                #region 数据验证
                Tuple<bool, string> verify = FunctionHelper.Verify(userId);
                if (verify.Item1 == false)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = verify.Item2;
                    return jsonModel;
                }
                #endregion

                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = GetFeedInfo(db, userId, cursor, count, status);
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询feed流信息(type=1时只返回first这一级的信息，type=2时返回first、second这两级的信息，type=3时返回first、second、third这三级的信息)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cursor">已经返回的数据条数</param>
        /// <param name="count">前台需要的数据条数</param>
        /// <param name="status">前台需要的数据状态（1：审核通过；2：待审核）</param>
        /// <returns></returns>
        private List<FeedFirstReturnModel> GetFeedInfoByType(SqlSugarClient db,int cursor, int count,int status)
        {
            try
            {
                //算出每一级要查询几条数据，如果有余数则余数的那部分当做type=3处理
                int typeCount = count / 3;
                //过滤查询出最新的count条数据
                var allData = db.Queryable<BUS_CAREERPATH_FIRST, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID&&a.STATE==b.STATE
                }).Where((a, b) => a.STATE == "A" && a.STATUS == status)
                .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Asc)
                .Select((a, b) => new
                {
                    a.ID,
                    a.TIMESTAMP_INT,
                    a.TITLE,
                    a.USER_ID,
                    b.NICK_NAME
                }).Skip(cursor).Take(count).ToList();
                //查询first这一级的ID，排除掉type=1的
                List<string> secondType = allData.Skip(typeCount).Select(x => x.ID).ToList();
                //查询出所有的二级信息
                var secondData = db.Queryable<BUS_CAREERPATH_SECOND>()
                    .Where(x => x.STATE == "A" && secondType.Contains(x.FIRST_ID) && x.STATUS == status)
                    .Select(x => new FeedSecondReturnModel
                    {
                        TIMESTAMP = x.TIMESTAMP_INT,
                        TITLE = x.TITLE,
                        ID = x.ID,
                        FIRST_ID = x.FIRST_ID
                    }).ToList();
                //查询second这一级的ID，排除掉type=2的
                List<string> thirdType = secondData.Skip(typeCount).Select(x => x.ID).ToList();
                //查询出所有的三级信息
                var thirdData = db.Queryable<BUS_CAREERPATH_THIRD>()
                    .Where(x => x.STATE == "A" && thirdType.Contains(x.SECOND_ID) && x.STATUS == status)
                    .Select(x => new FeedThirdReturnModel
                    {
                        TIMESTAMP = x.TIMESTAMP_INT,
                        TITLE = x.TITLE,
                        FIRST_ID = x.FIRST_ID,
                        SECOND_ID = x.SECOND_ID
                    }).ToList();


                List<FeedFirstReturnModel> list = new List<FeedFirstReturnModel>();
                for(int i = 0;i < allData.Count; i++)
                {
                    //如果type=1，只返回first这一级的信息
                    FeedFirstReturnModel firstModel = new FeedFirstReturnModel();
                    firstModel.Type = 1;
                    firstModel.FIRST_ID = allData[i].ID;
                    firstModel.TIMESTAMP = allData[i].TIMESTAMP_INT;
                    firstModel.TITLE = allData[i].TITLE;
                    firstModel.UserId = allData[i].USER_ID;
                    firstModel.UserInfo = new UserInfoReturnModel()
                    {
                        NiCK_NAME = allData[i].NICK_NAME
                    };
                    //如果type=2，返回first、second这两级的信息
                    if (i >= typeCount && i < typeCount * 2)
                    {
                        firstModel.Type = 2;
                        firstModel.SecondList = secondData.Where(x=>x.FIRST_ID== allData[i].ID).ToList();
                    }
                    //如果type=3，返回first、second、third这三级的信息
                    if (i >= typeCount * 2)
                    {
                        firstModel.Type = 3;
                        firstModel.SecondList = secondData.Where(x => x.FIRST_ID == allData[i].ID).ToList();

                        //third三级信息
                        foreach(var item in firstModel.SecondList)
                        {
                            item.ThirdList = thirdData.Where(x => x.SECOND_ID == item.ID && x.FIRST_ID == item.FIRST_ID).ToList();
                        }
                    }
                    list.Add(firstModel);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 查询所有feed信息
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="cursor">已经返回的数据条数</param>
        /// <param name="count">前台需要的数据条数</param>
        /// <param name="status">前台需要的数据状态（1：审核通过；2：待审核）</param>
        /// <param name="firstIdList">职业路径一级ID集合</param>
        /// <returns></returns>
        public List<FeedFirstReturnModel> GetFeedInfo(SqlSugarClient db, string userId,int cursor, int count,int status,List<string> firstIdList = null)
        {
            try
            {
                //过滤查询出最新的count条数据
                var firstData = db.Queryable<BUS_CAREERPATH_FIRST, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE
                }).Where((a, b) => a.STATE == "A" && a.STATUS == status)
                .WhereIF(firstIdList != null, (a, b) => firstIdList.Contains(a.ID))
                .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Asc)
                .Select((a, b) => new FeedFirstReturnModel
                {
                    FIRST_ID = a.ID,
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    UserId = a.USER_ID,
                    NICK_NAME = b.NICK_NAME
                }).Skip(cursor).Take(count).ToList();
                List<string> firstId = firstData.Select(x => x.FIRST_ID).ToList();
                //查询出所有的二级信息
                var secondData = db.Queryable<BUS_CAREERPATH_SECOND, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE
                }).Where((a, b) => a.STATE == "A" && firstId.Contains(a.FIRST_ID) && a.STATUS == status)
                .Select((a, b) => new FeedSecondReturnModel
                {
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    ID = a.ID,
                    FIRST_ID = a.FIRST_ID,
                    NICK_NAME = b.NICK_NAME
                }).ToList();
                List<string> secondId = secondData.Select(x => x.ID).ToList();
                //查询出所有的三级信息
                var thirdData = db.Queryable<BUS_CAREERPATH_THIRD, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE
                }).Where((a, b) => a.STATE == "A" && secondId.Contains(a.SECOND_ID) && a.STATUS == status)
                .Select((a, b) => new FeedThirdReturnModel
                {
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    FIRST_ID = a.FIRST_ID,
                    SECOND_ID = a.SECOND_ID,
                    ID = a.ID,
                    NICK_NAME= b.NICK_NAME
                }).ToList();
                //查询当前用户在查到的feed中是否有已关注的
                List<string> userFocus = db.Queryable<MAP_USER_CARREERPATH>()
                    .Where(x => x.USER_ID == userId && firstId.Contains(x.CP_FIRST_ID) && x.STATE == "A")
                    .Select(x => x.CP_FIRST_ID).ToList();

                List<FeedFirstReturnModel> list = new List<FeedFirstReturnModel>();
                foreach (var item in firstData)
                {
                    item.Type = 3;
                    item.IS_FOCUS = userFocus.Where(x => x == item.FIRST_ID).Count() == 1 ? true : false;
                    item.SecondList = secondData.Where(x => x.FIRST_ID == item.FIRST_ID).ToList();
                    foreach(var it in item.SecondList)
                    {
                        it.ThirdList = thirdData.Where(x => x.SECOND_ID == it.ID && x.FIRST_ID == it.FIRST_ID).ToList();
                    }

                    list.Add(item);
                }
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 审核接口
        /// </summary>
        /// <param name="firstId">父级ID集合</param>
        /// <param name="isPass">是否通过（0:未通过；1:通过）</param>
        /// <returns></returns>
        public RetJsonModel AuditPath(List<string> firstId,int isPass,string userId)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();

                //验证当前用户是否有审核的权限
                var isCheck = db.Queryable<SYS_USER_PERMISSION>()
                    .Where(x => x.USER_ID == userId && x.STATE == "A" && x.PERMISSION_CODE == "CHECK")
                    .Count();
                if (isCheck == 0)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = "您没有审核权限";
                    return jsonModel;
                }

                db.Ado.BeginTran();
                db.Updateable<BUS_CAREERPATH_FIRST>().SetColumns(x => new BUS_CAREERPATH_FIRST()
                {
                    STATUS = isPass,
                    DATETIME_MODIFIED = now
                }).ReSetValue(x => x.STATUS == isPass && x.DATETIME_MODIFIED == now).Where(x => firstId.Contains(x.ID)).ExecuteCommand();

                db.Updateable<BUS_CAREERPATH_SECOND>().SetColumns(x => new BUS_CAREERPATH_SECOND()
                {
                    STATUS = isPass,
                    DATETIME_MODIFIED = now
                }).Where(x => firstId.Contains(x.FIRST_ID)).ExecuteCommand();

                db.Updateable<BUS_CAREERPATH_THIRD>().SetColumns(x => new BUS_CAREERPATH_THIRD()
                {
                    STATUS = isPass,
                    DATETIME_MODIFIED = now
                }).Where(x => firstId.Contains(x.FIRST_ID)).ExecuteCommand();
                db.Ado.CommitTran();

                jsonModel.status = 1;
                jsonModel.msg = "成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <summary>
        /// 查询职业路径详情页
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pathId">职业路径ID</param>
        /// <returns></returns>
        public RetJsonModel GetPathDetailedInfo(string userId, string pathId)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();

                #region 数据验证
                Tuple<bool, string> verify = FunctionHelper.Verify("", "", pathId);
                if (verify.Item1 == false)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = verify.Item2;
                    return jsonModel;
                }
                #endregion

                //当前是只有查询所有三级信息的需求
                List<string> firstIdList = new List<string>();
                firstIdList.Add(pathId);
                FeedFirstReturnModel pathInfo = GetFeedInfo(db, userId, 0, 1, 1, firstIdList).FirstOrDefault();
                pathInfo.CommentList = GetCommentByPath(pathInfo.FIRST_ID);
                foreach(var item in pathInfo.SecondList)
                {
                    item.CommentList= GetCommentByPath(item.ID);
                    foreach(var it in item.ThirdList)
                    {
                        it.CommentList = GetCommentByPath(it.ID);
                    }
                }

                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = pathInfo;
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public object GetPathDetailedByID(int type,string pathId)
        {
            try
            {
                var db = DBContext.GetInstance;
                if (type == 1)
                {
                    var data = db.Queryable<BUS_CAREERPATH_FIRST, SYS_USER_INFO>((a, b) => new object[]
                       {
                            JoinType.Left,a.USER_ID==b.USER_ID&&a.STATE==b.STATE
                       }).Where((a, b) => a.ID == pathId && a.STATE == "A")
                    .Select((a, b) => new FeedFirstReturnModel
                    {
                        FIRST_ID = a.ID,
                        Type = 1,
                        TITLE = a.TITLE,
                        UserId = a.USER_ID,
                        TIMESTAMP = a.TIMESTAMP_INT,
                        UserInfo = new UserInfoReturnModel
                        {
                            NiCK_NAME = b.NICK_NAME
                        }
                    }).ToList();
                }
                return null;
            }
            catch (Exception)
            {

                throw;
            }
        }

        /// <summary>
        /// 根据职业路径ID查询评论信息
        /// </summary>
        /// <param name="pathId">职业路径ID</param>
        /// <returns></returns>
        public List<CommentReturnModel> GetCommentByPath(string pathId)
        {
            try
            {
                var db = DBContext.GetInstance;
                List<CommentReturnModel> CommentData = db.Queryable<BUS_CAREERPATH_COMMENT, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.FROM_UID==b.USER_ID&&a.STATE==b.STATE
                }).Where((a, b) => a.PATH_ID == pathId && a.STATE == "A")
                .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Desc)
                .Select((a, b) => new CommentReturnModel
                {
                    ID = a.ID,
                    CONTENT = a.CONTENT,
                    PUBLISH_TIME = a.TIMESTAMP_INT,
                    PATH_ID = a.PATH_ID,
                    FROM_UID = a.FROM_UID,
                    PATH_CLASS = a.PATH_CLASS,
                    NICK_NAME = b.NICK_NAME
                }).ToList();

                List<ReplyReturnModel> ReplyData = db.Queryable<BUS_COMMENT_REPLY, SYS_USER_INFO, SYS_USER_INFO>((a, b, c) => new object[]
                  {
                    JoinType.Left,a.FROM_UID==b.USER_ID && a.STATE==b.STATE,
                    JoinType.Left,a.TO_UID==c.USER_ID && a.STATE==c.STATE
                  }).Where((a, b, c) => a.COMMENT_ID == pathId && a.STATE == "A")
                .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Desc)
                .Select((a, b, c) => new ReplyReturnModel
                {
                    ID = a.ID,
                    CONTENT = a.CONTENT,
                    PUBLISH_TIME = a.TIMESTAMP_INT,
                    COMMENT_ID = a.COMMENT_ID,
                    REPLY_ID = a.REPLY_ID,
                    FROM_UID = a.FROM_UID,
                    REPLY_TYPE = a.REPLY_TYPE,
                    FROM_NICK_NAME = b.NICK_NAME,
                    TO_NICK_NAME = c.NICK_NAME
                }).ToList();
                foreach(var item in CommentData)
                {
                    item.ReplyList = ReplyData.Where(x => x.COMMENT_ID == item.ID).ToList();
                }

                return CommentData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}