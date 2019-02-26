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

                        #region 发布
                        //case "publish": _strContent.Append(Publish()); break;//读取配置
                        //Publish(); break;//读取配置
                        #endregion

                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        #region 轮循setting表
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
        private int uId = 0;
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
                {
                    uId = sInfo.userId;
                    Publish(uId);//创建发布线程
                    sInfo.isPubing = true;
                    bll.UpIsPubing(sInfo);//isPubing置true
                }
            }
        }
        #endregion

        #region 发布


        private string Publish(int uId)
        {
            try
            {
                Thread t = null;
                t = new Thread(new ParameterizedThreadStart(PubTitle));
                t.IsBackground = true;
                t.Start(uId);
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "创建线程成功", new { });
        }
        private void PubTitle(object uId)
        {
            CmUserBLL cBLL = new CmUserBLL();
            int sleeptime = 60;
            cmUserInfo model = cBLL.GetUser(string.Format(" where Id={0}", uId));
            try
            {
                //mgcs = getmgc();//敏感词
                bool islimited = false;//条数
                string title = "";
                string content = "";
                Random rnd = new Random();
                List<string> htmllist = new List<string>();
                //声明参数
                string txtgytitle = "", txtgydesc = "";
                string thumb = "";
                string sKeyword1 = "";

                //取待发标题
                titleBLL tBLL = new titleBLL();
                //取出未发布标题
                List<titleInfo> tList = tBLL.GetTitleList(string.Format(" where userId={0} and isSucceedPub=0 order by addTime", model.Id));
                if (tList == null || tList.Count < 1)
                {
                    log.wlog("发布停止：待发标题数量不足，请及时生成标题", model.Id.ToString(), model.username);
                    StopPub(model.Id);
                    return;
                    //System.Threading.Thread.CurrentThread.Abort();
                }
                for (int i = 0; i < tList.Count; i++)  //选中项遍历
                {
                    titleInfo tInfo = tList[i];
                    //公共信息
                    title = tInfo.title.Replace("&", "%26");
                    txtgytitle = title.Trim();//信息标题
                    //取模板
                    contentMouldBLL cmBLL = new contentMouldBLL();
                    List<contentMouldInfo> cList = cmBLL.GetContentList(string.Format(" where userId={0} and productId={1}", model.Id, tInfo.productId));//根据此标题所属产品Id找模板
                    if (cList == null || cList.Count < 1)
                    {
                        log.wlog("发布停止：模板数量不足，请及时配置模板", model.Id.ToString(), model.username);
                        StopPub(model.Id);
                        return;
                    }
                    int index = rnd.Next(cList.Count);
                    content = cList[index].contentMould;//随机调用模板
                    cmBLL.UpUsedCount(cList[index].Id);//模板调用次数加1

                    content = Regex.Replace(content, "(?i)<IMG.*>", "");//过滤用户插入的本地图片                                                
                    content = ReplaceHTMLWZ(content, tInfo, model);//替换模板中变量
                    if (content == "段落数量不足")
                    {
                        log.wlog("发布停止：段落数量不足，请及时生成段落", model.Id.ToString(), model.username);
                        StopPub(model.Id);
                        return;
                    }
                    txtgydesc = content;

                    #region 敏感词过滤
                    //手机号码
                    txtgytitle = Regex.Replace(txtgytitle, "0?(13|14|15|16|17|18)[0-9]{9}", " ");
                    //电话号码
                    txtgytitle = Regex.Replace(txtgytitle, "[0-9-()（）]{7,18}", " ");
                    //过滤a标签
                    txtgydesc = Regex.Replace(txtgydesc, @"(?is)<a[^>]*href=([""'])?(?<href>[^'""]+)\1[^>]*>", " ");

                    //敏感词过滤
                    //txtgytitle = changemgc(txtgytitle);
                    //txtgydesc = changemgc(txtgydesc);
                    //sKeyword1 = changemgc(sKeyword1);
                    //sKeyword2 = changemgc(sKeyword2);
                    //sKeyword3 = changemgc(sKeyword3);
                    #endregion
                    productBLL pBLL = new productBLL();
                    productInfo pInfo = pBLL.GetProductList(string.Format(" where userId={0}  and Id={1}", model.Id, tInfo.productId))[0];
                    string key = NetHelper.GetMD5(model.username + "100dh888");
                    StringBuilder strpost = new StringBuilder();
                    strpost.AppendFormat("catid={0}&", model.columnInfoId);
                    strpost.AppendFormat("title={0}&", txtgytitle);
                    strpost.AppendFormat("pinpai={0}&", pInfo.pinpai);
                    strpost.AppendFormat("xinghao={0}&", pInfo.xinghao);
                    strpost.AppendFormat("city={0}&", pInfo.city);
                    strpost.AppendFormat("gonghuo={0}&", pInfo.sumCount);
                    strpost.AppendFormat("qiding={0}&", pInfo.smallCount);
                    strpost.AppendFormat("price={0}&", pInfo.price);
                    strpost.AppendFormat("unit={0}&", pInfo.unit);
                    string desc = "<p>" + txtgydesc + "</p>";//内容,UrlEncode编码
                    //strpost.AppendFormat("content={0}&", Tools.Encode(desc,"12345678","87654321")); 
                    strpost.AppendFormat("content={0}&", desc);
                    strpost.AppendFormat("keywords={0}&", sKeyword1);
                    strpost.AppendFormat("style_color={0}&", "");
                    strpost.AppendFormat("style_font_weight={0}&", "");
                    strpost.AppendFormat("page_title_value={0}&", "");
                    strpost.AppendFormat("add_introduce={0}&", 1);
                    strpost.AppendFormat("introcude_length={0}&", 200);
                    strpost.AppendFormat("auto_thumb={0}&", 1);
                    strpost.AppendFormat("auto_thumb_no={0}&", 1);
                    strpost.AppendFormat("thumb={0}&", thumb);
                    strpost.AppendFormat("forward={0}&", "");
                    strpost.AppendFormat("id={0}&", "");
                    strpost.AppendFormat("username={0}&", model.username);
                    strpost.AppendFormat("key={0}&", key);
                    strpost.AppendFormat("dosubmit={0}&", "提交");
                    strpost.AppendFormat("version={0}&", "1.0.0.0");
                    #region 组织发布内容
                    //for (int i = 0; i < Myinfo.rjlist.Count; i++)
                    //{
                    //string host = Myinfo.rjlist[i].realmAddress;
                    //if (q % w == i)
                    //{
                    //地址根据不同网站变化，每个地址需要写一个接口
                    string titleurl = "";
                    string html = NetHelper.HttpPost("http://hyzx.100dh.cn/xinxi/handler/ModelHandler.ashx?action=moduleHtml", strpost.ToString());
                    JObject joo = (JObject)JsonConvert.DeserializeObject(html);
                    string code = joo["code"].ToString();
                    string msg = joo["msg"].ToString();
                    if (code == "1")//发布成功。
                    {
                        titleurl = joo["detail"]["url"].ToString();
                        //更新待发标题表
                        tInfo.isSucceedPub = true;
                        tInfo.returnMsg = titleurl;
                        tBLL.UpdateTitle(tInfo);
                        //更新userInfo表发布相关信息
                        //cBLL.UpUserPubInformation(model.Id);
                        log.wlog(titleurl, model.Id.ToString(), model.username);
                        Thread.Sleep(60 * 1000);
                        if (i == tList.Count - 1)
                        {
                            tList = tBLL.GetTitleList(string.Format(" where userId={0} and isSucceedPub=0 order by addTime", model.Id));
                            if (tList == null || tList.Count < 1)
                            {
                                log.wlog("发布停止：待发标题数量不足，请及时生成标题", model.Id.ToString(), model.username);
                                StopPub(model.Id);
                                return;
                            }
                        }
                        continue;//再发送本栏目信息
                    }
                    else if (code == "0")
                    {
                        if (msg.Contains("今日投稿已超过限制数"))
                        {
                            log.wlog(msg.ToString(), model.Id.ToString(), model.username);
                            StopPub(model.Id);
                            return;
                        }
                        if (msg.ToString().Contains("信息发布过快，请隔60秒再提交！"))
                        {
                            log.wlog(msg.ToString(), model.Id.ToString(), model.username);
                            Thread.Sleep(sleeptime * 1000);
                            continue;
                        }
                        else
                        {
                            log.wlog(msg.ToString(), model.Id.ToString(), model.username);
                            Thread.Sleep(sleeptime * 1000);
                            continue;
                        }
                    }
                    else if (html.Contains("无法解析此远程名称") || html.Contains("无法连接到远程服务器") || html.Contains("remote name could not be resolved"))
                    {
                        log.wlog(msg.ToString(), model.Id.ToString(), model.username);
                        Thread.Sleep(sleeptime * 1000);
                        continue;//再发送本栏目信息
                    }
                    else
                    {
                        log.wlog(msg.ToString(), model.Id.ToString(), model.username);
                        Thread.Sleep(sleeptime * 1000);
                        continue;
                    }
                    #endregion
                }
            }
            catch (Exception ex)
            {
                log.wlog(ex.ToString(), model.Id.ToString(), model.username);
                Thread.Sleep(sleeptime * 1000);
            }
        }
        /// <summary>
        /// 停止发布，终止本线程，将此用户setting表isPubing置false
        /// </summary>
        /// <param name="userId"></param>
        private void StopPub(int userId)
        {
            settingBLL bll = new settingBLL();
            settingInfo sInfo = new settingInfo();
            sInfo.userId = userId;
            sInfo.isPubing = false;
            bll.UpIsPubing(sInfo);
            //if (t != null)
            //    t.Abort();

        }

        /// <summary>
        /// 模板替换变量
        /// </summary>
        /// <param name="wz">模板</param>
        /// <param name="title"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public string ReplaceHTMLWZ(string wz, titleInfo tInfo, cmUserInfo user)
        {
            Regex r;
            Random rnd = new Random();
            string[] txt;
            string mybl = "";
            if (wz.Contains("{标题}"))
            {
                wz = wz.Replace("{标题}", tInfo.title);
            }
            imageBLL iBLL = new imageBLL();
            //根据userId,productId获取图片
            List<imageInfo> iList = iBLL.GetImgList(string.Format(" where userId='{0}' and productId='{1}' order by addTime desc", user.Id, tInfo.productId));
            while (wz.Contains("{图片}"))
            {
                r = new Regex("{图片}");
                if (iList.Count > 0)
                {
                    string t = iList[rnd.Next(iList.Count)].imageURL;
                    wz = r.Replace(wz, "<img src=\"" + t + "\" />", 1);
                }
                else
                {
                    break;
                }
            }
            paragraphBLL pBLL = new paragraphBLL();
            //根据userId,productId获取图片
            List<paragraphInfo> pList = pBLL.GetParagraphList(string.Format(" where userId='{0}' and productId='{1}' order by addTime desc", user.Id, tInfo.productId));
            if (pList == null || pList.Count < 1)
            {
                return "段落数量不足";
            }
            while (wz.Contains("{段落}"))
            {
                Regex regex = new Regex("{段落}");
                if (pList.Count <= 0)
                {
                    break;
                }
                try
                {
                    //更新写在这
                    //DataGridViewRow dataGridViewRow = this.dgvpracontent.Rows[rnd.Next(this.dgvpracontent.RowCount)];
                    //string str = dataGridViewRow.Cells[0].Value.ToString();
                    //int num3 = this.achelp.ExcuteSql("update paragraph set UsedCount=UsedCount+1 where ID=" + str);
                    int index = rnd.Next(pList.Count);
                    string text2 = pList[index].paraCotent;//调用段落
                    pBLL.UpUsedCount(pList[index].Id);//段落调用次数加1

                    wz = regex.Replace(wz, "<p>" + text2 + "</p>", 1);
                }
                catch (Exception ex)
                {
                }
            }
            wz = wz.Replace("&", "%26");
            return wz;
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