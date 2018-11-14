using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class cmUserInfo
    {
        public string Id { get; set; }
        public string username { get; set; }
        public string password { get; set; }
        public int accountGrade { get; set; }
        public int canPubCount { get; set; }
        public string realmNameInfo { get; set; }
        public string expirationTime { get; set; }
        public int endPubCount { get; set; }
        public int endTodayPubCount { get; set; }
        public string registerTime { get; set; }
        public string registerIP { get; set; }
    }
}
