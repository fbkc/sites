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
    public class contentMouldBLL
    {
        public DataTable GetContentList(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from contentMouldInfo " + sqlstr).Tables[0];
            return ds;
        }
        public void AddContent(contentMouldInfo content)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[contentMouldInfo]
           ([mouldId]
           ,[mouldName]
           ,contentMould
           ,usedCount
           ,addTime
           ,userId)
     VALUES
           (@mouldId
           ,@mouldName
           ,@contentMould
           ,@usedCount
           ,getdate()
           ,@userId)",
               new SqlParameter("@mouldId", SqlHelper.ToDBNull(content.mouldId)),
               new SqlParameter("@mouldName", SqlHelper.ToDBNull(content.mouldName)),
               new SqlParameter("@contentMould", SqlHelper.ToDBNull(content.contentMould)),
               new SqlParameter("@usedCount", SqlHelper.ToDBNull(content.usedCount)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(content.userId)));
        }
        public void UpdateContent(contentMouldInfo content)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[paragraphInfo]
   SET [mouldId] = @mouldId
      ,[mouldName] = @mouldName
      ,[contentMould] = @contentMould
      ,[usedCount] = @usedCount
      ,[addTime] = getdate()
      ,[userId] = @userId where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(content.Id)),
               new SqlParameter("@mouldId", SqlHelper.ToDBNull(content.mouldId)),
               new SqlParameter("@mouldName", SqlHelper.ToDBNull(content.mouldName)),
               new SqlParameter("@contentMould", SqlHelper.ToDBNull(content.contentMould)),
               new SqlParameter("@usedCount", SqlHelper.ToDBNull(content.usedCount)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(content.userId)));
        }
        public int DelContent(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from contentMouldInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
