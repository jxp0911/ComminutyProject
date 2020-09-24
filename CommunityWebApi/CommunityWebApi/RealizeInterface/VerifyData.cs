using CommunityWebApi.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.RealizeInterface
{
    public class VerifyFirstPath : IVerifyData
    {
        public bool Verify(string info)
        {
            Type aa = info.GetType();
        }
    }
}