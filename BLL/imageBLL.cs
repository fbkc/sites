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
    public class imageBLL
    {
        public DataTable GetImgList(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from imageInfo " + sqlstr).Tables[0];
            return ds;
        }
        public void AddImg(imageInfo img)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[imageInfo]
           ([imageId]
           ,[imageURL]
           ,addTime
           ,userId)
     VALUES
           (@imageId
           ,@imageURL
           ,getdate()
           ,@userId)",
               new SqlParameter("@imageId", SqlHelper.ToDBNull(img.imageId)),
               new SqlParameter("@imageURL", SqlHelper.ToDBNull(img.imageURL)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(img.userId)));
        }
        public int DelImg(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from imageInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
