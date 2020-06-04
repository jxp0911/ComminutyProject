using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityWebApi.Common;
using CommunityWebApi.Models;
using Entitys;
using Newtonsoft.Json;

namespace CommunityWebApi.Domains
{
    public class FeedDomain
    {
        public object FeedPath(string userId, FeedPathFirstModel feedModel)
        {
            var db = DBContext.GetInstance;
            RetJsonModel jsonModel = new RetJsonModel();
            jsonModel.time= FunctionHelper.GetTimestamp(); 
            DateTime now = db.GetDate();
            try
            {
                db.Ado.BeginTran();
                BUS_CAREERPATH_FIRST first = new BUS_CAREERPATH_FIRST();
                first.ID = Guid.NewGuid().ToString();
                first.DATETIME_CREATED = now;
                first.STATE = "A";
                first.TITLE = feedModel.Title;
                first.USER_ID = userId;
                db.Insertable(first).ExecuteCommand();

                foreach (var item in feedModel.SecondList)
                {
                    BUS_CAREERPATH_SECOND second = new BUS_CAREERPATH_SECOND();
                    second.ID = Guid.NewGuid().ToString();
                    second.DATETIME_CREATED = now;
                    second.STATE = "A";
                    second.TITLE = item.Title;
                    second.FIRST_ID = first.ID;
                    second.USER_ID = userId;
                    db.Insertable(second).ExecuteCommand();

                    foreach(var item2 in item.ThirdList)
                    {
                        BUS_CAREERPATH_THIRD third = new BUS_CAREERPATH_THIRD();
                        third.ID = Guid.NewGuid().ToString();
                        third.DATETIME_CREATED = now;
                        third.STATE = "A";
                        third.TITLE = item2.Title;
                        third.FIRST_ID = first.ID;
                        third.SECOND_ID = second.ID;
                        third.USER_ID = userId;
                        db.Insertable(third).ExecuteCommand();
                    }
                }
                db.Ado.CommitTran();

                jsonModel.status = "1";
                jsonModel.msg = "提交成功";
            }
            catch (Exception)
            {
                db.Ado.RollbackTran();
                jsonModel.status = "0";
                jsonModel.msg = "提交失败";
            }
            return jsonModel;
        }
    }
}