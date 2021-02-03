using CommunityWebApi.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityWebApi.Interface
{
    public interface ISingleBus
    {
        void Verify(string info);
        BusinessModel GetData(SqlSugarClient db, string info);
    }
}
