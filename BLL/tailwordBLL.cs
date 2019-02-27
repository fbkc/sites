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
        public List<tailwordInfo> GetTailwordList()
        {
            List<tailwordInfo> tList = new List<tailwordInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from tailwordInfo").Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                tailwordInfo tInfo = new tailwordInfo();
                tInfo.Id = (int)SqlHelper.FromDBNull(row["Id"]);
                tInfo.tailword = (string)SqlHelper.FromDBNull(row["tailword"]);
                tList.Add(tInfo);
            }
            return tList;
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
