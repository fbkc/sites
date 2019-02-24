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
        public List<imageInfo> GetImgList(string sqlstr)
        {
            List<imageInfo> iList = new List<imageInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from imageInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                imageInfo iInfo = new imageInfo();
                iInfo.Id = (long)row["Id"];
                iInfo.imageId = (string)SqlHelper.FromDBNull(row["imageId"]);
                iInfo.imageURL = (string)SqlHelper.FromDBNull(row["imageURL"]);
                iInfo.addTime = ((DateTime)SqlHelper.FromDBNull(row["addTime"])).ToString("yyyy-MM-dd HH:mm:ss");
                iInfo.userId = (int)SqlHelper.FromDBNull(row["userId"]);
                iInfo.productId = (int)SqlHelper.FromDBNull(row["productId"]);
                iList.Add(iInfo);
            }
            return iList;
        }
        public void AddImg(imageInfo img)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[imageInfo]
           ([imageId]
           ,[imageURL]
           ,addTime
           ,userId
           ,productId)
     VALUES
           (@imageId
           ,@imageURL
           ,getdate()
           ,@userId
           ,@productId)",
               new SqlParameter("@imageId", SqlHelper.ToDBNull(img.imageId)),
               new SqlParameter("@imageURL", SqlHelper.ToDBNull(img.imageURL)),
               new SqlParameter("@productId", SqlHelper.ToDBNull(img.productId)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(img.userId)));
        }
        public int DelImg(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from imageInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
        public string GetFathById(string Id)
        {
            DataTable ds = SqlHelper.ExecuteDataTable("select imageURL from imageInfo where Id=@Id",
                new SqlParameter("@Id",Id));
            if (ds.Rows.Count < 0)
                return "";
            DataRow row = ds.Rows[0];
            return row["imageURL"].ToString();
        }
    }
}
