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
    public class tailwordBLL
    {
        public DataTable GetTailwordList()
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from tailwordInfo").Tables[0];
            return ds;
        }
        public void AddTailword(tailwordInfo tailword)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[tailwordInfo]
           ([tailword])
     VALUES
           (@tailword)",
               new SqlParameter("@tailword", SqlHelper.ToDBNull(tailword.tailword)));
        }
        public void UpdateTailword(tailwordInfo tailword)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[tailwordInfo]
   SET [tailword] = @tailword where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(tailword.Id)),
               new SqlParameter("@tailword", SqlHelper.ToDBNull(tailword.tailword)));
        }
        public int DelTailword(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from tailwordInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
