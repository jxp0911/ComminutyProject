using CommunityWebApi.Interface;
using CommunityWebApi.Models;
using Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.RealizeInterface
{
    public class FeedClass : IFeed
    {
        public string topicId { get; }

        public string faqId { get; }
        public int status { get; }
        public bool isOwn { get; }
        public string userId { get; }
        public bool hasMore { get; set; }
        public FeedClass(string user, int sts, bool isown = false, string tid = "", string fid = "")
        {
            topicId = tid;
            faqId = fid;
            status = sts;
            isOwn = isown;
            userId = user;
        }

        public List<NewFeedFirstReturnModel> GetFeedInfo(SqlSugarClient db, int cursor, int count)
        {
            try
            {
                List<string> firstIdList = new List<string>();
                //如果是个人页feed，则先查询出已关注的职业规划id
                if (isOwn == true)
                {
                    firstIdList = GetFirstId(db);
                }
                //过滤查询出最新的count条数据
                var firstData = db.Queryable<BUS_CAREERPATH_FIRST, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE,
                }).Where((a, b) => a.STATE == "A" && a.STATUS == status)
                .WhereIF(isOwn, (a, b) => firstIdList.Contains(a.ID))
                .WhereIF(!string.IsNullOrEmpty(topicId), $"a.ID in (select p.FIRST_PATH_ID from MAP_PATH_TOPIC p where p.TOPIC_ID='{topicId}')")
                .WhereIF(!string.IsNullOrEmpty(faqId), $"a.ID in (select t.FIRST_PATH_ID from MAP_PATH_FAQ t where t.FAQ_ID='{faqId}')")
                .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Asc)
                .Select((a, b) => new NewFeedFirstReturnModel
                {
                    Type = 100,
                    ID = a.ID,
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    USER_ID = a.USER_ID,
                    NICK_NAME = b.NICK_NAME
                }).Skip(cursor).Take(count).ToList();

                List<string> firstId = firstData.Select(x => x.ID).ToList();
                //查询出所有的二级信息
                var secondData = db.Queryable<BUS_CAREERPATH_SECOND, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE
                }).Where((a, b) => a.STATE == "A" && firstId.Contains(a.FIRST_ID) && a.STATUS == status)
                .Select((a, b) => new NewFeedSecondReturnModel
                {
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    ID = a.ID,
                    FIRST_ID = a.FIRST_ID,
                    NICK_NAME = b.NICK_NAME,
                    USER_ID = a.USER_ID
                }).ToList();

                List<string> secondId = secondData.Select(x => x.ID).ToList();
                //查询出所有的三级信息
                var thirdData = db.Queryable<BUS_CAREERPATH_THIRD, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE
                }).Where((a, b) => a.STATE == "A" && secondId.Contains(a.SECOND_ID) && a.STATUS == status)
                .Select((a, b) => new NewFeedThirdReturnModel
                {
                    TIMESTAMP = a.TIMESTAMP_INT,
                    TITLE = a.TITLE,
                    FIRST_ID = a.FIRST_ID,
                    SECOND_ID = a.SECOND_ID,
                    ID = a.ID,
                    NICK_NAME = b.NICK_NAME,
                    USER_ID = a.USER_ID
                }).ToList();

                List<NewFeedFirstReturnModel> list = new List<NewFeedFirstReturnModel>();
                foreach (var item in firstData)
                {
                    item.SecondList = secondData.Where(x => x.FIRST_ID == item.ID).ToList();

                    foreach (var it in item.SecondList)
                    {
                        it.ThirdList = thirdData.Where(x => x.SECOND_ID == it.ID && x.FIRST_ID == it.FIRST_ID).ToList();
                    }

                    list.Add(item);
                }

                HasMore(db, cursor, list.Count);

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void HasMore(SqlSugarClient db, int cursor, int listCount)
        {
            try
            {
                //判断是否还有更多职业规划
                var pathCount = db.Queryable<BUS_CAREERPATH_FIRST>()
                    .Where(x => x.STATUS == status && x.STATE == "A")
                    .WhereIF(isOwn == true, $"ID in (select c.CP_FIRST_ID from MAP_USER_CARREERPATH c where c.USER_ID='{userId}' and c.STATE='A')")
                    .WhereIF(!string.IsNullOrEmpty(topicId), $"ID in (select a.FIRST_PATH_ID from MAP_PATH_TOPIC a where a.TOPIC_ID='{topicId}' and a.STATE='A')")
                    .WhereIF(!string.IsNullOrEmpty(faqId), $"ID in (select b.FIRST_PATH_ID from MAP_PATH_FAQ b where b.FAQ_ID='{faqId}' and b.STATE='A')")
                    .Count();
                bool hasmore = true;
                if (listCount + cursor >= pathCount)
                {
                    hasmore = false;
                }
                hasMore = hasmore;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private List<string> GetFirstId(SqlSugarClient db)
        {
            try
            {
                List<string> fIdList = db.Queryable<MAP_USER_CARREERPATH>()
                    .Where(x => x.USER_ID == userId && x.STATE == "A")
                    .Select(x => x.CP_FIRST_ID).ToList();

                return fIdList ?? new List<string>();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    public class OwnFeedTab1Class : IFeed
    {
        public string topicId { get; }

        public string faqId { get; }
        public int status { get; }
        public bool isOwn { get; }
        public string userId { get; }
        public bool hasMore { get; set; }
        public OwnFeedTab1Class(string user, int sts)
        {
            userId = user;
            status = sts;
        }

        public List<NewFeedFirstReturnModel> GetFeedInfo(SqlSugarClient db, int cursor, int count)
        {
            try
            {
                List<NewFeedFirstReturnModel> list = new List<NewFeedFirstReturnModel>();

                var head = db.Queryable<BUS_PLAN_HEADER, BUS_CAREERPATH_FIRST, SYS_USER_INFO>((a, b, c) => new object[]{
                    JoinType.Inner,a.FIRST_PATH_ID==b.ID&&a.STATE==b.STATE,
                    JoinType.Left,b.USER_ID==c.USER_ID&&b.STATE==c.STATE
                }).Where((a, b, c) => a.USER_ID == userId && a.STATE == "A")
                .OrderBy((a, b, c) => a.DATETIME_CREATED, OrderByType.Desc)
                .Select((a, b, c) => new NewFeedFirstReturnModel
                {
                    Type = 110,
                    ID = b.ID,
                    TIMESTAMP = b.TIMESTAMP_INT,
                    TITLE = b.TITLE,
                    USER_ID = b.USER_ID,
                    NICK_NAME = c.NICK_NAME,
                    PLAN_ID = a.ID,
                }).Skip(cursor).Take(count).ToList();
                List<string> ids = head.Select(x => x.PLAN_ID).ToList();
                var dtl = db.Queryable<BUS_PLAN_DETAIL>().Where(x => ids.Contains(x.HEADER_ID) && x.STATE == "A" && x.VISIBLE_TYPE == 1)
                    .Select(x => new NewFeedSecondReturnModel
                    {
                        ID = x.ID,
                        TITLE = x.CONTENT,
                        TIMESTAMP = x.TIMESTAMP_INT,
                        FIRST_ID = x.HEADER_ID
                    }).ToList();
                foreach (var item in head)
                {
                    item.SecondList = dtl.Where(x => x.FIRST_ID == item.PLAN_ID).ToList();
                    list.Add(item);
                }

                if(head.Count < count)
                {
                    //计算出要从职业规划取出的卡片数量
                    int planCount = db.Queryable<BUS_PLAN_HEADER, BUS_CAREERPATH_FIRST>((a, b) => new object[]
                    {
                        JoinType.Inner,a.FIRST_PATH_ID==b.ID&&a.STATE==b.STATE
                    }).Where((a, b) => b.USER_ID == userId && a.STATE == "A")
                    .Count();
                    cursor = cursor - planCount;
                    count = count - head.Count;

                    //过滤查询出最新的count条数据
                    var firstData = db.Queryable<BUS_CAREERPATH_FIRST, SYS_USER_INFO>((a, b) => new object[]{
                        JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE,
                    }).Where((a, b) => a.USER_ID == userId && a.STATE == "A" && a.STATUS == status)
                    .OrderBy((a, b) => a.DATETIME_CREATED, OrderByType.Desc)
                    .Select((a, b) => new NewFeedFirstReturnModel
                    {
                        Type = 100,
                        ID = a.ID,
                        TIMESTAMP = a.TIMESTAMP_INT,
                        TITLE = a.TITLE,
                        USER_ID = a.USER_ID,
                        NICK_NAME = b.NICK_NAME
                    }).Skip(cursor).Take(count).ToList();

                    List<string> firstId = firstData.Select(x => x.ID).ToList();
                    //查询出所有的二级信息
                    var secondData = db.Queryable<BUS_CAREERPATH_SECOND, SYS_USER_INFO>((a, b) => new object[]{
                        JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE
                    }).Where((a, b) => a.STATE == "A" && firstId.Contains(a.FIRST_ID) && a.STATUS == status)
                    .Select((a, b) => new NewFeedSecondReturnModel
                    {
                        TIMESTAMP = a.TIMESTAMP_INT,
                        TITLE = a.TITLE,
                        ID = a.ID,
                        FIRST_ID = a.FIRST_ID,
                        NICK_NAME = b.NICK_NAME,
                        USER_ID = a.USER_ID
                    }).ToList();

                    List<string> secondId = secondData.Select(x => x.ID).ToList();
                    //查询出所有的三级信息
                    var thirdData = db.Queryable<BUS_CAREERPATH_THIRD, SYS_USER_INFO>((a, b) => new object[]{
                        JoinType.Left,a.USER_ID==b.USER_ID && a.STATE==b.STATE
                    }).Where((a, b) => a.STATE == "A" && secondId.Contains(a.SECOND_ID) && a.STATUS == status)
                    .Select((a, b) => new NewFeedThirdReturnModel
                    {
                        TIMESTAMP = a.TIMESTAMP_INT,
                        TITLE = a.TITLE,
                        FIRST_ID = a.FIRST_ID,
                        SECOND_ID = a.SECOND_ID,
                        ID = a.ID,
                        NICK_NAME = b.NICK_NAME,
                        USER_ID = a.USER_ID
                    }).ToList();

                    foreach (var item in firstData)
                    {
                        item.SecondList = secondData.Where(x => x.FIRST_ID == item.ID).ToList();

                        foreach (var it in item.SecondList)
                        {
                            it.ThirdList = thirdData.Where(x => x.SECOND_ID == it.ID && x.FIRST_ID == it.FIRST_ID).ToList();
                        }
                        list.Add(item);
                    }
                }

                HasMore(db, cursor, list.Count);

                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void HasMore(SqlSugarClient db, int cursor, int listCount)
        {
            try
            {
                int planCount = db.Queryable<BUS_PLAN_HEADER, BUS_CAREERPATH_FIRST>((a, b) => new object[]
                {
                    JoinType.Inner,a.FIRST_PATH_ID==b.ID&&a.STATE==b.STATE
                }).Where((a, b) => b.USER_ID == userId && a.STATE == "A")
                .Count();
                int pathCount = db.Queryable<BUS_CAREERPATH_FIRST>()
                    .Where(x => x.USER_ID == userId && x.STATE == "A" && x.STATUS == status)
                    .Count();
                bool hasmore = true;
                if (listCount + cursor >= planCount + pathCount)
                {
                    hasmore = false;
                }
                hasMore = hasmore;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}