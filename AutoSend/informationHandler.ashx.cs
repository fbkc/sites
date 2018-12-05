using BLL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.Security;
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// informationHandler 的摘要说明
    /// </summary>
    public class informationHandler : BaseHandle, IHttpHandler, IRequiresSessionState
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
                        case "getparalist": _strContent.Append(GetParaList(context)); break;//获取此会员下所有段落
                        case "savepara": _strContent.Append(SavePara(context)); break;
                        case "delpara": _strContent.Append(DelPara(context)); break;//删除段落

                        case "getcontentlist": _strContent.Append(GetContentList(context)); break;//获取此会员下所有内容模板
                        case "savecontent": _strContent.Append(SaveContent(context)); break;
                        case "delcontent": _strContent.Append(DelContent(context)); break;//删除内容模板

                        case "gettailwordlist": _strContent.Append(GetTailwordList(context)); break;
                        case "savetailword": _strContent.Append(SaveTailword(context)); break;
                        case "deltailword": _strContent.Append(DelTailword(context)); break;

                        case "uploadpic": _strContent.Append(UploadPic(context)); break;//上传图片
                        case "getpiclist": _strContent.Append(GetPicList(context)); break;//获取图片
                        case "delpic": _strContent.Append(DelPic(context)); break;//删除图片
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        #region 段落
        /// <summary>
        /// 获取此会员下所有段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetParaList(HttpContext context)
        {
            paragraphBLL bll = new paragraphBLL();
            List<paragraphInfo> pList = new List<paragraphInfo>();
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            string userId = model.Id.ToString();
            if (string.IsNullOrEmpty(userId))
                return json.WriteJson(0, "Id不能为空", new { });
            try
            {
                DataTable dt = bll.GetParagraphList(userId);
                if (dt.Rows.Count < 1)
                    return json.WriteJson(0, "没有数据", new { });
                foreach (DataRow row in dt.Rows)
                {
                    paragraphInfo pInfo = new paragraphInfo();
                    pInfo.Id = (long)row["Id"];
                    pInfo.paraId = (string)row["paraId"];
                    pInfo.paraCotent = (string)row["paraCotent"];
                    pInfo.usedCount = (int)row["usedCount"];
                    pInfo.addTime = (DateTime)row["addTime"];
                    pInfo.userId = (int)row["userId"];
                    pList.Add(pInfo);
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { paraList = pList });
        }
        /// <summary>
        /// 增加或修改段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavePara(HttpContext context)
        {
            paragraphBLL bll = new paragraphBLL();
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            paragraphInfo para = JsonConvert.DeserializeObject<paragraphInfo>(strjson, js);
            if (para.Id == 0)
                bll.AddParagraph(para);
            else
                bll.UpdateParagraph(para);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelPara(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            paragraphBLL bll = new paragraphBLL();
            int a = bll.DelParagraph(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region 内容模板
        /// <summary>
        /// 获取此会员下内容模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetContentList(HttpContext context)
        {
            contentMouldBLL bll = new contentMouldBLL();
            List<contentMouldInfo> cList = new List<contentMouldInfo>();
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            string userId = model.Id.ToString();
            if (string.IsNullOrEmpty(userId))
                return json.WriteJson(0, "Id不能为空", new { });
            try
            {
                DataTable dt = bll.GetContentList(string.Format(" where userId='{0}'", userId));
                if (dt.Rows.Count < 1)
                    return "";
                foreach (DataRow row in dt.Rows)
                {
                    contentMouldInfo cInfo = new contentMouldInfo();
                    cInfo.Id = (int)row["Id"];
                    cInfo.mouldId = (string)row["mouldId"];
                    cInfo.mouldName = (string)row["mouldName"];
                    cInfo.contentMould = (string)row["contentMould"];
                    cInfo.usedCount = (int)row["usedCount"];
                    cInfo.addTime = (DateTime)row["addTime"];
                    cInfo.userId = (int)row["userId"];
                    cList.Add(cInfo);
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { contentList = cList });
        }
        /// <summary>
        /// 增加或修改内容模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveContent(HttpContext context)
        {
            contentMouldBLL bll = new contentMouldBLL();
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            contentMouldInfo content = JsonConvert.DeserializeObject<contentMouldInfo>(strjson, js);
            if (content.Id == 0)
                bll.AddContent(content);
            else
                bll.UpdateContent(content);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除内容模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelContent(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            contentMouldBLL bll = new contentMouldBLL();
            int a = bll.DelContent(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region 长尾词
        private string GetTailwordList(HttpContext context)
        {
            tailwordBLL bll = new tailwordBLL();
            List<tailwordInfo> tList = new List<tailwordInfo>();
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            string userId = model.Id.ToString();
            if (string.IsNullOrEmpty(userId))
                return json.WriteJson(0, "Id不能为空", new { });
            try
            {
                DataTable dt = bll.GetTailwordList(string.Format(" where userId={0}", userId));
                if (dt.Rows.Count < 1)
                    return "";
                foreach (DataRow row in dt.Rows)
                {
                    tailwordInfo tInfo = new tailwordInfo();
                    tInfo.Id = (int)row["Id"];
                    tInfo.tailword = (string)row["tailword"];
                    tInfo.addTime = (DateTime)row["addTime"];
                    tInfo.userId = (int)row["userId"];
                    tList.Add(tInfo);
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { tailwordList = tList });
        }
        /// <summary>
        /// 增加或修改长尾词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveTailword(HttpContext context)
        {
            tailwordBLL bll = new tailwordBLL();
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            tailwordInfo tailword = JsonConvert.DeserializeObject<tailwordInfo>(strjson, js);
            if (tailword.Id == 0)
                bll.AddTailword(tailword);
            else
                bll.UpdateTailword(tailword);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelTailword(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            tailwordBLL bll = new tailwordBLL();
            int a = bll.DelTailword(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region 图片库
        /// <summary>
        /// 上传并保存图片到服务器
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UploadPic(HttpContext context)
        {
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            string username = model.username.ToString();
            string fileUrl = "";
            try
            {
                HttpPostedFile _upfile = context.Request.Files["file"];
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
                        throw new Exception("文件最大只能传2M");
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
        /// 获取此用户下所有图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPicList(HttpContext context)
        {
            imageBLL bll = new imageBLL();
            List<imageInfo> iList = new List<imageInfo>();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                DataTable dt = bll.GetImgList(string.Format(" where userId='{0}' order by addTime desc", userId));
                if (dt.Rows.Count < 1)
                    return json.WriteJson(1, "", new { });
                foreach (DataRow row in dt.Rows)
                {
                    imageInfo iInfo = new imageInfo();
                    iInfo.Id = (long)row["Id"];
                    iInfo.imageId = (string)row["imageId"];
                    iInfo.imageURL = (string)row["imageURL"];
                    iInfo.addTime = (DateTime)row["addTime"];
                    iInfo.userId = (int)row["userId"];
                    iList.Add(iInfo);
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.Message, new { });
            }
            return json.WriteJson(1, "成功", new { picList = iList });
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelPic(HttpContext context)
        {
            string id = context.Request["Id"];
            imageBLL bll = new imageBLL();
            int a = bll.DelImg(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}