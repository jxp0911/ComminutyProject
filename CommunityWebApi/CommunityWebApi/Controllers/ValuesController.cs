using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using CommunityWebApi.Common;
using Entitys;

namespace CommunityWebApi.Controllers
{
    public class ValuesController : ApiController
    {
        // GET api/values
        public IHttpActionResult Get()
        {
            long aa = FunctionHelper.GetTimestamp();
            return Json("牛逼");
        }

        // GET api/values/5
        public string Get(int id,string qwe)
        {
            return "value" + qwe;
        }

        // POST api/values
        public void Post([FromBody]string value)
        {
        }

        // PUT api/values/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
