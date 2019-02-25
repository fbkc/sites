﻿using BLL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;
using System.Web.SessionState;

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
                        #region 发布设置
                        case "getsetting": _strContent.Append(GetSetting(context)); break;//读取配置
                        case "subsetting": _strContent.Append(SubSetting(context)); break;//提交配置
                        #endregion

                        #region 轮循setting表
                        case "roundsetting": _strContent.Append(RoundSetting(context)); break;//读取配置
                        #endregion

                        #region 发布
                        //case "publish": _strContent.Append(Publish(context)); break;//读取配置
                        #endregion

                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        #region 发布设置
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name = "context" ></ param >
        /// < returns ></ returns >
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
        /// <param name = "context" ></ param >
        /// < returns ></ returns >
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
        public bool isstoppub = true;
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
                int nowMin =dt.Minute/ 10;//当前分钟十位数
                int tMin = sInfo.pubMin / 10;//用户所定时间十位数
                if (dt.Hour == sInfo.pubHour && nowMin == tMin)//若时间符合，调用发布接口
                {
                    isstoppub = true;
                    Publish();//创建发布线程
                    sInfo.isPubing = true;
                    bll.UpIsPubing(sInfo);//isPubing置true
                }
            }
        }
        #endregion

        #region 发布
        
        Thread t = null;
        private string Publish()
        {
            try
            {
                ThreadStart start = null;

                if (start == null)
                {
                    //cmUserInfo model = (cmUserInfo)context.Session["UserModel"];
                    start = PubTitle;
                }
                if (isstoppub)
                {
                    if (t != null)
                        t.Abort();
                    t = new Thread(start);
                    t.IsBackground = true;
                    t.Start();
                    isstoppub = false;
                }
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
        }
        private void PubTitle()
        {
            CmUserBLL cBLL = new CmUserBLL();
            cmUserInfo model = cBLL.GetUser(string.Format(" where Id='{0}'", 1));
            //mgcs = getmgc();
            bool cfb1 = true, cfb2 = true, cfb3 = true, cfb4 = true;
            int stime = 30, etime = 40, sleeptime = 30;
            int nums = 0;
            bool islimited = false;//条数
            string title = "";
            string content = "";
            string baidumsg = "";
            Random rnd = new Random();
            List<string> htmllist = new List<string>();
            //声明参数
            string txtgytitle = "", txtgydesc = "", cbid = "";
            string thumb1 = "", thumb2 = "", thumb = "";
            string sKeyword1 = "", sKeyword2 = "", sKeyword3 = "";
            string prehtml = "";
            int havecont = 0;

            //取待发标题
            titleBLL tBLL = new titleBLL();
            //取出未发布标题
            List<titleInfo> tList = tBLL.GetTitleList(string.Format(" where userId={0} and isSucceedPub=0 order by addTime", model.Id));
            foreach (titleInfo tInfo in tList)  //选中项遍历
            {
                if (!isstoppub)//停止
                {
                    try
                    {
                        //公共信息
                        title = tInfo.title.Replace("&", "%26");
                        txtgytitle = title.Trim();//信息标题
                        //取模板
                        contentMouldBLL cmBLL = new contentMouldBLL();
                        List<contentMouldInfo> cList = cmBLL.GetContentList(string.Format(" where userId='{0}' and productId='{1}'", model.Id, tInfo.productId));//根据此标题所属产品Id找模板
                        int index = rnd.Next(cList.Count);
                        content = cList[index].contentMould;//随机调用模板
                        cmBLL.UpUsedCount(cList[index].Id);//模板调用次数加1

                        content = Regex.Replace(content, "(?i)<IMG.*>", "");//过滤用户插入的本地图片                                                
                        content = ReplaceHTMLWZ(content, tInfo, model);//替换模板中变量
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
                        productInfo pInfo = pBLL.GetProduct1(string.Format(" where userId='{0}'  and productId='{1}'", model.Id, tInfo.productId))[0];
                        string key = NetHelper.GetMD5(model.Id + "100dh888");
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
                        string html = NetHelper.HttpPost("http://xinxi.100dh.cn/handler/ModelHandler.ashx?action=moduleHtml", strpost.ToString());
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
                            cBLL.UpUserPubInformation(model.Id);
                            Thread.Sleep(60 * 1000);
                            continue;//再发送本栏目信息
                        }
                        else if (code == "0")
                        {
                            if (msg.Contains("今日投稿已超过限制数"))
                            {
                                StopPub(model.Id);
                                return;
                            }
                            if (msg.ToString().Contains("信息发布过快，请隔60秒再提交！"))
                            {
                                Thread.Sleep(sleeptime * 1000);
                                continue;
                            }
                            else
                            {
                                //txttishi.Text += "出错:" + msg + "\r\n";
                                ////lvi.SubItems[1].Text = msg;
                                //lvi.SubItems[1].Text = "等待发送";
                                //lsvdaifa.Items.Remove(lvi); // 按索引移除 
                                //lsvdaifa.Items.Add(lvi.Clone() as ListViewItem); //失败标题重新加入代发列表
                                //                                                 //lsvshibai.Items.Add(lvi.Clone() as ListViewItem); sbnum++;
                                //UpdateTabNum();
                                //sleeptime = stime + rnd.Next(etime - stime);
                                //txttishi.Text += "随机等待" + sleeptime + "秒。\r\n";
                                //waitsecond = sleeptime;
                                //isstarttime = true;
                                Thread.Sleep(sleeptime * 1000);
                                continue;
                            }
                        }
                        else if (html.Contains("无法解析此远程名称") || html.Contains("无法连接到远程服务器") || html.Contains("remote name could not be resolved"))
                        {
                            //lvi.SubItems[1].Text = "网络无法连接！";
                            //lvi.SubItems[1].Text = "等待发送";
                            //lsvdaifa.Items.Remove(lvi); // 按索引移除 
                            //lsvdaifa.Items.Add(lvi.Clone() as ListViewItem); //失败标题重新加入代发列表
                            //                                                 //lsvshibai.Items.Add(lvi.Clone() as ListViewItem); 
                            //sbnum++;
                            //txttishi.Text += "网络无法连接！\r\n";
                            //sleeptime = stime + rnd.Next(etime - stime);
                            //txttishi.Text += "随机等待" + sleeptime + "秒。\r\n";
                            //waitsecond = sleeptime;
                            //isstarttime = true;
                            Thread.Sleep(sleeptime * 1000);
                            continue;//再发送本栏目信息
                        }
                        else
                        {
                            //txttishi.Text += "出错:情况未知，请查看日志！\r\n";
                            //AShelp.SaveTXT(html, Application.StartupPath + "\\错误记录\\" + title + ".txt");
                            ////lvi.SubItems[1].Text = Application.StartupPath + "\\错误记录\\" + title + ".txt";
                            //lvi.SubItems[1].Text = "等待发送";
                            //lsvdaifa.Items.Remove(lvi); // 按索引移除 
                            //lsvdaifa.Items.Add(lvi.Clone() as ListViewItem); //失败标题重新加入代发列表
                            //                                                 //lsvshibai.Items.Add(lvi.Clone() as ListViewItem); 
                            //sbnum++;
                            //UpdateTabNum();
                            //sleeptime = stime + rnd.Next(etime - stime);
                            //txttishi.Text += "随机等待" + sleeptime + "秒。\r\n";
                            //waitsecond = sleeptime;
                            //isstarttime = true;
                            Thread.Sleep(sleeptime * 1000);
                            continue;
                        }
                        #endregion
                    }
                    catch (Exception ex)
                    {
                        if (!(ex.Message.Contains("正在中止线程") || ex.Message.Contains("aborted")))
                        {
                            //errcount++;
                            ////lvi.SubItems[1].Text = ex.Message;
                            //lvi.SubItems[1].Text = "等待发送";
                            //lsvdaifa.Items.Remove(lvi); // 按索引移除 
                            //lsvdaifa.Items.Add(lvi.Clone() as ListViewItem); //失败标题重新加入代发列表
                            //                                                 //lsvshibai.Items.Add(lvi.Clone() as ListViewItem); sbnum++;
                            //txttishi.Text += ex.Message + "\r\n";
                            //sleeptime = stime + rnd.Next(etime - stime);
                            //txttishi.Text += "随机等待" + sleeptime + "秒。\r\n";
                            //waitsecond = sleeptime;
                            //isstarttime = true;
                            Thread.Sleep(sleeptime * 1000);
                        }
                    }
                }
                else
                {
                    //if (ispausd)
                    //{
                    //    if (!txttishi.Text.EndsWith("文章发布暂停。\r\n"))
                    //        txttishi.Text += "文章发布暂停。\r\n";
                    //}
                    //else
                    //{
                    //    if (!txttishi.Text.EndsWith("文章发布停止。\r\n"))
                    //        txttishi.Text += "文章发布停止。\r\n";
                    //}
                    //t = null;
                }
                //havecont = 0;
                //havecont = lsvdaifa.Items.Count;
                //label45.Text = string.Format("共有{0}个信息待发", havecont);
            }
            //            txttishi.Text += "文章发布完。\r\n";
            //            havecont = 0;
            //            isstoppub = true;
            //            ispausd = false;
            //            isstarttime = false;
            //            if (radioButton26.Checked && havecont< 1 && configlistbox.Items.Count> 1)
            //            {
            //                configlistbox.Items.Remove(Myinfo.configname);
            //                configlistbox.Refresh();
            //                isPubOver = true;
            //            }
            //            if (ckbshut.Checked)
            //            {
            //                if (radioButton1.Checked)
            //                {
            //                    SaveConfig();
            //DoExitWin(1);
            //                }
            //            }
        }

        private void StopPub(int userId)
        {
            isstoppub = true;
            if (t != null)
                t.Abort();
            settingBLL bll = new settingBLL();
            settingInfo sInfo = new settingInfo();
            sInfo.userId = userId;
            sInfo.isPubing = false;
            bll.UpIsPubing(sInfo);
        }

        /// <summary>
        /// 模板替换变量
        /// </summary>
        /// <param name="wz">模板</param>
        /// <param name="title"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceHTMLWZ(string wz, titleInfo tInfo, cmUserInfo user)
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