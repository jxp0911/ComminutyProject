using CommunityWebApi.Interface;
using Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.RealizeInterface
{
    public class VerifyUser : IVerifyData
    {
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
    public class VerifyFirstPath : IVerifyData
    {
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
    public class VerifySecondPath : IVerifyData
    {
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
    public class VerifyThirdPath : IVerifyData
    {
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
    public class VerifyComment : IVerifyData
    {
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
    public class VerifyReply : IVerifyData
    {
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
    public class VerifyModifyPath : IVerifyData
    {
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
    public class VerifyTopic : IVerifyData
    {
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
    public class VerifyFaq : IVerifyData
    {
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
    public class VerifyPlanHead : IVerifyData
    {
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
    public class VerifyPlanDetail : IVerifyData
    {
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