using CommunityWebApi.Models.ReturnModels;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityWebApi.Interface
{
    public interface IMessage
    {
        dynamic GetMsgInfo(string userid, int cursor, int count);
        /// <summary>
        /// 保存通知信息
        /// </summary>
        /// <param name="db"></param>
        /// <param name="fromUid">发起通知的人</param>
        /// <param name="busId">内容ID</param>
        /// <param name="msgType">通知类型(1:对文章发起评论;2:对评论发起回复;3:对文章点赞;4:对评论点赞)</param>
        /// <param name="contentType">内容类型(1:一级规划;2:二级规划;3:三级规划;4:一级评论;5:二级评论;6:修改的职业规划;7:计划)</param>
        /// <param name="content2">通知内容</param>
        void SaveMsg(SqlSugarClient db, string fromUid, string busId, int msgType, int contentType, string content2 = "");

        void UpdateMsg(SqlSugarClient db, List<string> msgIds);
    }
}
