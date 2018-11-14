using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/html";
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append("{\"code\": \"0\", \"msg\": \"禁止访问！\",\"rows\": []}");
                }
                else
                {
                    switch (_strAction.Trim().ToLower())
                    {
                        case "login": _strContent.Append(UserLogin(context)); break;//会员登录
                        case "test": _strContent.Append("这是个测试"); break;
                        case "getuser": _strContent.Append(GetUserInfo(context)); break;//获取所有会员
                        case "adduser": _strContent.Append(addUser(context)); break;//添加会员
                        case "putuser": _strContent.Append(updateUser(context)); break;//修改会员信息
                        case "deluser": _strContent.Append(delUser(context)); break;//删除会员
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }
        /// <summary>
        /// 用户登录
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UserLogin(HttpContext context)
        {
            string result = "";
            string _username = context.Request["username"];
            string _password = context.Request["password"];
            if (string.IsNullOrEmpty(_username))
                return "{\"code\": \"0\", \"msg\": \"用户名不能为空！\"}";
            if (string.IsNullOrEmpty(_password))
                return "{\"code\": \"0\", \"msg\": \"密码不能为空！\"}";
            cmUserInfo model = new cmUserInfo();
            if (context.Session["UserModel"] != null)
            {   //当前浏览器已经有用户登录 判断是不是当前输入的用户
                model = (cmUserInfo)context.Session["UserModel"];
                if (model.username != _username)
                {
                    result = "{\"code\": \"0\", \"msg\": \"此浏览器已经有其他用户登录！\"}";
                }
                else
                {
                    result = "{\"code\": \"1\", \"msg\": \"登录成功！\"}";
                }
            }
            else
            {
                cmUserBLL bll = new cmUserBLL();
                DataTable dt = bll.GetUser(string.Format("where username='{0}'", _username.Trim()));
                if (dt.Rows.Count < 0 || dt.Rows.Count > 1)
                {
                    result = "{\"code\": \"0\", \"msg\": \"登录错误！\"}";
                }
                else if (dt.Rows.Count == 0)
                {
                    result = "{\"code\": \"0\", \"msg\": \"用户名不存在！\"}";
                }
                else if (dt.Rows.Count == 1)
                {
                    int _userid = 0;
                    int.TryParse(dt.Rows[0]["Id"].ToString(), out _userid);
                    model.Id = _userid.ToString();
                    model.username = dt.Rows[0]["username"].ToString();
                    model.password = dt.Rows[0]["password"].ToString();
                    if (model.password != _password)
                        result = "{\"code\": \"0\", \"msg\": \"密码错误！\"}";
                    else
                    {
                        context.Session["UserModel"] = model;
                        result = "{\"code\": \"1\", \"msg\": \"登录成功！\"}";
                    }
                }
            }
            return result;
        }
        /// <summary>
        /// 获取所有会员
        /// </summary>
        /// <returns></returns>
        public string GetUserInfo(HttpContext context)
        {
            cmUserBLL bll = new cmUserBLL();
            List<cmUserInfo> uList = new List<cmUserInfo>();
            DataTable dt = bll.GetUser("");
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                cmUserInfo userInfo = new cmUserInfo();
                userInfo.Id = (string)row["Id"].ToString();
                userInfo.username = (string)row["username"];
                userInfo.password = (string)row["password"];
                userInfo.accountGrade = (int)row["accountGrade"];
                userInfo.canPubCount = (int)row["canPubCount"];
                userInfo.realmNameInfo = (string)row["realmNameInfo"];
                userInfo.expirationTime = (string)row["expirationTime"];
                userInfo.endPubCount = (int)row["endPubCount"];
                userInfo.endTodayPubCount = (int)row["endTodayPubCount"];
                userInfo.registerTime = (string)row["registerTime"];
                userInfo.registerIP = (string)row["registerIP"];
                uList.Add(userInfo);
            }
            //将list对象集合转换为Json
            JavaScriptSerializer jss = new JavaScriptSerializer();
            return jss.Serialize(uList);
        }
        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string addUser(HttpContext context)
        {
            string strjson = context.Request["json"];
            cmUserBLL cmBLL = new cmUserBLL();
            cmUserInfo cm = SerializerHelper.DeserializeJsonToObject<cmUserInfo>(strjson);
            cmBLL.AddUser(cm);
            return "{\"code\": \"1\", \"msg\": \"添加成功！\"}";
        }
        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string updateUser(HttpContext context)
        {
            string strjson = context.Request["json"];
            cmUserBLL cmBLL = new cmUserBLL();
            cmUserInfo cm = SerializerHelper.DeserializeJsonToObject<cmUserInfo>(strjson);
            cmBLL.UpdateUser(cm,string.Format("where Id='{0}'", cm.Id));
            return "{\"code\": \"1\", \"msg\": \"更新成功！\"}";
        }
        /// <summary>
        /// 删除会员 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string delUser(HttpContext context)
        {
            string id = context.Request["Id"];
            cmUserBLL cmBLL = new cmUserBLL();
            cmBLL.DelUser(string.Format("where Id='{0}'", id));
            return "{\"code\": \"1\", \"msg\": \"删除成功！\"}";
        }
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}