using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.SessionState;

namespace AutoSend
{
    /// <summary>
    /// CreateHandler 的摘要说明
    /// </summary>
    public class CreateHandler : BaseHandle, IHttpHandler, IRequiresSessionState
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
                        case "createtitle": _strContent.Append(CreateTitle(context)); break;//生成标题
                        case "upsettitle": _strContent.Append(UpsetTitle(context)); break;//打乱标题
                        default: break;
                    }
                }
            }
            context.Response.Write(_strContent.ToString());
        }
        private string CreateTitle(HttpContext context)
        {
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            titleList tl = JsonConvert.DeserializeObject<titleList>(strjson, js);
            string rule = tl.rule;//规则
            List<string> sctitle = new List<string>();
            List<string> sctitle1 = new List<string>();

            List<string> cityList = tl.city;
            int intcity = cityList.Count;
            List<string> bl1 = tl.keyword;
            int intbl1 = bl1.Count;
            List<string> bl2 = tl.tailword;
            int intbl2 = bl2.Count;
            sctitle.Add(rule);
            bool ishvae = false;
            Random rd = new Random();
            do
            {
                ishvae = false;
                foreach (string st in sctitle)
                {
                    if (st.Contains("{城市}") && intcity > 0)
                    {
                        Regex r = new Regex("{城市}");
                        string t = "";
                        foreach (string s in cityList)
                        {
                            t = r.Replace(st, s,1);
                            sctitle1.Add(t);
                        }
                        ishvae = true;
                    }
                }
                if (sctitle1.Count > 0)
                {
                    sctitle.Clear();
                    foreach (string s in sctitle1)
                        sctitle.Add(s);
                    sctitle1.Clear();
                }

            } while (ishvae);
            do
            {
                ishvae = false;
                foreach (string st in sctitle)
                {
                    if (st.Contains("{关键词}") && intbl1 > 0)
                    {
                        Regex r = new Regex("{关键词}");
                        string t = "";
                        foreach (string s in bl1)
                        {
                            t = r.Replace(st, s, 1);
                            sctitle1.Add(t);
                        }
                        ishvae = true;
                    }
                }
                if (sctitle1.Count > 0)
                {
                    sctitle.Clear();
                    foreach (string s in sctitle1)
                        sctitle.Add(s);
                    sctitle1.Clear();
                }

            } while (ishvae);
            do
            {
                ishvae = false;
                foreach (string st in sctitle)
                {
                    if (st.Contains("{长尾词}") && intbl2 > 0)
                    {
                        Regex r = new Regex("{长尾词}");
                        string t = "";
                        foreach (string s in bl2)
                        {
                            t = r.Replace(st, s, 1);
                            sctitle1.Add(t);
                        }
                        ishvae = true;
                    }
                }
                if (sctitle1.Count > 0)
                {
                    sctitle.Clear();
                    foreach (string s in sctitle1)
                        sctitle.Add(s);
                    sctitle1.Clear();
                }

            } while (ishvae);
            string[] resultstr = null;
            resultstr = sctitle.ToArray();
            if (sctitle.Count > 10000)
            {
                string[] resultstr1 = new string[10000];
                Array.Copy(resultstr, 0, resultstr1, 0, 10000);
                resultstr = resultstr1;
            }
            return json.WriteJson(1, "成功", new { resultstr });
        }
        /// <summary>
        /// 将标题打乱返回前端
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        private string UpsetTitle(HttpContext context)
        {
            string strjson = context.Request["params"];
            var js = new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore };
            List<string> titleList = new List<string>();
            try
            {
                List<string> titleStrs = JsonConvert.DeserializeObject<List<string>>(strjson, js);
                titleList = RandomStrings(titleStrs);
            }
            catch (Exception ex)
            {
                return json.WriteJson(0, ex.Message, new { });
            }
            return json.WriteJson(1, "成功", new { titleList });
        }
        /// <summary>
        /// 打乱数据
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static List<string> RandomStrings(List<string> list)
        {
            if (list.Count > 0)
            {
                List<string> newstr = new List<string>();
                Random r = new Random();
                string temp = "";
                int c = list.Count;
                int t = 0;
                for (int i = 0; i < c; i++)
                {
                    t = r.Next(list.Count);
                    temp = list[t];
                    list.RemoveAt(t);
                    newstr.Add(temp);
                }
                return newstr;
            }
            else
            {
                return list;
            }
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