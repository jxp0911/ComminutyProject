using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Common
{
    public class FunctionHelper
    {
        /// <summary>
        /// 获取服务器当前时间戳
        /// </summary>
        /// <returns></returns>
        public static long GetTimestamp()
        {
            try
            {
                var db = DBContext.GetInstance;
                string sql = "select unix_timestamp(now())";
                long timestamp = db.Ado.SqlQuerySingle<long>(sql);
                return timestamp;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}