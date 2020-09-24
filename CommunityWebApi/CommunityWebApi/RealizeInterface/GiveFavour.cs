using CommunityWebApi.Interface;
using Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.RealizeInterface
{
    /// <summary>
    /// 点赞一级职业规划
    /// </summary>
    public class GiveFavourFirstPath : IGiveFavour
    {
        public void UpdateCount(SqlSugarClient db, string typeId, int favourCount, DateTime now)
        {
            db.Updateable<BUS_CAREERPATH_FIRST>().SetColumns(x => new BUS_CAREERPATH_FIRST()
            {
                DATETIME_MODIFIED = now,
                FAVOUR_COUNT = x.FAVOUR_COUNT + favourCount
            }).Where(x => x.ID == typeId && x.STATE == "A").ExecuteCommand();
        }
    }
    /// <summary>
    /// 点赞二级职业规划
    /// </summary>
    public class GiveFavourSecondPath : IGiveFavour
    {
        public void UpdateCount(SqlSugarClient db, string typeId, int favourCount, DateTime now)
        {
            db.Updateable<BUS_CAREERPATH_SECOND>().SetColumns(x => new BUS_CAREERPATH_SECOND()
            {
                DATETIME_MODIFIED = now,
                FAVOUR_COUNT = x.FAVOUR_COUNT + favourCount
            }).Where(x => x.ID == typeId && x.STATE == "A").ExecuteCommand();
        }
    }
    /// <summary>
    /// 点赞三级职业规划
    /// </summary>
    public class GiveFavourThirdPath : IGiveFavour
    {
        public void UpdateCount(SqlSugarClient db, string typeId, int favourCount, DateTime now)
        {
            db.Updateable<BUS_CAREERPATH_THIRD>().SetColumns(x => new BUS_CAREERPATH_THIRD()
            {
                DATETIME_MODIFIED = now,
                FAVOUR_COUNT = x.FAVOUR_COUNT + favourCount
            }).Where(x => x.ID == typeId && x.STATE == "A").ExecuteCommand();
        }
    }
    /// <summary>
    /// 点赞评论
    /// </summary>
    public class GiveFavourFirstComment : IGiveFavour
    {
        public void UpdateCount(SqlSugarClient db, string typeId, int favourCount, DateTime now)
        {
            db.Updateable<BUS_CAREERPATH_COMMENT>().SetColumns(x => new BUS_CAREERPATH_COMMENT()
            {
                DATETIME_MODIFIED = now,
                FAVOUR_COUNT = x.FAVOUR_COUNT + favourCount
            }).Where(x => x.ID == typeId && x.STATE == "A").ExecuteCommand();
        }
    }
    /// <summary>
    /// 点赞回复
    /// </summary>
    public class GiveFavourFirstReply : IGiveFavour
    {
        public void UpdateCount(SqlSugarClient db, string typeId, int favourCount, DateTime now)
        {
            db.Updateable<BUS_COMMENT_REPLY>().SetColumns(x => new BUS_COMMENT_REPLY()
            {
                DATETIME_MODIFIED = now,
                FAVOUR_COUNT = x.FAVOUR_COUNT + favourCount
            }).Where(x => x.ID == typeId && x.STATE == "A").ExecuteCommand();
        }
    }
    /// <summary>
    /// 点赞计划
    /// </summary>
    public class GiveFavourPlan : IGiveFavour
    {
        public void UpdateCount(SqlSugarClient db, string typeId, int favourCount, DateTime now)
        {
            db.Updateable<BUS_PLAN_HEADER>().SetColumns(x => new BUS_PLAN_HEADER()
            {
                DATETIME_MODIFIED = now,
                FAVOUR_COUNT = x.FAVOUR_COUNT + favourCount
            }).Where(x => x.ID == typeId && x.STATE == "A").ExecuteCommand();
        }
    }
}