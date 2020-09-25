using CommunityWebApi.Common;
using CommunityWebApi.Domains;
using CommunityWebApi.Models;
using CommunityWebApi.RealizeInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace CommunityWebApi.Controllers
{
    public class LoginController : ApiController
    {
        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/login/regist")]
        [HttpPost]
        public IHttpActionResult PostRegist([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string PassWord = Convert.ToString(value.password);
                LoginDomain LD = new LoginDomain();
                result = LD.Register(UserId, PassWord);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录错误日志
                FunctionHelper.SaveFailLog("Login", "PostRegist", "api/login/regist", "注册接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "注册失败，请重试";
                return Json(result);
            }
        }

        /// <summary>
        /// 登录
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/login/login")]
        [HttpPost]
        public IHttpActionResult PostLogin([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                string PassWord = Convert.ToString(value.password);
                LoginDomain LD = new LoginDomain();
                result = LD.Login(UserId, PassWord);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录错误日志
                FunctionHelper.SaveFailLog("Login", "PostLogin", "api/login/login", "登录接口", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.time = FunctionHelper.GetTimestamp();
                result.msg = "登录失败，请重试";
                return Json(result);
            }
        }

        /// <summary>
        /// 验证账号登录状态
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/login/status")]
        [HttpPost]
        public IHttpActionResult PostIsLogin([FromBody]dynamic value)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                string UserId = Convert.ToString(value.user_id);
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());

                LoginDomain LD = new LoginDomain();
                result = LD.IsLogin(UserId);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Login", "PostIsLogin", "api/login/status", "验证账号是否处在登陆中", Convert.ToString(value), ex.Message.ToString(), "POST");

                result.status = 0;
                result.msg = "数据异常，请重试";
                result.time = FunctionHelper.GetTimestamp();
                result.data = false;
                return Json(result);
            }

        }

        /// <summary>
        /// 获取账号所有权限
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        [Route("api/login/per")]
        [HttpGet]
        public IHttpActionResult GetPermission(string user_id)
        {
            RetJsonModel result = new RetJsonModel();
            try
            {
                LoginDomain LD = new LoginDomain();
                //数据校验
                RunVerify VD = new RunVerify();
                VD.Run(UserId, new VerifyUser());

                result = LD.GetRolePermission(user_id);
                return Json(result);
            }
            catch (Exception ex)
            {
                //记录失败日志
                FunctionHelper.SaveFailLog("Login", "GetPermission", "api/login/per", "获取当前账号拥有的权限", "当前登录用户ID：" + user_id, ex.Message.ToString(), "POST");

                result.status = 0;
                result.msg = "数据异常，请重试";
                result.time = FunctionHelper.GetTimestamp();
                result.data = new List<string>();
                return Json(result);
            }

        }
    }
}
