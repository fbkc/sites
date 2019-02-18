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
           (mouldName
           ,contentMould
           ,type
           ,usedCount
           ,addTime
           ,editTime
           ,userId
           ,productId
           ,productName)
     VALUES
           (@mouldName
           ,@contentMould
           ,@type
           ,@usedCount
           ,@addTime
           ,@editTime
           ,@userId
           ,@productId
           ,@productName)",
               new SqlParameter("@mouldName", SqlHelper.ToDBNull(content.mouldName)),
               new SqlParameter("@contentMould", SqlHelper.ToDBNull(content.contentMould)),
               new SqlParameter("@type", SqlHelper.ToDBNull(content.type)),
               new SqlParameter("@usedCount", SqlHelper.ToDBNull(content.usedCount)),
               new SqlParameter("@addTime", SqlHelper.ToDBNull(content.addTime)),
               new SqlParameter("@editTime", SqlHelper.ToDBNull(content.editTime)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(content.userId)),
               new SqlParameter("@productId", SqlHelper.ToDBNull(content.productId)),
               new SqlParameter("@productName", SqlHelper.ToDBNull(content.productName)));
        }
        public void UpdateContent(contentMouldInfo content)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[contentMouldInfo]
   SET [mouldName] = @mouldName
      ,[contentMould] = @contentMould
      ,[usedCount] = @usedCount
      ,[editTime] = getdate() where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(content.Id)),
               new SqlParameter("@mouldName", SqlHelper.ToDBNull(content.mouldName)),
               new SqlParameter("@contentMould", SqlHelper.ToDBNull(content.contentMould)),
               new SqlParameter("@usedCount", SqlHelper.ToDBNull(content.usedCount)));
        }
        public int DelContent(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from contentMouldInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
