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
    public class columnBLL
    {
        public List<columnInfo> GetColumnList(string sqlstr)
        {
            List<columnInfo> cList = new List<columnInfo>();
            DataTable dt = SqlHelper.ExecuteDataSet("select * from columnInfo " + sqlstr).Tables[0];
            if (dt.Rows.Count < 1)
                return null;
            foreach (DataRow row in dt.Rows)
            {
                columnInfo cInfo = new columnInfo();
                cInfo.Id = (int)SqlHelper.FromDBNull(row["Id"]);
                cInfo.columnName = (string)SqlHelper.FromDBNull(row["columnName"]);
                cList.Add(cInfo);
            }
            return cList;
        }
        public void AddColumn(columnInfo column)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[columnInfo]
           ([columnName])
     VALUES
           (@columnName)",
               new SqlParameter("@columnName", SqlHelper.ToDBNull(column.columnName)));
        }
        public void UpdateColumn(columnInfo column)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[columnInfo]
   SET [columnName] = @columnName where Id=@Id",
      new SqlParameter("@Id", SqlHelper.ToDBNull(column.Id)),
               new SqlParameter("@columnName", SqlHelper.ToDBNull(column.columnName)));
        }
        public int DelColumn(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from columnInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
