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
    public class FeedController : ApiController
    {
        [Route("bus/feed/save")]
        [HttpPost]
        public IHttpActionResult PostFeed([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                FeedPathFirstModel feedModel = JsonConvert.DeserializeObject<FeedPathFirstModel>(Convert.ToString(value.feed_info));
                FeedDomain FD = new FeedDomain();
                result = FD.FeedPath(UserId, feedModel);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "PostFeed", "bus/feed/save", "发文接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "提交失败";
                return Json(result);
            }

        }

        [Route("bus/feed/getfeed")]
        [HttpGet]
        public IHttpActionResult Get(int cursor, int count,int status)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                FeedDomain FD = new FeedDomain();
                result = FD.GetFeedPath(cursor, count, status);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "Get", "bus/feed/getfeed", "获取Feed信息", "已经获取的卡片数量:" + cursor + ";本次请求的卡片数量:" + count + "请求的feed状态" + status, ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "失败";
                return Json(result);
            }

        }


        [Route("bus/feed/aduit")]
        [HttpPost]
        public IHttpActionResult PostAduit([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                int isPass = Convert.ToInt32(value.is_pass);
                string userId = Convert.ToString(value.user_id);
                List<string> firstId = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(value.first_id));
                FeedDomain FD = new FeedDomain();
                result = FD.AuditPath(firstId, isPass, userId);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Feed", "PostAduit", "bus/feed/aduit", "审批职业路径接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "失败，请重试";
                return Json(result);
            }

        }
    }
}
