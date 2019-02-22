using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class contentMouldInfo
    {
        public int Id { get; set; }
        public string mouldName { get; set; }
        public string contentMould { get; set; }
        public string type { get; set; }
        public int usedCount { get; set; }
        public string addTime { get; set; }
        public string editTime { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }
        public string productName { get; set; }
    }
}
