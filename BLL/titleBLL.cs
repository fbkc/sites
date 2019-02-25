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
        public List<titleInfo> GetTitleList(string sqlstr)
        {
            List<titleInfo> tList = new List<titleInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from titleInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                titleInfo tInfo = new titleInfo();
                tInfo.Id = (long)row["Id"];
                tInfo.title = (string)row["title"];
                tInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                tInfo.editTime = ((DateTime)row["editTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                tInfo.isSucceedPub = (bool)row["isSucceedPub"];
                tInfo.returnMsg = (string)row["returnMsg"];
                tInfo.productId = (int)row["productId"];
                tInfo.userId = (int)row["userId"];
                tList.Add(tInfo);
            }
            return tList;
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
           ,getdate()
           ,getdate()
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
      ,[editTime] = getdate()
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
