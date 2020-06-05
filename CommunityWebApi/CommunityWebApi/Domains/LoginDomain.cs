using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityWebApi.Common;
using CommunityWebApi.Models;
using Entitys;
using Newtonsoft.Json;

namespace CommunityWebApi.Domains
{
    public class LoginDomain
    {
        /// <summary>
        /// 注册方法
        /// </summary>
        /// <param name="userId">账号</param>
        /// <param name="passWord">密码</param>
        public object Register(string userId,string passWord)
        {
            var db = DBContext.GetInstance;
            DateTime now = db.GetDate();
            //返给前台的JSON实体
            RetJsonModel jsonModel = new RetJsonModel();
            int timestamp = FunctionHelper.GetTimestamp();
            jsonModel.time = timestamp;
            try
            {
                var data = db.Queryable<SYS_USER_ACCOUNT>()
                    .Where(x => x.ACCOUNT_NUMBER == userId)
                    .First();
                if (data == null)
                {
                    SYS_USER_ACCOUNT model = new SYS_USER_ACCOUNT();
                    model.ID = System.Guid.NewGuid().ToString();
                    model.DATETIME_CREATED = now;
                    model.STATE = "A";
                    model.ACCOUNT_NUMBER = userId;
                    model.PASSWORD = passWord;
                    model.TIMESTAMP_INT = timestamp;

                    db.Insertable(model).ExecuteCommand();

                    LoginReturnModel lrm = new LoginReturnModel();
                    lrm.user_info = new RetModel
                    {
                        uid = data.ID,
                        user_name = data.ACCOUNT_NUMBER
                    };

                    jsonModel.status = 1;
                    jsonModel.msg = "注册成功";
                    jsonModel.data = lrm;
                }
                else
                {
                    jsonModel.status = 0;
                    jsonModel.msg = "账号已存在，请重新输入";
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                jsonModel.status = 0;
                jsonModel.msg = "注册失败，请重试";
                //记录日志
                //LogHelper.writeFTInfo("登录注册日志", ex.Message.ToString(), new { userId, passWord }, userId, now);
            }
            return jsonModel;
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="userId">账号</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public object Login(string userId,string passWord)
        {
            var db = DBContext.GetInstance;
            DateTime now = db.GetDate();
            //返给前台的JSON实体
            RetJsonModel jsonModel = new RetJsonModel();
            jsonModel.time = FunctionHelper.GetTimestamp();
            try
            {
                var data = db.Queryable<SYS_USER_ACCOUNT>()
                    .Where(x => x.ACCOUNT_NUMBER == userId)
                    .First();
                if (data != null)
                {
                    if(data.PASSWORD == passWord)
                    {
                        LoginReturnModel lrm = new LoginReturnModel();
                        lrm.user_info = new RetModel
                        {
                            uid = data.ID,
                            user_name = data.ACCOUNT_NUMBER
                        };

                        jsonModel.status = 1;
                        jsonModel.msg = "登录成功";
                        jsonModel.data = lrm;
                    }
                    else
                    {
                        jsonModel.status = 0;
                        jsonModel.msg = "密码错误，请重试";
                    }
                }
                else
                {
                    jsonModel.status = 0;
                    jsonModel.msg = "账号不存在，请重试";
                }
            }
            catch (Exception ex)
            {
                jsonModel.status = 0;
                jsonModel.msg = "登录失败,请重试";
                //记录日志
                //LogHelper.writeFTInfo("登录注册日志", ex.Message.ToString(), new { userId, passWord }, userId, now);
            }
            return jsonModel;
        }
    }
}