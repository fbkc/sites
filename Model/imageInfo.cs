using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class imageInfo
    {
        public long Id { get; set; }
        public string imageId { get; set; }
        public string imageURL { get; set; }
        public string addTime { get; set; }
        public int userId { get; set; }
    }
}
