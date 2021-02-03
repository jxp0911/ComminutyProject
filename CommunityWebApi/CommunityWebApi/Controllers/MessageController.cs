using CommunityWebApi.Common;
using CommunityWebApi.Domains;
using CommunityWebApi.Models;
using CommunityWebApi.RealizeInterface;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommunityWebApi.Controllers
{
    public class MessageController : ApiController
    {
        [Route("api/message/getunread")]
        [HttpGet]
        public IHttpActionResult GetUnreadMessage(string userid)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(userid, new VerifyUser());

                MessageDomain MD = new MessageDomain();
                result = MD.UnreadMessage(userid);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Message", "GetUnreadMessage", "api/message/getunread", "获取用户未读通知数量", $"用户ID：{userid}", ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }
        }

        [Route("api/message/getmsg")]
        [HttpGet]
        public IHttpActionResult GetMessageInfo(string userid, int cursor, int count)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(userid, new VerifyUser());

                MessageDomain MD = new MessageDomain();
                result = MD.GetMessageInfo(userid, cursor, count);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Message", "GetMessageInfo", "api/message/getmsg", "获取用户收到的通知", $"用户ID：{userid}；已下发数：{cursor}；本次请求数：{count}", ex.Message.ToString(), "GET");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "数据异常，请重试";
                return Json(result);
            }
        }

        /// <summary>
        /// 阅读通知
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/message/read")]
        [HttpPost]
        public IHttpActionResult ReadMsg([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                List<string> msgIds = JsonConvert.DeserializeObject<List<string>>(Convert.ToString(value.msg_ids));

                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());

                MessageDomain MD = new MessageDomain();
                result = MD.ReadMsg(UserId, msgIds);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Message", "ReadMsg", "api/message/read", "阅读通知接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "请求失败,请重试";
                return Json(result);
            }
        }
    }
}
