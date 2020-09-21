using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SqlSugar;

namespace CommunityWebApi.Common
{
    interface VerifyInterface
    {
        bool VerifyInfo(SqlSugarClient db, string info,string type);
    }
}
