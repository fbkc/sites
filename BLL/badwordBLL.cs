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
    public class badwordBLL
    {
        public DataTable GetBadwordList()
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from badwordInfo").Tables[0];
            return ds;
        }
        public void AddBadword(badwordInfo badword)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[badwordInfo]
           ([badword])
     VALUES
           (@badword)",
               new SqlParameter("@badword", SqlHelper.ToDBNull(badword.badword)));
        }
        public void UpdateBadword(badwordInfo badword)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[badwordInfo]
   SET [badword] = @badword where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(badword.Id)),
               new SqlParameter("@badword", SqlHelper.ToDBNull(badword.badword)));
        }
        public int DelBadword(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from badwordInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
