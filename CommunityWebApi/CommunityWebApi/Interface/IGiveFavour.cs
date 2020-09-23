using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityWebApi.Interface
{
    /// <summary>
    /// 点赞接口
    /// </summary>
    public interface IGiveFavour
    {
        void UpdateCount(SqlSugarClient db, string typeId, int favourCount, DateTime now);
    }
}
