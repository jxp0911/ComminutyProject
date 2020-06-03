using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Common
{
    public class LogHelper
    {
        public static void writeFTInfo(string title, string errInfo, object obj,string user,DateTime time)
        {
            bool writeLog = true;
            if (writeLog == false) return;
            try
            {
                string content = JsonConvert.SerializeObject(obj);

                using (System.IO.FileStream fs = new System.IO.FileStream("F://logs//" + title + time.ToString("yyyy-MM-dd HH:mm:ss") + ".txt", System.IO.FileMode.Append, System.IO.FileAccess.Write))
                {
                    System.IO.StreamWriter sw = new System.IO.StreamWriter(fs);
                    sw.WriteLine("时间：" + time.ToString("yyyy-MM-dd HH:mm:ss") + "，用户：" + user);
                    sw.WriteLine("抛出异常：" + errInfo);
                    sw.WriteLine(content);
                    sw.Flush();
                }
            }
            finally
            {

            }
        }
    }
}