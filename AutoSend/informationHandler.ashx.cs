﻿using BLL;
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
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// informationHandler 的摘要说明
    /// </summary>
    public class informationHandler : IHttpHandler, IRequiresSessionState
    {
        private static JavaScriptSerializer jss = new JavaScriptSerializer();
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    jsonInfo json = new jsonInfo();
                    json.code = "0";
                    json.msg = "禁止访问";
                    json.detail = new object { };
                    _strContent.Append(jss.Serialize(json));
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
            jsonInfo json = new jsonInfo();
            paragraphBLL bll = new paragraphBLL();
            List<paragraphInfo> pList = new List<paragraphInfo>();
            string userId = context.Request["Id"];
            if (string.IsNullOrEmpty(userId))
                return WriteJson(0, "Id不能为空", new { });
            try
            {
                DataTable dt = bll.GetParagraphList(string.Format(" where userId={0}", userId));
                if (dt.Rows.Count < 1)
                    return WriteJson(0, "没有数据", new { });
                foreach (DataRow row in dt.Rows)
                {
                    paragraphInfo pInfo = new paragraphInfo();
                    pInfo.Id = (int)row["Id"];
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
                return WriteJson(1, ex.ToString(), new { });
            }
            return WriteJson(1, "成功", new { paraList = pList });
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
            return WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelPara(HttpContext context)
        {
            string id = context.Request["Id"];
            paragraphBLL bll = new paragraphBLL();
            int a = bll.DelParagraph(id);
            jsonInfo json = new jsonInfo();
            if (a == 1)
                return WriteJson(1, "删除成功", new { });
            else
                return WriteJson(0, "删除失败", new { });
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
            string userId = context.Request["Id"];
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
            return WriteJson(1, "成功", new { contentList = cList });
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
            return WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除内容模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelContent(HttpContext context)
        {
            string id = context.Request["Id"];
            contentMouldBLL bll = new contentMouldBLL();
            int a = bll.DelContent(id);
            jsonInfo json = new jsonInfo();
            if (a == 1)
                return WriteJson(1, "删除成功", new { });
            else
                return WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region 长尾词
        private string GetTailwordList(HttpContext context)
        {
            tailwordBLL bll = new tailwordBLL();
            List<tailwordInfo> tList = new List<tailwordInfo>();
            string userId = context.Request["Id"];
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
            return WriteJson(1, "成功", new { tailwordList = tList });
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
            return WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelTailword(HttpContext context)
        {
            string id = context.Request["Id"];
            tailwordBLL bll = new tailwordBLL();
            int a = bll.DelTailword(id);
            jsonInfo json = new jsonInfo();
            if (a == 1)
                return WriteJson(1, "删除成功", new { });
            else
                return WriteJson(0, "删除失败", new { });
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
            jsonInfo json = new jsonInfo();
            try
            {
                //return context.Request.Files.ToString();
                //StreamReader reader = new StreamReader(context.Request.InputStream);
                //string strjson = HttpUtility.UrlDecode(reader.ReadToEnd());
                //return "liu"+strjson;
                //return "4";
                HttpPostedFile _upfile = context.Request.Files["file"];
                if (_upfile == null)
                {
                    return "2";
                    throw new Exception("请先选择文件！");

                }
                else
                {
                    string fileName = _upfile.FileName;/*获取文件名： C:\Documents and Settings\Administrator\桌面\123.jpg*/
                    string suffix = fileName.Substring(fileName.LastIndexOf(".") + 1).ToLower();/*获取后缀名并转为小写： jpg*/
                    int bytes = _upfile.ContentLength;//获取文件的字节大小  
                                                      //
                    if (!(suffix == "jpg" || suffix == "gif" || suffix == "png" || suffix == "jpeg"))
                        throw new Exception("只能上传JPE，GIF,PNG文件");
                    if (bytes > 1024 * 1024 * 10)
                        throw new Exception("文件最大只能传10M");
                    string newfileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileDir = HttpContext.Current.Server.MapPath("~/upfiles/");
                    if (!Directory.Exists(fileDir))
                    {
                        Directory.CreateDirectory(fileDir);
                    }
                    //string phyPath = context.Request.PhysicalApplicationPath;
                    //string savePath = phyPath + virPath;
                    string saveDir = fileDir + newfileName + "." + suffix;
                    string fileUrl = "~/upfiles/" + newfileName + "." + suffix;//文件服务器存放路径
                    _upfile.SaveAs(fileUrl);//保存图片
                    return "1";
                    #region 存到sql图片库
                    imageBLL bll = new imageBLL();
                    imageInfo img = new imageInfo();
                    img.imageId = newfileName;
                    img.imageURL = fileUrl;
                    img.userId = MyInfo.Id;
                    bll.AddImg(img);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                return WriteJson(0, ex.Message, new { });
                //json.code = "0";
                //json.msg = ex.Message;//异常信息
                //json.detail = new { };
            }
            return WriteJson(1, "上传成功", new { });
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
            var userId = context.Request["Id"];
            DataTable dt = bll.GetImgList(string.Format(" where userId='{0}' order by addTime desc", userId));
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                imageInfo iInfo = new imageInfo();
                iInfo.Id = (int)row["Id"];
                iInfo.imageId = (string)row["imageId"];
                iInfo.imageURL = (string)row["imageURL"];
                iInfo.addTime = (DateTime)row["addTime"];
                iInfo.userId = (int)row["userId"];
                iList.Add(iInfo);
            }
            return WriteJson(1, "成功", new { picList = iList });
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
            jsonInfo json = new jsonInfo();
            if (a == 1)
                return WriteJson(1, "删除成功", new { });
            else
                return WriteJson(0, "删除失败", new { });
        }
        #endregion
        /// <summary>
        /// json封装方法
        /// </summary>
        /// <param name="status1"></param>
        /// <param name="msg1"></param>
        /// <param name="data1"></param>
        /// <returns></returns>
        public string WriteJson(int status1, string msg1, object data1)
        {
            var obj = new { code = status1, msg = msg1, detail = data1 };
            string json = new JavaScriptSerializer().Serialize(obj);
            return json;
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