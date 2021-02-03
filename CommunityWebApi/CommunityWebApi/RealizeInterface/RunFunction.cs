using CommunityWebApi.Common;
using CommunityWebApi.Interface;
using CommunityWebApi.Models;
using Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.RealizeInterface
{
    /// <summary>
    /// 数据校验ID是否存在
    /// </summary>
    public class RunVerify
    {
        public void Run(string id, ISingleBus arg)
        {
            try
            {
                arg.Verify(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }

    /// <summary>
    /// 点赞通用类
    /// </summary>
    public class RunFavour
    {
        IGiveFavour GF { get; }

        /// <summary>
        /// 初始化时把更新的表类型传进来
        /// </summary>
        /// <param name="type"></param>
        public RunFavour(int type)
        {
            GF = InterfaceArray.DicGF[type];
        }
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
        public void Run(SqlSugarClient db, string userId, string typeId, int favourType, DateTime now, int timestamp)
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

                //保存通知
                MessageClass MC = new MessageClass();
                int msgType = (favourType == 1 || favourType == 2 || favourType == 3) ? 3 : 4;//3:对文章点赞；4:对评论点赞
                MC.SaveMsg(db, userId, typeId, msgType, favourType);
            }
            //修改相应业务表的点赞数量
            GF.UpdateCount(db, typeId, favourCount, now);
        }
    }
    /// <summary>
    /// 下发卡片通用类
    /// </summary>
    public class RunCard
    {
        public bool HasMore { get; set; }
        public List<NewFeedFirstReturnModel> Run(SqlSugarClient db, int cursor, int count, IFeed arg)
        {
            List<NewFeedFirstReturnModel> list = arg.GetFeedInfo(db, cursor, count);
            HasMore = arg.hasMore;
            return list;
        }
    }

    /// <summary>
    /// 下发一级二级评论通用类
    /// </summary>
    public class RunComment
    {
        public dynamic Run(string userId, string id, int cursor, int count, IComment arg)
        {
            dynamic data = arg.GetComment(userId, id, cursor, count);
            return data;
        }
    }

    public class RunMessage
    {
        public dynamic Run(string userId, string id, int cursor, int count, IComment arg)
        {
            dynamic data = arg.GetComment(userId, id, cursor, count);
            return data;
        }
    }
}