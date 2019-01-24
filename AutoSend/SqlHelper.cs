using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;
using NVelocity.App;
using NVelocity.Runtime;
using NVelocity;

namespace HRMSys.DAL
{
    class SqlHelper
    {
        private static string connStr = ConfigurationManager.ConnectionStrings["connStr"].ConnectionString.ToString();
        //封装的原则：把不变的放到方法里 吧变化的（string sql）传到参数里
        public static int ExecuteNonQuery(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    //foreach (SqlParameter param in parameters)
                    //{
                    //    cmd.Parameters.Add(param);
                    //}
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public static int ExecuteNonQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    //foreach (SqlParameter param in parameters)
                    //{
                    //    cmd.Parameters.Add(param);
                    //}
                    return cmd.ExecuteNonQuery();
                }
            }
        }
        public static object ExecuteScalar(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    return cmd.ExecuteScalar();
                }
            }
        }
        //只用来执行查询结果比较少的sql
        public static DataTable ExecuteDataTable(string sql, params SqlParameter[] parameters)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    cmd.Parameters.AddRange(parameters);
                    DataSet dataset = new DataSet();
                    SqlDataAdapter apdater = new SqlDataAdapter(cmd);
                    apdater.FillSchema(dataset, SchemaType.Source);//获得表信息必须要写
                    apdater.Fill(dataset);
                    return dataset.Tables[0];
                }
            }
        }
        //只用来执行查询结果比较少的sql
        public static DataSet ExecuteDataSet(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    DataSet dataset = new DataSet();
                    SqlDataAdapter apdater = new SqlDataAdapter(cmd);
                    apdater.FillSchema(dataset, SchemaType.Source);//获得表信息必须要写
                    apdater.Fill(dataset);
                    return dataset;
                }
            }
        }
        public static SqlDataReader ExecuteDataReader(string sql, params SqlParameter[] parameters)
        {
            //using (SqlConnection conn = new SqlConnection(connStr))
            //{
            //    conn.Open();
            //    using (SqlCommand cmd = conn.CreateCommand())
            //    {
            //        cmd.CommandText = sql;
            //        cmd.Parameters.AddRange(parameters);
            //        SqlDataReader sd = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            //        return sd;
            //    }
            //}
            SqlConnection conn = null;
            try
            {
                conn = new SqlConnection(connStr);
                conn.Open();
                SqlCommand comm = new SqlCommand(sql, conn);
                comm.Parameters.AddRange(parameters);
                SqlDataReader reader = comm.ExecuteReader(CommandBehavior.CloseConnection);
                return reader;
            }
            catch (Exception ex)
            {
                throw ex;
            }          
        }
        public static object FromDBNull(object values)
        {
            if (values==DBNull.Value)
            {
                return null;
            }
            else
            {
                return values;
            }
        }
        public static object ToDBNull(object values)
        {
            if (values == null)
            {
                return DBNull.Value;
            }
            else
            {
                return values;
            }
        }
        /// <summary>
        /// 渲染模板引擎
        /// </summary>
        /// <param name="dic">需要替换的参数</param>
        /// <param name="temp">html文件名</param>
        /// <returns></returns>
        public static string WriteTemplate(object data, string temp)
        {
            //用的时候考代码即可，只需改三个地方：模板所在文件夹、添加数据、设定模板
            VelocityEngine vltEngine = new VelocityEngine();
            vltEngine.SetProperty(RuntimeConstants.RESOURCE_LOADER, "file");
            vltEngine.SetProperty(RuntimeConstants.FILE_RESOURCE_LOADER_PATH, System.Web.Hosting.HostingEnvironment.MapPath("~/templates"));//模板文件所在的文件夹，例如我的模板为templates文件夹下的TestNV.html
            vltEngine.Init();

            VelocityContext vltContext = new VelocityContext();
            vltContext.Put("data", data);//可添加多个数据，基本支持所有数据类型，包括字典、数据、datatable等  添加数据，在模板中可以通过$dataName来引用数据
            Template vltTemplate = vltEngine.GetTemplate(temp);//设定模板
            System.IO.StringWriter vltWriter = new System.IO.StringWriter();
            vltTemplate.Merge(vltContext, vltWriter);

            string html = vltWriter.GetStringBuilder().ToString();
            return html;//返回渲染生成的标准html代码字符串
        }
    }
}
