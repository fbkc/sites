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
        /// 增加会员信息
        /// </summary>
        /// <param name="sqlstr"></param>
        public void AddProduct(productInfo product)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[productInfo]
           (productName,userId,pinpai,xinghao,price,smallCount,sumCount,unit,city,editTime,informationType,maxPubCount,endPubCount,endTodayPubCount,isPub) VALUES
           (@productName,@userId,@pinpai,@xinghao,@price,@smallCount,@sumCount,@unit,@city,getdate(),@informationType,@maxPubCount,@endPubCount,@endTodayPubCount,@isPub)",
               new SqlParameter("@productName", SqlHelper.ToDBNull(product.username)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(product.password)),
               new SqlParameter("@userType", SqlHelper.ToDBNull(product.userType)),
               new SqlParameter("@isStop", SqlHelper.ToDBNull(product.isStop)),
               new SqlParameter("@gradeId", SqlHelper.ToDBNull(product.gradeId)),
               new SqlParameter("@canPubCount", SqlHelper.ToDBNull(cmUser.canPubCount)),
               new SqlParameter("@realmNameInfo", SqlHelper.ToDBNull(cmUser.realmNameInfo)),
               new SqlParameter("@expirationTime", SqlHelper.ToDBNull(cmUser.expirationTime)),
               new SqlParameter("@endPubCount", SqlHelper.ToDBNull(cmUser.endPubCount)),
               new SqlParameter("@endTodayPubCount", SqlHelper.ToDBNull(cmUser.endTodayPubCount)),
               new SqlParameter("@registerTime", SqlHelper.ToDBNull(cmUser.registerTime)),
               new SqlParameter("@registerIP", SqlHelper.ToDBNull(cmUser.registerIP)),
               new SqlParameter("@companyName", SqlHelper.ToDBNull(cmUser.companyName)),
               new SqlParameter("@columnInfoId", SqlHelper.ToDBNull(cmUser.columnInfoId)),
               new SqlParameter("@person", SqlHelper.ToDBNull(cmUser.person)),
               new SqlParameter("@telephone", SqlHelper.ToDBNull(cmUser.telephone)),
               new SqlParameter("@modile", SqlHelper.ToDBNull(cmUser.modile)),
               new SqlParameter("@ten_qq", SqlHelper.ToDBNull(cmUser.ten_qq)),
               new SqlParameter("@keyword", SqlHelper.ToDBNull(cmUser.keyword)),
               new SqlParameter("@pinpai", SqlHelper.ToDBNull(cmUser.pinpai)),
               new SqlParameter("@xinghao", SqlHelper.ToDBNull(cmUser.xinghao)),
               new SqlParameter("@price", SqlHelper.ToDBNull(cmUser.price)),
               new SqlParameter("@smallCount", SqlHelper.ToDBNull(cmUser.smallCount)),
               new SqlParameter("@sumCount", SqlHelper.ToDBNull(cmUser.sumCount)),
               new SqlParameter("@unit", SqlHelper.ToDBNull(cmUser.unit)),
               new SqlParameter("@city", SqlHelper.ToDBNull(cmUser.city)),
               new SqlParameter("@address", SqlHelper.ToDBNull(cmUser.address)),
               new SqlParameter("@com_web", SqlHelper.ToDBNull(cmUser.com_web)),
               new SqlParameter("@companyRemark", SqlHelper.ToDBNull(cmUser.companyRemark)),
               new SqlParameter("@yewu", SqlHelper.ToDBNull(cmUser.yewu)),
               new SqlParameter("@ziduan1", SqlHelper.ToDBNull(cmUser.ziduan1))
               );
        }
        public bool IsExistUser(string name)
        {
            object o = SqlHelper.ExecuteScalar("select count(*) from userInfo where username=@username",
                new SqlParameter("@username", name));
            return Convert.ToBoolean(o);
        }

        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="sqlstr"></param>
        public void UpdateProduct(productInfo cmUser)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[productInfo]
   SET[username] = @username
      ,[password] = @password
      ,[userType] = @userType
      ,[isStop]=@isStop
      ,[gradeId] = @gradeId
      ,[canPubCount] = @canPubCount
      ,[realmNameInfo] = @realmNameInfo
      ,[expirationTime] = @expirationTime
      ,[endPubCount] = @endPubCount
      ,[endTodayPubCount] = @endTodayPubCount
      ,[registerTime] = @registerTime
      ,[registerIP] = @registerIP 
      ,[companyName] = @companyName
      ,[columnInfoId] = @columnInfoId
      ,[person] = @person
      ,[telephone] = @telephone
      ,[modile] = @modile
      ,[ten_qq] = @ten_qq
      ,[keyword] = @keyword
      ,[pinpai] = @pinpai
      ,[xinghao] = @xinghao
      ,[price] = @price
      ,[smallCount] = @smallCount
      ,[sumCount] = @sumCount
      ,[unit] = @unit
      ,[city] = @city
      ,[address] = @address
      ,[com_web] = @com_web
      ,[companyRemark] = @companyRemark
      ,[yewu] = @yewu
      ,[ziduan1] = @ziduan1 where Id=@Id",
      new SqlParameter("@Id", SqlHelper.ToDBNull(cmUser.Id)),
      new SqlParameter("@username", SqlHelper.ToDBNull(cmUser.username)),
      new SqlParameter("@password", SqlHelper.ToDBNull(cmUser.password)),
      new SqlParameter("@userType", SqlHelper.ToDBNull(cmUser.userType)),
      new SqlParameter("@isStop", SqlHelper.ToDBNull(cmUser.isStop)),
      new SqlParameter("@gradeId", SqlHelper.ToDBNull(cmUser.gradeId)),
      new SqlParameter("@canPubCount", SqlHelper.ToDBNull(cmUser.canPubCount)),
      new SqlParameter("@realmNameInfo", SqlHelper.ToDBNull(cmUser.realmNameInfo)),
      new SqlParameter("@expirationTime", SqlHelper.ToDBNull(cmUser.expirationTime)),
      new SqlParameter("@endPubCount", SqlHelper.ToDBNull(cmUser.endPubCount)),
      new SqlParameter("@endTodayPubCount", SqlHelper.ToDBNull(cmUser.endTodayPubCount)),
      new SqlParameter("@registerTime", SqlHelper.ToDBNull(cmUser.registerTime)),
      new SqlParameter("@registerIP", SqlHelper.ToDBNull(cmUser.registerIP)),
      new SqlParameter("@companyName", SqlHelper.ToDBNull(cmUser.companyName)),
      new SqlParameter("@columnInfoId", SqlHelper.ToDBNull(cmUser.columnInfoId)),
      new SqlParameter("@person", SqlHelper.ToDBNull(cmUser.person)),
      new SqlParameter("@telephone", SqlHelper.ToDBNull(cmUser.telephone)),
      new SqlParameter("@modile", SqlHelper.ToDBNull(cmUser.modile)),
      new SqlParameter("@ten_qq", SqlHelper.ToDBNull(cmUser.ten_qq)),
      new SqlParameter("@keyword", SqlHelper.ToDBNull(cmUser.keyword)),
      new SqlParameter("@pinpai", SqlHelper.ToDBNull(cmUser.pinpai)),
      new SqlParameter("@xinghao", SqlHelper.ToDBNull(cmUser.xinghao)),
      new SqlParameter("@price", SqlHelper.ToDBNull(cmUser.price)),
      new SqlParameter("@smallCount", SqlHelper.ToDBNull(cmUser.smallCount)),
      new SqlParameter("@unit", SqlHelper.ToDBNull(cmUser.unit)),
      new SqlParameter("@city", SqlHelper.ToDBNull(cmUser.city)),
      new SqlParameter("@address", SqlHelper.ToDBNull(cmUser.address)),
      new SqlParameter("@com_web", SqlHelper.ToDBNull(cmUser.com_web)),
      new SqlParameter("@companyRemark", SqlHelper.ToDBNull(cmUser.companyRemark)),
      new SqlParameter("@yewu", SqlHelper.ToDBNull(cmUser.yewu)),
      new SqlParameter("@ziduan1", SqlHelper.ToDBNull(cmUser.ziduan1)));
        }
        /// <summary>
        /// 删除用户
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
