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
        public List<paragraphInfo> GetParagraphList(string sqlstr)
        {
            List<paragraphInfo> pList = new List<paragraphInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from paragraphInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                paragraphInfo pInfo = new paragraphInfo();
                pInfo.Id = (long)row["Id"];
                pInfo.paraId = (string)row["paraId"];
                pInfo.paraCotent = (string)row["paraCotent"];
                pInfo.usedCount = (int)row["usedCount"];
                pInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                pInfo.userId = (int)row["userId"];
                pInfo.productId = (int)row["productId"];
                pList.Add(pInfo);
            }
            return pList;
        }
        public int AddParagraph(paragraphInfo paragraph)
        {
            return SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[paragraphInfo]
           ([paraId]
           ,[paraCotent]
           ,usedCount
           ,addTime
           ,userId
           ,productId)
     VALUES
           (@paraId
           ,@paraCotent
           ,0
           ,getdate()
           ,@userId
           ,@productId)",
               new SqlParameter("@paraId", SqlHelper.ToDBNull(paragraph.paraId)),
               new SqlParameter("@paraCotent", SqlHelper.ToDBNull(paragraph.paraCotent)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(paragraph.userId)),
               new SqlParameter("@productId", SqlHelper.ToDBNull(paragraph.productId)));
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
            object o = SqlHelper.ExecuteScalar("SELECT TOP 1 Id FROM paragraphInfo ORDER BY Id DESC");
            return int.Parse(o.ToString());
        }
    }
}
