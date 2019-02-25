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
            setInfo.everydayCount = (int)SqlHelper.FromDBNull(row["everydayCount"]);
            setInfo.isAutoPub = (bool)SqlHelper.FromDBNull(row["isAutoPub"]);
            setInfo.pubHour = (int)SqlHelper.FromDBNull(row["pubHour"]);
            setInfo.pubMin = (int)SqlHelper.FromDBNull(row["pubMin"]);
            setInfo.isPubing = (bool)SqlHelper.FromDBNull(row["isPubing"]);
            setInfo.userId = (int)SqlHelper.FromDBNull(row["userId"]);
            setInfo.username = (string)SqlHelper.FromDBNull(row["username"]);
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
        /// <summary>
        /// 更新配置
        /// </summary>
        /// <param name="set"></param>
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

        /// <summary>
        /// 轮循读取配置
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<settingInfo> RoundSetting(string sqlstr)
        {
            List<settingInfo> sList = new List<settingInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from setting " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                settingInfo setInfo = new settingInfo();
                setInfo.Id = (int)row["Id"];
                setInfo.everydayCount = (int)SqlHelper.FromDBNull(row["everydayCount"]);
                setInfo.isAutoPub = (bool)SqlHelper.FromDBNull(row["isAutoPub"]);
                setInfo.pubHour = (int)SqlHelper.FromDBNull(row["pubHour"]);
                setInfo.pubMin = (int)SqlHelper.FromDBNull(row["pubMin"]);
                setInfo.isPubing = (bool)SqlHelper.FromDBNull(row["isPubing"]);
                setInfo.userId = (int)SqlHelper.FromDBNull(row["userId"]);
                setInfo.username = (string)SqlHelper.FromDBNull(row["username"]);
                sList.Add(setInfo);
            }
            return sList;
        }
        /// <summary>
        /// isPubing 状态切换
        /// </summary>
        /// <param name="set"></param>
        public void UpIsPubing(settingInfo set)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[setting]
   SET [isPubing] = @isPubing  where userId=@userId",
               new SqlParameter("@userId", SqlHelper.ToDBNull(set.userId)),
               new SqlParameter("@isPubing", SqlHelper.ToDBNull(set.isPubing)));
        }
    }
}
