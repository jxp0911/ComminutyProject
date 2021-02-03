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
    /// 获取一级评论
    /// </summary>
    public class CommentClass : IComment
    {
        public dynamic GetComment(string userId, string pathId, int cursor, int count)
        {
            try
            {
                var db = DBContext.GetInstance;
                List<CommentReturnModel> CommentData = db.Queryable<BUS_CAREERPATH_COMMENT, SYS_USER_INFO, BUS_USER_FAVOUR>((a, b, c) => new object[]{
                    JoinType.Left,a.FROM_UID==b.USER_ID && a.STATE==b.STATE,
                    JoinType.Left,a.ID==c.TYPE_ID && a.STATE==c.STATE && c.USER_ID==userId
                }).Where((a, b, c) => a.PATH_ID == pathId && a.STATE == "A")
                .OrderBy((a, b) => a.FAVOUR_COUNT, OrderByType.Desc)
                .Select((a, b, c) => new CommentReturnModel
                {
                    ID = a.ID,
                    CONTENT = a.CONTENT,
                    PUBLISH_TIME = a.TIMESTAMP_INT,
                    PARENT_ID = a.PATH_ID,
                    USER_ID = a.FROM_UID,
                    NICK_NAME = b.NICK_NAME,
                    FAVOUR_COUNT = a.FAVOUR_COUNT,
                    IS_FAVOUR = c.ID != null && c.ID != ""
                }).Skip(cursor).Take(count).ToList();

                foreach(var item in CommentData)
                {
                    //判断一级评论下有几个二级评论
                    int replyCount = db.Queryable<BUS_COMMENT_REPLY>()
                                .Where(x => x.COMMENT_ID == item.ID && x.STATE == "A")
                                .Count();
                    if (replyCount == 0) continue;
                    List<ReplyReturnModel> ReplyData = db.Queryable<BUS_COMMENT_REPLY, SYS_USER_INFO, BUS_USER_FAVOUR>((a, b, c) => new object[]
                    {
                        JoinType.Left,a.FROM_UID==b.USER_ID && a.STATE==b.STATE,
                        JoinType.Left,a.ID==c.TYPE_ID&&a.STATE==c.STATE&&c.USER_ID==userId
                    }).Where((a, b, c) => a.COMMENT_ID == item.ID && a.STATE == "A")
                    .OrderBy((a, b) => a.FAVOUR_COUNT, OrderByType.Desc)
                    .Select((a, b, c) => new ReplyReturnModel
                    {
                        ID = a.ID,
                        CONTENT = a.CONTENT,
                        PUBLISH_TIME = a.TIMESTAMP_INT,
                        COMMENT_ID = a.COMMENT_ID,
                        PARENT_ID = a.REPLY_ID,
                        USER_ID = a.FROM_UID,
                        NICK_NAME = b.NICK_NAME,
                        FAVOUR_COUNT = a.FAVOUR_COUNT,
                        IS_FAVOUR = c.ID != null && c.ID != ""
                    }).Take(1).ToList();
                    //判断是否还有更多
                    bool replyHasMore = replyCount > 1 ? true : false;
                    item.ReplyInfo = new
                    {
                        reply_list = ReplyData,
                        has_more = replyHasMore
                    };
                }

                int allCount = db.Queryable<BUS_CAREERPATH_COMMENT>()
                    .Where(x => x.PATH_ID == pathId && x.STATE == "A")
                    .Count();
                bool hasmore = false;
                if(allCount> CommentData.Count + cursor)
                {
                    hasmore = true;
                }

                return new { list = CommentData, has_more = hasmore };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
    /// <summary>
    /// 获取二级评论
    /// </summary>
    public class ReplyClass : IComment
    {
        private int ReplyCode;
        public ReplyClass(int code)
        {
            ReplyCode = code;
        }
        /// <summary>
        /// ReplyCode为2时，commentId为一级评论ID；ReplyCode为3时，commentId为递归的二级评论
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="commentId"></param>
        /// <param name="cursor"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public dynamic GetComment(string userId, string commentId, int cursor, int count)
        {
            try
            {
                var db = DBContext.GetInstance;
                List<CommentReturnModel> ReplyData = db.Queryable<BUS_COMMENT_REPLY, SYS_USER_INFO, BUS_USER_FAVOUR>((a, b, c) => new object[]{
                    JoinType.Left,a.FROM_UID==b.USER_ID && a.STATE==b.STATE,
                    JoinType.Left,a.ID==c.TYPE_ID && a.STATE==c.STATE && c.USER_ID==userId
                }).Where((a, b, c) => a.STATE == "A")
                .WhereIF(ReplyCode == 2, (a, b, c) => a.COMMENT_ID == commentId)
                .WhereIF(ReplyCode == 3, (a, b, c) => a.REPLY_ID == commentId)
                .OrderBy((a, b) => a.FAVOUR_COUNT, OrderByType.Desc)
                .Select((a, b, c) => new CommentReturnModel
                {
                    ID = a.ID,
                    CONTENT = a.CONTENT,
                    PUBLISH_TIME = a.TIMESTAMP_INT,
                    PARENT_ID = a.REPLY_ID,
                    USER_ID = a.FROM_UID,
                    NICK_NAME = b.NICK_NAME,
                    FAVOUR_COUNT = a.FAVOUR_COUNT,
                    IS_FAVOUR = c.ID != null && c.ID != ""
                }).Skip(cursor).Take(count).ToList();

                foreach(var item in ReplyData)
                {
                    //判断二级评论下有几个评论
                    int replyCount = db.Queryable<BUS_COMMENT_REPLY>()
                                .Where(x => x.REPLY_ID == item.ID && x.STATE == "A")
                                .Count();
                    if (replyCount == 0) continue;

                    List<ReplyReturnModel> SecondReplyData = db.Queryable<BUS_COMMENT_REPLY, SYS_USER_INFO, BUS_USER_FAVOUR>((a, b, c) => new object[]
                    {
                        JoinType.Left,a.FROM_UID==b.USER_ID && a.STATE==b.STATE,
                        JoinType.Left,a.ID==c.TYPE_ID&&a.STATE==c.STATE&&c.USER_ID==userId
                    }).Where((a, b, c) => a.REPLY_ID == item.ID && a.STATE == "A")
                    .OrderBy((a, b) => a.FAVOUR_COUNT, OrderByType.Desc)
                    .Select((a, b, c) => new ReplyReturnModel
                    {
                        ID = a.ID,
                        CONTENT = a.CONTENT,
                        PUBLISH_TIME = a.TIMESTAMP_INT,
                        COMMENT_ID = a.COMMENT_ID,
                        PARENT_ID = a.REPLY_ID,
                        USER_ID = a.FROM_UID,
                        NICK_NAME = b.NICK_NAME,
                        FAVOUR_COUNT = a.FAVOUR_COUNT,
                        IS_FAVOUR = c.ID != null && c.ID != ""
                    }).Take(1).ToList();
                    //判断是否还有更多
                    bool replyHasMore = replyCount > 1 ? true : false;
                    item.ReplyInfo = new
                    {
                        reply_list = SecondReplyData,
                        has_more = replyHasMore
                    };
                }

                int allCount = db.Queryable<BUS_COMMENT_REPLY>()
                    .Where(x => x.STATE == "A")
                    .WhereIF(ReplyCode == 2, a => a.COMMENT_ID == commentId)
                    .WhereIF(ReplyCode == 3, a => a.REPLY_ID == commentId)
                    .Count();
                bool hasmore = false;
                if (allCount>cursor+ ReplyData.Count)
                {
                    hasmore = true;
                }

                return new { list = ReplyData, has_more = hasmore };
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}