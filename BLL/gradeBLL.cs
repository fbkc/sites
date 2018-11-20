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
    public class gradeBLL
    {
        /// <summary>
        /// 获取所有会员级别
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public DataTable GetGradeList(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from gradeInfo " + sqlstr).Tables[0];
            return ds;
        }
        /// <summary>
        /// 增加会员级别
        /// </summary>
        /// <param name="grade"></param>
        public void AddGrade(gradeInfo grade)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[gradeInfo]
           ([gradeName]
           ,[pubCount])
     VALUES
           (@gradeName
           ,@pubCount)",
               new SqlParameter("@gradeName", SqlHelper.ToDBNull(grade.gradeName)),
               new SqlParameter("@pubCount", SqlHelper.ToDBNull(grade.pubCount)));
        }
        /// <summary>
        /// 更新
        /// </summary>
        /// <param name="grade"></param>
        /// <param name="sqlstr"></param>
        public void UpdateGrade(gradeInfo grade, string sqlstr)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[gradeInfo]
   SET [gradeName] = @gradeName
      ,[pubCount] = @pubCount" + sqlstr,
      new SqlParameter("@Id", SqlHelper.ToDBNull(grade.Id)),
       new SqlParameter("@gradeName", SqlHelper.ToDBNull(grade.gradeName)),
               new SqlParameter("@pubCount", SqlHelper.ToDBNull(grade.pubCount)));
        }
        /// <summary>
        /// 删除会员级别
        /// </summary>
        /// <param name="Id"></param>
        public void DelGrade(string Id)
        {
            int a = SqlHelper.ExecuteNonQuery("delete * from gradeInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
