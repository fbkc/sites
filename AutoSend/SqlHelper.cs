using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;
using System.Data;

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
    }
}
