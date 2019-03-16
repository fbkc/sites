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
    public class titleBLL
    {
        /// <summary>
        /// 获取标题，分页
        /// </summary>
        /// <param name="userId">用户Id</param>
        /// <param name="pageIndex">页码</param>
        /// <param name="pageSize">每页条数</param>
        /// <returns></returns>
        public List<titleInfo> GetTitleList(string sqlstr, string orderby, int pageIndex, int pageSize)
        {
            List<titleInfo> tList = new List<titleInfo>();
            DataTable dt = SqlHelper.ExecuteDataTable(@"select * from 
                (select *, ROW_NUMBER() OVER(order by isSucceedPub,editTime " + orderby + ") AS RowId from titleInfo " + sqlstr + ") as b where b.RowId between @startNum and @endNum",
               new SqlParameter("@startNum", (pageIndex - 1) * pageSize + 1),
               new SqlParameter("@endNum", pageIndex * pageSize));
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                titleInfo tInfo = new titleInfo();
                tInfo.Id = (long)row["Id"];
                tInfo.title = (string)row["title"];
                tInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                tInfo.editTime = ((DateTime)row["editTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                tInfo.isSucceedPub = (bool)row["isSucceedPub"];
                tInfo.returnMsg = (string)row["returnMsg"];
                tInfo.productId = (int)row["productId"];
                tInfo.userId = (int)row["userId"];
                tList.Add(tInfo);
            }
            return tList;
        }
        public int GetPageTotal(string sqlstr)
        {
            return (int)SqlHelper.ExecuteScalar("select count(*)  from titleInfo " + sqlstr);
        }
        /// <summary>
        /// 获取标题，无分页,pub调用
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <param name="pageIndex"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public List<titleInfo> GetTitleList(string sqlstr)
        {
            List<titleInfo> tList = new List<titleInfo>();
            DataTable dt = SqlHelper.ExecuteDataTable(@"select * from titleInfo " + sqlstr);
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                titleInfo tInfo = new titleInfo();
                tInfo.Id = (long)row["Id"];
                tInfo.title = (string)row["title"];
                tInfo.addTime = ((DateTime)row["addTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                tInfo.editTime = ((DateTime)row["editTime"]).ToString("yyyy-MM-dd HH:mm:ss");
                tInfo.isSucceedPub = (bool)row["isSucceedPub"];
                tInfo.returnMsg = (string)row["returnMsg"];
                tInfo.productId = (int)row["productId"];
                tInfo.userId = (int)row["userId"];
                tList.Add(tInfo);
            }
            return tList;
        }

        public bool IsExitNoPubTitle(string sqlstr)
        {
            object obj= SqlHelper.ExecuteScalar("select isnull((select top(1) 1 from titleInfo " + sqlstr + "), 0) ");
            if (Convert.ToInt32(obj) == 1)
                return true;
            else
                return false;
        }

        public void AddTitle(titleInfo title)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[titleInfo]
           ([title]
           ,[addTime]
           ,editTime
           ,isSucceedPub
           ,returnMsg
           ,userId
           ,productId)
     VALUES
           (@title
           ,getdate()
           ,getdate()
           ,0
           ,@returnMsg
           ,@userId
           ,@productId)",
               new SqlParameter("@title", SqlHelper.ToDBNull(title.title)),
               new SqlParameter("@returnMsg", SqlHelper.ToDBNull(title.returnMsg)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(title.userId)),
               new SqlParameter("@productId", SqlHelper.ToDBNull(title.productId)));
        }
        public void UpdateTitle(titleInfo title)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[titleInfo]
   SET [title] = @title
      ,[editTime] = getdate()
      ,[isSucceedPub] = @isSucceedPub
      ,[returnMsg] = @returnMsg where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(title.Id)),
               new SqlParameter("@title", SqlHelper.ToDBNull(title.title)),
               new SqlParameter("@isSucceedPub", SqlHelper.ToDBNull(title.isSucceedPub)),
               new SqlParameter("@returnMsg", SqlHelper.ToDBNull(title.returnMsg)));
        }

        public int DelTitle(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from titleInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
        /// <summary>
        /// 清空已发布
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public int DelPubbedTitle(string userId)
        {
            return SqlHelper.ExecuteNonQuery("delete from titleInfo where userId=@userId and isSucceedPub=1",
                new SqlParameter("@userId", SqlHelper.ToDBNull(userId)));
        }
    }
}
