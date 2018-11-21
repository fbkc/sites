﻿using HRMSys.DAL;
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
        public DataTable GetColumnList(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from columnInfo " + sqlstr).Tables[0];
            return ds;
        }
        public void AddColumn(columnInfo column)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[columnInfo]
           ([columnId]
           ,[columnName])
     VALUES
           (@columnId
           ,@columnName)",
               new SqlParameter("@columnId", SqlHelper.ToDBNull(column.columnId)),
               new SqlParameter("@columnName", SqlHelper.ToDBNull(column.columnName)));
        }
        public void UpdateColumn(columnInfo column, string sqlstr)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[columnInfo]
   SET [columnId] = @columnId
      ,[columnName] = @columnName" + sqlstr,
      new SqlParameter("@Id", SqlHelper.ToDBNull(column.Id)),
       new SqlParameter("@columnId", SqlHelper.ToDBNull(column.columnId)),
               new SqlParameter("@columnName", SqlHelper.ToDBNull(column.columnName)));
        }
        public void DelColumn(string Id)
        {
            int a = SqlHelper.ExecuteNonQuery("delete * from columnInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}