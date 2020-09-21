using Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Domains
{
    public class BackManageDomain
    {
        public List<SYS_FAIL_LOG> GetFailLog()
        {
            try
            {
                var db = DBContext.GetInstance;
                var data = db.Queryable<SYS_FAIL_LOG>()
                    .OrderBy(x => x.DATETIME_CREATED, OrderByType.Desc)
                    .ToList();

                return data;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}