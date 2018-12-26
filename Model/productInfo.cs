using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class productInfo
    {
        public int Id { get; set; }
        public string productName { get; set; }
        public int userId { get; set; }
        public string pinpai { get; set; }
        public string xinghao { get; set; }
        public string price { get; set; }
        public string smallCount { get; set; }
        public string sumCount { get; set; }
        public string unit { get; set; }
        public string city { get; set; }
        public string createTime { get; set; }
        public string editTime { get; set; }
        public string informationType { get; set; }//PRODUCT/NEWS
        public int? maxPubCount { get; set; }
        public int? endPubCount { get; set; }
        public int? endTodayPubCount { get; set; }
        public string pub_startTime { get; set; }
        public int? pubInterval { get; set; }
        public bool isPub { get; set; }
    }
}
