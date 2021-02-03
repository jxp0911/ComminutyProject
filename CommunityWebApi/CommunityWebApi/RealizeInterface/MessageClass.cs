using CommunityWebApi.Common;
using CommunityWebApi.Interface;
using CommunityWebApi.Models;
using CommunityWebApi.Models.ReturnModels;
using Entitys;
using SqlSugar;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Web;

namespace CommunityWebApi.RealizeInterface
{
    public class MessageClass : Interface.IMessage
    {
        public dynamic GetMsgInfo(string userid, int cursor, int count)
        {
            try
            {
                var db = DBContext.GetInstance;
                var data = db.Queryable<SYS_MESSAGE_INFO, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.FROM_UID==b.USER_ID&&a.STATE==b.STATE
                }).Where((a, b) => a.TO_UID == userid && a.STATUS == 0 && a.STATE == "A")
                .Select((a, b) => new MsgReturnModel
                {
                    ID = a.ID,
                    CONTENT = a.CONTENT,
                    USER_ID = a.FROM_UID,
                    NICK_NAME = b.NICK_NAME
                }).Skip(cursor).Take(count).ToList();

                //判断是否还有更多职业规划
                var allCount = db.Queryable<SYS_MESSAGE_INFO>()
                    .Where(x => x.TO_UID == userid && x.STATUS == 0 && x.STATE == "A")
                    .Count();
                bool hasmore = true;
                if (data.Count + cursor >= allCount)
                {
                    hasmore = false;
                }

                return new { list = data, has_more = hasmore };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 保存通知信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="fromUid">发起通知的人</param>
        /// <param name="busId">内容ID</param>
        /// <param name="msgType">通知类型</param>
        /// <param name="contentType">内容类型(1:一级规划;2:二级规划;3:三级规划;4:一级评论;5:二级评论;6:修改的职业规划;7:计划)</param>
        /// <param name="content2">通知内容</param>
        public void SaveMsg(SqlSugarClient db, string fromUid, string busId, int msgType, int busType, string content2 = "")
        {
            try
            {
                DateTime now = db.GetDate();
                int timestamp = FunctionHelper.GetTimestamp();

                string fromUser = db.Queryable<SYS_USER_INFO>()
                    .Where(x => x.USER_ID == fromUid && x.STATE == "A")
                    .Select(x => x.NICK_NAME)
                    .First();

                string msgInfo = "";
                ISingleBus sb = InterfaceArray.DicVD[busType];
                BusinessModel toInfo = this.GetBusData(db, busId, sb);
                string toUid = toInfo.USER_ID;
                string oldContent = toInfo.CONTENT;
                if (msgType == 1)
                {
                    msgInfo = fromUser + "对你的文章" + oldContent + "进行评论：" + content2;
                }
                if (msgType == 2)
                {
                    msgInfo = fromUser + "对你的评论" + oldContent + "进行回复：" + content2;
                }
                if (msgType == 3)
                {
                    msgInfo = fromUser + "赞了你的文章" + oldContent;
                }
                if (msgType == 4)
                {
                    msgInfo = fromUser + "赞了你的评论" + oldContent;
                }

                SYS_MESSAGE_INFO smi = new SYS_MESSAGE_INFO();
                smi.ID = Guid.NewGuid().ToString();
                smi.DATETIME_CREATED = now;
                smi.STATE = "A";
                smi.TIMESTAMP_INT = timestamp;
                smi.FROM_UID = fromUid;
                smi.TO_UID = toUid;
                smi.STATUS = 0;
                smi.TYPE = msgType;
                smi.CONTENT = msgInfo;

                db.Insertable(smi).ExecuteCommand();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void UpdateMsg(SqlSugarClient db, List<string> msgIds)
        {
            try
            {
                db.Updateable<SYS_MESSAGE_INFO>()
                    .SetColumns(x => new SYS_MESSAGE_INFO()
                    {
                        STATUS = 1
                    })
                    .Where(x => msgIds.Contains(x.ID) && x.STATE == "A")
                    .ExecuteCommand();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        /// <summary>
        /// 获取业务表的信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="busId"></param>
        /// <param name="arg"></param>
        /// <returns></returns>
        private BusinessModel GetBusData(SqlSugarClient db, string busId, ISingleBus arg)
        {
            var data = arg.GetData(db, busId);
            return data;
        }
    }
}