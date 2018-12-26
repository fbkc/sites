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
