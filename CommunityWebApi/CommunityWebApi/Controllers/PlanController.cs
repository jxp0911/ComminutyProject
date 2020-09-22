using CommunityWebApi.Common;
using CommunityWebApi.Domains;
using CommunityWebApi.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommunityWebApi.Controllers
{
    public class PlanController : ApiController
    {
        [Route("bus/plan/post")]
        [HttpPost]
        public IHttpActionResult PostPlan([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                int IsShare = Convert.ToInt32(value.is_share);
                PlanHeaderModel planModel = JsonConvert.DeserializeObject<PlanHeaderModel>(Convert.ToString(value.plan_info));
                PlanDomain PD = new PlanDomain();
                result = PD.PostPlan(UserId, IsShare, planModel);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Plan", "PostPlan", "bus/plan/post", "发布计划接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "发布失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/plan/share")]
        [HttpPost]
        public IHttpActionResult SharePlan([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string PlanId = Convert.ToString(value.plan_id);
                PlanDomain PD = new PlanDomain();
                result = PD.SharePlan(UserId, PlanId);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Plan", "SharePlan", "bus/plan/share", "分享计划接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "分享失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/plan/sharecancel")]
        [HttpPost]
        public IHttpActionResult SharePlanCancel([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string PlanId = Convert.ToString(value.plan_id);
                PlanDomain PD = new PlanDomain();
                result = PD.SharePlanCancel(UserId, PlanId);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Plan", "SharePlanCancel", "bus/plan/sharecancel", "取消分享计划接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "取消失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/plan/update")]
        [HttpPost]
        public IHttpActionResult UpdatePlan([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                int IsShare = Convert.ToInt32(value.is_share);
                PlanHeaderModel planModel = JsonConvert.DeserializeObject<PlanHeaderModel>(Convert.ToString(value.plan_info));
                if (planModel.PLAN_DTL == null)
                {
                    throw new Exception("前台传的计划明细为空");
                }
                PlanDomain PD = new PlanDomain();
                result = PD.UpdatePlan(UserId, IsShare, planModel);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Plan", "UpdatePlan", "bus/plan/update", "修改计划接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "修改失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/plan/delete")]
        [HttpPost]
        public IHttpActionResult DeletePlan([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string PlanId = Convert.ToString(value.plan_id);
                PlanDomain PD = new PlanDomain();
                result = PD.DeletePlan(UserId, PlanId);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Plan", "DeletePlan", "bus/plan/delete", "删除计划接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "删除失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/plan/complete")]
        [HttpPost]
        public IHttpActionResult UpdateStatus([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string PlanId = Convert.ToString(value.plan_id);
                string PlanDtlId = Convert.ToString(value.plan_dtl_id);
                PlanDomain PD = new PlanDomain();
                result = PD.UpdateStatus(UserId, PlanId, PlanDtlId);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Plan", "UpdateStatus", "bus/plan/complete", "完成计划接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "请求失败，请重试";
                return Json(result);
            }
        }

        [Route("bus/plan/get")]
        [HttpGet]
        public IHttpActionResult Get(string user_id,string path_id, int cursor, int count)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                PlanDomain PD = new PlanDomain();
                result = PD.GetPlanByPath(user_id, path_id, cursor, count);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Plan", "Get", "bus/plan/get", "根基规划id获取计划", "用户ID：" + user_id + "已经获取的卡片数量:" + cursor + ";本次请求的卡片数量:" + count + "计划头ID" + path_id, ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }
        }

    }
}
