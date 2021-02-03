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
        public RetJsonModel PostPath(string userId, FeedPathFirstModel feedModel)
        {
            if(!string.IsNullOrEmpty(feedModel.TOPIC_ID)&& !string.IsNullOrEmpty(feedModel.FAQ_ID))
            {
                throw new Exception("不能同时发布话题和问答");
            }
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();
                RetJsonModel jsonModel = new RetJsonModel();
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

                if (!string.IsNullOrEmpty(feedModel.TOPIC_ID))
                {
                    MAP_PATH_TOPIC pt = new MAP_PATH_TOPIC();
                    pt.ID = Guid.NewGuid().ToString();
                    pt.DATETIME_CREATED = now;
                    pt.STATE = "A";
                    pt.FIRST_PATH_ID = first.ID;
                    pt.TOPIC_ID = feedModel.TOPIC_ID;
                    db.Insertable(pt).ExecuteCommand();
                }
                if (!string.IsNullOrEmpty(feedModel.FAQ_ID))
                {
                    MAP_PATH_FAQ pf = new MAP_PATH_FAQ();
                    pf.ID = Guid.NewGuid().ToString();
                    pf.DATETIME_CREATED = now;
                    pf.STATE = "A";
                    pf.FIRST_PATH_ID = first.ID;
                    pf.FAQ_ID = feedModel.FAQ_ID;
                    db.Insertable(pf).ExecuteCommand();
                }

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
                    second.VERSION_NO = 1;
                    db.Insertable(second).ExecuteCommand();

                    if (item.ThirdList == null)
                        continue;
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
                        third.VERSION_NO = 1;
                        db.Insertable(third).ExecuteCommand();
                    }
                }

                jsonModel.status = 1;
                jsonModel.msg = "提交成功";

                db.Ado.CommitTran();

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
        /// <param name="status">状态(1:审核通过；2：待审核)</param>
        /// <param name="topicId">话题ID</param>
        /// <param name="faqId">问答ID</param>
        /// <param name="isOwn">是否是个人关注页</param>
        /// <returns></returns>
        public RetJsonModel GetFeedPath(string userId,int cursor,int count,int status,string topicId,string faqId, bool isOwn)
        {
            var db = DBContext.GetInstance;
            try
            {
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();

                List<string> fIdList = null;
                if (isOwn)
                {
                    fIdList = new List<string>();
                    fIdList = db.Queryable<MAP_USER_CARREERPATH>()
                        .Where(x => x.USER_ID == userId && x.STATE == "A")
                        .Select(x => x.CP_FIRST_ID).ToList();
                    if (fIdList.Count == 0)
                    {
                        jsonModel.status = 1;
                        jsonModel.msg = "没有关注的职业规划";
                        jsonModel.data = null; 
                        return jsonModel;
                    }
                }
                List<FeedFirstReturnModel> path_list = GetFeedInfo(db, userId, cursor, count, status, isOwn, fIdList, topicId, faqId);
                //判断是否还有更多职业规划
                var pathCount = db.Queryable<BUS_CAREERPATH_FIRST>()
                    .Where(x => x.STATUS == status && x.STATE == "A")
                    .WhereIF(isOwn == true, $"ID in (select c.CP_FIRST_ID from MAP_USER_CARREERPATH c where c.USER_ID='{userId}' and c.STATE='A')")
                    .WhereIF(!string.IsNullOrEmpty(topicId), $"ID in (select a.FIRST_PATH_ID from MAP_PATH_TOPIC a where a.TOPIC_ID='{topicId}' and a.STATE='A')")
                    .WhereIF(!string.IsNullOrEmpty(faqId), $"ID in (select b.FIRST_PATH_ID from MAP_PATH_FAQ b where b.FAQ_ID='{faqId}' and b.STATE='A')")
                    .Count();
                bool has_more = true;
                if (path_list.Count + cursor >= pathCount)
                {
                    has_more = false;
                }

                //将查询出的评论插入到职业路径的数据结构中
                foreach (var item1 in path_list)
                {
                    //查询一级职业规划点赞量最多的评论
                    item1.CommentInfo = new
                    {
                        comment_list = GetCommentByPath(userId, item1.ID, 2, 1, false, 0, 1, out bool hasMore),
                        has_more = hasMore
                    };

                    foreach(var item2 in item1.SecondList)
                    {
                        item2.CommentInfo = new
                        {
                            comment_list = GetCommentByPath(userId, item2.ID, 2, 2, false, 0, 1, out hasMore),
                            has_more = hasMore
                        };
                        foreach (var item3 in item2.ThirdList)
                        {
                            item3.CommentInfo = new
                            {
                                comment_list = GetCommentByPath(userId, item2.ID, 2, 3, false, 0, 1, out hasMore),
                                has_more = hasMore
                            };
                        }
                    }
                }
                if(string.IsNullOrEmpty(topicId) && string.IsNullOrEmpty(faqId) && isOwn == false)
                {
                    FeedFirstReturnModel model = GetFaqs(db);
                    if (model != null)
                        path_list.Insert(0, model);
                }

                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = new
                {
                    path_list,
                    has_more
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 审核/下架接口
        /// </summary>
        /// <param name="firstId">父级ID集合</param>
        /// <param name="isPass">是否通过（0:未通过；1:通过；）</param>
        /// <returns></returns>
        public RetJsonModel AuditPath(List<string> firstId,int isPass,int isDel,string userId)
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
                string state = isDel == 0 ? "D" : "A";
                db.Ado.BeginTran();
                db.Updateable<BUS_CAREERPATH_FIRST>().SetColumns(x => new BUS_CAREERPATH_FIRST()
                {
                    STATUS = isPass,
                    DATETIME_MODIFIED = now,
                    STATE = state
                }).Where(x => firstId.Contains(x.ID) && x.STATE == "A").ExecuteCommand();

                db.Updateable<BUS_CAREERPATH_SECOND>().SetColumns(x => new BUS_CAREERPATH_SECOND()
                {
                    STATUS = isPass,
                    DATETIME_MODIFIED = now,
                    STATE = state
                }).Where(x => firstId.Contains(x.FIRST_ID) && x.STATE == "A").ExecuteCommand();

                db.Updateable<BUS_CAREERPATH_THIRD>().SetColumns(x => new BUS_CAREERPATH_THIRD()
                {
                    STATUS = isPass,
                    DATETIME_MODIFIED = now,
                    STATE = state
                }).Where(x => firstId.Contains(x.FIRST_ID) && x.STATE == "A").ExecuteCommand();
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

                //当前是只有查询所有三级信息的需求
                List<string> IdList = new List<string>();
                IdList.Add(pathId);
                //查询职业规划信息
                FeedFirstReturnModel pathInfo = GetFeedInfo(db, userId, 0, 1, 1, false, IdList).FirstOrDefault();
                //查询一级职业规划的所有评论
                bool hasMore = false;
                pathInfo.CommentInfo = new
                {
                    comment_list = GetCommentByPath(userId, pathId, 2, 1, true, 0, 5, out hasMore),
                    has_more = hasMore
                };

                //将查询出的评论插入到职业路径的数据结构中
                foreach (var item in pathInfo.SecondList)
                {
                    //查询5个点赞最多的二级规划的评论
                    item.CommentInfo = new
                    {
                        comment_list = GetCommentByPath(userId, item.ID, 2, 2, true, 0, 5, out hasMore),
                        has_more = hasMore
                    };
                    item.ModifyInfo = new
                    {
                        modify_list = GetModifyInfo(userId, item.ID, 0, 1, false, out bool hasHistory2),
                        has_history = hasHistory2
                    };

                    foreach (var it in item.ThirdList)
                    {
                        //查询5个点赞最多的三级规划的评论
                        it.CommentInfo = new
                        {
                            comment_list = GetCommentByPath(userId, it.ID, 2, 3, true, 0, 5, out hasMore),
                            has_more = hasMore
                        };
                        it.ModifyInfo = new
                        {
                            modify_list = GetModifyInfo(userId, it.ID, 0, 1, false, out bool hasHistory3),
                            has_history = hasHistory3
                        };
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

        /// <summary>
        /// 修改某一级职业规划
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pathId">职业规划ID</param>
        /// <param name="pathClass">这也规划等级</param>
        /// <param name="content">修改内容</param>
        /// <returns></returns>
        public RetJsonModel ModifyPath(string userId,string pathId,int pathClass,string content)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                //数据校验
                int count = db.Queryable<BUS_MODIFY_PATH>()
                    .Where(x => x.PATH_ID == pathId && x.PATH_CLASS == pathClass && x.STATE == "A" && x.IS_MERGE == "N")
                    .Count();
                if (count > 0)
                {
                    jsonModel.status = 0;
                    jsonModel.msg = "此级职业规划已经有正在讨论中的修改";
                    return jsonModel;
                }

                db.Ado.BeginTran();
                //插入修改职业路径表
                BUS_MODIFY_PATH modModel = new BUS_MODIFY_PATH();
                modModel.ID = Guid.NewGuid().ToString();
                modModel.DATETIME_CREATED = now;
                modModel.STATE = "A";
                modModel.TIMESTAMP_INT = timestamp;
                modModel.USER_ID = userId;
                modModel.PATH_ID = pathId;
                modModel.PATH_CLASS = pathClass;
                modModel.CONTENT = content;
                modModel.IS_MERGE = "N";
                db.Insertable(modModel).ExecuteCommand();
                //插入职业规划修改审核人表
                BUS_PATH_REVIEWER reModel = new BUS_PATH_REVIEWER();
                reModel.ID = Guid.NewGuid().ToString();
                reModel.DATETIME_CREATED = now;
                reModel.STATE = "A";
                reModel.TIMESTAMP_INT = timestamp;
                reModel.USER_ID = userId;
                reModel.MODIFY_PATH_ID = modModel.ID;
                reModel.IS_AGREE = 0;
                db.Insertable(reModel).ExecuteCommand();

                db.Ado.CommitTran();

                jsonModel.status = 1;
                jsonModel.msg = "发表成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <summary>
        /// 获取某级职业规划的历史修改
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pathId">职业规划ID</param>
        /// <param name="pathClass">职业规划等级</param>
        /// <param name="cursor">已经返回的数据条数</param>
        /// <param name="count">前台需要的数据条数</param>
        /// <returns></returns>
        public RetJsonModel GetHistortModify(string userId, string pathId, int pathClass, int cursor, int count)
        {
            try
            {
                var db = DBContext.GetInstance;
                int timestamp = FunctionHelper.GetTimestamp();
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;

                List<ModifyPathModel> modifyList = GetModifyInfo(userId, pathId, cursor, count, true, out bool hasMore);

                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = new
                {
                    modify_list = modifyList,
                    has_more = hasMore
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 查询修改详情页
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="modifyId">修改职业规划的ID</param>
        /// <returns></returns>
        public RetJsonModel GetModifyDetailedInfo(string userId,string modifyId)
        {
            try
            {
                var db = DBContext.GetInstance;
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();

                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, modifyId, "MODIFY");

                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = GetModifyById(userId, modifyId);
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
        private List<FeedFirstReturnModel> GetFeedInfoByType(SqlSugarClient db, int cursor, int count, int status)
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
                for (int i = 0; i < allData.Count; i++)
                {
                    //如果type=1，只返回first这一级的信息
                    FeedFirstReturnModel firstModel = new FeedFirstReturnModel();
                    firstModel.Type = 1;
                    firstModel.ID = allData[i].ID;
                    firstModel.TIMESTAMP = allData[i].TIMESTAMP_INT;
                    firstModel.TITLE = allData[i].TITLE;
                    firstModel.USER_ID = allData[i].USER_ID;
                    firstModel.UserInfo = new UserInfoReturnModel()
                    {
                        NiCK_NAME = allData[i].NICK_NAME
                    };
                    //如果type=2，返回first、second这两级的信息
                    if (i >= typeCount && i < typeCount * 2)
                    {
                        firstModel.Type = 2;
                        firstModel.SecondList = secondData.Where(x => x.FIRST_ID == allData[i].ID).ToList();
                    }
                    //如果type=3，返回first、second、third这三级的信息
                    if (i >= typeCount * 2)
                    {
                        firstModel.Type = 3;
                        firstModel.SecondList = secondData.Where(x => x.FIRST_ID == allData[i].ID).ToList();

                        //third三级信息
                        foreach (var item in firstModel.SecondList)
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
        public List<FeedFirstReturnModel> GetFeedInfo(SqlSugarClient db, string userId, int cursor, int count, int status, bool isOwn, List<string> firstIdList = null,string topicId = "",string faqId = "")
        {
            try
            {
                //过滤查询出最新的count条数据
                var firstData = db.Queryable<BUS_CAREERPATH_FIRST, SYS_USER_INFO, BUS_USER_FAVOUR, MAP_USER_CARREERPATH>((a, b, c, d) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE,
                    JoinType.Left,a.ID==c.TYPE_ID && a.USER_ID==c.USER_ID && a.STATE==c.STATE && c.USER_ID==userId && c.TYPE==1,
                    JoinType.Left,a.ID==d.CP_FIRST_ID&&a.STATE==d.STATE && d.USER_ID==userId
                }).Where((a, b) => a.STATE == "A" && a.STATUS == status)
                .WhereIF(firstIdList != null, (a, b) => firstIdList.Contains(a.ID))
                .WhereIF(!string.IsNullOrEmpty(topicId), $"a.ID in (select p.FIRST_PATH_ID from MAP_PATH_TOPIC p where p.TOPIC_ID='{topicId}')")
                .WhereIF(!string.IsNullOrEmpty(faqId), $"a.ID in (select t.FIRST_PATH_ID from MAP_PATH_FAQ t where t.FAQ_ID='{faqId}')")
                .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Asc)
                .Select((a, b, c, d) => new FeedFirstReturnModel
                {
                    ID = a.ID,
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    USER_ID = a.USER_ID,
                    NICK_NAME = b.NICK_NAME,
                    FAVOUR_COUNT = a.FAVOUR_COUNT,
                    IS_FAVOUR = c.ID != null && c.ID != "",
                    IS_FOCUS = d.ID != null && d.ID != ""
                }).Skip(cursor).Take(count).ToList();
                List<string> firstId = firstData.Select(x => x.ID).ToList();

                //查询出所有的二级信息
                var secondData = db.Queryable<BUS_CAREERPATH_SECOND, SYS_USER_INFO, BUS_USER_FAVOUR>((a, b, c) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE,
                    JoinType.Left,a.ID==c.TYPE_ID && a.USER_ID==c.USER_ID && a.STATE==b.STATE && c.USER_ID==userId && c.TYPE==1
                }).Where((a, b) => a.STATE == "A" && firstId.Contains(a.FIRST_ID) && a.STATUS == status)
                .Select((a, b, c) => new FeedSecondReturnModel
                {
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    ID = a.ID,
                    FIRST_ID = a.FIRST_ID,
                    NICK_NAME = b.NICK_NAME,
                    USER_ID = a.USER_ID,
                    FAVOUR_COUNT = a.FAVOUR_COUNT,
                    IS_FAVOUR = c.ID == null && c.ID != ""
                }).ToList();
                List<string> secondId = secondData.Select(x => x.ID).ToList();
                //查询出所有的三级信息
                var thirdData = db.Queryable<BUS_CAREERPATH_THIRD, SYS_USER_INFO, BUS_USER_FAVOUR>((a, b, c) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE,
                    JoinType.Left,a.ID==c.TYPE_ID && a.USER_ID==c.USER_ID && a.STATE==b.STATE && c.USER_ID==userId && c.TYPE==1
                }).Where((a, b) => a.STATE == "A" && secondId.Contains(a.SECOND_ID) && a.STATUS == status)
                .Select((a, b, c) => new FeedThirdReturnModel
                {
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    FIRST_ID = a.FIRST_ID,
                    SECOND_ID = a.SECOND_ID,
                    ID = a.ID,
                    NICK_NAME = b.NICK_NAME,
                    USER_ID = a.USER_ID,
                    FAVOUR_COUNT = a.FAVOUR_COUNT,
                    IS_FAVOUR = c.ID == null && c.ID != ""
                }).ToList();

                List<FeedFirstReturnModel> list = new List<FeedFirstReturnModel>();
                foreach (var item in firstData)
                {
                    item.Type = 3;
                    item.SecondList = secondData.Where(x => x.FIRST_ID == item.ID).ToList();
                    //查询当前职业规划所绑定的话题集合
                    item.TopicList = db.Queryable<MAP_PATH_TOPIC, BUS_TOPICS>((a, b) => new object[]
                    {
                        JoinType.Inner,a.TOPIC_ID==b.ID&&a.STATE==b.STATE
                    }).Where((a, b) => a.FIRST_PATH_ID == item.ID && a.STATE == "A" && b.STATUS == 1)
                    .Select((a, b) => new TopicsRetornModel
                    {
                        ID = b.ID,
                        TOPIC_NAME = b.TOPIC_NAME
                    }).ToList();
                    //查询当前职业规划所绑定的问题集合
                    item.FaqList = db.Queryable<MAP_PATH_FAQ, BUS_FAQS>((a, b) => new object[]
                    {
                        JoinType.Inner,a.FAQ_ID==b.ID&&a.STATE==b.STATE
                    }).Where((a, b) => a.FIRST_PATH_ID == item.ID && a.STATE == "A")
                    .Select((a, b) => new FaqsRetornModel
                    {
                        FAQ_ID = b.ID,
                        FAQ_DESC = b.QUESTION_DESC
                    }).ToList();
                    //查询分享人
                    item.ShareList = db.Queryable<BUS_USER_SHARE, SYS_USER_INFO>((a, b) => new object[]
                    {
                        JoinType.Inner,a.USER_ID==b.USER_ID&&a.STATE==b.STATE
                    }).Where((a, b) => a.STATE == "A" && a.CONTENT_ID == item.ID)
                    .Select((a, b) => new ShareRetornModel
                    {
                        USER_ID = b.USER_ID,
                        NICK_NAME = b.NICK_NAME
                    }).ToList();

                    item.PlanList = GetPlan(db, item.ID, isOwn);

                    foreach (var it in item.SecondList)
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
        /// 根据职业路径ID查询评论信息
        /// </summary>
        /// <param name="pathId">用户ID</param>
        /// <param name="pathIdList">职业路径ID</param>
        /// <param name="orderbyType">排序方式(1：时间；2：点赞量)</param>
        /// <param name="pathType">评论的职业规划类型</param>
        /// <param name="isReply">是否查询二级评论</param>
        /// <returns></returns>
        public List<CommentReturnModel> GetCommentByPath(string userId,string pathId,int orderbyType, int pathType, bool isReply, int cursor, int count,out bool hasMore)
        {
            try
            {
                var db = DBContext.GetInstance;
                hasMore = false;
                //当前职业规划下所有的一级评论的数量
                int commentCount = db.Queryable<BUS_CAREERPATH_COMMENT>()
                    .Where(x => x.PATH_ID == pathId && x.STATE == "A")
                    .Count();
                if (commentCount == 0) return null;

                List<CommentReturnModel> CommentData = db.Queryable<BUS_CAREERPATH_COMMENT, SYS_USER_INFO, BUS_USER_FAVOUR>((a, b, c) => new object[]{
                    JoinType.Left,a.FROM_UID==b.USER_ID && a.STATE==b.STATE,
                    JoinType.Left,a.ID==c.TYPE_ID && a.STATE==c.STATE && c.USER_ID==userId
                }).Where((a, b, c) => a.PATH_ID == pathId && a.PATH_TYPE == pathType && a.STATE == "A")
                .OrderByIF(orderbyType == 1, (a, b) => a.DATETIME_CREATED, OrderByType.Desc)
                .OrderByIF(orderbyType == 2, (a, b) => a.FAVOUR_COUNT, OrderByType.Desc)
                .Select((a, b, c) => new CommentReturnModel
                {
                    ID = a.ID,
                    CONTENT = a.CONTENT,
                    PUBLISH_TIME = a.TIMESTAMP_INT,
                    PARENT_ID = a.PATH_ID,
                    USER_ID = a.FROM_UID,
                    NICK_NAME = b.NICK_NAME,
                    FAVOUR_COUNT = a.FAVOUR_COUNT,
                    IS_FAVOUR = c.ID != null && c.ID != ""
                }).Skip(cursor).Take(count).ToList();
                //判断是否要查询二级评论
                if (isReply == true)
                {
                    foreach (var item in CommentData)
                    {
                        //判断一级评论下有几个二级评论
                        int replyCount = db.Queryable<BUS_COMMENT_REPLY>()
                                    .Where(x => x.COMMENT_ID == item.ID && x.STATE == "A")
                                    .Count();
                        if (replyCount == 0) continue;
                        List<ReplyReturnModel> ReplyData = db.Queryable<BUS_COMMENT_REPLY, SYS_USER_INFO, SYS_USER_INFO, BUS_USER_FAVOUR>((a, b, c, d) => new object[]
                        {
                            JoinType.Left,a.FROM_UID==b.USER_ID && a.STATE==b.STATE,
                            JoinType.Left,a.TO_UID==c.USER_ID && a.STATE==c.STATE,
                            JoinType.Left,a.ID==d.TYPE_ID&&a.STATE==d.STATE&&d.USER_ID==userId
                        }).Where((a, b, c) => a.COMMENT_ID == item.ID && a.STATE == "A")
                        .OrderByIF(orderbyType == 1, (a, b) => a.DATETIME_CREATED, OrderByType.Desc)
                        .OrderByIF(orderbyType == 2, (a, b) => a.FAVOUR_COUNT, OrderByType.Desc)
                        .Select((a, b, c, d) => new ReplyReturnModel
                        {
                            ID = a.ID,
                            CONTENT = a.CONTENT,
                            PUBLISH_TIME = a.TIMESTAMP_INT,
                            COMMENT_ID = a.COMMENT_ID,
                            PARENT_ID = a.REPLY_ID,
                            USER_ID = a.TO_UID,
                            NICK_NAME = b.NICK_NAME,
                            FAVOUR_COUNT = a.FAVOUR_COUNT,
                            IS_FAVOUR = d.ID != null && d.ID != ""
                        }).Take(1).ToList();
                        //判断是否还有更多
                        bool replyHasMore = replyCount > 1 ? true : false;
                        item.ReplyInfo = new
                        {
                            reply_list = ReplyData,
                            has_more = replyHasMore
                        };
                    }
                }
                hasMore = commentCount > cursor + count ? true : false;

                return CommentData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据职业规划ID查询提出修改的内容
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="pathIdList">职业路径ID</param>
        /// <param name="cursor">已经返回的数据条数</param>
        /// <param name="count">前台需要的数据条数</param>
        /// <param name="isHistory">是否是查询历史修改</param>
        /// <param name="outParam">是否还有未查询到的</param>
        /// <returns></returns>
        public List<ModifyPathModel> GetModifyInfo(string userId, string pathId,int cursor, int count, bool isHistory, out bool outParam)
        {
            try
            {
                var db = DBContext.GetInstance;

                //根据职业规划ID查询正在讨论中的修改
                var modifyData = db.Queryable<BUS_MODIFY_PATH, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && b.STATE=="A"
                }).Where((a, b) => a.PATH_ID == pathId)
                .WhereIF(isHistory == false, (a, b) => a.IS_MERGE == "N" && a.STATE == "A")
                .WhereIF(isHistory == true, (a, b) => (a.IS_MERGE == "Y" || a.STATE == "D"))
                .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Desc)
                .Select((a, b) => new ModifyPathModel
                {
                    ID = a.ID,
                    PATH_ID = a.PATH_ID,
                    PATH_CLASS = a.PATH_CLASS,
                    CONTENT = a.CONTENT,
                    USER_ID = a.USER_ID,
                    NICK_NAME = b.NICK_NAME
                }).Skip(cursor).Take(count).ToList();

                foreach (var item in modifyData)
                {
                    //根据PATH_ID查询评论信息
                    item.CommentInfo = new
                    {
                        comment_list = GetCommentByPath(userId, item.ID, 2, 6, false, 0, 1, out bool hasMore),
                        has_more = hasMore
                    };
                }

                //判断是否还有未查询出的记录
                int historyCount = db.Queryable<BUS_MODIFY_PATH>()
                        .Where(x => x.PATH_ID == pathId && (x.IS_MERGE == "Y" || x.STATE == "D"))
                        .Count();
                //查询历史过期信息
                if (isHistory == true)
                {
                    outParam = false;
                    if (historyCount > cursor + count)
                    {
                        outParam = true;
                    }
                }
                else //查询详情页的正在修改信息
                {
                    //判断是否有历史修改记录
                    outParam = historyCount > 0 ? true : false;
                }

                return modifyData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 根据修改职业规划表的ID查询详细信息
        /// </summary>
        /// <param name="modifyId"></param>
        /// <returns></returns>
        public ModifyPathModel GetModifyById(string userId, string modifyId)
        {
            try
            {
                var db = DBContext.GetInstance;
                ModifyPathModel modifyData = db.Queryable<BUS_MODIFY_PATH, SYS_USER_INFO, BUS_MODIFIED_VOTE>((a, b, c) => new object[]
                  {
                    JoinType.Left,a.USER_ID==b.USER_ID&&a.STATE==b.STATE,
                    JoinType.Left,a.ID==c.MODIFY_PATH_ID&&a.STATE==b.STATE&&c.USER_ID==userId
                  }).Where((a, b) => a.ID == modifyId && a.STATE == "A")
                .Select((a, b, c) => new ModifyPathModel
                {
                    ID = a.ID,
                    PATH_ID = a.PATH_ID,
                    PATH_CLASS = a.PATH_CLASS,
                    CONTENT = a.CONTENT,
                    SUPPORT = a.SUPPORT,
                    OPPOSE = a.OPPOSE,
                    USER_ID = a.USER_ID,
                    NICK_NAME = b.NICK_NAME,
                    IS_VOTE = c.ID != null && c.ID != ""
                }).First();
                //根据职业规划等级判断查询原职业规划内容
                if (modifyData.PATH_CLASS == 2)
                {
                    string oldTitle = db.Queryable<BUS_CAREERPATH_SECOND>()
                            .Where(x => x.ID == modifyData.PATH_ID && x.STATE == "A")
                            .Select(x => x.TITLE).First();
                    modifyData.OLD_PATH = oldTitle;
                }
                if (modifyData.PATH_CLASS == 3)
                {
                    string oldTitle = db.Queryable<BUS_CAREERPATH_THIRD>()
                            .Where(x => x.ID == modifyData.PATH_ID && x.STATE == "A")
                            .Select(x => x.TITLE).First();
                    modifyData.OLD_PATH = oldTitle;
                }
                List<ReviewerVote> reviewer = db.Queryable<BUS_PATH_REVIEWER, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID&&a.STATE==b.STATE
                }).Where((a, b) => a.MODIFY_PATH_ID == modifyId && a.STATE == "A")
                .Select((a, b) => new ReviewerVote
                {
                    IS_AGREE = a.IS_AGREE,
                    USER_ID = a.USER_ID,
                    NICK_NAME = b.NICK_NAME
                }).ToList();
                modifyData.CommentInfo = new
                {
                    comment_list = GetCommentByPath(userId, modifyId, 1, 4, false, 0, 5, out bool hasMore),
                    has_more = hasMore
                };
                modifyData.ReviewerList = reviewer;

                return modifyData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取话题
        /// </summary>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public RetJsonModel GetTopics(int cursor, int count,bool isNeedPath)
        {
            try
            {
                var db = DBContext.GetInstance;

                var data = db.Queryable<BUS_TOPICS>()
                    .Where(x => x.STATE == "A" && x.STATUS == 1)
                    .Select(x => new TopicsRetornModel
                    {
                        ID = x.ID,
                        TOPIC_NAME = x.TOPIC_NAME
                    }).Skip(cursor).Take(count).ToList();
                //判断是否还有未查询到的话题
                bool hasMore = false;
                int allCount = db.Queryable<BUS_TOPICS>().Where(x => x.STATE == "A" && x.STATUS == 1).Count();
                if (cursor + count < allCount)
                {
                    hasMore = true;
                }

                if (isNeedPath)
                {
                    foreach(var item in data)
                    {
                        item.PATH_LIST = GetTopicPathDtl(db, item.ID, 0, 1);
                    }
                }

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();
                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = new
                {
                    topic_list = data,
                    has_more = hasMore
                };

                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 在问答区发起的提问
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="desc">提问描述</param>
        /// <param name="topicName">话题名称</param>
        /// <returns></returns>
        public RetJsonModel PublishQues(string userId,string desc,string topicName)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                var count = db.Queryable<BUS_TOPICS>()
                    .Where(x => x.STATE == "A" && x.TOPIC_NAME == topicName)
                    .Count();
                if (count > 0)
                {
                    throw new Exception("该话题已存在");
                }

                db.Ado.BeginTran();
                //插入问答表
                BUS_FAQS faqModel = new BUS_FAQS();
                faqModel.ID = Guid.NewGuid().ToString();
                faqModel.DATETIME_CREATED = now;
                faqModel.STATE = "A";
                faqModel.TIMESTAMP_INT = timestamp;
                faqModel.QUESTION_DESC = desc;
                faqModel.TOPIC_NAME = topicName;
                faqModel.USER_ID = userId;
                db.Insertable(faqModel).ExecuteCommand();

                //插入话题表，状态为2(还未成为正式话题)
                BUS_TOPICS topicModel = new BUS_TOPICS();
                topicModel.ID = Guid.NewGuid().ToString();
                topicModel.DATETIME_CREATED = now;
                topicModel.STATE = "A";
                topicModel.TIMESTAMP_INT = timestamp;
                topicModel.TOPIC_NAME = desc;
                topicModel.STATUS = 2;
                db.Insertable(topicModel).ExecuteCommand();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;
                jsonModel.status = 1;
                jsonModel.msg = "提交成功";

                db.Ado.CommitTran();

                return jsonModel;
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <summary>
        /// 获取话题下的明细
        /// </summary>
        /// <param name="db"></param>
        /// <param name="topicId">话题ID</param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public FeedFirstReturnModel GetTopicPathDtl(SqlSugarClient db,string topicId, int cursor, int count)
        {
            try
            {
                var firstdata = db.Queryable<MAP_PATH_TOPIC, BUS_CAREERPATH_FIRST, SYS_USER_INFO>((x, y, z) => new object[]{
                    JoinType.Inner,x.FIRST_PATH_ID==y.ID&&x.STATE==y.STATE,
                    JoinType.Left,y.USER_ID==z.USER_ID && y.STATE==z.STATE
                }).Where((x, y, z) => x.TOPIC_ID == topicId && x.STATE == "A" && y.STATUS == 1)
                .OrderBy((x, y, z) => y.FAVOUR_COUNT, OrderByType.Desc)
                .Select((x, y, z) => new FeedFirstReturnModel()
                {
                    ID = y.ID,
                    TITLE = y.TITLE,
                    TIMESTAMP = y.TIMESTAMP_INT,
                    FAVOUR_COUNT = y.FAVOUR_COUNT,
                    USER_ID = y.USER_ID,
                    NICK_NAME = z.NICK_NAME
                }).First();
                if (firstdata == null)
                {
                    return null;
                }

                var seconddata = db.Queryable<BUS_CAREERPATH_SECOND, SYS_USER_INFO>((x, y) => new object[]{
                    JoinType.Left,x.USER_ID==y.USER_ID&&x.STATE==y.STATE
                }).Where((x, y) => x.FIRST_ID == firstdata.ID && x.STATE == "A" && x.STATUS == 1)
                .Select((x, y) => new FeedSecondReturnModel()
                {
                    TITLE = x.TITLE,
                    ID = x.ID,
                    TIMESTAMP = x.TIMESTAMP_INT,
                    FAVOUR_COUNT = x.FAVOUR_COUNT,
                    USER_ID = x.USER_ID,
                    NICK_NAME = y.NICK_NAME
                }).ToList();

                firstdata.SecondList = seconddata;

                foreach (var item in seconddata)
                {
                    var thirddata = db.Queryable<BUS_CAREERPATH_THIRD, SYS_USER_INFO>((x, y) => new object[]{
                        JoinType.Left,x.USER_ID==y.USER_ID&&x.STATE==y.STATE
                    }).Where((x, y) => x.FIRST_ID == firstdata.ID && x.SECOND_ID == item.ID && x.STATE == "A" && x.STATUS == 1)
                    .Select((x, y) => new FeedThirdReturnModel()
                    {
                        TITLE = x.TITLE,
                        ID = x.ID,
                        TIMESTAMP = x.TIMESTAMP_INT,
                        FAVOUR_COUNT = x.FAVOUR_COUNT,
                        USER_ID = x.USER_ID,
                        NICK_NAME = y.NICK_NAME
                    }).ToList();

                    item.ThirdList = thirddata;
                }

                return firstdata;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据话题ID查询某个话题的详情
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="topicId">话题ID</param>
        /// <returns></returns>
        public RetJsonModel GetTopicDtlById(string userId,string topicId)
        {
            try
            {
                var db = DBContext.GetInstance;
                int timestamp = FunctionHelper.GetTimestamp();
                var data = db.Queryable<BUS_TOPICS>()
                    .Where(x => x.ID == topicId && x.STATUS == 1 && x.STATE == "A")
                    .Select(x => new TopicsRetornModel
                    {
                        ID = x.ID,
                        TOPIC_NAME = x.TOPIC_NAME
                    }).First();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = new
                {
                    topic_info = data
                };

                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 用户分享
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="contentId">分享内容ID</param>
        /// <param name="toSharedId">分享到哪里</param>
        /// <param name="shareType">分享类型(1：职业规划分享到问答区)</param>
        /// <returns></returns>
        public RetJsonModel ShareContent(string userId, string contentId, string toSharedId,int shareType)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                db.Ado.BeginTran();
                //插入用户分享表
                BUS_USER_SHARE us = new BUS_USER_SHARE();
                us.ID = Guid.NewGuid().ToString();
                us.DATETIME_CREATED = now;
                us.STATE = "A";
                us.TIMESTAMP_INT = timestamp;
                us.USER_ID = userId;
                us.CONTENT_ID = contentId;
                us.TO_SHARED_ID = toSharedId;
                us.SHARE_TYPE = shareType;
                db.Insertable(us).ExecuteCommand();

                if (shareType == 1)
                {
                    //插入职业规划和问答映射表
                    MAP_PATH_FAQ pf = new MAP_PATH_FAQ();
                    pf.ID = Guid.NewGuid().ToString();
                    pf.DATETIME_CREATED = now;
                    pf.STATE = "A";
                    pf.FIRST_PATH_ID = contentId;
                    pf.FAQ_ID = toSharedId;
                    db.Insertable(pf).ExecuteCommand();
                }

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;
                jsonModel.status = 1;
                jsonModel.msg = "分享成功";

                db.Ado.CommitTran();

                return jsonModel;
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                throw ex;
            }
        }

        public FeedFirstReturnModel GetFaqs(SqlSugarClient db)
        {
            try
            {
                var data = db.Queryable<BUS_FAQS, SYS_USER_INFO>((a, b) => new object[]
                   {
                    JoinType.Left,a.USER_ID==b.USER_ID&&a.STATE==b.STATE
                   }).Where((a, b) => a.STATE == "A")
                .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Asc)
                .Select((a, b) => new FeedFirstReturnModel
                {
                    ID = a.ID,
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.QUESTION_DESC,
                    USER_ID = a.USER_ID,
                    NICK_NAME = b.NICK_NAME,
                    Type = 4
                }).First();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 根据问答ID查询某个话题的详情
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="topicId">话题ID</param>
        /// <returns></returns>
        public RetJsonModel GetFaqDtlById(string userId, string faqId)
        {
            try
            {
                var db = DBContext.GetInstance;
                int timestamp = FunctionHelper.GetTimestamp();
                var data = db.Queryable<BUS_FAQS, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Inner,a.USER_ID==b.USER_ID&&a.STATE==b.STATE
                }).Where((a, b) => a.ID == faqId && a.STATE == "A")
                .Select((a, b) => new
                {
                    a.ID,
                    a.QUESTION_DESC,
                    a.USER_ID,
                    b.NICK_NAME
                }).First();
                FaqsRetornModel faq = new FaqsRetornModel
                {
                    FAQ_ID = data.ID,
                    FAQ_DESC = data.QUESTION_DESC
                };
                UserInfoReturnModel user = new UserInfoReturnModel
                {
                    USER_ID = data.USER_ID,
                    NiCK_NAME = data.NICK_NAME
                };

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = new
                {
                    faq_info = faq,
                    user_info = user
                };

                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<PlanHeaderReturnModel> GetPlan(SqlSugarClient db,string pathId,bool isOwn)
        {
            try
            {
                var data = db.Queryable<BUS_PLAN_HEADER, SYS_USER_INFO, SYS_USER_INFO>((a, b, c) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID&&a.STATE==b.STATE,
                    JoinType.Left,a.SOURCE_USER_ID==c.USER_ID&&a.STATE==c.STATE
                }).Where((a, b, c) => a.FIRST_PATH_ID == pathId && a.STATE == "A")
                .WhereIF(isOwn == false, (a, b, c) => a.IS_SHARED == 1)
                .OrderBy((a, b, c) => a.FAVOUR_COUNT, OrderByType.Desc)
                .Select((a, b, c) => new PlanHeaderReturnModel
                {
                    PLAN_ID = a.ID,
                    TIMESTAMP = a.TIMESTAMP_INT,
                    IS_FAVOUR = false,
                    SOURCE_TYPE = a.SOURCE_TYPE,
                    IS_SHARED = a.IS_SHARED,
                    SHARED_COUNT = a.SHARED_COUNT,
                    SHARE_VERSION = a.SHARE_VERSION,
                    USER_ID = a.USER_ID,
                    NICK_NAME = b.NICK_NAME,
                    SOURCE_USER_ID = a.SOURCE_USER_ID,
                    SOURCE_NICK_NAME = c.NICK_NAME
                }).ToList();
                int visible = isOwn ? 2 : 1;
                foreach(var item in data)
                {
                    var dtl = db.Queryable<BUS_PLAN_DETAIL>()
                        .Where(x => x.HEADER_ID == item.PLAN_ID && x.STATE == "A" && x.VISIBLE_TYPE == visible)
                        .OrderBy(x => x.SEQ, OrderByType.Asc)
                        .Select(x => new PlanDetailReturnModel
                        {
                            PLAN_ID=item.PLAN_ID,
                            PLAN_DTL_ID = x.ID,
                            TIMESTAMP = x.TIMESTAMP_INT,
                            CONTENT = x.CONTENT,
                            STATUS = x.STATUS,
                            COMPLETE_TIME = x.COMPLETE_TIME,
                            VISIBLE_TYPE = x.VISIBLE_TYPE
                        }).ToList();
                    item.PLAN_DTL = dtl;
                }

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 下发所有tab信息
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RetJsonModel GetTabsInfo(string userId)
        {
            try
            {
                var db = DBContext.GetInstance;
                //查询用户信息
                var userInfo = db.Queryable<SYS_USER_INFO>()
                    .Where(x => x.USER_ID == userId && x.STATE == "A")
                    .Select(x => new UserInfoReturnModel
                    {
                        USER_ID = x.USER_ID,
                        NiCK_NAME = x.NICK_NAME
                    }).First();
                //查询所有的tab
                var tabInfo = db.Queryable<SYS_TABS_INFO>()
                    .Where(x => x.STATE == "A")
                    .OrderBy(x => x.SEQ, OrderByType.Asc)
                    .Select(x => new TabDtlModel
                    {
                        CODE = x.TAB_CODE,
                        NAME = x.TAB_NAME,
                        SEQ = x.SEQ
                    }).ToList();
                TabsModel data = new TabsModel();
                data.UserInfo = userInfo;
                data.TabDtlList = tabInfo;

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();
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
        /// 根据code下发不同的卡片
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <param name="status"></param>
        /// <param name="topicId"></param>
        /// <param name="faqId"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public RetJsonModel GetCardInfo(string userId, int cursor, int count, int status, string topicId, string faqId, string code)
        {
            try
            {
                var db = DBContext.GetInstance;
                IFeed ownfeed = null;
                //个人关注页
                if (code == "T01")
                {
                    ownfeed = new FeedClass(userId, status, true);
                }
                //个人发布页
                if (code == "T02")
                {
                    ownfeed = new OwnFeedTab1Class(userId, status);
                }
                //简易feed
                if (code == "F01")
                {
                    ownfeed = new FeedClass(userId, status, false, topicId, faqId);
                }
                RunCard run = new RunCard();
                List<NewFeedFirstReturnModel> list = run.Run(db, cursor, count, ownfeed);
                bool has_more = run.HasMore;

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();
                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = new
                {
                    card_list = list,
                    has_more
                };
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取评论信息
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="id"></param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <param name="code">1:职业规划下面的一级评论传;2:一级评论下面的二级;3:二级下面的递归</param>
        /// <returns></returns>
        public RetJsonModel GetComment(string userId, string id, int cursor, int count, int code)
        {
            try
            {
                IComment CC = null;
                if (code == 1)
                {
                    CC = new CommentClass();
                }
                else if (code == 2 || code ==3)
                {
                    CC = new ReplyClass(code);
                }
                else
                {
                    throw new Exception("code参数值无效");
                }
                RunComment run = new RunComment();
                dynamic data = run.Run(userId, id, cursor, count, CC);

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();
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
    }
}