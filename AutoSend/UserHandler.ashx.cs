using BLL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
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
        private static JavaScriptSerializer jss = new JavaScriptSerializer();
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
                    jsonInfo json = new jsonInfo();
                    json.code = "0";
                    json.msg = "禁止访问";
                    json.detail = null;
                    _strContent.Append(jss.Serialize(json));
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
                        case "getAccount": _strContent.Append(GetAccount(context)); break;//判断是否登录接口
                        case "getrealmlist": _strContent.Append(GetRealmList(context)); break;//获取域名   
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
            if (header.Contains(MyInfo.cookie))
            {
                jsonInfo json = new jsonInfo();
                json.code = "1";
                json.msg = "";
                //json.detail.cmUser = (cmUserInfo)context.Session["UserModel"];
                result = jss.Serialize(json);
            }
            else
            {
                jsonInfo json = new jsonInfo();
                json.code = "0";
                json.msg = "您尚未登录";
                json.detail = null;
                result = jss.Serialize(json);
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
            if (string.IsNullOrEmpty(_username) || string.IsNullOrEmpty(_password))
                return "";
            cmUserInfo model = new cmUserInfo();
            CmUserBLL bll = new CmUserBLL();
            DataTable dt = bll.GetUser(string.Format("where username='{0}'", _username.Trim()));
            if (dt.Rows.Count < 0 || dt.Rows.Count > 1)
            {
                jsonInfo json = new jsonInfo();
                json.code = "0";
                json.msg = "登录错误";
                json.detail = null;
                result = jss.Serialize(json);
            }
            else if (dt.Rows.Count == 0)
            {
                jsonInfo json = new jsonInfo();
                json.code = "0";
                json.msg = "用户名不存在";
                json.detail = null;
                result = jss.Serialize(json);
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
                {
                    jsonInfo json = new jsonInfo();
                    json.code = "0";
                    json.msg = "密码错误";
                    json.detail = null;
                    result = jss.Serialize(json);
                }
                if (model.isStop)
                {
                    jsonInfo json = new jsonInfo();
                    json.code = "0";
                    json.msg = "该用户已被停用";
                    json.detail = null;
                    result = jss.Serialize(json);
                }
                else
                {
                    context.Session["UserModel"] = model;
                    JavaScriptSerializer jss = new JavaScriptSerializer();
                    //detail d = new detail();
                    //d.cmUser = model;
                    //d.userCookie = GetMD5(model.username);
                    StringBuilder _strContent = new StringBuilder();
                    _strContent.Append(model);
                    string md5 = GetMD5(model.username);
                    _strContent.Append(md5);
                    MyInfo.user = model.username;//用户名
                    MyInfo.cmUser = model;//用户信息
                    MyInfo.cookie = md5;//cookie存到全局变量
                    jsonInfo json = new jsonInfo();
                    json.code = "1";
                    json.msg = "登陆成功";
                    json.detail = _strContent;
                    result = jss.Serialize(json);
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
                userInfo.isStop = (bool)row["isStop"];
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
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "成功";
            //detail d = new detail();
            //d.det1 = uList;
            StringBuilder _strContent = new StringBuilder();
            _strContent.Append(uList);
            json.detail = _strContent;
            return jss.Serialize(json);
        }
        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string AddUser(HttpContext context)
        {
            var strjson = context.Request["json"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            CmUserBLL cmBLL = new CmUserBLL();
            cmUserInfo cm = JsonConvert.DeserializeObject<cmUserInfo>(strjson, js);
            cmBLL.AddUser(cm);
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "添加成功";
            json.detail = null;
            return jss.Serialize(json);
        }
        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string UpdateUser(HttpContext context)
        {
            var strjson = context.Request["json"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            CmUserBLL cmBLL = new CmUserBLL();
            cmUserInfo cm = JsonConvert.DeserializeObject<cmUserInfo>(strjson, js);
            cmBLL.UpdateUser(cm, string.Format("where Id='{0}'", cm.Id));
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "更新成功";
            json.detail = null;
            return jss.Serialize(json);
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
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "删除成功";
            json.detail = null;
            return jss.Serialize(json);
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
        /// 获取域名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetRealmList(HttpContext context)
        {
            realmBLL bll = new realmBLL();
            List<realmNameInfo> rList = new List<realmNameInfo>();
            DataTable dt = bll.GetRealmList("");
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                realmNameInfo rInfo = new realmNameInfo();
                rInfo.Id = (int)row["Id"];
                rInfo.realmName = (string)row["realmName"];
                rInfo.realmAddress = (string)row["realmAddress"];
                rInfo.isUseing = (bool)row["isUseing"];
                rList.Add(rInfo);
            }
            //将list对象集合转换为Json
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "成功";
            //detail d = new detail();
            //d.det1 = rList;
            StringBuilder _strContent = new StringBuilder();
            _strContent.Append(rList);
            json.detail = _strContent;
            return jss.Serialize(json);
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