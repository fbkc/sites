using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Net;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace AutoSend
{
    public static class NetHelper
    {
        private static CookieContainer cookie = new CookieContainer();

        public static string HttpGet(string url, string postDataStr,Encoding enc)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + ((postDataStr == "") ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = cookie;
            
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookie = request.CookieContainer;
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return string.Empty;
            }
            StreamReader reader = new StreamReader(responseStream, enc);
            string str = reader.ReadToEnd();
            reader.Close();
            responseStream.Close();
            return str;
        }
        
        public static string HttpGetUTF(string url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url + ((postDataStr == "") ? "" : "?") + postDataStr);
            request.Method = "GET";
            request.ContentType = "text/html;charset=UTF-8";
            request.CookieContainer = cookie;

            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookie = request.CookieContainer;
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return string.Empty;
            }
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            responseStream.Close();
            return str;
        }

        public static Image HttpGetImage(string url)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "GET";
            request.UserAgent = " Mozilla/5.0 (Windows NT 6.3; Trident/7.0; rv:11.0) like Gecko";
            request.CookieContainer = cookie;
            request.Accept = "image/png, image/svg+xml, image/*;q=0.8, */*;q=0.5";
            request.Headers.Add("X-HttpWatch-RID", " 46990-10314");
            request.Headers.Add("Accept-Language", "zh-Hans-CN,zh-Hans;q=0.8,en-US;q=0.5,en;q=0.3");
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookie = request.CookieContainer;
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return null;
            }
            return Image.FromStream(responseStream);
        }

        public static string HttpPost(string url, string postDataStr = "", string refer = "")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookie;
            request.ContentLength = 0L;
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/56.0.2924.87 Safari/537.36";
            request.Referer = refer;
            if (!string.IsNullOrEmpty(postDataStr))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postDataStr);
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookie = request.CookieContainer;

            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return string.Empty;
            }
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return str;
        }
        public static string HttpPostUTF(string url, string postDataStr)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookie;
            request.ContentLength = 0L;
            request.Referer = url;
            if (!string.IsNullOrEmpty(postDataStr))
            {
                byte[] bytes = Encoding.Default.GetBytes(postDataStr);
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookie = request.CookieContainer;
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return string.Empty;
            }
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return str;
        }
        public static string HttpPostR(string url, string postDataStr,string refer)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(new Uri(url));
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            request.CookieContainer = cookie;
            request.ContentLength = 0L;
            request.Referer = refer;
            
            if (!string.IsNullOrEmpty(postDataStr))
            {
                byte[] bytes = Encoding.UTF8.GetBytes(postDataStr);
                request.ContentLength = bytes.Length;
                Stream requestStream = request.GetRequestStream();
                requestStream.Write(bytes, 0, bytes.Length);
                requestStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            cookie = request.CookieContainer;
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return string.Empty;
            }
            StreamReader reader = new StreamReader(responseStream, Encoding.UTF8);
            string str = reader.ReadToEnd();
            reader.Close();
            response.Close();
            return str;
        }

        public static string HttpPostData(string url, string fileKeyName, string filePath, NameValueCollection stringDict)
        {
            string str;
            int num;
            MemoryStream stream = new MemoryStream();
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            string str2 = "-----------------------------7e0d3365052e";
            byte[] bytes = Encoding.ASCII.GetBytes("--" + str2 + "\r\n");
            FileStream stream2 = new FileStream(filePath, FileMode.Open, FileAccess.Read);
            byte[] buffer = Encoding.ASCII.GetBytes("\r\n--" + str2 + "--\r\n");
            request.Method = "POST";
            request.ContentType = "multipart/form-data; boundary=" + str2;
            request.CookieContainer = cookie;
            FileInfo fimy = new FileInfo(filePath);
            string filename = fimy.Name;
            string format = "Content-Disposition: form-data; name=\"{0}\"; filename=\"{1}\"\r\nContent-Type: image/jpeg\r\n\r\n";
            string s = string.Format(format, fileKeyName, filename);
            byte[] buffer3 = Encoding.UTF8.GetBytes(s);
            stream.Write(bytes, 0, bytes.Length);
            stream.Write(buffer3, 0, buffer3.Length);
            byte[] buffer4 = new byte[0x400];
            while ((num = stream2.Read(buffer4, 0, buffer4.Length)) != 0)
            {
                stream.Write(buffer4, 0, num);
            }
            stream2.Close();
            string stringKeyHeader = "\r\n--" + str2 + "\r\nContent-Disposition: form-data; name=\"{0}\"\r\n\r\n{1}";
            //foreach (byte[] buffer5 in from key in stringDict.Keys.Cast<string>()
            //                           select string.Format(stringKeyHeader, key, stringDict[key]) into formitem
            //                           select Encoding.UTF8.GetBytes(formitem))
            //{
            //    stream.Write(buffer5, 0, buffer5.Length);
            //}
            foreach (string key in stringDict.Keys)
            {
                string formitem = string.Format(stringKeyHeader, key, stringDict[key]);
                byte[] buffer5 = Encoding.UTF8.GetBytes(formitem);
                stream.Write(buffer5, 0, buffer5.Length);
            }
            stream.Write(buffer, 0, buffer.Length);
            request.ContentLength = stream.Length;
            Stream requestStream = request.GetRequestStream();
            stream.Position = 0L;
            byte[] buffer6 = new byte[stream.Length];
            stream.Read(buffer6, 0, buffer6.Length);
            stream.Close();
            requestStream.Write(buffer6, 0, buffer6.Length);
            requestStream.Close();
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            Stream responseStream = response.GetResponseStream();
            if (responseStream == null)
            {
                return string.Empty;
            }
            using (StreamReader reader = new StreamReader(responseStream, Encoding.UTF8))
            {
                str = reader.ReadToEnd();
            }
            response.Close();
            request.Abort();
            return str;
        }

        public static List<KeyValuePair<string, string>> ToKeyValuePair(this string str)
        {
            List<KeyValuePair<string, string>> list = new List<KeyValuePair<string, string>>();
            string[] strArray = str.Split(new char[] { '&' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string str2 in strArray)
            {
                string[] strArray2 = str2.Split(new char[] { '=' }, StringSplitOptions.RemoveEmptyEntries);
                list.Add(new KeyValuePair<string, string>(strArray2[0], strArray2[1]));
            }
            return list;
        }
        public static string GetMD5(string str)
        {
            string cl = str;
            string pwd = "";
            MD5 md5 = MD5.Create();//实例化一个md5对像
            // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(cl));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("x2");

            }
            return pwd;
        }
    }
}
