using Data;
using Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class cmUserBLL
    {
        /// <summary>
        /// 查找用户
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public DataTable GetUser(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from userInfo " + sqlstr).Tables[0];
            return ds;
        }
        /// <summary>
        /// 增加会员信息
        /// </summary>
        /// <param name="sqlstr"></param>
        public void AddUser(cmUserInfo cmUser)
        {
            int a = SqlHelper.ExecuteNoQuery(@"INSERT INTO [AutouSend].[dbo].[userInfo]
           ([username]
           ,[password]
           ,[accountGrade]
           ,[canPubCount]
           ,[realmNameInfo]
           ,[expirationTime]
           ,[endPubCount]
           ,[endTodayPubCount]
           ,[registerTime]
           ,[registerIP])
     VALUES
           (<username, nvarchar(50),>
           ,<password, nvarchar(50),>
           ,<accountGrade, int,>
           ,<canPubCount, int,>
           ,<realmNameInfo, nvarchar(50),>
           ,<expirationTime, nvarchar(50),>
           ,<endPubCount, int,>
           ,<endTodayPubCount, int,>
           ,<registerTime, nvarchar(50),>
           ,<registerIP, nvarchar(50),>)");
        }
        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="sqlstr"></param>
        public void UpdateUser(cmUserInfo cmUser,string sqlstr)
        {
            int a = SqlHelper.ExecuteNoQuery(@"UPDATE [AutouSend].[dbo].[userInfo]
   SET[username] = < username, nvarchar(50),>
      ,[password] = < password, nvarchar(50),>
      ,[accountGrade] = < accountGrade, int,>
      ,[canPubCount] = < canPubCount, int,>
      ,[realmNameInfo] = < realmNameInfo, nvarchar(50),>
      ,[expirationTime] = < expirationTime, nvarchar(50),>
      ,[endPubCount] = < endPubCount, int,>
      ,[endTodayPubCount] = < endTodayPubCount, int,>
      ,[registerTime] = < registerTime, nvarchar(50),>
      ,[registerIP] = < registerIP, nvarchar(50),>  " + sqlstr);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public void DelUser(string sqlstr)
        {
            int a = SqlHelper.ExecuteNoQuery("delete * from userInfo " + sqlstr);
        }
    }
}
