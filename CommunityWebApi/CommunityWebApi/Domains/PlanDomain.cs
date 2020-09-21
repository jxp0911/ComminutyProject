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
    //计划
    public class PlanDomain
    {
        /// <summary>
        /// 发布计划
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="isShare">是否直接分享</param>
        /// <param name="feedModel">职业路径三级信息</param>
        /// <returns></returns>
        public RetJsonModel PostPlan(string userId, int isShare,PlanHeaderModel planModel)
        {
            var db = DBContext.GetInstance;
            try
            {
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, planModel.FIRST_PATH_ID, "FIRST_PATH");
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                db.Ado.BeginTran();
                //插入计划头表
                BUS_PLAN_HEADER head = new BUS_PLAN_HEADER();
                head.ID = Guid.NewGuid().ToString();
                head.DATETIME_CREATED = now;
                head.STATE = "A";
                head.TIMESTAMP_INT = timestamp;
                head.USER_ID = userId;
                head.STATUS = 0;
                head.FAVOUR_COUNT = 0;
                head.SOURCE_TYPE = 1;
                head.FIRST_PATH_ID = planModel.FIRST_PATH_ID;
                head.SHARED_COUNT = 0;
                head.IS_SHARED = isShare;
                if (isShare == 1)
                    head.SHARE_VERSION = 1;
                db.Insertable(head).ExecuteCommand();

                int seq = 1;
                foreach(var item in planModel.PLAN_DTL)
                {
                    //插入计划明细表（个人可见）
                    BUS_PLAN_DETAIL dtl1 = new BUS_PLAN_DETAIL();
                    dtl1.ID = Guid.NewGuid().ToString();
                    dtl1.DATETIME_CREATED = now;
                    dtl1.STATE = "A";
                    dtl1.TIMESTAMP_INT = timestamp;
                    dtl1.HEADER_ID = head.ID;
                    dtl1.SEQ = seq;
                    dtl1.CONTENT = item.CONTENT;
                    dtl1.STATUS = 0;
                    dtl1.VISIBLE_TYPE = 1;
                    db.Insertable(dtl1).ExecuteCommand();

                    if (isShare == 1)
                    {
                        //插入计划明细表（全员可见）
                        BUS_PLAN_DETAIL dtl2 = new BUS_PLAN_DETAIL();
                        dtl2.ID = Guid.NewGuid().ToString();
                        dtl2.DATETIME_CREATED = now;
                        dtl2.STATE = "A";
                        dtl2.TIMESTAMP_INT = timestamp;
                        dtl2.HEADER_ID = head.ID;
                        dtl2.SEQ = seq;
                        dtl2.CONTENT = item.CONTENT;
                        dtl2.STATUS = 0;
                        dtl2.VISIBLE_TYPE = 2;
                        db.Insertable(dtl2).ExecuteCommand();
                    }

                    seq++;
                }

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
        /// 分享计划
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="planId">计划头ID</param>
        /// <returns></returns>
        public RetJsonModel SharePlan(string userId,string planId)
        {
            var db = DBContext.GetInstance;
            try
            {
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, planId, "PLAN");
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                var data = db.Queryable<BUS_PLAN_HEADER>()
                    .Where(x => x.STATE == "A" && x.ID == planId)
                    .First();
                db.Ado.BeginTran();
                //分享计划的方法
                SharePlanFun(db, planId, now, timestamp);

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

        /// <summary>
        /// 取消分享
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="planId">计划头ID</param>
        public RetJsonModel SharePlanCancel(string userId,string planId)
        {
            var db = DBContext.GetInstance;
            try
            {
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, planId, "PLAN");
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                var data = db.Queryable<BUS_PLAN_HEADER>()
                    .Where(x => x.ID == planId && x.STATE == "A")
                    .First();
                if (data.USER_ID != userId.Trim())
                    throw new Exception("不能取消分享他人的计划");
                if (data.IS_SHARED == 0)
                    throw new Exception("该计划还未分享");

                db.Ado.BeginTran();
                //更新头表的状态为未分享
                db.Updateable<BUS_PLAN_HEADER>().SetColumns(x => new BUS_PLAN_HEADER
                {
                    IS_SHARED = 0,
                    SHARE_VERSION = 0
                }).Where(x => x.ID == planId && x.STATE == "A").ExecuteCommand();
                //删除明细表的已分享的数据
                db.Deleteable<BUS_PLAN_DETAIL>()
                    .Where(x => x.HEADER_ID == planId && x.STATE == "A" && x.VISIBLE_TYPE == 2)
                    .ExecuteCommand();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;
                jsonModel.status = 1;
                jsonModel.msg = "取消成功";

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
        /// 修改计划
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="isShare">是否分享</param>
        /// <param name="planId">计划头ID</param>
        /// <param name="planDtlId">计划明细集合</param>
        /// <returns></returns>
        public RetJsonModel UpdatePlan(string userId, int isShare, PlanHeaderModel planModel)
        {
            var db = DBContext.GetInstance;
            try
            {
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");
                FunctionHelper.VerifyInfo(db, planModel.ID, "PLAN_HRAD");
                foreach(var item in planModel.PLAN_DTL.Where(x=>string.IsNullOrEmpty(x.ID)).ToList())
                {
                    FunctionHelper.VerifyInfo(db, item.ID, "PLAN_DTL");
                }
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();
                int seq = 1;
                foreach(var item in planModel.PLAN_DTL)
                {
                    if (!string.IsNullOrEmpty(item.ID))
                    {
                        db.Updateable<BUS_PLAN_DETAIL>().SetColumns(x => new BUS_PLAN_DETAIL
                        {
                            DATETIME_MODIFIED = now,
                            CONTENT = item.CONTENT,
                            SEQ = seq
                        }).Where(x => x.ID == item.ID && x.STATE == "A" && x.VISIBLE_TYPE == 1).ExecuteCommand();
                    }
                    else
                    {
                        BUS_PLAN_DETAIL plandtl = new BUS_PLAN_DETAIL();
                        plandtl.ID = Guid.NewGuid().ToString();
                        plandtl.DATETIME_CREATED = now;
                        plandtl.STATE = "A";
                        plandtl.TIMESTAMP_INT = timestamp;
                        plandtl.HEADER_ID = planModel.ID;
                        plandtl.SEQ = seq;
                        plandtl.CONTENT = item.CONTENT;
                        plandtl.STATUS = 0;
                        plandtl.VISIBLE_TYPE = 1;
                        db.Insertable(plandtl).ExecuteCommand();
                    }

                    seq++;
                }
                List<string> idList = planModel.PLAN_DTL.Where(x => !string.IsNullOrEmpty(x.ID)).Select(x => x.ID).ToList();
                db.Deleteable<BUS_PLAN_DETAIL>()
                    .Where(x => !idList.Contains(x.ID) && x.STATE == "A")
                    .ExecuteCommand();
                if (isShare == 1)
                {
                    SharePlanFun(db, planModel.ID, now, timestamp);
                }

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;
                jsonModel.status = 1;
                jsonModel.msg = "修改成功";

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
        /// 分享计划方法
        /// </summary>
        /// <param name="db"></param>
        /// <param name="planId">计划头ID</param>
        /// <param name="now"></param>
        /// <param name="timestamp"></param>
        public void SharePlanFun(SqlSugarClient db,string planId,DateTime now,int timestamp)
        {
            try
            {
                //删除上一个版本的计划
                db.Deleteable<BUS_PLAN_DETAIL>()
                    .Where(x => x.HEADER_ID == planId && x.STATE == "A" && x.VISIBLE_TYPE == 2)
                    .ExecuteCommand();
                //生成COPY一个新的计划,状态为分享
                var dtlData = db.Queryable<BUS_PLAN_DETAIL>()
                    .Where(x => x.HEADER_ID == planId && x.STATE == "A" && x.VISIBLE_TYPE == 1)
                    .Select(x => new BUS_PLAN_DETAIL
                    {
                        STATE = x.STATE,
                        HEADER_ID = x.HEADER_ID,
                        SEQ = x.SEQ,
                        CONTENT = x.CONTENT,
                        STATUS = x.STATUS,
                        VISIBLE_TYPE = 2,
                        DATETIME_CREATED = now,
                        TIMESTAMP_INT = timestamp,
                        ID = Guid.NewGuid().ToString()
                    }).ToList();
                db.Insertable(dtlData).ExecuteCommand();
                //更新头表状态为已分享，版本号加1
                db.Updateable<BUS_PLAN_HEADER>().SetColumns(x => new BUS_PLAN_HEADER
                {
                    DATETIME_MODIFIED = now,
                    IS_SHARED = 1,
                    SHARE_VERSION = x.SHARE_VERSION + 1
                }).Where(x => x.ID == planId && x.STATE == "A").ExecuteCommand();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}