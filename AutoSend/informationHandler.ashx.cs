﻿using BLL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
                        #region 产品分类
                        case "getproductlist": _strContent.Append(GetProductList(context)); break;//获取此会员下所有产品分类
                        case "getproductbyid": _strContent.Append(GetProductById(context)); break;//获取某个产品具体信息
                        case "saveproduct": _strContent.Append(SaveProduct(context)); break;
                        case "delproduct": _strContent.Append(DelProduct(context)); break;//删除产品分类
                        #endregion

                        #region 图片
                        case "uploadpic": _strContent.Append(UploadPic(context)); break;//上传图片
                        case "getpiclist": _strContent.Append(GetPicList(context)); break;//获取图片
                        case "delpic": _strContent.Append(DelPic(context)); break;//删除图片
                        #endregion

                        #region 段落
                        case "getparalist": _strContent.Append(GetParaList(context)); break;//获取此会员下所有段落
                        case "savepara": _strContent.Append(SavePara(context)); break;
                        case "delpara": _strContent.Append(DelPara(context)); break;//删除段落
                        case "delparas": _strContent.Append(DelParas(context)); break;//多行删除
                        case "onekeydealpara": _strContent.Append(OneKeyDealPara(context)); break;//一键整理
                        case "onekeygather": _strContent.Append(OneKeyGather(context)); break;//一键采集
                        case "preview": _strContent.Append(Preview(context)); break;//预览
                        case "onekeyedit": _strContent.Append(OneKeyEdit(context)); break;//一键编辑
                        #endregion

                        #region 内容模板
                        case "getcontentlist": _strContent.Append(GetContentList(context)); break;//获取此会员下所有内容模板
                        case "savecontent": _strContent.Append(SaveContent(context)); break;
                        case "delcontent": _strContent.Append(DelContent(context)); break;//删除内容模板
                        #endregion

                        #region 长尾词/关键词
                        case "getpublictailwordlist": _strContent.Append(GetPublicTailwordList(context)); break;//获取公共长尾词
                        case "getwordslist": _strContent.Append(GetWordsList(context)); break;//获取私人长尾词/关键词
                        case "savewords": _strContent.Append(SaveWords(context)); break;
                        case "delwords": _strContent.Append(DelWords(context)); break;
                        case "digwords": _strContent.Append(DigWords(context)); break;//挖词(暂时未做)
                        #endregion

                        #region 标题
                        case "gettitlelist": _strContent.Append(GetTitleList(context)); break;//获取此会员下单个产品的标题
                        case "getwaittingtitlelist": _strContent.Append(GetAllTitleList(context, 0, "")); break;//获取此会员下所有待发标题，按时间正序排列
                        case "getpubbedtitlelist": _strContent.Append(GetAllTitleList(context, 1, "desc")); break;//获取此会员下所有已发标题，按时间倒序排列
                        case "savetitle": _strContent.Append(SaveTitle(context)); break;
                        case "deltitle": _strContent.Append(DelTitle(context)); break;//删除标题
                        case "deltitles": _strContent.Append(DelTitles(context)); break;//多行删除
                        case "delpubbedtitles": _strContent.Append(DelPubbedTitles(context)); break;//一键清空此用户已发布信息
                        #endregion

                        #region 公司信息
                        case "getorginfo": _strContent.Append(GetOrgInfo(context)); break;//获取此会员公司
                        case "updateorginfo": _strContent.Append(UpdateOrgInfo(context)); break;
                        #endregion

                        #region 轮播图
                        case "upbanner": _strContent.Append(UpBanner(context)); break;//上传轮播图
                        case "savebanner": _strContent.Append(SaveBanner(context)); break;//保存轮播图，从公共图库选择
                        //case "upuserbanner": _strContent.Append(UpUserBanner(context)); break;//上传轮播图
                        case "getbanner": _strContent.Append(GetBanner(context)); break;//获取公共轮播图（userId=0）
                        case "getuserbanner": _strContent.Append(GetUserBanner(context)); break;//获取私人轮播图
                        case "delbanner": _strContent.Append(DelBanner(context)); break;//删除
                        #endregion

                        #region 发布设置
                        case "getsetting": _strContent.Append(GetSetting(context)); break;//读取配置
                        case "subsetting": _strContent.Append(SubSetting(context)); break;//提交配置
                        #endregion

                        #region 手动发布
                        case "startpub": _strContent.Append(StartPub(context)); break;//开始发布
                        case "stoppub": _strContent.Append(StopPub(context)); break;//停止发布
                        #endregion

                        #region 日志读取
                        case "readlog": _strContent.Append(ReadLog(context)); break;//日志读取前十条
                        #endregion

                        #region 首页条数及信息显示
                        case "getuserpubcount": _strContent.Append(GetUserPubCount(context)); break;
                        #endregion

                        #region 首页发布详情
                        case "getpubdetail": _strContent.Append(GetPubDetail(context)); break;
                        #endregion

                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        #region 产品/新闻
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetProductList(HttpContext context)
        {
            productBLL bll = new productBLL();
            List<productInfo> pList = new List<productInfo>();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                pList = bll.GetProductList(string.Format(" where userId='{0}'", userId));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { productList = pList });
        }
        /// <summary>
        /// 获取单个产品信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetProductById(HttpContext context)
        {
            string productId = context.Request["productId"];
            productBLL bll = new productBLL();
            productInfo pInfo = new productInfo();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                pInfo = bll.GetProductInfo(string.Format(" where userId='{0}' and Id='{1}'", userId, productId));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { product = pInfo });
        }
        /// <summary>
        /// 增加或修改产品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveProduct(HttpContext context)
        {
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                productBLL bll = new productBLL();
                string strjson = context.Request["params"];
                var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                productInfo product = JsonConvert.DeserializeObject<productInfo>(strjson, js);
                product.userId = model.Id;
                if (product.Id == 0)
                    bll.AddProduct(product);
                else
                    bll.UpdateProduct(product);
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除产品列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelProduct(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            productBLL bll = new productBLL();
            int a = bll.DelProduct(id);
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
            string pId = context.Request["productId"];
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
                        throw new Exception("图片最大只能传2M");
                    string newfileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileDir = HttpContext.Current.Server.MapPath("~/upfiles/" + MyInfo.user + "/");
                    if (!Directory.Exists(fileDir))
                    {
                        Directory.CreateDirectory(fileDir);
                    }
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
        /// 获取此用户下所有图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPicList(HttpContext context)
        {
            imageBLL bll = new imageBLL();
            string pId = context.Request["productId"];
            List<imageInfo> iList = new List<imageInfo>();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                iList = bll.GetImgList(string.Format(" where userId='{0}' and productId='{1}' order by addTime desc", userId, pId));
                if (iList == null || iList.Count < 1)
                    return json.WriteJson(1, "", new { });
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
            int a = 0;
            try
            {
                string id = context.Request["Id"];
                string imgfath = context.Request["imageURL"];
                imageBLL bll = new imageBLL();
                if (imgfath != "")
                    File.Delete(HttpContext.Current.Server.MapPath("~" + imgfath));
                a = bll.DelImg(id);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region 段落
        /// <summary>
        /// 获取此会员下所有段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetParaList(HttpContext context)
        {
            string pId = context.Request["productId"];
            string pageIndex = context.Request["page"];
            string pageSize = context.Request["pageSize"];
            if (string.IsNullOrEmpty(pageIndex))
                pageIndex = "1";
            if (string.IsNullOrEmpty(pageSize))
                pageSize = "10";
            paragraphBLL bll = new paragraphBLL();
            List<paragraphInfo> pList = new List<paragraphInfo>();
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            string userId = model.Id.ToString();
            int total = 0;
            try
            {
                pList = bll.GetParagraphList(string.Format(" where userId='{0}' and productId='{1}'", userId, pId), "desc", int.Parse(pageIndex), int.Parse(pageSize));
                total = bll.GetPageTotal(string.Format(" where userId='{0}' and productId='{1}' ", userId, pId));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { total, paraList = pList });
        }
        /// <summary>
        /// 增加或修改段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavePara(HttpContext context)
        {
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            paragraphBLL bll = new paragraphBLL();
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            try
            {
                List<paragraphInfo> paraList = JsonConvert.DeserializeObject<List<paragraphInfo>>(strjson, js);
                if (paraList[0].Id == 0)//新增段落，针对多个
                {
                    if (paraList != null && paraList.Count > 0)
                    {
                        int num = 1000001 + bll.GetNumId();
                        for (int i = 0; i < paraList.Count; i++)
                        {
                            paraList[i].paraId = "N" + num.ToString();
                            paraList[i].userId = model.Id;//会员Id
                            if (bll.AddParagraph(paraList[i]) == 1)
                                num++;
                        }
                    }
                }
                else//更新，只针对一个
                    bll.UpdateParagraph(paraList[0]);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
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
            try
            {
                int a = bll.DelParagraph(id);
                return json.WriteJson(1, "删除成功", new { });
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
        }
        /// <summary>
        /// 多行删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelParas(HttpContext context)
        {
            paragraphBLL bll = new paragraphBLL();
            string id = context.Request["Ids"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            try
            {
                string[] ids = id.Split(',');
                int a = 0;
                foreach (string d in ids)
                    a = bll.DelParagraph(d);
                return json.WriteJson(1, "删除成功", new { });
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
        }
        /// <summary>
        /// 段落一键整理
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string OneKeyDealPara(HttpContext context)
        {
            string para = context.Request["params"];
            List<string> strList = new List<string>();
            try
            {
                para = Regex.Replace(para, "0?(13|14|15|16|17|18)[0-9]{9}", " ");
                para = Regex.Replace(para, "[0-9-()（）]{7,18}", " ");
                para = Regex.Replace(para, "^((https|http|ftp|rtsp|mms)?:\\/\\/)[^\\s]+", " ");
                para = Regex.Replace(para, "^(http|https|ftp)\\://[a-zA-Z0-9\\-\\.]+\\.[a-zA-Z]{2,3}(:[a-zA-Z0-9]*)?/?([a-zA-Z0-9\\-\\._\\?\\,\\'/\\\\\\+&$%\\$#\\=~])*$", " ");
                para = Regex.Replace(para, "((http|ftp|https)://)(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\\&%_\\./-~-]*)?", " ");
                para = Regex.Replace(para, "(([a-zA-Z0-9\\._-]+\\.[a-zA-Z]{2,6})|([0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}))(:[0-9]{1,4})*(/[a-zA-Z0-9\\&%_\\./-~-]*)?", " ");
                para = Regex.Replace(para, "\\w[-\\w.+]*@([A-Za-z0-9][-A-Za-z0-9]+\\.)+[A-Za-z]{2,14}", " ");
                para = Regex.Replace(para, "[1-9]([0-9]{5,11})", " ");
                //para = Regex.Replace(para, @"([零一二三四五六七八九十百千万壹贰叁肆伍陆柒捌玖拾佰仟亿]+亿)?零?([一二三四五六七八九十百千壹贰叁肆伍陆柒捌玖拾佰仟]+万)?零?([一二三四五六七八九十百壹贰叁肆伍陆柒捌玖拾佰][千仟])?零?([一二三四五六七八九十壹贰叁肆伍陆柒捌玖拾][百佰])?零?([一二三四五六七八九壹贰叁肆伍陆柒捌玖]?[十拾])?零?([一二三四五六七八九壹贰叁肆伍陆柒捌玖])?", " ");
                para = Regex.Replace(para, @"\d", " ").Replace("、", " ").Replace(".", " ").Replace("：", " ").Replace("①", " ")
                    .Replace("②", " ").Replace("③", " ").Replace("④", " ").Replace("⑤", " ").Replace("⑥", " ").Replace("⑦", " ").Replace("⑧", " ").Replace("⑨", " ")
                    .Replace("⑩", " ").Replace("⑪", " ").Replace("⑫", " ").Replace("⑬", " ").Replace("（", " ").Replace("）", " ").Replace("(", " ").Replace("）", " ")
                    .Replace("⑴", " ").Replace("⑵", " ").Replace("⑶", " ").Replace("⑷", " ").Replace("⑸", " ").Replace("⑹", " ").Replace("⑺", " ").Replace("⑻", " ").Replace("⑼", " ")
                    .Replace("⒈", " ").Replace("⒉", " ").Replace("⒊", " ").Replace("⒋", " ").Replace("⒌", " ").Replace("⒍", " ").Replace("⒎", " ").Replace("⒏", " ").Replace("⒐", " ").Replace("⒑", " ");
                string text = para.Replace("\r\n", "").Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\u3000", "").Replace("“", "").Replace("”", "");
                string[] array = text.Split(new char[]
                {
                '。',
                '？',
                '！'
                });
                if (array.Length > 1)
                {
                    string text2 = "";
                    for (int i = 0; i < array.Length; i++)
                    {
                        string text3 = array[i];
                        if (text3.Length <= 200)
                        {
                            text2 = text2 + text3 + "。";
                            if (text2.Length > 200)
                            {
                                if (text3.StartsWith("\u3000\u3000"))
                                {
                                    strList.Add(text2);
                                }
                                else
                                {
                                    strList.Add("\u3000\u3000" + text2);
                                }
                                text2 = "";
                            }
                        }
                        else if (text3.Length > 200 && text3.Length < 250)
                        {
                            if (text3.StartsWith("\u3000\u3000"))
                            {
                                strList.Add(text3 + "。");
                            }
                            else
                            {
                                strList.Add("\u3000\u3000" + text3 + "。");
                            }
                        }
                        else if (text3.Length >= 250)
                        {
                            text3 = text3.Substring(0, 250);
                            if (text3.StartsWith("\u3000\u3000"))
                            {
                                strList.Add(text3 + "。");
                            }
                            else
                            {
                                strList.Add("\u3000\u3000" + text3 + "。");
                            }
                        }
                    }
                }
                else if (array.Length < 2 && text.Length > 220)
                {
                    for (int i = 0; i < text.Length - 220; i += 220)
                    {
                        string text3 = text.Substring(i, 220);
                        if (text3.StartsWith("\u3000\u3000"))
                        {
                            strList.Add(text3 + "。");
                        }
                        else
                        {
                            strList.Add("\u3000\u3000" + text3 + "。");
                        }
                    }
                }
                //para = string.Join("\r\n", strList.ToArray());
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { paralist = strList });
        }
        /// <summary>
        /// 段落一键采集
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string OneKeyGather(HttpContext context)
        {
            string strjson = context.Request["params"];//网址
            string urltxt = strjson;
            List<gather> galist = new List<gather>();
            try
            {
                if (!urltxt.EndsWith("/"))
                    urltxt += "/";
                string html = "";
                try
                {
                    html = NetHelper.HttpGet(urltxt, "", Encoding.GetEncoding("UTF-8"));
                }
                catch (Exception ex)
                {
                    string pattern2 = "(http|https)://(?<domain>[^(:|/]*)";
                    Match match2 = Regex.Match(strjson, pattern2);
                    urltxt = match2.Groups[0].ToString();
                    if (!urltxt.EndsWith("/"))
                        urltxt += "/";
                    html = NetHelper.HttpGet(urltxt, "", Encoding.GetEncoding("UTF-8"));
                }
                if (isLuan(html))
                    html = NetHelper.HttpGet(urltxt, "", Encoding.GetEncoding("gb2312"));

                string pattern3 = "(?is)<a(?:(?!href=).)*href=(['\"]?)(?<url>[^\"\\s>]*)\\1[^>]*>(?<text>(?:(?!</?a\\b).)*)</a>";
                MatchCollection matchCollection = Regex.Matches(html, pattern3, RegexOptions.Multiline);
                foreach (Match match3 in matchCollection)
                {
                    string input = Regex.Replace(match3.Groups["text"].Value, "<[^>]*>", string.Empty);
                    string title = Regex.Replace(input, "\\s", "").Replace("&nbsp;", "").Replace("&quot", "").Replace("&raquo", "");
                    if (Encoding.Default.GetByteCount(title) < 18)
                        continue;
                    gather gather = new gather();
                    gather.title = title;
                    string url = match3.Groups["url"].Value;
                    if (string.IsNullOrEmpty(url))
                        continue;
                    if (url.StartsWith("http://") || url.StartsWith("https://"))
                        gather.url = url;
                    else
                    {
                        string pattern2 = "(http|https)://(?<domain>[^(:|/]*)";
                        Match match2 = Regex.Match(strjson, pattern2);
                        urltxt = match2.Groups[0].ToString();
                        if (!urltxt.EndsWith("/"))
                            urltxt += "/";
                        gather.url = urltxt + url;
                    }
                    gather.isdeal = false;
                    galist.Add(gather);
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { paraList = galist });
        }
        /// <summary>
        /// 段落预览
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string Preview(HttpContext context)
        {
            string url = context.Request["params"];//网址
            string content = "";
            try
            {
                //string url = dgvtitlegather.Rows[e.RowIndex].Cells["urls"].Value.ToString();
                string html = NetHelper.HttpGet(url, "", Encoding.GetEncoding("UTF-8"));
                if (isLuan(html))//判断是否有乱码
                    html = NetHelper.HttpGet(url, "", Encoding.GetEncoding("gb2312"));//有乱码，用gbk编码再查一次
                string p = GetContent(html);//获取p标签内容
                //p = HttpUtility.UrlDecode(p, System.Text.Encoding.Unicode);
                if (p == "")
                    return json.WriteJson(0, "获取段落失败，跳过", new { para = content });
                else
                    content = formatHTML(p);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, "获取段落失败，跳过", new { para = content });
            }
            return json.WriteJson(1, "成功", new { para = content });
        }
        /// <summary>
        /// 一键编辑
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string OneKeyEdit(HttpContext context)
        {
            string content = "";
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            try
            {
                List<string> pUrl = JsonConvert.DeserializeObject<List<string>>(strjson, js);
                int page = pUrl.Count;//采集到的行数
                if (page > 30)
                    page = 30;
                int i = 0;
                while (i < page)
                {
                    string url = pUrl[i];
                    string text = "";
                    try
                    {
                        text = NetHelper.HttpGet(url, "", Encoding.GetEncoding("UTF-8"));
                    }
                    catch (Exception ex)
                    {
                        i++;
                        continue;
                    }
                    if (this.isLuan(text))
                    {
                        text = NetHelper.HttpGet(url, "", Encoding.GetEncoding("gb2312"));
                    }
                    string htmlContent = this.GetContent(text);
                    content += this.formatHTML(htmlContent);
                    i++;
                }
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { paras = content });
        }
        /// <summary>
        /// 根据链接采集文章
        /// </summary>
        /// <param name="html"></param>
        /// <returns></returns>
        public string GetContent(string html)
        {
            string content = string.Empty;
            string reg = @"(?i)<p[^>]*?>([\s\S]*?)</p>";
            string pp = "";
            MatchCollection mcTable = Regex.Matches(html, reg);
            foreach (Match mTable in mcTable)
            {
                pp = Regex.Replace(mTable.Groups[1].Value, "<[^>]*>", string.Empty, RegexOptions.Singleline);
                string p1 = Regex.Replace(pp, @"\s", "");
                if (System.Text.Encoding.Default.GetByteCount(p1) > 150)
                {
                    if (p1.Contains("Copyright") || p1.Contains("copyright") || p1.Contains("版权所有") || p1.Contains("技术支持："))
                        continue;
                    if (p1.Contains("新浪") || p1.Contains("关于我们") || p1.Contains("ICP") || p1.Contains("法律顾问："))
                        continue;
                    if (p1.Contains("上一篇：") || p1.Contains("下一篇：") || p1.Contains("微信扫描二维码") || p1.Contains("微信扫码二维码")
                        || p1.Contains("分享至好友和朋友圈"))
                        continue;
                    if (p1.Contains("来源：") || p1.Contains("【") || p1.Contains("s后自动返回") || p1.Contains("用户名")
                        || p1.Contains("地址：") || p1.Contains("客户端"))
                        continue;
                    if (p1.Contains("|退出") || p1.Contains("◎文/图")
                        || p1.Contains("评论()"))
                        continue;
                    content += "    " + p1 + "\r\n";
                }
            }
            //if (content.EndsWith("\r\n") && content.Length > 2)
            //    return content.Substring(0, content.Length - 2);
            //else
            return content;
        }
        /// <summary>
        /// 描述:格式化网页源码
        /// </summary>
        /// <param name="htmlContent"></param>
        /// <returns></returns>
        public string formatHTML(string htmlContent)
        {
            string result = "";

            result = htmlContent.Replace("&raquo;", "").Replace("&nbsp;", "")
                    .Replace("&copy;", "").Replace("/r", "").Replace("/t", "")
                    .Replace("/n", "").Replace("&amp;", "&").Replace("&rdquo;", "")
                    .Replace("&ldquo;", "").Replace("&quot;", "").Replace("-", "")
                    .Replace("原文链接", "").Replace("推荐帖子", "").Replace("注册", "")
                    .Replace("登陆", "").Replace("登录", "").Replace("&hellip;", "")
                    .Replace("相关阅读", "").Replace("特价影票4折起在线选座", "")
                    .Replace("忘记密码？", "").Replace("&rarr", "").Replace("&", "");
            return result;
        }
        /// <summary>
        /// 判断网页是否有乱码
        /// </summary>
        /// <param name="txt"></param>
        /// <returns></returns>
        bool isLuan(string txt)
        {
            var bytes = Encoding.UTF8.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
        }
        bool isLuan1(string txt)
        {
            var bytes = Encoding.Default.GetBytes(txt);
            //239 191 189
            for (var i = 0; i < bytes.Length; i++)
            {
                if (i < bytes.Length - 3)
                    if (bytes[i] == 239 && bytes[i + 1] == 191 && bytes[i + 2] == 189)
                    {
                        return true;
                    }
            }
            return false;
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
            string pId = context.Request["productId"];
            contentMouldBLL bll = new contentMouldBLL();
            List<contentMouldInfo> cList = new List<contentMouldInfo>();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                cList = bll.GetContentList(string.Format(" where userId='{0}' and productId='{1}'", userId, pId));
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
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            contentMouldInfo content = JsonConvert.DeserializeObject<contentMouldInfo>(strjson, js);
            content.userId = model.Id;
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

        #region 长尾词/关键词
        /// <summary>
        /// 获取公共长尾词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPublicTailwordList(HttpContext context)
        {
            tailwordBLL bll = new tailwordBLL();
            List<tailwordInfo> tList = new List<tailwordInfo>();
            try
            {
                tList = bll.GetTailwordList();
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { tailwordList = tList });
        }
        /// <summary>
        /// 获取私人长尾词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetWordsList(HttpContext context)
        {
            string pId = context.Request["productId"];
            wordsBLL bll = new wordsBLL();
            string wordType = context.Request["wordType"];//长尾词/关键词类型
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            string userId = model.Id.ToString();//用户id
            wordsInfo wInfo = new wordsInfo();
            try
            {
                DataTable dt = bll.GetWords(string.Format(" where userId='{0}' and wordType='{1}' and productId='{2}'", userId, wordType, pId));
                if (dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    wInfo.Id = (int)row["Id"];
                    wInfo.words = (string)row["words"];
                    wInfo.editTime = ((DateTime)row["editTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                    wInfo.wordType = (string)row["wordType"];
                    wInfo.userId = (int)row["userId"];
                    wInfo.productId = (int)row["productId"];
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { wordsList = wInfo });
        }
        /// <summary>
        /// 增加或修改长尾词/关键词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveWords(HttpContext context)
        {
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            wordsBLL bll = new wordsBLL();
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            wordsInfo words = JsonConvert.DeserializeObject<wordsInfo>(strjson, js);
            words.userId = model.Id;
            if (words.Id == 0)
                bll.AddWords(words);
            else
                bll.UpdateWords(words);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除长尾词/关键词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelWords(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            wordsBLL bll = new wordsBLL();
            int a = bll.DelWords(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        /// <summary>
        /// 关键词挖掘
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DigWords(HttpContext context)
        {
            string word = context.Request["word"];//搜索关键词
            if (string.IsNullOrEmpty(word))
                return json.WriteJson(0, "关键词不能为空", new { });
            List<string> wList = new List<string>();
            try
            {
                string uname = "huachengtl";
                string key = NetHelper.GetMD5(uname + "fangyuan888");
                var f = new StringBuilder();
                f.AppendFormat("username={0}&", uname);
                f.AppendFormat("key={0}&", key);
                f.AppendFormat("word={0}&", word.Trim());
                string main1 = NetHelper.HttpPost("http://vip.hsoow.com/index.php?m=member&c=index&a=caiji", f.ToString(), "");
                if (main1 == "")
                { return json.WriteJson(0, "暂未搜到相关数据", new { }); }
                JObject jo = (JObject)JsonConvert.DeserializeObject(main1);
                string code = jo["code"].ToString();
                string count = jo["count"].ToString();
                string data = jo["data"].ToString();
                if (code == "0")//失败
                { throw new Exception(); }
                else if (code == "1")//成功
                {
                    foreach (var w in jo["data"])
                        wList.Add(w["word"].ToString());
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, "暂未搜到相关数据," + ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { wordsList = wList });
        }
        #endregion

        #region 标题
        /// <summary>
        /// 发布详情用，获取所有标题列表,待发，已发
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetAllTitleList(HttpContext context, int isPub, string orderby)
        {
            string pageIndex = context.Request["page"];
            string pageSize = context.Request["pageSize"];
            if (string.IsNullOrEmpty(pageIndex))
                pageIndex = "1";
            if (string.IsNullOrEmpty(pageSize))
                pageSize = "10";
            titleBLL bll = new titleBLL();
            List<titleInfo> tList = new List<titleInfo>();
            int total = 0;
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                tList = bll.GetTitleList(string.Format(" where userId={0} and isSucceedPub={1} ", userId, isPub), orderby, int.Parse(pageIndex), int.Parse(pageSize));
                total = bll.GetPageTotal(string.Format(" where userId='{0}' and isSucceedPub='{1}' ", userId, isPub));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { total, titleList = tList, dtNow = DateTime.Now });
        }
        /// <summary>
        /// 产品内，获取标题列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetTitleList(HttpContext context)
        {
            string pId = context.Request["productId"];
            string pageIndex = context.Request["page"];
            string pageSize = context.Request["pageSize"];
            if (string.IsNullOrEmpty(pageIndex))
                pageIndex = "1";
            if (string.IsNullOrEmpty(pageSize))
                pageSize = "10";
            titleBLL bll = new titleBLL();
            List<titleInfo> tList = new List<titleInfo>();
            int total = 0;
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                tList = bll.GetTitleList(string.Format(" where userId='{0}' and productId='{1}' ", userId, pId), "desc", int.Parse(pageIndex),
                    int.Parse(pageSize));
                total = bll.GetPageTotal(string.Format(" where userId='{0}' and productId='{1}' ", userId, pId));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { total, titleList = tList });
        }
        /// <summary>
        /// 增加或修改标题列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveTitle(HttpContext context)
        {
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            titleBLL bll = new titleBLL();
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            try
            {
                List<titleInfo> tList = JsonConvert.DeserializeObject<List<titleInfo>>(strjson, js);
                foreach (titleInfo t in tList)
                {
                    t.userId = model.Id;
                    if (t.Id == 0)
                        bll.AddTitle(t);
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除标题列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelTitle(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            titleBLL bll = new titleBLL();
            int a = bll.DelTitle(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        /// <summary>
        /// 多行删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelTitles(HttpContext context)
        {
            titleBLL bll = new titleBLL();
            string id = context.Request["Ids"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            try
            {
                string[] ids = id.Split(',');
                int a = 0;
                foreach (string d in ids)
                    a = bll.DelTitle(d);
                return json.WriteJson(1, "删除成功", new { });
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
        }
        /// <summary>
        /// 一键清空已发布标题
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelPubbedTitles(HttpContext context)
        {
            titleBLL bll = new titleBLL();
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            try
            {
                int a = 0;
                a = bll.DelPubbedTitle(model.Id.ToString());
                return json.WriteJson(1, "删除成功", new { });
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
        }
        #endregion

        #region 公司信息
        /// <summary>
        /// 获取公司信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetOrgInfo(HttpContext context)
        {
            CmUserBLL bll = new CmUserBLL();
            cmUserInfo orgInfo = new cmUserInfo();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                orgInfo = bll.GetUser(string.Format(" where Id='{0}'", userId));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { orgInfo });
        }
        /// <summary>
        /// 修改公司信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpdateOrgInfo(HttpContext context)
        {
            CmUserBLL bll = new CmUserBLL();
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            try
            {
                cmUserInfo orjInfo = JsonConvert.DeserializeObject<cmUserInfo>(strjson, js);
                bll.UpdateUser(orjInfo);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { });
        }
        #endregion

        #region 轮播图
        /// <summary>
        /// 上传轮播图
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpBanner(HttpContext context)
        {
            int num = int.Parse(context.Request["num"]);
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
                        throw new Exception("图片最大只能传2M");
                    string newfileName = DateTime.Now.ToString("yyyyMMddHHmmss");
                    string fileDir = HttpContext.Current.Server.MapPath("~/banner/" + MyInfo.user + "/");
                    if (!Directory.Exists(fileDir))
                    {
                        Directory.CreateDirectory(fileDir);
                    }
                    string saveDir = fileDir + newfileName + "." + suffix;//文件服务器存放路径
                    fileUrl = "/banner/" + username + "/" + newfileName + "." + suffix;
                    _upfile.SaveAs(saveDir);//保存图片
                    #region 存到sql图片库
                    bannerBLL bll = new bannerBLL();
                    bannerInfo bInfo = new bannerInfo();
                    bInfo.banner = fileUrl;
                    bInfo.num = num;
                    bInfo.userId = model.Id;
                    bll.AddBanner(bInfo);
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
        /// 保存轮播图，从公共库选择
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveBanner(HttpContext context)
        {
            int num = int.Parse(context.Request["num"]);
            string fileUrl = context.Request["fileUrl"];
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                #region 存到sql图片库
                bannerBLL bll = new bannerBLL();
                bannerInfo bInfo = new bannerInfo();
                bInfo.banner = fileUrl;
                bInfo.num = num;
                bInfo.userId = model.Id;
                bll.AddBanner(bInfo);
                #endregion
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.Message, new { });
            }
            return json.WriteJson(1, "保存成功", new { });
        }

        /// <summary>
        /// 更新私人轮播图
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpUserBanner(HttpContext context)
        {
            bannerBLL bll = new bannerBLL();
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            bannerInfo bInfo = new bannerInfo();
            try
            {
                string strjson = context.Request["params"];
                var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                bInfo = JsonConvert.DeserializeObject<bannerInfo>(strjson, js);
                bll.UpUserBanner(bInfo);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 获取公共轮播图
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetBanner(HttpContext context)
        {
            bannerBLL bll = new bannerBLL();
            List<bannerInfo> bInfo = new List<bannerInfo>();
            try
            {
                //cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                bInfo = bll.GetBanner(" where userId=0");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { bInfo });
        }
        /// <summary>
        /// 获取私人轮播图
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserBanner(HttpContext context)
        {
            bannerBLL bll = new bannerBLL();
            List<bannerInfo> bInfo = new List<bannerInfo>();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                bInfo = bll.GetBanner(string.Format(" where userId={0}", model.Id));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { bInfo });
        }
        /// <summary>
        /// 删除图片
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelBanner(HttpContext context)
        {
            int a = 0;
            try
            {
                string id = context.Request["Id"];
                string imgfath = context.Request["imageURL"];
                bannerBLL bll = new bannerBLL();
                if (imgfath != "")
                    File.Delete(HttpContext.Current.Server.MapPath("~" + imgfath));
                a = bll.DelBanner(id);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region 发布设置
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name = "context" ></param >
        /// <returns></returns>
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
        /// <param name = "context" ></param>
        /// <returns ></returns>
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
                setInfo.username = model.username;
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

        #region 手动发布
        private settingBLL sBll = new settingBLL();
        /// <summary>
        /// 开始手动发布
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string StartPub(HttpContext context)
        {
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            try
            {
                //取待发标题
                 titleBLL tBLL = new titleBLL();
                //取出未发布标题
                bool isExit = tBLL.IsExitNoPubTitle(string.Format(" where userId={0} and isSucceedPub=0", model.Id));
                if (!isExit)
                {
                    log.wlog("发布停止：待发标题数量不足，请及时生成标题", model.Id.ToString(), model.username);
                }
                else
                {
                    //Pub.Publish(model.Id);//创建发布线程
                    //更新此用户发布状态
                    settingBLL sBll = new settingBLL();
                    sBll.UpIsPubing(1, model.Id);//isPubing置true
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.Message, new { });
            }
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 停止手动发布
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string StopPub(HttpContext context)
        {
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            try
            {
                sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.Message, new { });
            }
            return json.WriteJson(1, "成功", new { });
        }
        #endregion

        #region 日志读取
        /// <summary>
        /// 倒序读取前十条日志
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ReadLog(HttpContext context)
        {
            logBLL lbll = new logBLL();
            List<logInfo> logList = new List<logInfo>();
            cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
            try
            {
                logList = lbll.GetLogs(string.Format(" where userId={0} order by addtime desc", model.Id));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.Message, new { });
            }
            return json.WriteJson(1, "成功", new { logList });
        }
        #endregion

        #region 首页条数及信息显示
        /// <summary>
        /// 首页条数及信息显示
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetUserPubCount(HttpContext context)
        {
            CmUserBLL bll = new CmUserBLL();
            userPubCount uPub = new userPubCount();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                uPub = bll.GetUserPubCount(string.Format(" where Id='{0}'", userId));
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { UserPubCount = uPub });
        }
        #endregion

        #region 首页发布详情
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetPubDetail(HttpContext context)
        {
            settingBLL bll = new settingBLL();
            pubDetail pDetail = new pubDetail();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                pDetail = bll.GetPubDeitail(userId);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            return json.WriteJson(1, "成功", new { pDetail });
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