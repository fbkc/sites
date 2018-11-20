using System;
using System.Collections.Generic;
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
                        case "getParalist": _strContent.Append(GetParaInfo(context)); break;//获取此会员下所有段落
                        case "addPara": _strContent.Append(AddPara(context)); break;//添加段落
                        case "updatePara": _strContent.Append(UpdatePara(context)); break;//修改段落
                        case "delPara": _strContent.Append(DelPara(context)); break;//删除段落
                        case "getAccount": _strContent.Append(GetAccount(context)); break;//判断是否登录接口
                        case "getrealmlist": _strContent.Append(GetRealmList(context)); break;//获取域名 
                        case "addrealm": _strContent.Append(AddRealm(context)); break;//增加域名 
                        case "updaterealm": _strContent.Append(UpdateRealm(context)); break;//更新域名 
                        case "delrealm": _strContent.Append(DeleteRealm(context)); break;//删除域名 
                        case "getgradelist": _strContent.Append(GetGradeList(context)); break;//获取账号级别列表 
                        case "addgrade": _strContent.Append(AddGrade(context)); break;//增加账号级别 
                        case "updategrade": _strContent.Append(UpdateGrade(context)); break;//更新账号级别
                        case "delgrade": _strContent.Append(DeleteGrade(context)); break;//删除账号级别
                        case "getcolumnlist": _strContent.Append(GetColumnList(context)); break;//获取栏目列表 
                        case "addcolumn": _strContent.Append(AddColumn(context)); break;//增加栏目 
                        case "updatecolumn": _strContent.Append(UpdateColumn(context)); break;//更新栏目
                        case "delcolumn": _strContent.Append(DeleteColumn(context)); break;//删除栏目
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
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