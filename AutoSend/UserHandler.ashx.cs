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

                        case "gettailwordlist": _strContent.Append(GetTailwordList(context)); break;//获取公共长尾词
                        case "savetailword": _strContent.Append(SaveTailword(context)); break;
                        case "deltailword": _strContent.Append(DelTailword(context)); break;

                        case "getbadwordlist": _strContent.Append(GetBadwordList(context)); break;//获取敏感词
                        case "savebadword": _strContent.Append(SaveBadword(context)); break;
                        case "delbadword": _strContent.Append(DelBadword(context)); break;

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
            string searchValue = context.Request["searchValue"];
            string pageIndex = context.Request["page"];
            string pageSize = context.Request["pageSize"];
            if (string.IsNullOrEmpty(pageIndex))
                pageIndex = "1";
            if (string.IsNullOrEmpty(pageSize))
                pageSize = "10";
            List<cmUserInfo> uList = bll.GetUserList(string.Format(" where username like '%{0}%'",searchValue));
            if (uList.Count < 1)
                return json.WriteJson(0, "未获取到会员信息", new { });
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
                var strjson = context.Request["params"];
                var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
                CmUserBLL cmBLL = new CmUserBLL();
                cmUserInfo cm = JsonConvert.DeserializeObject<cmUserInfo>(strjson, js);
                if (cm.Id == 0)
                {
                    if (cmBLL.IsExistUser(cm.username))
                        return json.WriteJson(0, "此用户名已被注册", new { });
                    cmBLL.AddUser(cm);
                }
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
            List<realmNameInfo> rList = bll.GetRealmList("");
            if (rList.Count < 1)
                return json.WriteJson(0, "未获取到任何域名", new { });
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
            try
            {

                gList = bll.GetGradeList("");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
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
            try
            {
                if (grade.Id == 0)
                    bll.AddGrade(grade);
                else
                    bll.UpdateGrade(grade);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
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
            try
            {
                cList = bll.GetColumnList("");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
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
            try
            {
                nList = bll.GetNoticeList(" where isuse=1 order by pubTime desc");
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
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

        #region 长尾词管理
        private string GetTailwordList(HttpContext context)
        {
            tailwordBLL bll = new tailwordBLL();
            string pageIndex = context.Request["page"];
            string pageSize = context.Request["pageSize"];
            if (string.IsNullOrEmpty(pageIndex))
                pageIndex = "1";
            if (string.IsNullOrEmpty(pageSize))
                pageSize = "10";
            List<tailwordInfo> tList = new List<tailwordInfo>();
            try
            {
                tList = bll.GetTailwordList();
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            //查询分页数据
            var pageData = tList.Where(u => u.Id > 0)
                .OrderByDescending(u => u.Id)
                .Skip((int.Parse(pageIndex) - 1) * int.Parse(pageSize))
                .Take(int.Parse(pageSize)).ToList();
            return json.WriteJson(1, "成功", new { total = tList.Count(), tailwordList = pageData });
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

        #region 敏感词管理
        private string GetBadwordList(HttpContext context)
        {
            badwordBLL bll = new badwordBLL();
            string pageIndex = context.Request["page"];
            string pageSize = context.Request["pageSize"];
            if (string.IsNullOrEmpty(pageIndex))
                pageIndex = "1";
            if (string.IsNullOrEmpty(pageSize))
                pageSize = "10";
            List<badwordInfo> bList = new List<badwordInfo>();
            try
            {
                bList = bll.GetBadwordList();
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.ToString(), new { });
            }
            //查询分页数据
            var pageData = bList.Where(u => u.Id > 0)
                .OrderByDescending(u => u.Id)
                .Skip((int.Parse(pageIndex) - 1) * int.Parse(pageSize))
                .Take(int.Parse(pageSize)).ToList();
            return json.WriteJson(1, "成功", new { total = bList.Count(), badwordList = pageData });
        }
        /// <summary>
        /// 增加或修改长尾词
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string SaveBadword(HttpContext context)
        {
            badwordBLL bll = new badwordBLL();
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            badwordInfo badword = JsonConvert.DeserializeObject<badwordInfo>(strjson, js);
            if (badword.Id == 0)
                bll.AddBadword(badword);
            else
                bll.UpdateBadword(badword);
            return json.WriteJson(1, "成功", new { });
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public string DelBadword(HttpContext context)
        {
            string id = context.Request["Id"];
            if (string.IsNullOrEmpty(id))
                return json.WriteJson(0, "Id不能为空", new { });
            badwordBLL bll = new badwordBLL();
            int a = bll.DelBadword(id);
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