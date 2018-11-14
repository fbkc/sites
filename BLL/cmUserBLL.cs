using Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace BLL
{
    public class cmUserBLL
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
        /// 删除用户
        /// </summary>
        /// <param name="sqlstr"></param>
        /// <returns></returns>
        public void DelUser(string sqlstr)
        {
            int a = SqlHelper.ExecuteNoQuery("delete * from userInfo " + sqlstr);
        }
    }
}
