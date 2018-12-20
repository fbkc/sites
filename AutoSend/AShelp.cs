using System;
using System.Collections.Generic;
using System.IO;

using System.Text;
using System.Xml;
using System.Configuration;
using System.Xml.Serialization;
namespace AutoSend
{
    public static class AShelp
    {
        public static string[] RandomStrings(string[] str, int count)
        {
            if (str.Length > 0)
            {
                List<System.String> list = new List<System.String>(str);
                List<string> newstr = new List<string>();
                Random r = new Random();
                string temp = "";
                int c = list.Count;
                if (c >= count)
                {
                    int t = 0;
                    for (int i = 0; i < count; i++)
                    {
                        t = r.Next(list.Count);
                        temp = list[t];
                        list.RemoveAt(t);
                        newstr.Add(temp);
                    }
                    return newstr.ToArray();
                }
                else
                {
                    newstr = list;
                    int addc = count - list.Count;
                    int indext = 0;
                    for (int i = 0; i < addc; i++)
                    {
                        indext = i % list.Count;
                        newstr.Add(list[indext]);
                    }
                    return newstr.ToArray();
                }
            }
            else
            {
                return str;
            }
        }
        public static void SaveTXT(string html, string path, Encoding en)
        {
            File.WriteAllText(path, html, en);
        }
        public static string UrlEncode(string temp, Encoding encoding)
        {
            StringBuilder stringBuilder = new StringBuilder();
            for (int i = 0; i < temp.Length; i++)
            {
                string t = temp[i].ToString();
                string k = System.Web.HttpUtility.UrlEncode(t, encoding);
                if (t == k)
                {
                    stringBuilder.Append(t);
                }
                else
                {
                    stringBuilder.Append(k.ToUpper());
                }
            }
            return stringBuilder.ToString();
        }

        public static void UpdateAppConfig(string newKey, string newValue)
        {
            bool isModified = false;
            foreach (string key in ConfigurationManager.AppSettings)
            {
                if (key == newKey)
                {
                    isModified = true;
                }
            }
            Configuration config =
                ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            if (isModified)
            {
                config.AppSettings.Settings.Remove(newKey);
            }
            config.AppSettings.Settings.Add(newKey, newValue);
            config.Save(ConfigurationSaveMode.Modified);
            ConfigurationManager.RefreshSection("appSettings");
        }

        public static string DateDiff(DateTime DateTime1, DateTime DateTime2)
        {
            string dateDiff = null;
            TimeSpan ts1 = new TimeSpan(DateTime1.Ticks);
            TimeSpan ts2 = new TimeSpan(DateTime2.Ticks);
            TimeSpan ts = ts1.Subtract(ts2).Duration();
            //ts.Subtract(new TimeSpan(0, 0, 1));
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;
        }
        public static string DateDiff(ref TimeSpan ts)
        {
            string dateDiff = null;            
            ts=ts.Subtract(new TimeSpan(0, 0, 1));
            dateDiff = ts.Days.ToString() + "天" + ts.Hours.ToString() + "小时" + ts.Minutes.ToString() + "分钟" + ts.Seconds.ToString() + "秒";
            return dateDiff;
        }
        /// <summary>
        /// 打乱数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] RandomStrings(string[] str)
        {
            if (str.Length > 0)
            {
                List<System.String> list = new List<System.String>(str);
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
                return newstr.ToArray();
            }
            else
            {
                return str;
            }
        }
        /// <summary>
        /// 去重复数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] delreStrings(string[] str)
        {
            if (str.Length > 0)
            {
                List<string> jg = new List<string>();

                foreach (string s in str)
                {                    
                    jg.Add(s);
                }
                return delreStrings(jg);
            }
            else
                return str;
        }
        /// <summary>
        /// 去重复数据
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string[] delreStrings(List<System.String> str)
        {
            if (str.Count > 0)
            { 
                string t = "";
                for (int i = 0; i < str.Count; i++)
                {
                    t = str[i];
                    for (int j = i + 1; j < str.Count; j++)
                    {
                        if (t == str[j]) {
                            str.RemoveAt(j);
                            j--;
                        }
                    }
                }                
            }            
            return str.ToArray();            
        }
        /// <summary>
        /// 去空数据
        /// </summary>
        /// <param name="s"></param>
        /// <returns></returns>
        public static string[] delspaceStrings(string[] s)
        {
            List<System.String> str = new List<string>(s);
            List<string> newstr = new List<string>();
            if (str.Count > 0)
            {                
                string t = "";
                for (int i = 0; i < str.Count; i++)
                {
                    t = str[i];
                    if (t.Trim().Length > 0 && t.Trim() != "\r\n")
                        newstr.Add(t);
                }
            }
            return newstr.ToArray();
        }

        static void CopyDirectory(string srcDir, string tgtDir)
        {
            DirectoryInfo source = new DirectoryInfo(srcDir);
            DirectoryInfo target = new DirectoryInfo(tgtDir);

            //if (target.FullName.StartsWith(source.FullName, StringComparison.CurrentCultureIgnoreCase))
            //{
            //    throw new Exception("父目录不能拷贝到子目录！");
            //}

            if (!source.Exists)
            {
                return;
            }

            if (!target.Exists)
            {
                target.Create();
            }

            FileInfo[] files = source.GetFiles();

            for (int i = 0; i < files.Length; i++)
            {
                File.Copy(files[i].FullName, target.FullName + @"\" + files[i].Name, true);
            }

            DirectoryInfo[] dirs = source.GetDirectories();

            for (int j = 0; j < dirs.Length; j++)
            {
                CopyDirectory(dirs[j].FullName, target.FullName + @"\" + dirs[j].Name);
            }
        }
    }
}
