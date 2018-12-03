using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class paragraphInfo
    {
        public long Id { get; set; }
        public string paraId { get; set; }
        public string paraCotent { get; set; }
        public int usedCount { get; set; }
        public DateTime addTime { get; set; }
        public int userId { get; set; }
    }
}
