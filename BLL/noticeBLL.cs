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
        public DataTable GetNoticeList(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from noticeInfo " + sqlstr).Tables[0];
            return ds;
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
