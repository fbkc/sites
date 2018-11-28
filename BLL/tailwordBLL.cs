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
        public DataTable GetTailwordList(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from tailwordInfo " + sqlstr).Tables[0];
            return ds;
        }
        public void AddTailword(tailwordInfo tailword)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[tailwordInfo]
           ([tailword]
           ,addTime
           ,[userId])
     VALUES
           (@tailword
           ,getdate()
           ,@userId)",
               new SqlParameter("@Content", SqlHelper.ToDBNull(tailword.tailword)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(tailword.userId)));
        }
        public void UpdateTailword(tailwordInfo tailword)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[tailwordInfo]
   SET [tailword] = @tailword
      ,[addTime] = getdate()
      ,[userId] = @userId where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(tailword.Id)),
               new SqlParameter("@paraId", SqlHelper.ToDBNull(tailword.tailword)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(tailword.userId)));
        }
        public int DelTailword(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from tailwordInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
