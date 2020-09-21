using Entitys;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using SqlSugar;

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
        public static bool VerifyInfo(SqlSugarClient db, string info, string type)
        {
            try
            {
                VerifyHelper varify = new VerifyHelper();
                return varify.VerifyInfo(db, info, type);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        ///<summary>
        ///生成随机字符串 
        ///</summary>
        ///<param name="length">目标字符串的长度</param>
        ///<param name="useNum">是否包含数字，1=包含，默认为包含</param>
        ///<param name="useLow">是否包含小写字母，1=包含，默认为包含</param>
        ///<param name="useUpp">是否包含大写字母，1=包含，默认为包含</param>
        ///<param name="useSpe">是否包含特殊字符，1=包含，默认为不包含</param>
        ///<param name="custom">要包含的自定义字符，直接输入要包含的字符列表</param>
        ///<returns>指定长度的随机字符串</returns>
        public static string GetRandomString(int length, bool useNum, bool useLow, bool useUpp, bool useSpe, string custom)
        {
            byte[] b = new byte[4];
            new System.Security.Cryptography.RNGCryptoServiceProvider().GetBytes(b);
            Random r = new Random(BitConverter.ToInt32(b, 0));
            string s = null, str = custom;
            if (useNum == true) { str += "0123456789"; }
            if (useLow == true) { str += "abcdefghijklmnopqrstuvwxyz"; }
            if (useUpp == true) { str += "ABCDEFGHIJKLMNOPQRSTUVWXYZ"; }
            if (useSpe == true) { str += "!\"#$%&'()*+,-./:;<=>?@[\\]^_`{|}~"; }
            for (int i = 0; i < length; i++)
            {
                s += str.Substring(r.Next(0, str.Length - 1), 1);
            }
            return s;
        }

        /// <summary>
        /// 评论接口favour_type传过来的code对应的评论内容类型
        /// </summary>
        /// <param name="code">区分哪张表的ID(1:一级职业规划表;2:二级职业规划表;3:三级职业规划表;4:一级评论表;5:二级评论表;6:修改职业规划表;)</param>
        /// <returns></returns>
        public static string GetDescByCode(int code)
        {
            if (code == 1)
            {
                return "FIRST_PATH";
            }
            if (code == 2)
            {
                return "SECOND_PATH";
            }
            if (code == 3)
            {
                return "THIRD_PATH";
            }
            if (code == 4)
            {
                return "COMMENT";
            }
            if (code == 5)
            {
                return "REPLY";
            }
            if (code == 6)
            {
                return "MODIFY";
            }

            return "";
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