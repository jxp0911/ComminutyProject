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
        public static IGiveFavour[] ListGF =
        {
            new GiveFavourFirstPath(),
            new GiveFavourSecondPath(),
            new GiveFavourThirdPath(),
            new GiveFavourFirstComment(),
            new GiveFavourFirstReply()
        };
    }
}