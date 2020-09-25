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
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<SYS_USER_ACCOUNT>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyFirstPath : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_CAREERPATH_FIRST>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifySecondPath : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_CAREERPATH_SECOND>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyThirdPath : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_CAREERPATH_THIRD>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyComment : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_CAREERPATH_COMMENT>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyReply : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_COMMENT_REPLY>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyModifyPath : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_MODIFY_PATH>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyTopic : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_TOPICS>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyFaq : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_FAQS>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyPlanHead : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_PLAN_HEADER>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
    public class VerifyPlanDetail : IVerifyData
    {
        public bool Verify(string tableId)
        {
            var db = DBContext.GetInstance;
            int count = db.Queryable<BUS_PLAN_DETAIL>()
                .Where(x => x.ID == tableId && x.STATE == "A")
                .Count();
            return count == 0 ? false : true;
        }
    }
}