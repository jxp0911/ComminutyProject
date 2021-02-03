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
    public class VerifyUser : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<SYS_USER_INFO>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.NICK_NAME,
                    USER_ID = x.USER_ID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }

        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<SYS_USER_ACCOUNT>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if(count == 0)
                throw new Exception("当前用户不存在");
        }
    }
    public class VerifyFirstPath : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_CAREERPATH_FIRST>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.TITLE,
                    USER_ID = x.USER_ID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }

        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_CAREERPATH_FIRST>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("一级职业规划不存在");
        }
    }
    public class VerifySecondPath : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_CAREERPATH_SECOND>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.TITLE,
                    USER_ID = x.USER_ID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_CAREERPATH_SECOND>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("二级职业规划不存在");
        }
    }
    public class VerifyThirdPath : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_CAREERPATH_THIRD>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.TITLE,
                    USER_ID = x.USER_ID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_CAREERPATH_THIRD>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("三级职业规划不存在");
        }
    }
    public class VerifyComment : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_CAREERPATH_COMMENT>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.CONTENT,
                    USER_ID = x.FROM_UID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_CAREERPATH_COMMENT>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("评论不存在");
        }
    }
    public class VerifyReply : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_COMMENT_REPLY>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.CONTENT,
                    USER_ID = x.FROM_UID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_COMMENT_REPLY>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("回复不存在");
        }
    }
    public class VerifyModifyPath : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_MODIFY_PATH>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.CONTENT,
                    USER_ID = x.USER_ID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_MODIFY_PATH>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("修改后的职业规划不存在");
        }
    }
    public class VerifyTopic : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_TOPICS>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.TOPIC_NAME,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_TOPICS>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("话题不存在");
        }
    }
    public class VerifyFaq : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_FAQS>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.QUESTION_DESC,
                    USER_ID = x.USER_ID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_FAQS>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("问答不存在");
        }
    }
    public class VerifyPlanHead : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_PLAN_HEADER>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.FIRST_PATH_ID,
                    USER_ID = x.USER_ID,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_PLAN_HEADER>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("计划头不存在");
        }
    }
    public class VerifyPlanDetail : ISingleBus
    {
        public BusinessModel GetData(SqlSugarClient db, string tableId)
        {
            var data = db.Queryable<BUS_PLAN_DETAIL>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Select(x => new BusinessModel
                {
                    ID = x.ID,
                    CONTENT = x.CONTENT,
                    DATETIME_CREATED = x.DATETIME_CREATED,
                    TIMESTAMP_INT = x.TIMESTAMP_INT
                }).First();
            return data;
        }
        public void Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_PLAN_DETAIL>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            if (count == 0)
                throw new Exception("计划明细不存在");
        }
    }
}