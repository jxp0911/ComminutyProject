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
            string UserId = Convert.ToString(value.user_id);
            FeedPathFirstModel feedModel = JsonConvert.DeserializeObject<FeedPathFirstModel>(Convert.ToString(value.feed_info));
            FeedDomain FD = new FeedDomain();
            var result = FD.FeedPath(UserId, feedModel);
            return Json(result);
        }

        [Route("bus/feed/getfeed")]
        [HttpGet]
        public IHttpActionResult Get(int cursor, int count)
        {
            FeedDomain FD = new FeedDomain();
            var result = FD.GetFeedPath(cursor, count);
            return Json(result);
        }
    }
}
