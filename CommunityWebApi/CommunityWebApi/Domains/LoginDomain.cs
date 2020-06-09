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
        public RetJsonModel Register(string userId,string passWord)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();

                //返给前台的JSON实体
                RetJsonModel jsonModel = new RetJsonModel();
                int timestamp = FunctionHelper.GetTimestamp();
                jsonModel.time = timestamp;

                //查询账号是否已存在
                var data = db.Queryable<SYS_USER_ACCOUNT>()
                    .Where(x => x.ACCOUNT_NUMBER == userId)
                    .First();
                if (data == null)
                {
                    db.Ado.BeginTran();
                    //账号表新增记录
                    SYS_USER_ACCOUNT model = new SYS_USER_ACCOUNT();
                    model.ID = System.Guid.NewGuid().ToString();
                    model.DATETIME_CREATED = now;
                    model.STATE = "A";
                    model.ACCOUNT_NUMBER = userId;
                    model.PASSWORD = passWord;
                    model.TIMESTAMP_INT = timestamp;
                    model.STATUS = 1;
                    db.Insertable(model).ExecuteCommand();

                    //先在用户注册时，默认在用户个人信息表插入一条记录
                    SYS_USER_INFO uInfoModel = new SYS_USER_INFO();
                    uInfoModel.ID = System.Guid.NewGuid().ToString();
                    uInfoModel.DATETIME_CREATED = now;
                    uInfoModel.STATE = "A";
                    uInfoModel.TIMESTAMP_INT = timestamp;
                    uInfoModel.USER_ID = model.ID;
                    uInfoModel.NICK_NAME = "挚爱灬(啥也不是)";
                    db.Insertable(uInfoModel).ExecuteCommand();


                    LoginReturnModel lrm = new LoginReturnModel();
                    lrm.user_info = new RetModel
                    {
                        uid = model.ID,
                        user_name = model.ACCOUNT_NUMBER
                    };
                    jsonModel.status = 1;
                    jsonModel.msg = "注册成功";
                    jsonModel.data = lrm;

                    db.Ado.CommitTran();
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
                db.Ado.RollbackTran();
                throw ex;
            }
        }

        /// <summary>
        /// 登录方法
        /// </summary>
        /// <param name="userId">账号</param>
        /// <param name="passWord">密码</param>
        /// <returns></returns>
        public RetJsonModel Login(string userId,string passWord)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                //返给前台的JSON实体
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();

                var data = db.Queryable<SYS_USER_ACCOUNT>()
                    .Where(x => x.ACCOUNT_NUMBER == userId && x.STATE=="A")
                    .First();
                if (data != null)
                {
                    if(data.PASSWORD == passWord)
                    {
                        db.Updateable<SYS_USER_ACCOUNT>().UpdateColumns(x => new 
                        {
                            STATUS = 1
                        }).Where(x => x.ID == data.ID).ExecuteCommand();
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
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 验证账号是否在登陆中
        /// </summary>
        /// <param name="userId">用户ID</param>
        /// <returns></returns>
        public RetJsonModel IsLogin(string userId)
        {
            var db = DBContext.GetInstance;
            try
            {
                DateTime now = db.GetDate();
                //返给前台的JSON实体
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();

                SYS_USER_ACCOUNT account = db.Queryable<SYS_USER_ACCOUNT>()
                    .Where(x => x.STATE == "A" && x.ID == userId)
                    .First();
                if (account != null&&account.STATUS == 1)
                {
                    jsonModel.status = 1;
                    jsonModel.msg = "账号登陆中";
                    jsonModel.data = true;
                }
                else
                {
                    jsonModel.status = 0;
                    jsonModel.msg = "账号未登录";
                    jsonModel.data = false;
                }
                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}