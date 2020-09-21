using CommunityWebApi.Domains;
using Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace CommunityWebApi.Controllers
{
    public class BackManageController : ApiController
    {
        [Route("back/feed/log")]
        [HttpGet]
        public IHttpActionResult GetFailLog()
        {
            try
            {
                BackManageDomain BD = new BackManageDomain();
                List<SYS_FAIL_LOG> data = BD.GetFailLog();
                return Json(data);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
