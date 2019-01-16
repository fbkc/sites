using BLL;
using HRMSys.DAL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Services;
using System.Web.SessionState;
using System.Web.UI;

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
        private static string host = "http://39.105.196.3:1874/test";
        private static string uname = "";
        private static List<realmNameInfo> realList = null;

        /// <summary>
        /// 登录接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [WebMethod(Description = "登录", EnableSession = true)]
        public string Login(string strJson)
        {
            CmUserBLL bll = new CmUserBLL();
            realmBLL rbll = new realmBLL();
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
                    return json.WriteJson(0, "参数错误", new { });
                if (key != keyValue)
                    return json.WriteJson(0, "值错误", new { });
                cmUserInfo userInfo = bll.GetUser(string.Format("where username='{0}'", username.Trim()));
                if (userInfo == null)
                    return json.WriteJson(0, "用户名不存在", new { });
                DateTime.TryParse(userInfo.expirationTime, out s);//到期时间
                if (userInfo.password != password)
                    return json.WriteJson(0, "密码错误", new { });
                if (userInfo.isStop)
                    return json.WriteJson(0, "该用户已被停用", new { });
                if (s <= n)
                    return json.WriteJson(0, "登录失败，账号已到期", new { });
                Context.Session["SoftUser"] = userInfo;
                uname = username;
                List<realmNameInfo> rList = rbll.GetRealmList("");//获取所有域名
                if (rList.Count < 1)
                    return json.WriteJson(0, "登录失败，域名为空", new { });
                realList = rList;
                return json.WriteJson(1, "登录成功", new { cmUser = userInfo, realmList = rList });
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
        }

        /// <summary>
        /// 上传图片
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [WebMethod(Description = "上传图片", EnableSession = true)]
        public string Upload(Image strJson)
        {
            string pId = Context.Request["productId"];
            cmUserInfo model = (cmUserInfo)Context.Session["UserModel"];
            string username = model.username.ToString();
            string fileUrl = "";
            try
            {
                HttpPostedFile _upfile = Context.Request.Files["file"];
                if (_upfile == null)
                    throw new Exception("请先选择文件！");
                else
                {
                    string fileName = _upfile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                    string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                    int bytes = _upfile.ContentLength;//获取文件的字节大小  
                    if (!(suffix == "jpg" || suffix == "gif" || suffix == "png" || suffix == "jpeg"))
                        throw new Exception("只能上传JPE，GIF,PNG文件");
                    if (bytes > 1024 * 1024 * 2)
                        throw new Exception("图片最大只能传2M");
                    string newfileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileDir = HttpContext.Current.Server.MapPath("~/upfiles/" + MyInfo.user + "/");
                    if (!Directory.Exists(fileDir))
                    {
                        Directory.CreateDirectory(fileDir);
                    }
                    //string phyPath = context.Request.PhysicalApplicationPath;
                    //string savePath = phyPath + virPath;
                    string saveDir = fileDir + newfileName + "." + suffix;//文件服务器存放路径
                    fileUrl = "/upfiles/" + username + "/" + newfileName + "." + suffix;
                    _upfile.SaveAs(saveDir);//保存图片
                    #region 存到sql图片库
                    imageBLL bll = new imageBLL();
                    imageInfo img = new imageInfo();
                    img.imageId = newfileName;
                    img.imageURL = fileUrl;
                    img.userId = model.Id;
                    img.productId = int.Parse(pId);
                    bll.AddImg(img);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.Message, new { });
            }
            return json.WriteJson(1, "上传成功", new { imgUrl = fileUrl });
        }

        /// <summary>
        /// post接口
        /// </summary>
        /// <param name="strJson"></param>
        /// <returns></returns>
        [WebMethod(Description = "post接口", EnableSession = true)]
        public string Post(string strJson)
        {
            //需要做一个时间，每隔多长时间才允许访问一次
            string keyValue = NetHelper.GetMD5("liu" + "100dh888");
            string username = "";
            string url = "";
            try
            {
                JObject jo = (JObject)JsonConvert.DeserializeObject(strJson);
                string key = jo["key"].ToString();
                if (key != keyValue)
                    return json.WriteJson(0, "参数错误", new { });
                htmlInfo hInfo = new htmlInfo();
                username = jo["username"].ToString();
                hInfo.userId = GetUserId(username);//用户名
                hInfo.title = jo["title"].ToString();
                string cid = jo["catid"].ToString();
                if (string.IsNullOrEmpty(cid))
                    return json.WriteJson(0, "行业或栏目不能为空", new { });

                //命名规则：ip/目录/用户名/show_行业id+(五位数id)
                string showName = "show_" + cid + (GetMaxId() + 1).ToString() + ".html";
                url = host + "/" + username + "/" + showName;
                hInfo.titleURL = url;
                hInfo.articlecontent = HttpUtility.UrlDecode(jo["content"].ToString(),Encoding.UTF8);//内容,UrlDecode解码
                hInfo.columnId = cid;//行业id，行业新闻id=23
                hInfo.pinpai = jo["pinpai"].ToString();
                hInfo.xinghao = jo["xinghao"].ToString();
                hInfo.price = jo["price"].ToString();
                hInfo.smallCount = jo["qiding"].ToString();
                hInfo.sumCount = jo["gonghuo"].ToString();
                hInfo.unit = jo["unit"].ToString();
                hInfo.city = jo["city"].ToString();
                hInfo.titleImg = jo["thumb"].ToString();
                hInfo.addTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                hInfo.realmNameId = "1";//发到哪个站
                AddHtml(hInfo);//存入数据库

                //公司/会员信息
                CmUserBLL uBLL = new CmUserBLL();
                cmUserInfo uInfo = uBLL.GetUser(string.Format("where username='{0}'", username));

                WriteFile(hInfo, uInfo, username, showName);//写模板
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "发布成功", new { url, username });
        }

        #region 定义模版页
        public static string SiteTemplate()
        {
            string str = "";
            str += "....";//模版页html代码
            return str;
        }
        #endregion

        /// <summary>
        /// 写模板
        /// </summary>
        /// <param name="hInfo"></param>
        /// <param name="uInfo"></param>
        /// <param name="username"></param>
        /// <param name="hName"></param>
        /// <returns></returns>
        public static bool WriteFile(htmlInfo hInfo, cmUserInfo uInfo, string username, string hName)
        {
            //文件输出目录
            string path = HttpContext.Current.Server.MapPath("~/test/" + username + "/");

            // 读取模板文件
            string temp = HttpContext.Current.Server.MapPath("~/HtmlPage1.html");//模版文件

            //string str = SiteTemplate();//读取模版页面html代码
            string str = "";
            using (StreamReader sr = new StreamReader(temp, Encoding.GetEncoding("gb2312")))
            {
                str = sr.ReadToEnd();
                sr.Close();
            }
            string htmlfilename = hName;//静态文件名
            // 替换内容
            str = str.Replace("companyName_Str", uInfo.companyName);
            if (hInfo.title.Length > 6)
                str = str.Replace("keywords_Str", hInfo.title + "," + hInfo.title.Substring(0, 2) + "," + hInfo.title.Substring(2, 2) + "," + hInfo.title.Substring(4, 2));
            else
                str = str.Replace("keywords_Str", hInfo.title);
            str = str.Replace("description_Str", hInfo.articlecontent.Substring(0, 80));
            str = str.Replace("host_Str", host);
            str = str.Replace("catid_Str", hInfo.columnId);
            str = str.Replace("Id_Str", hInfo.Id.ToString());
            str = str.Replace("title_Str", hInfo.title);
            str = str.Replace("addTime_Str", hInfo.addTime);

            str = str.Replace("pinpai_Str", hInfo.pinpai);
            str = str.Replace("price_Str", hInfo.price);
            str = str.Replace("qiding_Str", hInfo.smallCount);
            str = str.Replace("gonghuo_Str", hInfo.sumCount);
            str = str.Replace("xinghao_Str", hInfo.xinghao);
            str = str.Replace("city_Str", hInfo.city);
            str = str.Replace("unit_Str", hInfo.unit);

            str = str.Replace("titleImg_Str", hInfo.titleImg);
            str = str.Replace("content_Str", hInfo.articlecontent);
            // 写文件
            using (StreamWriter sw = new StreamWriter(path + htmlfilename, true))
            {
                sw.Write(str);
                sw.Flush();
                sw.Close();
            }
            return true;
        }
        /// <summary>
        /// 通过用户名找Id
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public string GetUserId(string username)
        {
            object ob = SqlHelper.ExecuteScalar("select Id from userInfo where username='" + username + "'");
            return ob.ToString();
        }
        /// <summary>
        /// 获取当前表最大Id
        /// </summary>
        /// <returns></returns>
        public int GetMaxId()
        {
            object ob = "";
            try
            {
                ob = SqlHelper.ExecuteScalar("select Id  from htmlInfo order by Id desc");
            }
            catch (Exception ex)
            { return 1; }
            return int.Parse(ob.ToString());
        }
        /// <summary>
        /// 将html内容参数存入数据库
        /// </summary>
        /// <param name="info"></param>
        public void AddHtml(htmlInfo info)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[htmlInfo]
           ([title]
           ,[titleURL]
           ,[articlecontent]
           ,[columnId]
           ,[pinpai]
           ,[xinghao]
           ,[price]
           ,[smallCount]
           ,[sumCount]
           ,[unit]
           ,[city]
           ,[titleImg]
           ,[addTime]
           ,[realmNameId]
           ,[userId])
     VALUES
           (@title
           ,@titleURL
           ,@articlecontent
           ,@columnId
           ,@pinpai
           ,@xinghao
           ,@price
           ,@smallCount
           ,@sumCount
           ,@unit
           ,@city
           ,@titleImg
           ,@addTime
           ,@realmNameId
           ,@userId)",
               new SqlParameter("@title", SqlHelper.ToDBNull(info.title)),
               new SqlParameter("@titleURL", SqlHelper.ToDBNull(info.titleURL)),
               new SqlParameter("@articlecontent", SqlHelper.ToDBNull(info.articlecontent)),
               new SqlParameter("@columnId", SqlHelper.ToDBNull(info.columnId)),
               new SqlParameter("@pinpai", SqlHelper.ToDBNull(info.pinpai)),
               new SqlParameter("@xinghao", SqlHelper.ToDBNull(info.xinghao)),
               new SqlParameter("@price", SqlHelper.ToDBNull(info.price)),
               new SqlParameter("@smallCount", SqlHelper.ToDBNull(info.smallCount)),
               new SqlParameter("@sumCount", SqlHelper.ToDBNull(info.sumCount)),
               new SqlParameter("@unit", SqlHelper.ToDBNull(info.unit)),
               new SqlParameter("@city", SqlHelper.ToDBNull(info.city)),
               new SqlParameter("@titleImg", SqlHelper.ToDBNull(info.titleImg)),
               new SqlParameter("@addTime", SqlHelper.ToDBNull(info.addTime)),
               new SqlParameter("@realmNameId", SqlHelper.ToDBNull(info.realmNameId)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(info.userId)));
        }
    }
}
