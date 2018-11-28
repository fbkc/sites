﻿using BLL;
using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
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
                        #region 段落处理
                        case "getparalist": _strContent.Append(GetParaList(context)); break;//获取此会员下所有段落
                        case "savepara": _strContent.Append(SavePara(context)); break;
                        case "delpara": _strContent.Append(DelPara(context)); break;//删除段落
                        #endregion
                        #region 内容模板
                        case "getcontentlist": _strContent.Append(GetContentList(context)); break;//获取此会员下所有内容模板
                        case "savecontent": _strContent.Append(SaveContent(context)); break;
                        case "delcontent": _strContent.Append(DelContent(context)); break;//删除内容模板
                        #endregion
                        #region 长尾词
                        case "gettailwordlist": _strContent.Append(GetTailwordList(context)); break;
                        case "savetailword": _strContent.Append(SaveTailword(context)); break;
                        case "deltailword": _strContent.Append(DelTailword(context)); break;
                        #endregion
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
            var userId = context.Request["Id"];
            DataTable dt = bll.GetParagraphList(string.Format(" where Id='{0}'", userId));
            if (dt.Rows.Count < 1)
                return "";
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
            //将list对象集合转换为Json
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "成功";
            json.detail = new { paraList = pList };
            return jss.Serialize(json);
        }
        /// <summary>
        /// 增加或修改段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SavePara(HttpContext context)
        {
            paragraphBLL bll = new paragraphBLL();
            var strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            paragraphInfo para = JsonConvert.DeserializeObject<paragraphInfo>(strjson, js);
            if (para.Id == 0)
                bll.AddParagraph(para);
            else
                bll.UpdateParagraph(para);
            //将list对象集合转换为Json
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "成功";
            json.detail = new { };
            return jss.Serialize(json);
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
            {
                json.code = "1";
                json.msg = "删除成功";
            }
            else
            {
                json.code = "0";
                json.msg = "删除失败";
            }
            json.detail = new { };
            return jss.Serialize(json);
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
            var userId = context.Request["Id"];
            DataTable dt = bll.GetContentList(string.Format(" where Id='{0}'", userId));
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
            //将list对象集合转换为Json
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "成功";
            json.detail = new { contentList = cList };
            return jss.Serialize(json);
        }
        /// <summary>
        /// 增加或修改内容模板
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveContent(HttpContext context)
        {
            contentMouldBLL bll = new contentMouldBLL();
            var strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            contentMouldInfo content = JsonConvert.DeserializeObject<contentMouldInfo>(strjson, js);
            if (content.Id == 0)
                bll.AddContent(content);
            else
                bll.UpdateContent(content);
            //将list对象集合转换为Json
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "成功";
            json.detail = new { };
            return jss.Serialize(json);
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
            {
                json.code = "1";
                json.msg = "删除成功";
            }
            else
            {
                json.code = "0";
                json.msg = "删除失败";
            }
            json.detail = new { };
            return jss.Serialize(json);
        }
        #endregion

        #region 长尾词
        private string GetTailwordList(HttpContext context)
        {
            tailwordBLL bll = new tailwordBLL();
            List<tailwordInfo> tList = new List<tailwordInfo>();
            var userId = context.Request["Id"];
            DataTable dt = bll.GetTailwordList(string.Format(" where Id='{0}'", userId));
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
            //将list对象集合转换为Json
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "成功";
            json.detail = new { tailwordList = tList };
            return jss.Serialize(json);
        }
        /// <summary>
        /// 增加或修改长尾词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveTailword(HttpContext context)
        {
            tailwordBLL bll = new tailwordBLL();
            var strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            tailwordInfo tailword = JsonConvert.DeserializeObject<tailwordInfo>(strjson, js);
            if (tailword.Id == 0)
                bll.AddTailword(tailword);
            else
                bll.UpdateTailword(tailword);
            //将list对象集合转换为Json
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "成功";
            json.detail = new { };
            return jss.Serialize(json);
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
            {
                json.code = "1";
                json.msg = "删除成功";
            }
            else
            {
                json.code = "0";
                json.msg = "删除失败";
            }
            json.detail = new { };
            return jss.Serialize(json);
        }
        #endregion

        #region 图片库

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