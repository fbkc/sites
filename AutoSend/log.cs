using BLL;
using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AutoSend
{
    public static class log
    {
        public static void wlog(string errorMsg,string userId,string username)
        {
            logBLL lbll = new logBLL();
            logInfo lInfo = new logInfo();
            lInfo.ErroMsg = errorMsg;
            lInfo.userId = userId;
            lInfo.username = username;
            lbll.AddLog(lInfo);
        }
    }
}