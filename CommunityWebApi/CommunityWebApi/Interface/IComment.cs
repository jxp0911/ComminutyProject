using CommunityWebApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunityWebApi.Interface
{
    public interface IComment
    {
        dynamic GetComment(string userId, string id, int cursor, int count);
    }
}
