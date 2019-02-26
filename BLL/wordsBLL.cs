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
    public class wordsBLL
    {
        public DataTable GetWords(string sqlstr)
        {
            DataTable ds = SqlHelper.ExecuteDataSet("select * from wordsInfo " + sqlstr).Tables[0];
            return ds;
        }
        public void AddWords(wordsInfo words)
        {
            int a = SqlHelper.ExecuteNonQuery(@"INSERT INTO [AutouSend].[dbo].[wordsInfo]
           (words,editTime,wordType,userId,productId)
     VALUES
           (@words,getdate(),@wordType,@userId,@productId)",
               new SqlParameter("@words", SqlHelper.ToDBNull(words.words)),
               new SqlParameter("@wordType", SqlHelper.ToDBNull(words.wordType)),
               new SqlParameter("@userId", SqlHelper.ToDBNull(words.userId)),
               new SqlParameter("@productId", SqlHelper.ToDBNull(words.productId)));
        }
        public void UpdateWords(wordsInfo words)
        {
            int a = SqlHelper.ExecuteNonQuery(@"UPDATE [AutouSend].[dbo].[wordsInfo]
   SET [words] = @words,[editTime] = getdate() where Id=@Id",
               new SqlParameter("@Id", SqlHelper.ToDBNull(words.Id)),
               new SqlParameter("@words", SqlHelper.ToDBNull(words.words)));
        }
        public int DelWords(string Id)
        {
            return SqlHelper.ExecuteNonQuery("delete from wordsInfo where Id=@Id",
                new SqlParameter("@Id", SqlHelper.ToDBNull(Id)));
        }
    }
}
