using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// Login 的摘要说明
    /// </summary>
    public class Login :IHttpHandler, IRequiresSessionState
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8;";
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append(_strContent.Append(json.WriteJson(0, "禁止访问", new { })));
                }
                else
                {
                    switch (_strAction.Trim().ToLower())
                    {
                        case "login": _strContent.Append(UserLogin(context)); break;//会员登录
                        case "getaccount": _strContent.Append(GetAccount(context)); break;//判断是否登录接口
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
            string header = context.Request.Headers["Authorization"];
            if (header.Contains(MyInfo.cookie))
                return json.WriteJson(1, "", new { });
            else
                return json.WriteJson(0, "您尚未登录", new { });
        }

        #region 用户登录
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
                return json.WriteJson(0, "登录错误", new { });
            else if (dt.Rows.Count == 0)
                return json.WriteJson(0, "用户名不存在", new { });
            else if (dt.Rows.Count == 1)
            {
                int _userid = 0;
                int.TryParse(dt.Rows[0]["Id"].ToString(), out _userid);
                model.Id = _userid;
                model.username = dt.Rows[0]["username"].ToString();
                model.password = dt.Rows[0]["password"].ToString();
                int _userType = 0;
                int.TryParse(dt.Rows[0]["userType"].ToString(), out _userType);
                model.userType = _userType;//用户角色
                model.isStop = (bool)dt.Rows[0]["isStop"];
                if (model.password != _password)
                    return json.WriteJson(0, "密码错误", new { });
                else if (model.isStop)
                    return json.WriteJson(0, "该用户已被停用", new { });
                else
                {
                    context.Session["UserModel"] = model;
                    string md5 = GetMD5(model.username);
                    MyInfo.user = model.username;//用户名
                    MyInfo.Id = model.Id;//userId
                    MyInfo.cmUser = model;//用户信息
                    MyInfo.cookie = md5;//cookie存到全局变量
                    return json.WriteJson(1, "登陆成功", new { userCookie = md5, cmUser = model });
                }
            }
            return result;
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
        #endregion

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}