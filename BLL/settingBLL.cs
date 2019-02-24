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
    public class settingBLL
    {
        /// <summary>
        /// 读取配置
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public settingInfo GetSetting(string sqlstr)
        {
            DataTable dt = SqlHelper.ExecuteDataSet("select * from setting " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            DataRow row = dt.Rows[0];
            settingInfo setInfo = new settingInfo();
            setInfo.Id = (int)row["Id"];
            setInfo.everydayCount = (int)row["everydayCount"];
            setInfo.isAutoPub = (bool)row["isAutoPub"];
            setInfo.pubHour = (int)row["pubHour"];
            setInfo.pubMin = (int)row["pubMin"];
            setInfo.isPubing = (bool)row["isPubing"];
            setInfo.userId = (int)row["userId"];
            setInfo.username = (string)row["username"];
            return setInfo;
        }

        /// <summary>
        /// 添加配置
        /// </summary>
        /// <param name="sqlstr"></param>
        public void AddSetting(settingInfo set)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[setting]
           ([everydayCount]
           ,[isAutoPub]
           ,[pubHour]
           ,[pubMin]
           ,[isPubing]
           ,[userId]
           ,[username])
     VALUES
           (@everydayCount
           ,@isAutoPub
           ,@pubHour
           ,@pubMin
           ,@isPubing
           ,@userId
           ,@username)",
               new SqlParameter("@username", SqlHelper.ToDBNull(set.username)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(set.userId)),
               new SqlParameter("@isPubing", SqlHelper.ToDBNull(set.isPubing)),
               new SqlParameter("@pubHour", SqlHelper.ToDBNull(set.pubHour)),
               new SqlParameter("@pubMin", SqlHelper.ToDBNull(set.pubMin)),
               new SqlParameter("@isAutoPub", SqlHelper.ToDBNull(set.isAutoPub)),
               new SqlParameter("@everydayCount", SqlHelper.ToDBNull(set.everydayCount)));
        }
        public void UpdateSetting(settingInfo set)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[setting]
   SET [everydayCount] = @everydayCount
      ,[isAutoPub] = @isAutoPub 
      ,[pubHour] = @pubHour 
      ,[pubMin] = @pubMin
      ,[isPubing] = @isPubing  where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(set.Id)),
               new SqlParameter("@everydayCount", SqlHelper.ToDBNull(set.everydayCount)),
               new SqlParameter("@isAutoPub", SqlHelper.ToDBNull(set.isAutoPub)),
               new SqlParameter("@pubHour", SqlHelper.ToDBNull(set.pubHour)),
               new SqlParameter("@pubMin", SqlHelper.ToDBNull(set.pubMin)),
               new SqlParameter("@isPubing", SqlHelper.ToDBNull(set.isPubing)));
        }
        public int DelSetting(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from setting where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
