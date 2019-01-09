using HRMSys.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace AutoSend
{
    public class realmBLL
    {
        /// <summary>
        /// 获取域名
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public List<realmNameInfo> GetRealmList(string sqlstr)
        {
            List<realmNameInfo> rList = new List<realmNameInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from realmNameInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                realmNameInfo rInfo = new realmNameInfo();
                rInfo.Id = (int)row["Id"];
                rInfo.realmName = (string)row["realmName"];
                rInfo.realmAddress = (string)row["realmAddress"];
                rInfo.isUseing = (bool)row["isUseing"];
                rList.Add(rInfo);
            }
            return rList;
        }
        /// <summary>
        /// 获取单个域名
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public realmNameInfo GetRealm(string sqlstr)
        {
            DataTable dt = SqlHelper.ExecuteDataSet("select * from realmNameInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            DataRow row = dt.Rows[0];
            realmNameInfo rInfo = new realmNameInfo();
            rInfo.Id = (int)row["Id"];
            rInfo.realmName = (string)row["realmName"];
            rInfo.realmAddress = (string)row["realmAddress"];
            rInfo.isUseing = (bool)row["isUseing"];
            return rInfo;
        }
        /// <summary>
        /// 增加域名
        /// </summary>
        /// <param name="realm"></param>
        public void AddRealm(realmNameInfo realm)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[realmNameInfo]
           ([realmName]
           ,[realmAddress]
           ,[isUseing])
     VALUES
           (@realmName
           ,@realmAddress
           ,@isUseing)",
               new SqlParameter("@realmName", SqlHelper.ToDBNull(realm.realmName)),
               new SqlParameter("@realmAddress", SqlHelper.ToDBNull(realm.realmAddress)),
               new SqlParameter("@isUseing", SqlHelper.ToDBNull(realm.isUseing)));
        }
        /// <summary>
        /// 更新域名
        /// </summary>
        /// <param name="realm"></param>
        /// <param name="sqlstr"></param>
        public void UpdateRealm(realmNameInfo realm)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[realmNameInfo]
   SET [realmName] = @realmName
      ,[realmAddress] = @realmAddress
      ,[isUseing] = @isUseing where Id=@Id",
      new SqlParameter("@Id", SqlHelper.ToDBNull(realm.Id)),
       new SqlParameter("@realmName", SqlHelper.ToDBNull(realm.realmName)),
               new SqlParameter("@realmAddress", SqlHelper.ToDBNull(realm.realmAddress)),
               new SqlParameter("@isUseing", SqlHelper.ToDBNull(realm.isUseing)));
        }
        /// <summary>
        /// 删除域名
        /// </summary>
        /// <param name="sqlstr"></param>
        public int DelRealm(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from realmNameInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}