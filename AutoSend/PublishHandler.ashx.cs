using BLL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// PublishHandler 的摘要说明
    /// </summary>
    public class PublishHandler : BaseHandle, IHttpHandler, IRequiresSessionState
    {

        public override void OnLoad(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8;";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append(json.WriteJson(0, "禁止访问", new { }));
                }
                else
                {
                    switch (_strAction.Trim().ToLower())
                    {
                        #region 发布设置
                        case "getsetting": _strContent.Append(GetSetting(context)); break;//读取配置
                        case "subsetting": _strContent.Append(SubSetting(context)); break;//提交配置
                        #endregion

                        #region 轮循setting表
                        case "roundsetting": _strContent.Append(RoundSetting(context)); break;//读取配置
                        #endregion

                        #region 发布
                        case "publish": _strContent.Append(Publish(context)); break;//读取配置
                        #endregion

                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        #region 发布设置
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name = "context" ></ param >
        /// < returns ></ returns >
        private string GetSetting(HttpContext context)
        {
            settingBLL bll = new settingBLL();
            settingInfo setInfo = new settingInfo();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                setInfo = bll.GetSetting(string.Format(" where userId='{0}'", userId));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { setInfo });
        }

        /// <summary>
        /// 提交配置
        /// </summary>
        /// <param name = "context" ></ param >
        /// < returns ></ returns >
        private string SubSetting(HttpContext context)
        {
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                settingBLL bll = new settingBLL();
                string strjson = context.Request["params"];
                var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                settingInfo setInfo = JsonConvert.DeserializeObject<settingInfo>(strjson, js);
                setInfo.userId = model.Id;
                if (setInfo.Id == 0)
                    bll.AddSetting(setInfo);
                else
                    bll.UpdateSetting(setInfo);
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
        }
        #endregion

        #region 轮循setting表
        private string RoundSetting(HttpContext context)
        {
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                settingBLL bll = new settingBLL();
                string strjson = context.Request["params"];
                var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                settingInfo setInfo = JsonConvert.DeserializeObject<settingInfo>(strjson, js);
                setInfo.userId = model.Id;
                if (setInfo.Id == 0)
                    bll.AddSetting(setInfo);
                else
                    bll.UpdateSetting(setInfo);
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
        }
        #endregion

        #region 发布
        private string Publish(HttpContext context)
        {
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                settingBLL bll = new settingBLL();
                string strjson = context.Request["params"];
                var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                settingInfo setInfo = JsonConvert.DeserializeObject<settingInfo>(strjson, js);
                setInfo.userId = model.Id;
                if (setInfo.Id == 0)
                    bll.AddSetting(setInfo);
                else
                    bll.UpdateSetting(setInfo);
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
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