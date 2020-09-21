using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using CommunityWebApi.Common;
using CommunityWebApi.Models;
using Entitys;
using Newtonsoft.Json;
using SqlSugar;

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
                    bool isExist = true;
                    string nickName = "";
                    do
                    {
                        nickName = FunctionHelper.GetRandomString(8, true, false, true, false, "");
                        int count = db.Queryable<SYS_USER_INFO>().Where(x => x.NICK_NAME == nickName && x.STATE == "A").Count();
                        isExist = count == 0;
                    } while (!isExist);

                    SYS_USER_INFO uInfoModel = new SYS_USER_INFO();
                    uInfoModel.ID = System.Guid.NewGuid().ToString();
                    uInfoModel.DATETIME_CREATED = now;
                    uInfoModel.STATE = "A";
                    uInfoModel.TIMESTAMP_INT = timestamp;
                    uInfoModel.USER_ID = model.ID;
                    uInfoModel.NICK_NAME = nickName;
                    db.Insertable(uInfoModel).ExecuteCommand();


                    LoginReturnModel lrm = new LoginReturnModel();
                    lrm.user_info = new UserInfoReturnModel
                    {
                        USER_ID = model.ID,
                        NiCK_NAME = uInfoModel.NICK_NAME
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

                var data = db.Queryable<SYS_USER_ACCOUNT, SYS_USER_INFO>((a, b) => new object[]{
                    JoinType.Left,a.ID==b.USER_ID&&a.STATE==b.STATE
                }).Where((a, b) => a.ACCOUNT_NUMBER == userId && a.STATE == "A")
                .Select((a, b) => new
                {
                    a.ID,
                    a.PASSWORD,
                    b.NICK_NAME
                }).First();
                if (data != null)
                {
                    if(data.PASSWORD == passWord)
                    {
                        db.Updateable<SYS_USER_ACCOUNT>().SetColumns(x => new SYS_USER_ACCOUNT()
                        {
                            STATUS = 1,
                            DATETIME_MODIFIED = now
                        }).Where(x => x.ID == data.ID).ExecuteCommand();

                        LoginReturnModel lrm = new LoginReturnModel();
                        lrm.user_info = new UserInfoReturnModel
                        {
                            USER_ID = data.ID,
                            NiCK_NAME = data.NICK_NAME
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
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");

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

        /// <summary>
        /// 获取用户所有权限
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public RetJsonModel GetRolePermission(string userId)
        {
            var db = DBContext.GetInstance;
            try
            {
                //数据校验
                FunctionHelper.VerifyInfo(db, userId, "USER_ID");

                //返给前台的JSON实体
                RetJsonModel jsonModel = new RetJsonModel();
                jsonModel.time = FunctionHelper.GetTimestamp();

                List<string> data = db.Queryable<SYS_USER_PERMISSION>()
                    .Where(x => x.USER_ID == userId && x.STATE == "A")
                    .Select(x => x.PERMISSION_CODE).ToList();

                jsonModel.status = 1;
                jsonModel.msg = "成功";
                jsonModel.data = data;

                return jsonModel;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}