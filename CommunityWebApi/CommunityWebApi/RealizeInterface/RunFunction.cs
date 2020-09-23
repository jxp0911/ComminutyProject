using CommunityWebApi.Interface;
using Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.RealizeInterface
{
    public class RunFunction
    {
        public void RunFavour(SqlSugarClient db, string userId, string typeId, int favourType, DateTime now,int timestamp, IGiveFavour arg)
        {
            var count = db.Queryable<BUS_USER_FAVOUR>()
                    .Where(x => x.USER_ID == userId && x.TYPE_ID == typeId && x.TYPE == favourType && x.STATE == "A")
                    .Count();
            int favourCount = 0;
            //判断当前用户是否点过赞
            if (count == 1)
            {
                db.Deleteable<BUS_USER_FAVOUR>().Where(x => x.USER_ID == userId && x.TYPE_ID == typeId && x.STATE == "A" && x.TYPE == favourType).ExecuteCommand();
                favourCount = -1;
            }
            else
            {
                BUS_USER_FAVOUR model = new BUS_USER_FAVOUR();
                model.ID = Guid.NewGuid().ToString();
                model.DATETIME_CREATED = now;
                model.STATE = "A";
                model.TIMESTAMP_INT = timestamp;
                model.USER_ID = userId;
                model.TYPE_ID = typeId;
                model.TYPE = favourType;
                db.Insertable(model).ExecuteCommand();

                favourCount = 1;
            }
            //修改相应业务表的点赞数量
            arg.UpdateCount(db, typeId, favourCount, now);
        }
    }
}