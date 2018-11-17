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
    public class CmUserBLL
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
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[userInfo]
           (username,password,userType,accountGrade,canPubCount,realmNameInfo,expirationTime,endPubCount,endTodayPubCount,registerTime,registerIP) VALUES
           (@username,@password,@userType,@gradeId,@canPubCount,@realmNameInfo,@expirationTime,@endPubCount,@endTodayPubCount,@registerTime,@registerIP)",
           new SqlParameter("@username", SqlHelper.ToDBNull(cmUser.username)),
           new SqlParameter("@password", SqlHelper.ToDBNull(cmUser.password)),
           new SqlParameter("@userType", SqlHelper.ToDBNull(cmUser.userType)),
           new SqlParameter("@gradeId", SqlHelper.ToDBNull(cmUser.gradeId)),
           new SqlParameter("@canPubCount", SqlHelper.ToDBNull(cmUser.canPubCount)),
           new SqlParameter("@realmNameInfo", SqlHelper.ToDBNull(cmUser.realmNameInfo)),
           new SqlParameter("@expirationTime", SqlHelper.ToDBNull(cmUser.expirationTime)),
           new SqlParameter("@endPubCount", SqlHelper.ToDBNull(cmUser.endPubCount)),
           new SqlParameter("@endTodayPubCount", SqlHelper.ToDBNull(cmUser.endTodayPubCount)),
           new SqlParameter("@registerTime", SqlHelper.ToDBNull(cmUser.registerTime)),
           new SqlParameter("@registerIP", SqlHelper.ToDBNull(cmUser.registerIP)));
        }
        /// <summary>
        /// 更新会员信息
        /// </summary>
        /// <param name="sqlstr"></param>
        public void UpdateUser(cmUserInfo cmUser, string sqlstr)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[userInfo]
   SET[username] = @username
      ,[password] = @password
,[userType] = @userType
      ,[gradeId] = @gradeId
      ,[canPubCount] = @canPubCount
      ,[realmNameInfo] = @realmNameInfo
      ,[expirationTime] = @expirationTime
      ,[endPubCount] = @endPubCount
      ,[endTodayPubCount] = @endTodayPubCount
      ,[registerTime] = @registerTime
      ,[registerIP] = @registerIP" + sqlstr);
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public void DelUser(string sqlstr)
        {
            int a = SqlHelper.ExecuteNonQuery("delete * from userInfo " + sqlstr);
        }
    }
}
