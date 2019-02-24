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
        public List<contentMouldInfo> GetContentList(string sqlstr)
        {
            List<contentMouldInfo> cList = new List<contentMouldInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from contentMouldInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                contentMouldInfo cInfo = new contentMouldInfo();
                cInfo.Id = (int)row["Id"];
                cInfo.mouldName = (string)row["mouldName"];
                cInfo.contentMould = (string)row["contentMould"];
                cInfo.type = (string)row["type"];
                cInfo.usedCount = (int)row["usedCount"];
                cInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                cInfo.editTime = ((DateTime)row["editTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                cInfo.userId = (int)row["userId"];
                cInfo.productId = (int)row["productId"];
                cInfo.productName = (string)row["productName"];
                cList.Add(cInfo);
            }
            return cList;
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
           ,0
           ,getdate()
           ,getdate()
           ,@userId
           ,@productId
           ,@productName)",
               new SqlParameter("@mouldName", SqlHelper.ToDBNull(content.mouldName)),
               new SqlParameter("@contentMould", SqlHelper.ToDBNull(content.contentMould)),
               new SqlParameter("@type", SqlHelper.ToDBNull(content.type)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(content.userId)),
               new SqlParameter("@productId", SqlHelper.ToDBNull(content.productId)),
               new SqlParameter("@productName", SqlHelper.ToDBNull(content.productName)));
        }
        public void UpdateContent(contentMouldInfo content)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[contentMouldInfo]
   SET [mouldName] = @mouldName
      ,[contentMould] = @contentMould
      ,[type] = @type
      ,[editTime] = getdate() where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(content.Id)),
               new SqlParameter("@mouldName", SqlHelper.ToDBNull(content.mouldName)),
               new SqlParameter("@contentMould", SqlHelper.ToDBNull(content.contentMould)),
               new SqlParameter("@type", SqlHelper.ToDBNull(content.type)));
        }
        public int DelContent(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from contentMouldInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
