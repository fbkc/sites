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
        public List<cmUserInfo> GetUserList(string sqlstr)
        {
            List<cmUserInfo> uList = new List<cmUserInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from userInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                cmUserInfo userInfo = new cmUserInfo();
                userInfo.Id = (int)row["Id"];
                userInfo.username = (string)row["username"];
                userInfo.password = (string)row["password"];
                userInfo.userType = (int)row["userType"];
                userInfo.isStop = (bool)row["isStop"];
                userInfo.gradeId = (int)row["gradeId"];
                userInfo.canPubCount = (int)row["canPubCount"];
                userInfo.realmNameInfo = (string)row["realmNameInfo"];
                userInfo.expirationTime = ((DateTime)row["expirationTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                userInfo.endPubCount = (int)row["endPubCount"];
                userInfo.endTodayPubCount = (int)row["endTodayPubCount"];
                userInfo.registerTime = ((DateTime)row["registerTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                userInfo.registerIP = (string)row["registerIP"];
                userInfo.companyName = (string)row["companyName"];
                userInfo.columnInfoId = (int)row["columnInfoId"];
                userInfo.person = (string)row["person"];
                userInfo.telephone = (string)row["telephone"];
                userInfo.modile = (string)row["modile"];
                userInfo.ten_qq = (string)row["ten_qq"];
                userInfo.address = (string)row["address"];
                userInfo.com_web = (string)row["com_web"];
                userInfo.companyRemark = (string)row["companyRemark"];
                userInfo.yewu = (string)row["yewu"];
                userInfo.beforePubTime = ((DateTime)row["beforePubTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                uList.Add(userInfo);
            }
            return uList;
        }
        /// <summary>
        /// 查找单个用户
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public cmUserInfo GetUser(string sqlstr)
        {
            DataTable dt = SqlHelper.ExecuteDataSet("select * from userInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            DataRow row = dt.Rows[0];
            cmUserInfo userInfo = new cmUserInfo();
            userInfo.Id = (int)row["Id"];
            userInfo.username = (string)row["username"];
            userInfo.password = (string)row["password"];
            userInfo.userType = (int)row["userType"];
            userInfo.isStop = (bool)row["isStop"];
            userInfo.gradeId = (int)row["gradeId"];
            userInfo.canPubCount = (int)row["canPubCount"];
            userInfo.realmNameInfo = (string)row["realmNameInfo"];
            userInfo.expirationTime = ((DateTime)row["expirationTime"]).ToString("yyyy-MM-dd HH:mm:ss");
            userInfo.endPubCount = (int)row["endPubCount"];
            userInfo.endTodayPubCount = (int)row["endTodayPubCount"];
            userInfo.registerTime = ((DateTime)row["registerTime"]).ToString("yyyy-MM-dd HH:mm:ss");
            userInfo.registerIP = (string)row["registerIP"];
            userInfo.companyName = (string)row["companyName"];
            userInfo.columnInfoId = (int)row["columnInfoId"];
            userInfo.person = (string)row["person"];
            userInfo.telephone = (string)row["telephone"];
            userInfo.modile = (string)row["modile"];
            userInfo.ten_qq = (string)row["ten_qq"];
            userInfo.address = (string)row["address"];
            userInfo.com_web = (string)row["com_web"];
            userInfo.companyRemark = (string)row["companyRemark"];
            userInfo.yewu = (string)row["yewu"];
            userInfo.beforePubTime = ((DateTime)row["beforePubTime"]).ToString("yyyy-MM-dd HH:mm:ss");
            return userInfo;
        }
        /// <summary>
        /// 增加会员信息
        /// </summary>
        /// <param name="sqlstr"></param>
        public void AddUser(cmUserInfo cmUser)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[userInfo]
           (username,password,userType,isStop,gradeId,canPubCount,realmNameInfo,expirationTime,endPubCount,endTodayPubCount,registerTime,registerIP,companyName,columnInfoId,
person,telephone,modile,ten_qq,address,com_web,companyRemark,yewu,beforePubTime) VALUES
           (@username,@password,@userType,@isStop,@gradeId,@canPubCount,@realmNameInfo,@expirationTime,@endPubCount,@endTodayPubCount,getdate(),@registerIP,@companyName,
@columnInfoId,@person,@telephone,@modile,@ten_qq,@address,@com_web,@companyRemark,@yewu,getdate())",
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
               new SqlParameter("@registerIP", SqlHelper.ToDBNull(cmUser.registerIP)),
               new SqlParameter("@companyName", SqlHelper.ToDBNull(cmUser.companyName)),
               new SqlParameter("@columnInfoId", SqlHelper.ToDBNull(cmUser.columnInfoId)),
               new SqlParameter("@person", SqlHelper.ToDBNull(cmUser.person)),
               new SqlParameter("@telephone", SqlHelper.ToDBNull(cmUser.telephone)),
               new SqlParameter("@modile", SqlHelper.ToDBNull(cmUser.modile)),
               new SqlParameter("@ten_qq", SqlHelper.ToDBNull(cmUser.ten_qq)),
               new SqlParameter("@address", SqlHelper.ToDBNull(cmUser.address)),
               new SqlParameter("@com_web", SqlHelper.ToDBNull(cmUser.com_web)),
               new SqlParameter("@companyRemark", SqlHelper.ToDBNull(cmUser.companyRemark)),
               new SqlParameter("@yewu", SqlHelper.ToDBNull(cmUser.yewu)));
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
        public void UpdateUser(cmUserInfo cmUser)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[userInfo]
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
      ,[address] = @address
      ,[com_web] = @com_web
      ,[companyRemark] = @companyRemark
      ,[yewu] = @yewu
      ,[beforePubTime] = getdate() where Id=@Id",
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
      new SqlParameter("@address", SqlHelper.ToDBNull(cmUser.address)),
      new SqlParameter("@com_web", SqlHelper.ToDBNull(cmUser.com_web)),
      new SqlParameter("@companyRemark", SqlHelper.ToDBNull(cmUser.companyRemark)),
      new SqlParameter("@yewu", SqlHelper.ToDBNull(cmUser.yewu)));
        }
        /// <summary>
        /// 删除用户
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public int DelUser(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from userInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
