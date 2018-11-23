using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;

namespace AutoSend
{
    public class Global : System.Web.HttpApplication
    {

        protected void Application_Start(object sender, EventArgs e)
        {

        }

        protected void Session_Start(object sender, EventArgs e)
        {

        }

        protected void Application_BeginRequest(object sender, EventArgs e)
        {

        }

        protected void Application_AuthenticateRequest(object sender, EventArgs e)
        {

        }

        protected void Application_Error(object sender, EventArgs e)
        {
            // 在出现未处理的错误时运行的代码

            Exception erroy = Server.GetLastError();

            string err = "出错页面是：" + Request.Url.ToString() + "</br>";

            err += "异常信息：" + erroy.Message + "</br>";

            err += "Source:" + erroy.Source + "</br>";

            err += "StackTrace:" + erroy.StackTrace + "</br>";

            //清除前一个异常

            Server.ClearError();

            //此处理用Session["ProError"]出错。所以用 Application["ProError"]

            Application["erroy"] = err;

            //此处不是page中，不能用Response.Redirect("../frmSysError.aspx");

            HttpContext.Current.Response.Redirect(HttpContext.Current.Request.ApplicationPath + "/ApplicationErroy.ashx");
        }

        protected void Session_End(object sender, EventArgs e)
        {

        }

        protected void Application_End(object sender, EventArgs e)
        {

        }
    }
}