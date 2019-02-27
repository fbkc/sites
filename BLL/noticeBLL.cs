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
    public class noticeBLL
    {
        public List<noticeInfo> GetNoticeList(string sqlstr)
        {
            List<noticeInfo> nList = new List<noticeInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from noticeInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                noticeInfo nInfo = new noticeInfo();
                nInfo.Id = (int)SqlHelper.FromDBNull(row["Id"]);
                nInfo.notice = (string)SqlHelper.FromDBNull(row["notice"]);
                nInfo.pubTime = (DateTime)SqlHelper.FromDBNull(row["pubTime"]);
                nInfo.isImprotant = (bool)SqlHelper.FromDBNull(row["isImprotant"]);
                nInfo.issue = (bool)SqlHelper.FromDBNull(row["issue"]);
                nList.Add(nInfo);
            }
            return nList;
        }
        public void AddNotice(noticeInfo notice)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[noticeInfo]
           ([notice]
           ,[pubTime]
           ,isImprotant
           ,issue)
     VALUES
           (@notice
           ,getdate()
           ,@isImprotant
           ,@issue)",
               new SqlParameter("@notice", SqlHelper.ToDBNull(notice.notice)),
               new SqlParameter("@isImprotant", SqlHelper.ToDBNull(notice.isImprotant)),
               new SqlParameter("@issue", SqlHelper.ToDBNull(notice.issue)));
        }
        public void UpdateNotice(noticeInfo notice)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[noticeInfo]
   SET [notice] = @notice
      ,[pubTime] = getdate()
      ,[isImprotant] = @isImprotant
      ,[issue] = @issue where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(notice.Id)),
               new SqlParameter("@paraId", SqlHelper.ToDBNull(notice.notice)),
               new SqlParameter("@isImprotant", SqlHelper.ToDBNull(notice.isImprotant)),
               new SqlParameter("@issue", SqlHelper.ToDBNull(notice.issue)));
        }
        public int DelNotice(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from noticeInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
