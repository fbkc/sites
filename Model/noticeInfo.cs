using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
   public class noticeInfo
    {
        public int Id { get; set; }
        public string notice { get; set; }
        public DateTime pubTime { get; set; }
        public bool isImprotant { get; set; }
        public bool issue { get; set; }
    }
}
