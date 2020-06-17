using Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace CommunityWebApi.Common
{
    public static class FunctionHelper
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

        /// <summary>
        /// 验证账号是否存在
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <param name="firstId">职业路径一级ID</param>
        /// <param name="pathId">某一级职业路径的ID</param>
        /// <param name="commentId">评论表ID</param>
        /// <param name="replyId">回复表ID</param>
        /// <returns></returns>
        public static Tuple<bool,string> Verify(string userId,string firstId = "",string pathId = "",string commentId="",string replyId="")
        {
            try
            {
                var db = DBContext.GetInstance;
                if (!string.IsNullOrEmpty(userId))
                {
                    var userCount = db.Queryable<SYS_USER_ACCOUNT>()
                    .Where(x => x.ID == userId && x.STATE == "A")
                    .Count();
                    if (userCount == 0)
                    {
                        return new Tuple<bool, string>(false,"账号异常");
                    }
                }
                if (!string.IsNullOrEmpty(firstId))
                {
                    var careerCount = db.Queryable<BUS_CAREERPATH_FIRST>()
                    .Where(x => x.ID == firstId && x.STATE == "A")
                    .Count();
                    if (careerCount == 0)
                    {
                        return new Tuple<bool, string>(false, "请求异常请刷新重新");
                    }
                }
                if (!string.IsNullOrEmpty(pathId))
                {
                    var careerCount = db.Queryable<BUS_CAREERPATH_THIRD>()
                    .Where(x => (x.ID == pathId || x.FIRST_ID == pathId || x.SECOND_ID == pathId))
                    .Count();
                    if (careerCount == 0)
                    {
                        return new Tuple<bool, string>(false, "请求异常请刷新重新");
                    }
                }
                if (!string.IsNullOrEmpty(commentId))
                {
                    var careerCount = db.Queryable<BUS_CAREERPATH_COMMENT>()
                    .Where(x => x.ID == commentId && x.STATE == "A")
                    .Count();
                    if (careerCount == 0)
                    {
                        return new Tuple<bool, string>(false, "请求异常请刷新重新");
                    }
                }
                if (!string.IsNullOrEmpty(replyId))
                {
                    var careerCount = db.Queryable<BUS_COMMENT_REPLY>()
                    .Where(x => x.ID == replyId && x.STATE == "A")
                    .Count();
                    if (careerCount == 0)
                    {
                        return new Tuple<bool, string>(false, "请求异常请刷新重新");
                    }
                }
                return new Tuple<bool, string>(true, "");
            }
            catch (Exception ex)
            {
                return new Tuple<bool, string>(false, "请重试");
            }
        }


        #region -------------------------DES加密解密-------------------------
        /// <summary>
        /// 8位字符的密钥字符串
        /// </summary>
        public static string StrKey = "FBEA4277";
        /// <summary>
        /// 8位字符的初始化向量字符串
        /// </summary>
        public static string StrIv = "151B8E9C";
        /// <summary>
        /// DES加密
        /// </summary>
        /// <param name="sourceString">加密数据</param> 
        /// <returns>值</returns>
        public static string Encrypt(this string sourceString)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(sourceString)) return "";
                byte[] btKey = Encoding.UTF8.GetBytes(StrKey);

                byte[] btIv = Encoding.UTF8.GetBytes(StrIv);

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                using (MemoryStream ms = new MemoryStream())
                {
                    byte[] inData = Encoding.UTF8.GetBytes(sourceString);
                    try
                    {
                        using (CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(btKey, btIv), CryptoStreamMode.Write))
                        {
                            cs.Write(inData, 0, inData.Length);

                            cs.FlushFinalBlock();
                        }

                        return Convert.ToBase64String(ms.ToArray());
                    }
                    catch (Exception)
                    {
                        return sourceString;
                    }
                }
            }
            catch
            {
                return "DES加密出错";
            }
        }

        /// <summary>
        /// DES解密
        /// </summary>
        /// <param name="encryptedString">解密数据</param>
        /// <returns>值</returns>
        public static string Decrypt(this string encryptedString)
        {
            var btKey = Encoding.UTF8.GetBytes(StrKey);

            var btIv = Encoding.UTF8.GetBytes(StrIv);

            var des = new DESCryptoServiceProvider();

            using (MemoryStream ms = new MemoryStream())
            {
                var inData = Convert.FromBase64String(encryptedString);
                try
                {
                    using (var cs = new CryptoStream(ms, des.CreateDecryptor(btKey, btIv), CryptoStreamMode.Write))
                    {
                        cs.Write(inData, 0, inData.Length);

                        cs.FlushFinalBlock();
                    }

                    return Encoding.UTF8.GetString(ms.ToArray());
                }
                catch
                {
                    return "解密错误";
                }
            }
        }
        #endregion

    }
}