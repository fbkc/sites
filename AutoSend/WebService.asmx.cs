using BLL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// WebService 的摘要说明
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // 若要允许使用 ASP.NET AJAX 从脚本中调用此 Web 服务，请取消注释以下行。 
    // [System.Web.Script.Services.ScriptService]
    public class WebService : System.Web.Services.WebService, IRequiresSessionState
    {

        [WebMethod]
        public string HelloWorld()
        {
            return "Hello World";
        }
        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Login(string strJson)
        {
            cmUserInfo model = new cmUserInfo();
            CmUserBLL bll = new CmUserBLL();
            realmBLL rbll = new realmBLL();
            List<realmNameInfo> rList = new List<realmNameInfo>();
            DateTime s, n;
            s = DateTime.Now;
            n = DateTime.Now;
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);
                string username = jo["username"].ToString();
                string password = jo["password"].ToString();
                string dosubmit = jo["dosubmit"].ToString();
                string key = jo["key"].ToString();

                string keyValue = NetHelper.GetMD5(username + "100dh888");
                if (dosubmit != "1")
                    return json.WriteJson(0, "登录失败", new { });
                if (key != keyValue)
                    return json.WriteJson(0, "登录失败", new { });

                DataTable dt = bll.GetUser(string.Format("where username='{0}'", username.Trim()));
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
                    model.registerTime = ((DateTime)dt.Rows[0]["registerTime"]).ToString("yyyy-MM-dd HH:mm:ss");//注册时间
                    model.expirationTime = ((DateTime)dt.Rows[0]["expirationTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    model.realmNameInfo = (string)dt.Rows[0]["realmNameInfo"];
                    DateTime.TryParse(model.expirationTime, out s);//到期时间
                    if (model.password != password)
                        return json.WriteJson(0, "密码错误", new { });
                    else if (model.isStop)
                        return json.WriteJson(0, "该用户已被停用", new { });
                    else if (s <= n)
                        return json.WriteJson(0, "登录失败，账号已到期", new { });
                    else
                    {
                        Context.Session["SoftUser"] = model;
                        #region 返回所有域名
                        DataTable rdt = rbll.GetRealmList("");
                        if (rdt.Rows.Count < 1)
                            return "";
                        foreach (DataRow row in rdt.Rows)
                        {
                            realmNameInfo rInfo = new realmNameInfo();
                            rInfo.Id = (int)row["Id"];
                            rInfo.realmName = (string)row["realmName"];
                            rInfo.realmAddress = (string)row["realmAddress"];
                            rInfo.isUseing = (bool)row["isUseing"];
                            rList.Add(rInfo);
                        }
                        #endregion
                    }
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "登陆成功", new { cmUser = model, realmList = rList });
        }

        /// <summary>
        /// post接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [WebMethod(EnableSession = true)]
        public string Post(string strJson)
        {
            try
            {

            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "登陆成功", new { });
        }
    }
}
