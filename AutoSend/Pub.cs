﻿using BLL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Web;

namespace AutoSend
{
    public static class Pub
    {
        #region 发布
        /// <summary>
        /// 创建发布线程
        /// </summary>
        /// <param name="uId"></param>
        /// <returns></returns>
        public static bool Publish(int uId)
        {
            try
            {
                Thread t = null;
                t = new Thread(new ParameterizedThreadStart(PubTitle));
                t.IsBackground = true;
                t.Start(uId);
            }
            catch (Exception ex)
            {
                log.wlog(ex.ToString(), uId.ToString(), "");
                return false;
            }
            return true;
        }
        private static void PubTitle(object uId)
        {
            //线程创建成功在更新此用户发布状态
            settingBLL sBll = new settingBLL();
            sBll.UpIsPubing(1, uId);//isPubing置true
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
                #region 待发接口
                realmBLL rbll = new realmBLL();
                List<realmNameInfo> slist = rbll.GetRealmList(" where isUseing=1");
                int q = -1;
                int w = slist.Count;
                if (w < 1)
                {
                    log.wlog("发布停止：您没有发布权限", model.Id.ToString(), model.username);
                    sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
                    return;
                }
                #endregion
                //取待发标题
                titleBLL tBLL = new titleBLL();
                //取出未发布标题
                List<titleInfo> tList = tBLL.GetTitleList(string.Format(" where userId={0} and isSucceedPub=0 order by addTime", model.Id));
                if (tList == null || tList.Count < 1)
                {
                    log.wlog("发布停止：待发标题数量不足，请及时生成标题", model.Id.ToString(), model.username);
                    sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
                    return;
                }
                for (int i = 0; i < tList.Count; i++)  //选中项遍历
                {
                    try
                    {
                        q++;
                        //先查询此用户setting表isPubing是否为1
                        settingInfo sInfo = sBll.GetSetting(string.Format(" where userId={0}", model.Id));
                        if (!sInfo.isPubing)
                        {
                            log.wlog("发布停止：发布状态无效，请先手动停止发布，再点开始", model.Id.ToString(), model.username);
                            break;
                        }
                        titleInfo tInfo = tList[i];
                        //公共信息
                        title = tInfo.title.Replace("&", "%26");
                        txtgytitle = title.Trim();//信息标题
                                                  //取模板
                        contentMouldBLL cmBLL = new contentMouldBLL();
                        List<contentMouldInfo> cmList = cmBLL.GetContentList(string.Format(" where userId={0} and productId={1}", model.Id, tInfo.productId));//根据此标题所属产品Id找模板
                        if (cmList == null || cmList.Count < 5)
                        {
                            log.wlog("发布停止：模板数量不足五个，请及时配置模板", model.Id.ToString(), model.username);
                            sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
                            break;
                        }
                        int index = rnd.Next(cmList.Count);
                        content = cmList[index].contentMould;//随机调用模板
                        cmBLL.UpUsedCount(cmList[index].Id);//模板调用次数加1

                        imageBLL iBLL = new imageBLL();
                        //根据userId,productId获取图片
                        List<imageInfo> iList = iBLL.GetImgList(string.Format(" where userId='{0}' and productId='{1}' order by addTime desc", model.Id, tInfo.productId));
                        if (iList == null || iList.Count < 1)
                        {
                            log.wlog("发布停止：图片数量不足，请及时上传图片", model.Id.ToString(), model.username);
                            sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
                            break;
                        }
                        string titleImg = "http://vip.100dh.cn/lookImg" + iList[rnd.Next(iList.Count)].imageURL;//选一张标题图片

                        content = Regex.Replace(content, "(?i)<IMG.*>", "");//过滤用户插入的本地图片     
                        content = Regex.Replace(content, " style=\"(.*?)\"", "");//过滤用户插入的本地图片  
                        content = ReplaceHTMLWZ(content, tInfo, iList, model);//替换模板中变量
                        if (content == "段落数量不足50")
                        {
                            log.wlog("发布停止：段落数量不足50个，请及时添加段落", model.Id.ToString(), model.username);
                            sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
                            break;
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
                        txtgytitle = changemgc(txtgytitle);
                        txtgydesc = changemgc(txtgydesc);
                        //sKeyword1 = changemgc(sKeyword1);
                        #endregion
                        productBLL pBLL = new productBLL();
                        productInfo pInfo = pBLL.GetProductList(string.Format(" where userId={0}  and Id={1}", model.Id, tInfo.productId))[0];
                        string key = NetHelper.GetMD5(model.username + "100dh888");
                        columnBLL clBLL = new columnBLL();
                        List<columnInfo> cList = clBLL.GetColumnList("");
                        Random r = new Random();
                        int cId = r.Next(1,cList.Count+1);
                        StringBuilder strpost = new StringBuilder();
                        strpost.AppendFormat("catid={0}&", cId);
                        strpost.AppendFormat("title={0}&", txtgytitle);
                        strpost.AppendFormat("pinpai={0}&", pInfo.pinpai);
                        strpost.AppendFormat("xinghao={0}&", pInfo.xinghao);
                        strpost.AppendFormat("city={0}&", pInfo.city);
                        strpost.AppendFormat("gonghuo={0}&", pInfo.sumCount);
                        strpost.AppendFormat("qiding={0}&", pInfo.smallCount);
                        strpost.AppendFormat("price={0}&", pInfo.price);
                        strpost.AppendFormat("unit={0}&", pInfo.unit);
                        strpost.AppendFormat("content={0}&", txtgydesc);//内容,UrlEncode编码 strpost.AppendFormat("content={0}&", Tools.Encode(desc,"12345678","87654321"));
                        strpost.AppendFormat("keywords={0}&", sKeyword1);
                        strpost.AppendFormat("style_color={0}&", "");
                        strpost.AppendFormat("style_font_weight={0}&", "");
                        strpost.AppendFormat("page_title_value={0}&", "");
                        strpost.AppendFormat("add_introduce={0}&", 1);
                        strpost.AppendFormat("introcude_length={0}&", 200);
                        strpost.AppendFormat("auto_thumb={0}&", 1);
                        strpost.AppendFormat("auto_thumb_no={0}&", 1);
                        strpost.AppendFormat("thumb={0}&", titleImg);
                        strpost.AppendFormat("forward={0}&", "");
                        strpost.AppendFormat("id={0}&", "");
                        strpost.AppendFormat("username={0}&", model.username);
                        strpost.AppendFormat("key={0}&", key);
                        strpost.AppendFormat("dosubmit={0}&", "提交");
                        strpost.AppendFormat("version={0}&", "1.0.0.0");

                        #region 组织发布内容
                        for (int j = 0; j < slist.Count; j++)
                        {
                            if (q % w == j)
                            {
                                //log.wlog(slist[j].realmAddress.Trim(), model.Id.ToString(), model.username);
                                string html = NetHelper.HttpPost(slist[j].realmAddress.Trim(), strpost.ToString());
                                string titleurl = "";
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
                                    //成功链接写入日志表
                                    //log.wlog(titleurl, model.Id.ToString(), model.username);
                                    Thread.Sleep(60 * 1000);
                                    //生最后一条标题时，重新查询一次标题库
                                    if (i == tList.Count - 1)
                                    {
                                        tList = tBLL.GetTitleList(string.Format(" where userId={0} and isSucceedPub=0 order by addTime", model.Id));
                                        if (tList == null || tList.Count < 1)
                                        {
                                            log.wlog("发布停止：待发标题数量不足，请及时生成标题", model.Id.ToString(), model.username);
                                            sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
                                            break;
                                        }
                                    }
                                    continue;//再发送本栏目信息
                                }
                                else if (code == "0")
                                {
                                    if (msg.Contains("今日投稿已超过限制数"))//停
                                    {
                                        log.wlog("发布停止：" + msg.ToString(), model.Id.ToString(), model.username);
                                        sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
                                        break;
                                    }
                                    if (msg.ToString().Contains("信息发布过快，请隔60秒再提交！"))
                                    {
                                        log.wlog(msg.ToString(), model.Id.ToString(), model.username);
                                        Thread.Sleep(sleeptime * 1000);
                                        continue;
                                    }
                                    if (msg.ToString().Contains("信息条数已发完！"))//所有条数发完了，停
                                    {
                                        log.wlog("发布停止：" + msg.ToString(), model.Id.ToString(), model.username);
                                        sBll.UpIsPubing(0, model.Id);//setting表isPubing置0
                                        break;
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
                    }
                    catch (Exception ex)
                    {
                        log.wlog("用户名:" + model.username + "用户Id:" + model.Id.ToString() + ex.ToString(), "0", "system");
                        Thread.Sleep(sleeptime * 1000);
                        continue;
                    }
                }
            }
            catch (Exception ex)
            {
                log.wlog("用户名:"+ model.username+"用户Id:"+ model.Id.ToString()+ex.ToString(), "0", "system");
                Thread.Sleep(sleeptime * 1000);
                Publish(model.Id);//重开线程
            }
        }
        /// <summary>
        /// 敏感词替换
        /// </summary>
        /// <param name="ss"></param>
        /// <returns></returns>
        private static string changemgc(string ss)
        {
            badwordBLL bBll = new badwordBLL();
            List<badwordInfo> bList = bBll.GetBadwordList();
            foreach (badwordInfo b in bList)
            {
                ss = ss.Replace(b.badword, "");
            }
            return ss;
        }
        /// <summary>
        /// 模板替换变量
        /// </summary>
        /// <param name="wz">模板</param>
        /// <param name="title"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string ReplaceHTMLWZ(string wz, titleInfo tInfo, List<imageInfo> iList, cmUserInfo user)
        {
            Regex r;
            Random rnd = new Random();
            while (wz.Contains("{图片}"))
            {
                r = new Regex("{图片}");
                if (iList.Count > 0)
                {
                    string t = "http://vip.100dh.cn/lookImg" + iList[rnd.Next(iList.Count)].imageURL;
                    wz = r.Replace(wz, "<img src='" + t + "' alt='{标题}' width='600' height='400' />", 1);
                }
                else
                {
                    break;
                }
            }
            if (wz.Contains("{标题}"))
            {
                wz = wz.Replace("{标题}", tInfo.title);
            }
            paragraphBLL pBLL = new paragraphBLL();
            //根据userId,productId获取段落
            List<paragraphInfo> pList = pBLL.GetParagraphList(string.Format(" where userId='{0}' and productId='{1}' order by addTime desc", user.Id, tInfo.productId));
            if (pList == null || pList.Count < 50)
            {
                return "段落数量不足50";
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
                    int index = rnd.Next(pList.Count);
                    string text2 = pList[index].paraCotent;//调用段落
                    pBLL.UpUsedCount(pList[index].Id);//段落调用次数加1
                    wz = regex.Replace(wz, text2.Replace("\u3000\u3000", ""), 1);
                }
                catch (Exception ex)
                {
                }
            }
            wz = wz.Replace("&", "%26");
            return wz;
        }

        #endregion
    }
}