using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace AutoSend
{
    /// <summary>
    /// Uploader 的摘要说明
    /// </summary>
    public class Uploader : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            string NowPath = Path.Combine(context.Server.MapPath("."), context.Request["path"]);

            if (!Directory.Exists(NowPath))
            {
                Directory.CreateDirectory(NowPath);
            }

            foreach (string fileKey in context.Request.Files)
            {
                HttpPostedFile file = context.Request.Files[fileKey];
                string FilePath = Path.Combine(NowPath, file.FileName);
                if (File.Exists(FilePath))
                {
                    if (Convert.ToBoolean(context.Request["overwrite"]))
                    {
                        File.Delete(FilePath);
                    }
                    else
                    {
                        continue;
                    }
                }
                file.SaveAs(FilePath);
            }
            context.Response.Write("Hello World");
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