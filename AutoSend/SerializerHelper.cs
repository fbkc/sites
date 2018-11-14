using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.IO;
using System.Xml.Serialization;
using Newtonsoft.Json;

/// <summary>
/// SerializerHelper 的摘要说明
/// </summary>
public static class SerializerHelper
{
    /// <summary>
    /// 反序列化XML文件
    /// </summary>
    public static T LoadFromXmlFile<T>(string filepath) where T : class
    {
        using (FileStream stream = new FileStream(filepath, FileMode.Open, FileAccess.Read))
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stream);
        }
    }

    /// <summary>
    /// 反序列化XML字符串
    /// </summary>
    public static T LoadFromXmlString<T>(string xml) where T : class
    {
        XmlSerializer serializer = new XmlSerializer(typeof(T));
        byte[] bytes = Encoding.UTF8.GetBytes(xml);

        using (MemoryStream stream = new MemoryStream(bytes))
        {
            return (T)serializer.Deserialize(stream);
        }
    }

    /// <summary>
    /// 序列化XML对象
    /// </summary>
    public static string SaveToXmlString<T>(T entity) where T : class
    {
        using (MemoryStream stream = new MemoryStream())
        {
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stream, entity);
            return Encoding.UTF8.GetString(stream.ToArray());
        }
    }

    /// <summary>
    /// 序列化Json对象
    /// </summary>
    public static string ToJsonString(object obj)
    {
        return ToJsonString<object>(obj);
    }

    /// <summary>
    /// 序列化Json对象
    /// </summary>
    public static string ToJsonString<T>(T obj) where T : class
    {
        string text = JsonConvert.SerializeObject(obj);
        return text;
    }

    /// <summary>
    /// 反序列化Json字符串
    /// </summary>
    public static T ToJsonObject<T>(string text) where T : class
    {
        T obj = (T)JsonConvert.DeserializeObject(text, typeof(T));
        return obj;
    }

    /// <summary>
    /// 将对象序列化为JSON格式
        /// </summary>
        /// <param name="o">对象</param>
        /// <returns>json字符串</returns>
    public static string SerializeObject(object o)
    {
        string json = JsonConvert.SerializeObject(o);
        return json;
    }


    /// <summary>
    /// 解析JSON字符串生成对象实体
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="json">json字符串(eg.{"ID":"112","Name":"石子儿"})</param>
    /// <returns>对象实体</returns>
    public static T DeserializeJsonToObject<T>(string json) where T : class
    {
        try
        {
            JsonSerializer serializer = new JsonSerializer();
            StringReader sr = new StringReader(json);
            object o = serializer.Deserialize(new JsonTextReader(sr), typeof(T));
            T t = o as T;
            return t;
        }
        catch (Exception ex)
        {
            return null;
        }
    }
    /// <summary>
    /// 解析JSON数组生成对象实体集合
    /// </summary>
    /// <typeparam name="T">对象类型</typeparam>
    /// <param name="json">json数组字符串(eg.[{"ID":"112","Name":"石子儿"}])</param>
    /// <returns>对象实体集合</returns>
    public static List<T> DeserializeJsonToList<T>(string json) where T : class
    {
        JsonSerializer serializer = new JsonSerializer();
        StringReader sr = new StringReader(json);
        object o = serializer.Deserialize(new JsonTextReader(sr), typeof(List<T>));
        List<T> list = o as List<T>;
        return list;
    }
}