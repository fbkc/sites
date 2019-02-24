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
    public class bannerBLL
    {
        public List<bannerInfo> GetBanner(string sqlstr)
        {
            List<bannerInfo> bList = new List<bannerInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from bannerInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                bannerInfo bInfo = new bannerInfo();
                bInfo.Id = (int)row["Id"];
                bInfo.num = (int)row["num"];
                bInfo.banner = (string)row["banner"];
                bInfo.userId = (int)row["userId"];
                bList.Add(bInfo);
            }
            return bList;
        }
        public void AddBanner(bannerInfo bInfo)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[bannerInfo]
           ([banner]
           ,[num]
           ,[userId])
     VALUES
           (@banner
           ,@num
           ,@userId)",
               new SqlParameter("@banner", SqlHelper.ToDBNull(bInfo.banner)),
               new SqlParameter("@num", SqlHelper.ToDBNull(bInfo.num)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(bInfo.userId)));
        }
        public void UpUserBanner(bannerInfo bInfo)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[bannerInfo]
   SET [banner] = @banner where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(bInfo.Id)),
               new SqlParameter("@banner", SqlHelper.ToDBNull(bInfo.banner)));
        }
        public int DelBanner(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from bannerInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
