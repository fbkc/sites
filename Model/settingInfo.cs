using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class settingInfo
    {
        public int Id { get; set; }
        public int everydayCount { get; set; }
        public bool isAutoPub { get; set; }
        public int pubHour { get; set; }
        public int pubMin { get; set; }
        public bool isPubing { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
    }
}
