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
    public class productBLL
    {
        public DataTable GetProduct(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from productInfo " + sqlstr).Tables[0];
            return ds;
        }
        public List<productInfo> GetProduct1(string sqlstr)
        {
            List<productInfo> pList = new List<productInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from productInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                productInfo pInfo = new productInfo();
                pInfo.Id = (int)row["Id"];
                pInfo.productName = (string)row["productName"];
                pInfo.userId = (int)row["userId"];
                if (row["pinpai"] == DBNull.Value)
                    pInfo.pinpai = null;
                else
                    pInfo.pinpai = (string)row["pinpai"];
                if (row["xinghao"] == DBNull.Value)
                    pInfo.xinghao = null;
                else
                    pInfo.xinghao = (string)row["xinghao"];
                if (row["price"] == DBNull.Value)
                    pInfo.price = null;
                else
                    pInfo.price = (string)row["price"];
                if (row["smallCount"] == DBNull.Value)
                    pInfo.smallCount = null;
                else
                    pInfo.smallCount = (string)row["smallCount"];
                if (row["sumCount"] == DBNull.Value)
                    pInfo.sumCount = null;
                else
                    pInfo.sumCount = (string)row["sumCount"];
                if (row["unit"] == DBNull.Value)
                    pInfo.unit = null;
                else
                    pInfo.unit = (string)row["unit"];
                if (row["city"] == DBNull.Value)
                    pInfo.city = null;
                else
                    pInfo.city = (string)row["city"];
                pInfo.createTime = ((DateTime)row["createTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                pInfo.editTime = ((DateTime)row["editTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                if (row["informationType"] == DBNull.Value)
                    pInfo.informationType = null;
                else
                    pInfo.informationType = (string)row["informationType"];//产品/新闻
                if (row["maxPubCount"] == DBNull.Value)
                    pInfo.maxPubCount = null;
                else
                    pInfo.maxPubCount = (int)row["maxPubCount"];
                if (row["endPubCount"] == DBNull.Value)
                    pInfo.endPubCount = null;
                else
                    pInfo.endPubCount = (int)row["endPubCount"];
                if (row["endTodayPubCount"] == DBNull.Value)
                    pInfo.endTodayPubCount = null;
                else
                    pInfo.endTodayPubCount = (int)row["endTodayPubCount"];
                if (row["pub_startTime"] == DBNull.Value)
                    pInfo.pub_startTime = null;
                else
                    pInfo.pub_startTime = ((DateTime)row["pub_startTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                if (row["pubInterval"] == DBNull.Value)
                    pInfo.pubInterval = null;
                else
                    pInfo.pubInterval = (int)row["pubInterval"];
                pInfo.isPub = (bool)row["isPub"];
                pInfo.isdel = (bool)row["isdel"];
                pList.Add(pInfo);
            }
            return pList;
        }
        /// <summary>
        /// 增加产品类型信息
        /// </summary>
        /// <param name="sqlstr"></param>
        public void AddProduct(productInfo product)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[productInfo]
           (productName,userId,pinpai,xinghao,price,smallCount,sumCount,unit,city,createTime,editTime,informationType,maxPubCount,endPubCount,endTodayPubCount,pub_startTime,pubInterval,isPub,isdel) VALUES
           (@productName,@userId,@pinpai,@xinghao,@price,@smallCount,@sumCount,@unit,@city,getdate(),getdate(),@informationType,@maxPubCount,@endPubCount,@endTodayPubCount,@pub_startTime,@pubInterval,0,0)",
               new SqlParameter("@productName", SqlHelper.ToDBNull(product.productName)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(product.userId)),
               new SqlParameter("@pinpai", SqlHelper.ToDBNull(product.pinpai)),
               new SqlParameter("@xinghao", SqlHelper.ToDBNull(product.xinghao)),
               new SqlParameter("@price", SqlHelper.ToDBNull(product.price)),
               new SqlParameter("@smallCount", SqlHelper.ToDBNull(product.smallCount)),
               new SqlParameter("@sumCount", SqlHelper.ToDBNull(product.sumCount)),
               new SqlParameter("@unit", SqlHelper.ToDBNull(product.unit)),
               new SqlParameter("@city", SqlHelper.ToDBNull(product.city)),
               new SqlParameter("@informationType", SqlHelper.ToDBNull(product.informationType)),
               new SqlParameter("@maxPubCount", SqlHelper.ToDBNull(product.maxPubCount)),
               new SqlParameter("@endPubCount", SqlHelper.ToDBNull(product.endPubCount)),
               new SqlParameter("@pub_startTime", SqlHelper.ToDBNull(product.pub_startTime)),
               new SqlParameter("@pubInterval", SqlHelper.ToDBNull(product.pubInterval)),
               new SqlParameter("@endTodayPubCount", SqlHelper.ToDBNull(product.endTodayPubCount)));
        }
        /// <summary>
        /// 更新产品类型信息
        /// </summary>
        /// <param name="sqlstr"></param>
        public void UpdateProduct(productInfo product)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[productInfo]
   SET [productName] = @productName
      ,[pinpai] = @pinpai
      ,[xinghao] = @xinghao
      ,[price]=@price
      ,[smallCount] = @smallCount
      ,[sumCount] = @sumCount
      ,[unit] = @unit
      ,[city] = @city
      ,[editTime] = getdate()
      ,[informationType] = @informationType
      ,[maxPubCount] = @maxPubCount
      ,[endPubCount] = @endPubCount 
      ,[endTodayPubCount] = @endTodayPubCount
      ,[pub_startTime] = @pub_startTime
      ,[pubInterval] = @pubInterval
      ,[isPub] = @isPub
      ,[isdel] = @isdel where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(product.Id)),
               new SqlParameter("@productName", SqlHelper.ToDBNull(product.productName)),
               new SqlParameter("@pinpai", SqlHelper.ToDBNull(product.pinpai)),
               new SqlParameter("@xinghao", SqlHelper.ToDBNull(product.xinghao)),
               new SqlParameter("@price", SqlHelper.ToDBNull(product.price)),
               new SqlParameter("@smallCount", SqlHelper.ToDBNull(product.smallCount)),
               new SqlParameter("@sumCount", SqlHelper.ToDBNull(product.sumCount)),
               new SqlParameter("@unit", SqlHelper.ToDBNull(product.unit)),
               new SqlParameter("@city", SqlHelper.ToDBNull(product.city)),
               new SqlParameter("@informationType", SqlHelper.ToDBNull(product.informationType)),
               new SqlParameter("@maxPubCount", SqlHelper.ToDBNull(product.maxPubCount)),
               new SqlParameter("@endPubCount", SqlHelper.ToDBNull(product.endPubCount)),
               new SqlParameter("@endTodayPubCount", SqlHelper.ToDBNull(product.endTodayPubCount)),
               new SqlParameter("@pub_startTime", SqlHelper.ToDBNull(product.pub_startTime)),
               new SqlParameter("@pubInterval", SqlHelper.ToDBNull(product.pubInterval)),
               new SqlParameter("@isPub", SqlHelper.ToDBNull(product.isPub)),
               new SqlParameter("@isdel", SqlHelper.ToDBNull(product.isdel)));
        }
        /// <summary>
        /// 删除产品类型
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public int DelProduct(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from productInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
