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
        /// <summary>
        /// 获取产品列表
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<productInfo> GetProductList(string sqlstr)
        {
            List<productInfo> pList = new List<productInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from productInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                productInfo pInfo = new productInfo();
                pInfo.Id = (int)SqlHelper.FromDBNull(row["Id"]);
                pInfo.productName = (string)SqlHelper.FromDBNull(row["productName"]);
                pInfo.userId = (int)SqlHelper.FromDBNull(row["userId"]);
                pInfo.pinpai = (string)SqlHelper.FromDBNull(row["pinpai"]);
                pInfo.xinghao = (string)SqlHelper.FromDBNull(row["xinghao"]);
                pInfo.price = (string)SqlHelper.FromDBNull(row["price"]);
                pInfo.smallCount = (string)SqlHelper.FromDBNull(row["smallCount"]);
                pInfo.sumCount = (string)SqlHelper.FromDBNull(row["sumCount"]);
                pInfo.unit = (string)SqlHelper.FromDBNull(row["unit"]);
                pInfo.city = (string)SqlHelper.FromDBNull(row["city"]);
                pInfo.createTime = ((DateTime)SqlHelper.FromDBNull(row["createTime"])).ToString("yyyy-MM-dd HH:mm:ss");
                pInfo.editTime = ((DateTime)SqlHelper.FromDBNull(row["editTime"])).ToString("yyyy-MM-dd HH:mm:ss");
                pInfo.informationType = (string)SqlHelper.FromDBNull(row["informationType"]);//产品/新闻
                pList.Add(pInfo);
            }
            return pList;
        }
        /// <summary>
        /// 获取单个产品信息
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public productInfo GetProductInfo(string sqlstr)
        {
            DataTable dt = SqlHelper.ExecuteDataSet("select * from productInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            DataRow row = dt.Rows[0];
            productInfo pInfo = new productInfo();
            pInfo.Id = (int)SqlHelper.FromDBNull(row["Id"]);
            pInfo.productName = (string)SqlHelper.FromDBNull(row["productName"]);
            pInfo.userId = (int)SqlHelper.FromDBNull(row["userId"]);
            pInfo.pinpai = (string)SqlHelper.FromDBNull(row["pinpai"]);
            pInfo.xinghao = (string)SqlHelper.FromDBNull(row["xinghao"]);
            pInfo.price = (string)SqlHelper.FromDBNull(row["price"]);
            pInfo.smallCount = (string)SqlHelper.FromDBNull(row["smallCount"]);
            pInfo.sumCount = (string)SqlHelper.FromDBNull(row["sumCount"]);
            pInfo.unit = (string)SqlHelper.FromDBNull(row["unit"]);
            pInfo.city = (string)SqlHelper.FromDBNull(row["city"]);
            pInfo.createTime = ((DateTime)SqlHelper.FromDBNull(row["createTime"])).ToString("yyyy-MM-dd HH:mm:ss");
            pInfo.editTime = ((DateTime)SqlHelper.FromDBNull(row["editTime"])).ToString("yyyy-MM-dd HH:mm:ss");
            pInfo.informationType = (string)SqlHelper.FromDBNull(row["informationType"]);//产品/新闻
            return pInfo;
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
            return SqlHelper.ExecuteNonQuery("delete from productInfo where Id=@Id "//删除该产品
                + "delete from wordsInfo where productId=@Id"//删除长尾词和关键词
                + "delete from titleInfo where isSucceedPub=0 and productId=@Id"//删除此产品下未发布标题
                + "delete from paragraphInfo where productId=@Id"//删除段落
                + "delete from imageInfo where productId=@Id"//删除图片，为删除图片文件
                + "delete from contentMouldInfo where productId=@Id",//删除内容模板
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
