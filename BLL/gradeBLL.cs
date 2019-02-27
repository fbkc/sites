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
        public List<gradeInfo> GetGradeList(string sqlstr)
        {
            List<gradeInfo> gList = new List<gradeInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from gradeInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                gradeInfo gInfo = new gradeInfo();
                gInfo.Id = (int)SqlHelper.FromDBNull(row["Id"]);
                gInfo.gradeName = (string)SqlHelper.FromDBNull(row["gradeName"]);
                gInfo.pubCount = (int)SqlHelper.FromDBNull(row["pubCount"]);
                gList.Add(gInfo);
            }
            return gList;
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
        public void UpdateGrade(gradeInfo grade)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[gradeInfo]
   SET [gradeName] = @gradeName
      ,[pubCount] = @pubCount where Id=@Id",
      new SqlParameter("@Id", SqlHelper.ToDBNull(grade.Id)),
       new SqlParameter("@gradeName", SqlHelper.ToDBNull(grade.gradeName)),
               new SqlParameter("@pubCount", SqlHelper.ToDBNull(grade.pubCount)));
        }
        /// <summary>
        /// 删除会员级别
        /// </summary>
        /// <param name="Id"></param>
        public int DelGrade(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from gradeInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
