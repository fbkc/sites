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
    public class paragraphBLL
    {
        public DataTable GetParagraphList(string id)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from paragraphInfo where userId=" + id).Tables[0];
            return ds;
        }
        public int AddParagraph(paragraphInfo paragraph)
        {
            return SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[paragraphInfo]
           ([paraId]
           ,[paraCotent]
           ,usedCount
           ,addTime
           ,userId)
     VALUES
           (@paraId
           ,@paraCotent
           ,0
           ,getdate()
           ,@userId)",
               new SqlParameter("@paraId", SqlHelper.ToDBNull(paragraph.paraId)),
               new SqlParameter("@paraCotent", SqlHelper.ToDBNull(paragraph.paraCotent)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(paragraph.userId)));
        }
        public void UpdateParagraph(paragraphInfo paragraph)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[paragraphInfo]
   SET [paraCotent] = @paraCotent
      ,[usedCount] = @usedCount
      ,[addTime] = getdate() where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(paragraph.Id)),
               new SqlParameter("@paraCotent", SqlHelper.ToDBNull(paragraph.paraCotent)),
               new SqlParameter("@usedCount", SqlHelper.ToDBNull(paragraph.usedCount)));
        }
        public int DelParagraph(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from paragraphInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
        public int GetNumId()
        {
            object o= SqlHelper.ExecuteScalar("SELECT TOP 1 Id FROM paragraphInfo ORDER BY Id DESC");
            return int.Parse(o.ToString());
        }
    }
}
