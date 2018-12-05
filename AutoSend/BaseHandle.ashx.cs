using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// BaseHandle 的摘要说明
    /// </summary>
    public class BaseHandle : IHttpHandler,IRequiresSessionState
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8;";
            string header = context.Request.Headers["Authorization"];
            //if (string.IsNullOrEmpty(header))
            //{
            //    json.WriteJson(0, "请先登录", new { });
            //    context.Response.StatusCode = 405;
            //    context.Response.End();
            //}
            //if (!header.Contains(MyInfo.cookie))
            //{
            //    json.WriteJson(0, "请先登录", new { });
            //    context.Response.StatusCode = 405;
            //    context.Response.End();
            //}
            if (context.Session == null)
            {
                context.Response.StatusCode = 401;
                context.Response.End();
            }
            if (context.Session["UserModel"] == null)
            {
                context.Response.StatusCode = 401;
                context.Response.End();
            }
            OnLoad(context);
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
        /// <summary>
        /// 代码实现
        /// </summary>
        /// <param name="context"></param>
        public virtual void OnLoad(HttpContext context)
        {
        }
    }
}