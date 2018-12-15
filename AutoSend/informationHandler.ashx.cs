using BLL;
using Model;
using Newtonsoft.Json;
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
                        case "getparalist": _strContent.Append(GetParaList(context)); break;//获取此会员下所有段落
                        case "savepara": _strContent.Append(SavePara(context)); break;
                        case "delpara": _strContent.Append(DelPara(context)); break;//删除段落
                        case "onekeydealpara": _strContent.Append(OneKeyDealPara(context)); break;//一键整理
                        case "onekeygather": _strContent.Append(OneKeyGather(context)); break;//一键采集
                        case "preview": _strContent.Append(Preview(context)); break;//一键采集

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
                    pInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
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
            int a = bll.DelParagraph(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
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
                string text = para.Replace("\r\n", "").Replace(" ", "").Replace("\r", "").Replace("\n", "").Replace("\u3000", "").Replace("“","").Replace("”","");
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
                    gather.isdeal = "---";
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
                {
                    html = NetHelper.HttpGet(url, "", Encoding.GetEncoding("gb2312"));//有乱码，用gbk编码再查一次
                }
                string p = GetContent(html);//获取p标签内容
                //p = HttpUtility.UrlDecode(p, System.Text.Encoding.Unicode);
                if (p == "")
                    return json.WriteJson(0, "获取段落失败，跳过", new { para = content });
                else
                {
                    //frmparagraphdeal frmpar = new frmparagraphdeal(f);
                    //frmpar.par = formatHTML(p);
                    //frmpar.isAdd = true;
                    //frmpar.ShowDialog();
                    //f.databind1();
                    content = formatHTML(p);
                }
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, "获取段落失败，跳过", new { para = content });
                //MessageBox.Show("获取段落失败，跳过");
                //dgvtitlegather.Rows[e.RowIndex].Cells["IsState"].Value = "跳过";
            }
            return json.WriteJson(1, "成功", new { para = content });
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
            contentMouldBLL bll = new contentMouldBLL();
            List<contentMouldInfo> cList = new List<contentMouldInfo>();
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                DataTable dt = bll.GetContentList(string.Format(" where userId='{0}'", userId));
                if (dt.Rows.Count < 1)
                    return json.WriteJson(0, "没有数据", new { });
                foreach (DataRow row in dt.Rows)
                {
                    contentMouldInfo cInfo = new contentMouldInfo();
                    cInfo.Id = (int)row["Id"];
                    cInfo.mouldId = (string)row["mouldId"];
                    cInfo.mouldName = (string)row["mouldName"];
                    cInfo.contentMould = (string)row["contentMould"];
                    cInfo.usedCount = (int)row["usedCount"];
                    cInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
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
            try
            {
                cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                string userId = model.Id.ToString();
                DataTable dt = bll.GetTailwordList(string.Format(" where userId={0}", userId));
                if (dt.Rows.Count < 1)
                    return json.WriteJson(0, "没有数据", new { });
                foreach (DataRow row in dt.Rows)
                {
                    tailwordInfo tInfo = new tailwordInfo();
                    tInfo.Id = (int)row["Id"];
                    tInfo.tailword = (string)row["tailword"];
                    tInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
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
                    iInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
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

        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}