using CommunityWebApi.Interface;
using CommunityWebApi.RealizeInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Common
{
    public static class InterfaceArray
    {
        //存储所有点赞实例化的字典
        public static Dictionary<int, IGiveFavour> DicGF = new Dictionary<int, IGiveFavour>();
        static InterfaceArray()
        {
            DicGF.Add(1, new GiveFavourFirstPath());//一级职业规划
            DicGF.Add(2, new GiveFavourSecondPath());//二级职业规划
            DicGF.Add(3, new GiveFavourThirdPath());//三级职业规划
            DicGF.Add(4, new GiveFavourFirstComment());//评论
            DicGF.Add(5, new GiveFavourFirstReply());//回复
            DicGF.Add(6, new GiveFavourPlan());//计划
        }
    }
}