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
        public DataTable GetParagraphList(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from paragraphInfo " + sqlstr).Tables[0];
            return ds;
        }
        public void AddParagraph(paragraphInfo paragraph)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[paragraphInfo]
           ([paraId]
           ,[paraCotent]
           ,usedCount
           ,userId)
     VALUES
           (@paraId
           ,@paraCotent
           ,usedCount
           ,userId)",
               new SqlParameter("@paraId", SqlHelper.ToDBNull(paragraph.paraId)),
               new SqlParameter("@paraCotent", SqlHelper.ToDBNull(paragraph.paraCotent)),
               new SqlParameter("@usedCount", SqlHelper.ToDBNull(paragraph.usedCount)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(paragraph.userId)));
        }
        public void UpdateParagraph(paragraphInfo paragraph, string sqlstr)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[paragraphInfo]
   SET [paraId] = @paraId
      ,[paraCotent] = @paraCotent
      ,[usedCount] = @usedCount
      ,[userId] = @userId" + sqlstr,
               new SqlParameter("@Id", SqlHelper.ToDBNull(paragraph.Id)),
               new SqlParameter("@paraId", SqlHelper.ToDBNull(paragraph.paraId)),
               new SqlParameter("@paraCotent", SqlHelper.ToDBNull(paragraph.paraCotent)),
               new SqlParameter("@usedCount", SqlHelper.ToDBNull(paragraph.usedCount)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(paragraph.userId)));
        }
        public int DelParagraph(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from paragraphInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
