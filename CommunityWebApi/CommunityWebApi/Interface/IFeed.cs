using CommunityWebApi.Models;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityWebApi.Interface
{
    public interface IFeed
    {
        string userId { get; }
        string topicId { get; }
        string faqId { get; }
        int status { get; }
        bool isOwn { get; }
        bool hasMore { get; set; }
        List<NewFeedFirstReturnModel> GetFeedInfo(SqlSugarClient db, int cursor, int count);
        void HasMore(SqlSugarClient db, int cursor, int listCount);
    }
}
