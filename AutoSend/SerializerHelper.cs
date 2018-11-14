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
}