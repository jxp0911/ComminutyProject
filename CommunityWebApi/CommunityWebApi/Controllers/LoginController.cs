using CommunityWebApi.Domains;
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
            string UserId = Convert.ToString(value.user_id);
            string PassWord = Convert.ToString(value.password);
            LoginDomain LD = new LoginDomain();
            var result = LD.Register(UserId, PassWord);
            return Json(result);
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
            string UserId = Convert.ToString(value.user_id);
            string PassWord = Convert.ToString(value.password);
            LoginDomain LD = new LoginDomain();
            var result = LD.Login(UserId, PassWord);
            return Json(result);
        }
    }
}
