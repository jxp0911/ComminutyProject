using CommunityWebApi.Common;
using CommunityWebApi.Models;
using CommunityWebApi.Models.ReturnModels;
using CommunityWebApi.RealizeInterface;
using Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Domains
{
    public class MessageDomain
    {
        /// <summary>
        /// 获取当前用户未读通知数
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RetJsonModel UnreadMessage(string userId)
        {
            try
            {
                var db = DBContext.GetInstance;
                var count = db.Queryable<SYS_MESSAGE_INFO>()
                    .Where(x => x.TO_UID == userId && x.STATUS == 0 && x.STATE == "A")
                    .Count();
                int timestamp = FunctionHelper.GetTimestamp();

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = timestamp;
                jsonModel.status = 1;
                jsonModel.data = count;
                jsonModel.msg = "成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 获取当前用户的通知详情
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public RetJsonModel GetMessageInfo(string userId, int cursor, int count)
        {
            try
            {
                var db = DBContext.GetInstance;
                MessageClass MC = new MessageClass();
                dynamic data = MC.GetMsgInfo(userId, cursor, count);

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();
                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = data;
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public RetJsonModel ReadMsg(string userId,List<string> msgIds)
        {
            try
            {
                var db = DBContext.GetInstance;
                MessageClass MC = new MessageClass();
                MC.UpdateMsg(db, msgIds);

                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();
                jsonModel.status = 1;
                jsonModel.msg = "成功";
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}