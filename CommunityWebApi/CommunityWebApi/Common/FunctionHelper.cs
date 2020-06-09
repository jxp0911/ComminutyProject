using Entitys;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CommunityWebApi.Common
{
    public class FunctionHelper
    {
        /// <summary>
        /// 获取服务器当前时间戳
        /// </summary>
        /// <returns></returns>
        public static int GetTimestamp()
        {
            try
            {
                var db = DBContext.GetInstance;
                string sql = "select unix_timestamp(now())";
                int timestamp = db.Ado.SqlQuerySingle<int>(sql);
                return timestamp;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }

        /// <summary>
        /// 记录异常日志
        /// </summary>
        /// <param name="cotrollerName">控制器名称</param>
        /// <param name="funName">方法名称</param>
        /// <param name="url">URL</param>
        /// <param name="desc">接口描述</param>
        /// <param name="data">接收到的报文</param>
        /// <param name="failMsg">异常信息</param>
        /// <param name="reqType">请求方式</param>
        public static void SaveFailLog(string cotrollerName,string funName,string url,string desc,string data,string failMsg,string reqType)
        {
            try
            {
                var db = DBContext.GetInstance;
                DateTime now = db.GetDate();
                SYS_FAIL_LOG failLog = new SYS_FAIL_LOG();
                failLog.ID = Guid.NewGuid().ToString();
                failLog.DATETIME_CREATED = now;
                failLog.STATE = "A";
                failLog.TIMESTAMP_INT = GetTimestamp();
                failLog.CONTROLLER_NAME = cotrollerName;
                failLog.FUN_NAME = funName;
                failLog.URL = url;
                failLog.INTERFACE_DESC = desc;
                failLog.RECEIVE_DATA = data;
                failLog.FAIL_MSG = failMsg;
                failLog.REQUEST_TYPE = reqType;

                db.Insertable(failLog).ExecuteCommand();
            }
            catch (Exception ex)
            {
            }
        }
    }
}