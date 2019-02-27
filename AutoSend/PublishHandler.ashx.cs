using BLL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.SessionState;
using static System.Net.Mime.MediaTypeNames;

namespace AutoSend
{
    /// <summary>
    /// PublishHandler 的摘要说明
    /// </summary>
    public class PublishHandler : IHttpHandler, IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
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
                        #region 轮循setting表
                        case "roundsetting": _strContent.Append(RoundSetting(context)); break;//读取配置
                        #endregion
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        #region 轮循setting表
        /// <summary>
        /// 服务访问接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RoundSetting(HttpContext context)
        {
            try
            {
                RoundSetting();
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 轮循setting表，开启线程发布任务
        /// </summary>
        private void RoundSetting()
        {
            settingBLL bll = new settingBLL();
            //查找设置定时发布并且非在发配置
            List<settingInfo> sList = bll.RoundSetting(string.Format(" where isAutoPub=1  and isPubing=0"));
            DateTime dt = DateTime.Now;
            foreach (settingInfo sInfo in sList)
            {
                if (!sInfo.isAutoPub || sInfo.isPubing)
                    continue;
                int nowMin = dt.Minute / 10;//当前分钟十位数
                int tMin = sInfo.pubMin / 10;//用户所定时间十位数
                if (dt.Hour == sInfo.pubHour && nowMin == tMin)//若时间符合，调用发布接口
                    Pub.Publish(sInfo.userId);//创建发布线程
            }
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