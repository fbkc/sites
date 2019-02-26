using HRMSys.DAL;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace BLL
{
    public class logBLL
    {
        /// <summary>
        /// 读取单条日志
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public logInfo GetLogInfo(string sqlstr)
        {
            DataTable dt = SqlHelper.ExecuteDataSet("select * from logInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            DataRow row = dt.Rows[0];
            logInfo lInfo = new logInfo();
            lInfo.Id = (long)row["Id"];
            lInfo.addtime = ((DateTime)SqlHelper.FromDBNull(row["addtime"])).ToString("yyyy-MM-dd HH:mm:ss");
            lInfo.ErroMsg = (string)SqlHelper.FromDBNull(row["ErroMsg"]);
            lInfo.userId = (string)SqlHelper.FromDBNull(row["userId"]);
            lInfo.username = (string)SqlHelper.FromDBNull(row["username"]);
            return lInfo;
        }
        /// <summary>
        /// 读取前十条日志
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<logInfo> GetLogs(string sqlstr)
        {
            List<logInfo> logList = new List<logInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select top 10 * from logInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                logInfo lInfo = new logInfo();
                lInfo.Id = (long)row["Id"];
                lInfo.addtime = ((DateTime)SqlHelper.FromDBNull(row["addtime"])).ToString("yyyy-MM-dd HH:mm:ss");
                lInfo.ErroMsg = (string)SqlHelper.FromDBNull(row["ErroMsg"]);
                lInfo.userId = (string)SqlHelper.FromDBNull(row["userId"]);
                lInfo.username = (string)SqlHelper.FromDBNull(row["username"]);
                logList.Add(lInfo);
            }
            return logList;
        }
        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="sqlstr"></param>
        public void AddLog(logInfo lInfo)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[logInfo]
           ([addtime]
           ,[ErroMsg]
           ,[userId]
           ,[username])
     VALUES
           (getdate()
           ,@ErroMsg
           ,@userId
           ,@username)",
               new SqlParameter("@ErroMsg", SqlHelper.ToDBNull(lInfo.ErroMsg)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(lInfo.userId)),
               new SqlParameter("@username", SqlHelper.ToDBNull(lInfo.username)));
        }
    }
}
