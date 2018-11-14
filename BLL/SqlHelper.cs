using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace Data
{
    #region lhc
    public class SqlHelper
    {
        //private static string connStr = ConfigurationManager.ConnectionStrings["data"].ConnectionString;User ID=fangyuan001;Password=fangyuan001 @"Data Source=localhost;database = 你的数据库名;Integrated security = true "
        private static string connStr = "Data Source=39.105.196.3;Initial Catalog=AutouSend;User ID=lhc;Password=123456";
        /// <summary>
        /// 返回受影响的数据行数
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static int ExecuteNoQuery(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    return cmd.ExecuteNonQuery();

                }
            }
        }
        /// <summary>
        /// 返回一个数据集
        /// </summary>
        /// <param name="sql"></param>
        /// <returns></returns>
        public static DataSet ExecuteDataSet(string sql)
        {
            using (SqlConnection xonn = new SqlConnection(connStr))
            {
                xonn.Open();
                using (SqlCommand cmd = xonn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                    DataSet dataset = new DataSet();
                    adapter.Fill(dataset);
                    return dataset;
                }
            }
        }
        public static object ExecuteScalar(string sql)
        {
            using (SqlConnection conn = new SqlConnection(connStr))
            {
                conn.Open();
                using (SqlCommand cmd = conn.CreateCommand())
                {
                    cmd.CommandText = sql;
                    return cmd.ExecuteScalar();
                }
            }
        }
    }
    #endregion
}