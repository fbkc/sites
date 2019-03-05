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
                userInfo.Id = (int) SqlHelper.FromDBNull( row["Id"]);
                userInfo.username = (string)SqlHelper.FromDBNull(row["username"]);
                userInfo.password = (string)SqlHelper.FromDBNull(row["password"]);
                userInfo.userType = (int)SqlHelper.FromDBNull(row["userType"]);
                userInfo.isStop = (bool)SqlHelper.FromDBNull(row["isStop"]);
                userInfo.gradeId = (int)SqlHelper.FromDBNull(row["gradeId"]);
                userInfo.canPubCount = (int)SqlHelper.FromDBNull(row["canPubCount"]);
                userInfo.realmNameInfo = (string)SqlHelper.FromDBNull(row["realmNameInfo"]);
                userInfo.expirationTime = ((DateTime)SqlHelper.FromDBNull(row["expirationTime"])).ToString("yyyy-MM-dd HH:mm:ss");
                userInfo.endPubCount = (int)SqlHelper.FromDBNull(row["endPubCount"]);
                userInfo.endTodayPubCount = (int)SqlHelper.FromDBNull(row["endTodayPubCount"]);
                userInfo.registerTime = ((DateTime)SqlHelper.FromDBNull(row["registerTime"])).ToString("yyyy-MM-dd HH:mm:ss");
                userInfo.registerIP = (string)SqlHelper.FromDBNull(row["registerIP"]);
                userInfo.companyName = (string)SqlHelper.FromDBNull(row["companyName"]);
                userInfo.columnInfoId = (int)SqlHelper.FromDBNull(row["columnInfoId"]);
                userInfo.person = (string)SqlHelper.FromDBNull(row["person"]);
                userInfo.telephone = (string)SqlHelper.FromDBNull(row["telephone"]);
                userInfo.modile = (string)SqlHelper.FromDBNull(row["modile"]);
                userInfo.ten_qq = (string)SqlHelper.FromDBNull(row["ten_qq"]);
                userInfo.address = (string)SqlHelper.FromDBNull(row["address"]);
                userInfo.com_web = (string)SqlHelper.FromDBNull(row["com_web"]);
                userInfo.companyRemark = (string)SqlHelper.FromDBNull(row["companyRemark"]);
                userInfo.yewu = (string)SqlHelper.FromDBNull(row["yewu"]);
                userInfo.beforePubTime = ((DateTime)SqlHelper.FromDBNull(row["beforePubTime"])).ToString("yyyy-MM-dd HH:mm:ss");
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
            userInfo.Id = (int)SqlHelper.FromDBNull(row["Id"]);
            userInfo.username = (string)SqlHelper.FromDBNull(row["username"]);
            userInfo.password = (string)SqlHelper.FromDBNull(row["password"]);
            userInfo.userType = (int)SqlHelper.FromDBNull(row["userType"]);
            userInfo.isStop = (bool)SqlHelper.FromDBNull(row["isStop"]);
            userInfo.gradeId = (int)SqlHelper.FromDBNull(row["gradeId"]);
            userInfo.canPubCount = (int)SqlHelper.FromDBNull(row["canPubCount"]);
            userInfo.realmNameInfo = (string)SqlHelper.FromDBNull(row["realmNameInfo"]);
            userInfo.expirationTime = ((DateTime)SqlHelper.FromDBNull(row["expirationTime"])).ToString("yyyy-MM-dd HH:mm:ss");
            userInfo.endPubCount = (int)SqlHelper.FromDBNull(row["endPubCount"]);
            userInfo.endTodayPubCount = (int)SqlHelper.FromDBNull(row["endTodayPubCount"]);
            userInfo.registerTime = ((DateTime)SqlHelper.FromDBNull(row["registerTime"])).ToString("yyyy-MM-dd HH:mm:ss");
            userInfo.registerIP = (string)SqlHelper.FromDBNull(row["registerIP"]);
            userInfo.companyName = (string)SqlHelper.FromDBNull(row["companyName"]);
            userInfo.columnInfoId = (int)SqlHelper.FromDBNull(row["columnInfoId"]);
            userInfo.person = (string)SqlHelper.FromDBNull(row["person"]);
            userInfo.telephone = (string)SqlHelper.FromDBNull(row["telephone"]);
            userInfo.modile = (string)SqlHelper.FromDBNull(row["modile"]);
            userInfo.ten_qq = (string)SqlHelper.FromDBNull(row["ten_qq"]);
            userInfo.address = (string)SqlHelper.FromDBNull(row["address"]);
            userInfo.com_web = (string)SqlHelper.FromDBNull(row["com_web"]);
            userInfo.companyRemark = (string)SqlHelper.FromDBNull(row["companyRemark"]);
            userInfo.yewu = (string)SqlHelper.FromDBNull(row["yewu"]);
            userInfo.beforePubTime = ((DateTime)SqlHelper.FromDBNull(row["beforePubTime"])).ToString("yyyy-MM-dd HH:mm:ss");
            return userInfo;
        }
        /// <summary>
        /// 获取单个用户，首页用
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public cmUserInfo GetUserInfo(string sqlstr)
        {
            DataTable dt = SqlHelper.ExecuteDataSet("select *,(select gradeName from gradeInfo where Id=u.userType) as gName from userInfo u  " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            DataRow row = dt.Rows[0];
            cmUserInfo userInfo = new cmUserInfo();
            userInfo.Id = (int)SqlHelper.FromDBNull(row["Id"]);
            userInfo.username = (string)SqlHelper.FromDBNull(row["username"]);
            userInfo.password = (string)SqlHelper.FromDBNull(row["password"]);
            userInfo.userType = (int)SqlHelper.FromDBNull(row["userType"]);
            userInfo.isStop = (bool)SqlHelper.FromDBNull(row["isStop"]);
            userInfo.gradeId = (int)SqlHelper.FromDBNull(row["gradeId"]);
            userInfo.canPubCount = (int)SqlHelper.FromDBNull(row["canPubCount"]);
            userInfo.realmNameInfo = (string)SqlHelper.FromDBNull(row["gName"]);//借用，存会员级别
            userInfo.expirationTime = ((DateTime)SqlHelper.FromDBNull(row["expirationTime"])).ToString("yyyy-MM-dd HH:mm:ss");
            userInfo.endPubCount = (int)SqlHelper.FromDBNull(row["endPubCount"]);
            userInfo.endTodayPubCount = (int)SqlHelper.FromDBNull(row["endTodayPubCount"]);
            userInfo.registerTime = ((DateTime)SqlHelper.FromDBNull(row["registerTime"])).ToString("yyyy-MM-dd HH:mm:ss");
            userInfo.registerIP = (string)SqlHelper.FromDBNull(row["registerIP"]);
            userInfo.companyName = (string)SqlHelper.FromDBNull(row["companyName"]);
            userInfo.columnInfoId = (int)SqlHelper.FromDBNull(row["columnInfoId"]);
            userInfo.person = (string)SqlHelper.FromDBNull(row["person"]);
            userInfo.telephone = (string)SqlHelper.FromDBNull(row["telephone"]);
            userInfo.modile = (string)SqlHelper.FromDBNull(row["modile"]);
            userInfo.ten_qq = (string)SqlHelper.FromDBNull(row["ten_qq"]);
            userInfo.address = (string)SqlHelper.FromDBNull(row["address"]);
            userInfo.com_web = (string)SqlHelper.FromDBNull(row["com_web"]);
            userInfo.companyRemark = (string)SqlHelper.FromDBNull(row["companyRemark"]);
            userInfo.yewu = (string)SqlHelper.FromDBNull(row["yewu"]);
            userInfo.beforePubTime = ((DateTime)SqlHelper.FromDBNull(row["beforePubTime"])).ToString("yyyy-MM-dd HH:mm:ss");
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
   SET [username] = @username
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
      ,[yewu] = @yewu where Id=@Id",
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
        /// 更新会员 总的已发条数，今日已发条数，上一条发布时间
        /// </summary>
        /// <param name="sqlstr"></param>
        public void UpUserPubInformation(int Id)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[userInfo]
   SET [endPubCount] = endPubCount+1
      ,[endTodayPubCount] = endTodayPubCount+1
      ,[beforePubTime] = getdate() where Id=@Id",
      new SqlParameter("@Id",SqlHelper.ToDBNull(Id)));
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
