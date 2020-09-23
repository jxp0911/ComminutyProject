using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityWebApi.Interface;
using Entitys;
using SqlSugar;

namespace CommunityWebApi.Common
{
    public class VerifyHelper : VerifyInterface
    {
        public bool VerifyInfo(SqlSugarClient db, string info, string type)
        {
            try
            {
                //验证用户ID
                if (type == "USER_ID")
                {
                    var userCount = db.Queryable<SYS_USER_ACCOUNT>()
                        .Where(x => x.ID == info && x.STATE == "A")
                        .Count();
                    if (userCount == 0)
                    {
                        throw new Exception("当前用户不存在");
                    }
                }
                //验证一级职业路径
                if (type == "FIRST_PATH")
                {
                    var careerCount = db.Queryable<BUS_CAREERPATH_FIRST>()
                        .Where(x => x.ID == info && x.STATE == "A")
                        .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("一级职业规划不存在");
                    }
                }
                //验证二级职业路径
                if (type == "SECOND_PATH")
                {
                    var careerCount = db.Queryable<BUS_CAREERPATH_SECOND>()
                        .Where(x => x.ID == info && x.STATE == "A")
                        .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("二级职业规划不存在");
                    }
                }
                //验证三级职业路径
                if (type == "THIRD_PATH")
                {
                    var careerCount = db.Queryable<BUS_CAREERPATH_THIRD>()
                        .Where(x => x.ID == info && x.STATE == "A")
                        .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("三级职业规划不存在");
                    }
                }
                //验证某级职业路径
                if (type == "ANY_PATH")
                {
                    var careerCount = db.Queryable<BUS_CAREERPATH_THIRD>()
                    .Where(x => (x.ID == info || x.FIRST_ID == info || x.SECOND_ID == info))
                    .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("职业规划不存在");
                    }
                }
                //验证一级评论
                if (type == "COMMENT")
                {
                    var careerCount = db.Queryable<BUS_CAREERPATH_COMMENT>()
                            .Where(x => x.ID == info && x.STATE == "A")
                            .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("一级评论不存在");
                    }
                }
                //验证二级回复
                if (type == "REPLY")
                {
                    var careerCount = db.Queryable<BUS_COMMENT_REPLY>()
                            .Where(x => x.ID == info && x.STATE == "A")
                            .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("二级回复不存在");
                    }
                }
                //验证是否是修改的职业规划
                if (type == "MODIFY")
                {
                    var careerCount = db.Queryable<BUS_MODIFY_PATH>()
                            .Where(x => x.ID == info && x.STATE == "A")
                            .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("修改内容不存在");
                    }
                }
                //验证正式话题
                if (type == "TOPIC")
                {
                    var careerCount = db.Queryable<BUS_TOPICS>()
                            .Where(x => x.ID == info && x.STATE == "A" && x.STATUS == 1)
                            .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("话题不存在或不是正式话题");
                    }
                }
                //验证问答区的提问是否存在
                if (type == "FAQ")
                {
                    var careerCount = db.Queryable<BUS_FAQS>()
                            .Where(x => x.ID == info && x.STATE == "A")
                            .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("当前提问不存在");
                    }
                }
                //验证计划头是否存在
                if (type == "PLAN_HRAD")
                {
                    var careerCount = db.Queryable<BUS_PLAN_HEADER>()
                            .Where(x => x.ID == info && x.STATE == "A")
                            .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("当前计划头不存在");
                    }
                }
                //验证计划明细是否存在
                if (type == "PLAN_DTL")
                {
                    var careerCount = db.Queryable<BUS_PLAN_DETAIL>()
                            .Where(x => x.ID == info && x.STATE == "A")
                            .Count();
                    if (careerCount == 0)
                    {
                        throw new Exception("当前计划明细不存在");
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}