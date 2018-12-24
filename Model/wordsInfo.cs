using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class wordsInfo
    {
        public int Id { get; set; }
        public string words { get; set; }
        public string editTime { get; set; }
        public string wordType { get; set; }
        public int userId { get; set; }
        public int productId { get; set; }
    }
}
