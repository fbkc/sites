using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class titleInfo
    {
        public long Id { get; set; }
        public string title { get; set; }
        public string addTime { get; set; }
        public string editTime { get; set; }
        public bool isSucceedPub { get; set; }
        public string returnMsg { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }
    }
}
