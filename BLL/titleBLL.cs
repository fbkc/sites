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
    public class titleBLL
    {
        public DataTable GetTitleList(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from titleInfo " + sqlstr).Tables[0];
            return ds;
        }
        public void AddTitle(titleInfo title)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[titleInfo]
           ([title]
           ,[addTime]
           ,editTime
           ,isSucceedPub
           ,returnMsg
           ,userId
           ,productId)
     VALUES
           (@title
           ,getdata()
           ,getdata()
           ,0
           ,@returnMsg
           ,@userId
           ,@productId)",
               new SqlParameter("@title", SqlHelper.ToDBNull(title.title)),
               new SqlParameter("@returnMsg", SqlHelper.ToDBNull(title.returnMsg)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(title.userId)),
               new SqlParameter("@productId", SqlHelper.ToDBNull(title.productId)));
        }
        public void UpdateTitle(titleInfo title)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[titleInfo]
   SET [title] = @title
      ,[editTime] = getdata()
      ,[isSucceedPub] = @isSucceedPub
      ,[returnMsg] = @returnMsg where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(title.Id)),
               new SqlParameter("@title", SqlHelper.ToDBNull(title.title)),
               new SqlParameter("@isSucceedPub", SqlHelper.ToDBNull(title.isSucceedPub)),
               new SqlParameter("@returnMsg", SqlHelper.ToDBNull(title.returnMsg)));
        }
        public int DelTitle(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from titleInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
