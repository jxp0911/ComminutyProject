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
        public object FeedPath(string userId, FeedPathFirstModel feedModel)
        {
            var db = DBContext.GetInstance;
            RetJsonModel jsonModel = new RetJsonModel();
            int timestamp = FunctionHelper.GetTimestamp();
            jsonModel.time= timestamp; 
            DateTime now = db.GetDate();
            try
            {
                db.Ado.BeginTran();
                BUS_CAREERPATH_FIRST first = new BUS_CAREERPATH_FIRST();
                first.ID = Guid.NewGuid().ToString();
                first.DATETIME_CREATED = now;
                first.STATE = "A";
                first.TITLE = feedModel.Title;
                first.USER_ID = userId;
                first.TIMESTAMP_INT = timestamp;
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
                        db.Insertable(third).ExecuteCommand();
                    }
                }
                db.Ado.CommitTran();

                jsonModel.status = 1;
                jsonModel.msg = "提交成功";
            }
            catch (Exception ex)
            {
                db.Ado.RollbackTran();
                jsonModel.status = 0;
                jsonModel.msg = "提交失败";
            }
            return jsonModel;
        }

        /// <summary>
        /// 获取feed流信息
        /// </summary>
        /// <param name="cursor">已经返回的数据条数</param>
        /// <param name="count">前台需要的数据条数</param>
        /// <returns></returns>
        public object GetFeedPath(int cursor,int count)
        {
            var db = DBContext.GetInstance;
            RetJsonModel jsonModel = new RetJsonModel();
            jsonModel.time = FunctionHelper.GetTimestamp();
            DateTime now = db.GetDate();
            try
            {
                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = OrderFeedInfo(db,cursor, count);
            }
            catch (Exception ex)
            {
                jsonModel.status = 0;
                jsonModel.msg = "失败";
            }
            return jsonModel;
        }

        /// <summary>
        /// 查询feed流信息(type=1时只返回first这一级的信息，type=2时返回first、second这两级的信息，type=3时返回first、second、third这三级的信息)
        /// </summary>
        /// <param name="db"></param>
        /// <param name="cursor">已经返回的数据条数</param>
        /// <param name="count">前台需要的数据条数</param>
        /// <returns></returns>
        private List<FeedFirstReturnModel> OrderFeedInfo(SqlSugarClient db,int cursor, int count)
        {
            try
            {
                //算出每一级要查询几条数据，如果有余数则余数的那部分当做type=3处理
                int typeCount = count / 3;
                //过滤查询出最新的count条数据
                var allData = db.Queryable<BUS_CAREERPATH_FIRST, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Inner,a.USER_ID==b.USER_ID&&a.STATE==b.STATE
                }).Where((a, b) => a.STATE == "A")
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
                    .Where(x => x.STATE == "A" && secondType.Contains(x.FIRST_ID))
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
                    .Where(x => x.STATE == "A" && thirdType.Contains(x.SECOND_ID))
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
    }
}