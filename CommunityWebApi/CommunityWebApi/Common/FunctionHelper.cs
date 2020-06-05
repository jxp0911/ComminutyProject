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
        public static int GetTimestamp()
        {
            try
            {
                var db = DBContext.GetInstance;
                string sql = "select unix_timestamp(now())";
                int timestamp = db.Ado.SqlQuerySingle<int>(sql);
                return timestamp;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
    }
}