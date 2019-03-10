using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class pubDetail
    {
        public int userId { get; set; }
        /// <summary>
        /// 今日可发条数，setting表
        /// </summary>
        public int todayCanPub { get; set; }
        public int titleCount { get; set; }
        /// <summary>
        /// 获取段落极差的数量
        /// </summary>
        public int paraCount { get; set; }
        public bool isPubing { get; set; }
    }
}
