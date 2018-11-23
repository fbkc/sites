using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSend
{
    /// <summary>
    /// Handler1 的摘要说明
    /// </summary>
    public class ApplicationErroy : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            if (context.Application["erroy"] != null)
            {
                context.Response.Write(context.Application["erroy"].ToString());
            }
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}