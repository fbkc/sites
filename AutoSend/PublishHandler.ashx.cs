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

                        #region 凌晨置零
                        case "uptodaycount": _strContent.Append(UpTodayCount(context)); break;//凌晨置零
                        #endregion

                        #region 查标题
                        case "obtaintitle": _strContent.Append(ObtainTitle(context)); break;//查标题
                        #endregion

                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }

        public static List<realmNameInfo> slist = new List<realmNameInfo>();

        #region 轮循setting表
        /// <summary>
        /// 服务访问接口,轮循setting表，开启线程发布任务
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string RoundSetting(HttpContext context)
        {
            try
            {
                settingBLL bll = new settingBLL();
                //查找设置定时发布并且非在发配置
                List<settingInfo> sList = bll.RoundSetting(string.Format(" where isAutoPub=1  and isPubing=0"));
                if (sList != null && sList.Count > 0)
                {
                    DateTime dt = DateTime.Now;
                    foreach (settingInfo sInfo in sList)
                    {
                        if (!sInfo.isAutoPub || sInfo.isPubing)
                            continue;
                        int nowMin = dt.Minute / 10;//当前分钟十位数
                        int tMin = sInfo.pubMin / 10;//用户所定时间十位数
                        if (dt.Hour == sInfo.pubHour && nowMin == tMin)//若时间符合，调用发布接口
                        {
                            //Pub.Publish(sInfo.userId);//创建发布线程

                            //待发标题是否足够
                            titleBLL tBLL = new titleBLL();
                            bool isExit =tBLL.IsExitNoPubTitle(string.Format(" where userId={0} and isSucceedPub=0", sInfo.Id));
                            if (!isExit)
                            {
                                log.wlog("发布停止：待发标题数量不足，请及时生成标题", sInfo.Id.ToString(), sInfo.username);
                            }
                            else
                            {
                                settingBLL sBll = new settingBLL();//更新此用户发布状态
                                sBll.UpIsPubing(1, sInfo.userId);//isPubing置true
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
        }
        #endregion

        #region 凌晨置零
        /// <summary>
        /// 服务访问接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpTodayCount(HttpContext context)
        {
            try
            {
                settingBLL bll = new settingBLL();
                bll.UpEveryDayCount();
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
        }
        #endregion

        #region 读待发标题
        private bool isStart = false;
        public static Queue<titleInfo> tQueue = new Queue<titleInfo>();
        /// <summary>
        /// 服务访问接口
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string ObtainTitle(HttpContext context)
        {
            List<titleInfo> tList = new List<titleInfo>();
            try
            {
                settingBLL bll = new settingBLL();
                realmBLL rbll = new realmBLL();
                slist = rbll.GetRealmList(" where isUseing=1");
                tList = bll.ObtainTitleList();
                if (tList != null && tList.Count > 0)
                {
                    foreach (titleInfo t in tList)
                    {
                        tQueue.Enqueue(t);//插入队列
                    }
                    if (tQueue.Count > 0)
                    {
                        ThreadPool.SetMaxThreads(5, 5);
                        for (int i = 0; i < tQueue.Count; i++)
                        {
                            titleReam tr = new titleReam();
                            tr.tinfo = tQueue.Dequeue();
                            tr.realm =slist[new Random().Next(0,slist.Count)].realmAddress;
                            ThreadPool.QueueUserWorkItem(new WaitCallback(Pub1.PubTitle), tr);
                        }
                    }
                }
            }
            catch (Exception ex)
            { return json.WriteJson(0, ex.ToString(), new { }); }
            return json.WriteJson(1, "成功", new { });
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