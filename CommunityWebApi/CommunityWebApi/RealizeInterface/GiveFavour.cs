using CommunityWebApi.Interface;
using Entitys;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.RealizeInterface
{
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
}