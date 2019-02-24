using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class cmUserInfo
    {
        public int Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int userType { get; set; }
        public bool isStop { get; set; }
        public int gradeId { get; set; }
        public int canPubCount { get; set; }
        public string realmNameInfo { get; set; }
        public string expirationTime { get; set; }
        public int endPubCount { get; set; }
        public int endTodayPubCount { get; set; }
        public string registerTime { get; set; }
        public string registerIP { get; set; }
        public string companyName { get; set; }
        public int columnInfoId { get; set; }
        public string person { get; set; }
        public string telephone { get; set; }
        public string modile { get; set; }
        public string ten_qq { get; set; }
        public string address { get; set; }
        public string com_web { get; set; }
        public string companyRemark { get; set; }
        public string yewu { get; set; }
        public string beforePubTime { get; set; }
    }
}
