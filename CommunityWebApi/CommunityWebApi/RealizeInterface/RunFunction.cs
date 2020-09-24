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
        /// <summary>
        /// 点赞通用方法
        /// </summary>
        /// <param name="db"></param>
        /// <param name="userId"></param>
        /// <param name="typeId">业务表ID</param>
        /// <param name="favourType">点赞类型(1:一级规划；2：二级规划；3：三级规划；4：评论；5：回复；6：计划)</param>
        /// <param name="now"></param>
        /// <param name="timestamp"></param>
        /// <param name="arg">接口的实现类</param>
        public void RunFavour(SqlSugarClient db, string userId, string typeId, int favourType, DateTime now,int timestamp, IGiveFavour arg)
        {
            var count = db.Queryable<BUS_USER_FAVOUR>()
                    .Where(x => x.USER_ID == userId && x.TYPE_ID == typeId && x.TYPE == favourType && x.STATE == "A")
                    .Count();
            int favourCount = 0;
            //判断当前用户是否点过赞，修改用户点赞表
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