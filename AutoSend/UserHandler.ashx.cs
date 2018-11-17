using BLL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
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
        /// <summary>
        /// 主进程
        /// </summary>
        /// <param name="context"></param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append("{\"code\": \"0\", \"msg\": \"禁止访问！\",\"detail\": \"\"}");
                }
                else
                {
                    switch (_strAction.Trim().ToLower())
                    {
                        case "login": _strContent.Append(UserLogin(context)); break;//会员登录
                        case "getuserlist": _strContent.Append(GetUserInfo(context)); break;//获取所有会员
                        case "adduser": _strContent.Append(AddUser(context)); break;//添加会员
                        case "putuser": _strContent.Append(UpdateUser(context)); break;//修改会员信息
                        case "deluser": _strContent.Append(DelUser(context)); break;//删除会员
                        case "getAccount": _strContent.Append(GetAccount(context)); break;//获取所有会员
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }
        /// <summary>
        /// 判断是否登录接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAccount(HttpContext context)
        {
            string result = "";
            string header = context.Request.Headers["Authorization"];
            if (string.IsNullOrEmpty(header))
            {
                return "{\"code\": \"0\", \"msg\": \"\",\"detail\": \"\"}";
            }
            if (header.Contains(MyInfo.cookie))
            {
                JavaScriptSerializer jss = new JavaScriptSerializer();
                return "{\"code\": \"0\", \"msg\": \"\",\"detail\": \"" + jss.Serialize((cmUserInfo)context.Session["UserModel"]) + "\"}";
            }
            return result;
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
                return "{\"code\": \"0\", \"msg\": \"用户名不能为空！\",\"detail\": \"\"}";
            if (string.IsNullOrEmpty(_password))
                return "{\"code\": \"0\", \"msg\": \"密码不能为空！\",\"detail\": \"\"}";
            cmUserInfo model = new cmUserInfo();
            //if (context.Session["UserModel"] != null)
            //{   //当前浏览器已经有用户登录 判断是不是当前输入的用户
            //    model = (cmUserInfo)context.Session["UserModel"];
            //    if (model.username != _username)
            //    {
            //        result = "{\"code\": \"0\", \"msg\": \"此浏览器已经有其他用户登录！\",\"detail\": \"\"}";
            //    }
            //    else
            //    {
            //        result = "{\"code\": \"1\", \"msg\": \"登录成功！\",\"detail\": \"\"}";
            //        context.Response.Redirect("http://localhost:6808/UserHandler.ashx?action=login&username=liu&password=");
            //    }
            //}
            //else
            //{
            CmUserBLL bll = new CmUserBLL();
            DataTable dt = bll.GetUser(string.Format("where username='{0}'", _username.Trim()));
            if (dt.Rows.Count < 0 || dt.Rows.Count > 1)
            {
                result = "{\"code\": \"0\", \"msg\": \"登录错误！\",\"detail\": \"\"}";
            }
            else if (dt.Rows.Count == 0)
            {
                result = "{\"code\": \"0\", \"msg\": \"用户名不存在！\",\"detail\": \"\"}";
            }
            else if (dt.Rows.Count == 1)
            {
                int _userid = 0;
                int.TryParse(dt.Rows[0]["Id"].ToString(), out _userid);
                model.Id = _userid;
                model.username = dt.Rows[0]["username"].ToString();
                model.password = dt.Rows[0]["password"].ToString();
                int _userType = 0;
                int.TryParse(dt.Rows[0]["userType"].ToString(), out _userType);
                model.userType = _userType;
                if (model.password != _password)
                    result = "{\"code\": \"0\", \"msg\": \"密码错误！\",\"detail\": \"\"}";
                else
                {
                    context.Session["UserModel"] = model;
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    detail d = new detail();
                    d.cmUser = model;
                    d.userCookie = GetMD5(model.username);
                    MyInfo.user = model.username;//用户名
                    MyInfo.cmUser = model;//用户信息
                    MyInfo.cookie = d.userCookie;//cookie存到全局变量
                    result = "{\"code\": \"1\", \"msg\": \"登录成功！\",\"detail\": \"" + jss.Serialize(d) + "\"}";
                    //context.Response.Redirect("~/");
                }
            }
            //}
            return result;
        }
        /// <summary>
        /// 获取所有会员
        /// </summary>
        /// <returns></returns>
        public string GetUserInfo(HttpContext context)
        {
            CmUserBLL bll = new CmUserBLL();
            List<cmUserInfo> uList = new List<cmUserInfo>();
            DataTable dt = bll.GetUser("");
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                cmUserInfo userInfo = new cmUserInfo();
                userInfo.Id = (int)row["Id"];
                userInfo.username = (string)row["username"];
                userInfo.password = (string)row["password"];
                userInfo.userType = (int)row["userType"];
                userInfo.gradeId = (int)row["gradeId"];
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
            return "{\"code\": \"1\", \"msg\": \"添加成功！\",\"detail\": \"" + jss.Serialize(uList) + "\"}";

        }
        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string AddUser(HttpContext context)
        {
            var strjson = context.Request["json"];
            var jss = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            CmUserBLL cmBLL = new CmUserBLL();
            cmUserInfo cm = JsonConvert.DeserializeObject<cmUserInfo>(strjson, jss);
            cmBLL.AddUser(cm);
            return "{\"code\": \"1\", \"msg\": \"添加成功！\",\"detail\": \"\"}";
        }
        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string UpdateUser(HttpContext context)
        {
            var strjson = context.Request["json"];
            var jss = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            CmUserBLL cmBLL = new CmUserBLL();
            cmUserInfo cm = JsonConvert.DeserializeObject<cmUserInfo>(strjson, jss);
            cmBLL.UpdateUser(cm, string.Format("where Id='{0}'", cm.Id));
            return "{\"code\": \"1\", \"msg\": \"更新成功！\",\"detail\": \"\"}";
        }
        /// <summary>
        /// 删除会员 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelUser(HttpContext context)
        {
            string id = context.Request["Id"];
            CmUserBLL cmBLL = new CmUserBLL();
            cmBLL.DelUser(string.Format("where Id='{0}'", id));
            return "{\"code\": \"1\", \"msg\": \"删除成功！\",\"detail\": \"\"}";
        }
        /// <summary>
        /// MD5
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string GetMD5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;
        }
        /// <summary>
        /// IsReusable
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}