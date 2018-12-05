using BLL;
using Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Script.Serialization;
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// Handler 的摘要说明
    /// </summary>
    public class Handler : BaseHandle, IHttpHandler, IRequiresSessionState
    {
        /// <summary>
        /// 主进程
        /// </summary>
        /// <param name="context"></param>
        public override void OnLoad(HttpContext context)
        {
            context.Response.ContentType = "text/plain;charset=utf-8;";
            StringBuilder _strContent = new StringBuilder();
            if (_strContent.Length == 0)
            {
                string _strAction = context.Request.Params["action"];
                if (string.IsNullOrEmpty(_strAction))
                {
                    _strContent.Append(_strContent.Append(json.WriteJson(0, "禁止访问", new { })));
                }
                else
                {
                    switch (_strAction.Trim().ToLower())
                    {
                        case "getuserlist": _strContent.Append(GetUserInfo(context)); break;//获取所有会员
                        case "saveuser": _strContent.Append(SaveUser(context)); break;//添加或修改会员信息
                        case "deluser": _strContent.Append(DelUser(context)); break;//删除会员
                        case "getrealmlist": _strContent.Append(GetRealmList(context)); break;//获取域名 
                        case "saverealm": _strContent.Append(SaveRealm(context)); break;//增加或修改域名 
                        case "delrealm": _strContent.Append(DeleteRealm(context)); break;//删除域名 
                        case "getgradelist": _strContent.Append(GetGradeList(context)); break;//获取账号级别列表 
                        case "savegrade": _strContent.Append(SaveGrade(context)); break;//增加或更新账号级别 
                        case "delgrade": _strContent.Append(DeleteGrade(context)); break;//删除账号级别
                        case "getcolumnlist": _strContent.Append(GetColumnList(context)); break;//获取栏目列表 
                        case "savecolumn": _strContent.Append(SaveColumn(context)); break;//增加或更新栏目 
                        case "delcolumn": _strContent.Append(DeleteColumn(context)); break;//删除栏目
                        case "getnoticelist": _strContent.Append(GetNoticeList(context)); break;//获取公告
                        case "savenotice": _strContent.Append(SaveNotice(context)); break;//增加或更新公告 
                        case "delnotice": _strContent.Append(DeleteNotice(context)); break;//删除公告
                        case "logout": _strContent.Append(LogOut(context)); break;//登出
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }
        /// <summary>
        /// 登出
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string LogOut(HttpContext context)
        {
            context.Session["UserModel"] = null;
            return json.WriteJson(1, "成功", new { });
        }

        #region  会员管理
        /// <summary>
        /// 获取所有会员
        /// </summary>
        /// <returns></returns>
        public string GetUserInfo(HttpContext context)
        {
            CmUserBLL bll = new CmUserBLL();
            string pageIndex = context.Request["page"];
            string pageSize = context.Request["pageSize"];
            if (string.IsNullOrEmpty(pageIndex))
                pageIndex = "1";
            if (string.IsNullOrEmpty(pageSize))
                pageSize = "10";
            List<cmUserInfo> uList = new List<cmUserInfo>();
            DataTable dt = bll.GetUser("");
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                cmUserInfo userInfo = new cmUserInfo();
                userInfo.Id = (int)row["Id"];
                userInfo.username = (string)row["username"];
                userInfo.password = (string)row["password"];
                userInfo.userType = (int)row["userType"];
                userInfo.isStop = (bool)row["isStop"];
                userInfo.gradeId = (int)row["gradeId"];
                userInfo.canPubCount = (int)row["canPubCount"];
                userInfo.realmNameInfo = (string)row["realmNameInfo"];
                userInfo.expirationTime = (string)row["expirationTime"];
                userInfo.endPubCount = (int)row["endPubCount"];
                userInfo.endTodayPubCount = (int)row["endTodayPubCount"];
                userInfo.registerTime = (string)row["registerTime"];
                userInfo.registerIP = (string)row["registerIP"];
                userInfo.companyName = (string)row["companyName"];
                userInfo.columnInfoId = (int)row["columnInfoId"];
                userInfo.person = (string)row["person"];
                userInfo.telephone = (string)row["telephone"];
                userInfo.modile = (string)row["modile"];
                userInfo.ten_qq = (string)row["ten_qq"];
                userInfo.keyword = (string)row["keyword"];
                userInfo.pinpai = (string)row["pinpai"];
                userInfo.xinghao = (string)row["xinghao"];
                userInfo.price = (string)row["price"];
                userInfo.smallCount = (string)row["smallCount"];
                userInfo.sumCount = (string)row["sumCount"];
                userInfo.unit = (string)row["unit"];
                userInfo.city = (string)row["city"];
                userInfo.address = (string)row["address"];
                userInfo.com_web = (string)row["com_web"];
                userInfo.companyRemark = (string)row["companyRemark"];
                userInfo.yewu = (string)row["yewu"];
                userInfo.ziduan1 = (string)row["ziduan1"];
                uList.Add(userInfo);
            }
            //获取会员信息
            //var uInfo = uList.Join(
            //    u => u.ClassNum,
            //    (u) => new { u.Id,u.username,u.password,u.userType }
            //    );

            //查询分页数据
            var pageData = uList.Where(u => u.Id > 0)
                .OrderByDescending(u => u.Id)
                .Skip((int.Parse(pageIndex) - 1) * int.Parse(pageSize))
                .Take(int.Parse(pageSize)).ToList();
            return json.WriteJson(1, "成功", new { total = uList.Count(), cmUserList = pageData });
        }
        /// <summary>
        /// 添加会员
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string SaveUser(HttpContext context)
        {
            try
            {
                //StreamReader reader = new StreamReader(context.Request.InputStream);
                //string strjson = HttpUtility.UrlDecode(reader.ReadToEnd());
                //context.Request.Form["data[xm]"]

                //Dictionary<string, object> str = (Dictionary<string, object>)new JavaScriptSerializer().DeserializeObject(strjson);
                //JObject jo = new JObject();
                //foreach (var item in str)
                //{
                //    //把字典转换成Json对象
                //    jo.Add(item.Key, item.Value.ToString());
                //}

                //return jo["username"].ToString();
                //if (string.IsNullOrEmpty(strjson))
                //{

                //    return "0";
                //}
                //else
                //    return strjson;
                //Stream stream = context.Request.InputStream;
                //byte[] bytes = new byte[stream.Length];
                //stream.Read(bytes, 0, bytes.Length);
                //string parameters = Encoding.Default.GetString(bytes);
                //cmUserInfo cm = new cmUserInfo();
                //JObject jObject = (JObject)JsonConvert.DeserializeObject(parameters);
                //cm.username = jObject["username"].ToString();
                //cm.password = jObject["password"].ToString();

                var strjson = context.Request["params"];
                var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                CmUserBLL cmBLL = new CmUserBLL();
                cmUserInfo cm = JsonConvert.DeserializeObject<cmUserInfo>(strjson, js);
                if (cm.Id == 0)
                    cmBLL.AddUser(cm);
                else
                    cmBLL.UpdateUser(cm);
                return json.WriteJson(1, "成功", new { });
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
        }
        /// <summary>
        /// 删除会员 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelUser(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            CmUserBLL cmBLL = new CmUserBLL();
            int a = cmBLL.DelUser(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region 域名管理
        /// <summary>
        /// 获取所有域名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetRealmList(HttpContext context)
        {
            realmBLL bll = new realmBLL();
            List<realmNameInfo> rList = new List<realmNameInfo>();
            DataTable dt = bll.GetRealmList("");
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                realmNameInfo rInfo = new realmNameInfo();
                rInfo.Id = (int)row["Id"];
                rInfo.realmName = (string)row["realmName"];
                rInfo.realmAddress = (string)row["realmAddress"];
                rInfo.isUseing = (bool)row["isUseing"];
                rList.Add(rInfo);
            }
            return json.WriteJson(1, "成功", new { realmList = rList });
        }
        /// <summary>
        /// 增加或更新域名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveRealm(HttpContext context)
        {
            realmBLL bll = new realmBLL();
            var strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            realmNameInfo rm = JsonConvert.DeserializeObject<realmNameInfo>(strjson, js);
            if (rm.Id == 0)
                bll.AddRealm(rm);
            else
                bll.UpdateRealm(rm);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除域名
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteRealm(HttpContext context)
        {
            realmBLL bll = new realmBLL();
            var id = context.Request["Id"];
            int a = bll.DelRealm(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region  账号级别
        /// <summary>
        /// 获取所有账户级别列表
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetGradeList(HttpContext context)
        {
            gradeBLL bll = new gradeBLL();
            List<gradeInfo> gList = new List<gradeInfo>();
            DataTable dt = bll.GetGradeList("");
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                gradeInfo gInfo = new gradeInfo();
                gInfo.Id = (int)row["Id"];
                gInfo.gradeName = (string)row["gradeName"];
                gInfo.pubCount = (int)row["pubCount"];
                gList.Add(gInfo);
            }
            return json.WriteJson(1, "成功", new { gradeList = gList });
        }
        /// <summary>
        /// 添加或修改账号级别信息
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveGrade(HttpContext context)
        {
            var strjson = context.Request.Form["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            gradeInfo grade = JsonConvert.DeserializeObject<gradeInfo>(strjson, js);
            gradeBLL bll = new gradeBLL();
            if (grade.Id == 0)
                bll.AddGrade(grade);
            else
                bll.UpdateGrade(grade);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除账号级别
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteGrade(HttpContext context)
        {
            gradeBLL bll = new gradeBLL();
            var id = context.Request["Id"];
            int a = bll.DelGrade(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region  栏目管理
        /// <summary>
        /// 获取所有栏目
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string GetColumnList(HttpContext context)
        {
            columnBLL bll = new columnBLL();
            List<columnInfo> cList = new List<columnInfo>();
            DataTable dt = bll.GetColumnList("");
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                columnInfo cInfo = new columnInfo();
                cInfo.Id = (int)row["Id"];
                cInfo.columnName = (string)row["columnName"];
                cList.Add(cInfo);
            }
            return json.WriteJson(1, "成功", new { columnList = cList });
        }
        /// <summary>
        /// 增加或修改栏目
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveColumn(HttpContext context)
        {
            columnBLL bll = new columnBLL();
            var strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            columnInfo column = JsonConvert.DeserializeObject<columnInfo>(strjson, js);
            if (column.Id == 0)
                bll.AddColumn(column);
            else
                bll.UpdateColumn(column);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteColumn(HttpContext context)
        {
            columnBLL bll = new columnBLL();
            var id = context.Request["Id"];
            int a = bll.DelColumn(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        #region 公告管理
        private string GetNoticeList(HttpContext context)
        {
            noticeBLL bll = new noticeBLL();
            List<noticeInfo> nList = new List<noticeInfo>();
            DataTable dt = bll.GetNoticeList(" where isuse=1 order by pubTime desc");
            if (dt.Rows.Count < 1)
                return "";
            foreach (DataRow row in dt.Rows)
            {
                noticeInfo nInfo = new noticeInfo();
                nInfo.Id = (int)row["Id"];
                nInfo.notice = (string)row["notice"];
                nInfo.pubTime = (DateTime)row["pubTime"];
                nInfo.isImprotant = (bool)row["isImprotant"];
                nInfo.issue = (bool)row["issue"];
                nList.Add(nInfo);
            }
            return json.WriteJson(1, "成功", new { noticeList = nList });
        }
        /// <summary>
        /// 增加或修改栏目
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveNotice(HttpContext context)
        {
            noticeBLL bll = new noticeBLL();
            var strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            noticeInfo notice = JsonConvert.DeserializeObject<noticeInfo>(strjson, js);
            if (notice.Id == 0)
                bll.AddNotice(notice);
            else
                bll.UpdateNotice(notice);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除栏目
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string DeleteNotice(HttpContext context)
        {
            noticeBLL bll = new noticeBLL();
            var id = context.Request["Id"];
            int a = bll.DelNotice(id);
            if (a == 1)
                return json.WriteJson(1, "删除成功", new { });
            else
                return json.WriteJson(0, "删除失败", new { });
        }
        #endregion

        /// <summary>
        /// IsReusable
        /// </summary>
        public bool IsReusable
        {
            get
            {
                return true;
            }
        }
    }
}