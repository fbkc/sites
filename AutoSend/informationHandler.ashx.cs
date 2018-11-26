using BLL;
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
                        case "getParalist": _strContent.Append(GetParaList(context)); break;//获取此会员下所有段落
                        case "addPara": _strContent.Append(AddPara(context)); break;//添加段落
                        case "updatePara": _strContent.Append(UpdatePara(context)); break;//修改段落
                        case "delPara": _strContent.Append(DelPara(context)); break;//删除段落
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }
        /// <summary>
        /// 获取此会员下所有段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetParaList(HttpContext context)
        {
            paragraphBLL bll = new paragraphBLL();
            List<paragraphInfo> pList = new List<paragraphInfo>();
            var userId = context.Request["userId"];
            DataTable dt = bll.GetParagraphList(string.Format("where Id='{0}'", userId));
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                paragraphInfo pInfo = new paragraphInfo();
                pInfo.Id = (int)row["Id"];
                pInfo.paraId = (string)row["paraId"];
                pInfo.paraCotent = (string)row["paraCotent"];
                pInfo.usedCount = (int)row["usedCount"];
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
        public string AddPara(HttpContext context)
        {
            var strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            paragraphBLL bll = new paragraphBLL();
            paragraphInfo p = JsonConvert.DeserializeObject<paragraphInfo>(strjson, js);
            bll.AddParagraph(p);
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "添加成功";
            json.detail = new { };
            return jss.Serialize(json);
        }
        /// <summary>
        /// 更新段落
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string UpdatePara(HttpContext context)
        {
            var strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            paragraphBLL bll = new paragraphBLL();
            paragraphInfo p = JsonConvert.DeserializeObject<paragraphInfo>(strjson, js);
            bll.UpdateParagraph(p, string.Format("where Id='{0}'", p.Id));
            jsonInfo json = new jsonInfo();
            json.code = "1";
            json.msg = "更新成功";
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
            int a = bll.DelParagraph(string.Format("where Id='{0}'", id));
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
        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}