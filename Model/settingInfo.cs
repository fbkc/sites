using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Model
{
    public class settingInfo
    {
        public int Id { get; set; }
        /// <summary>
        /// 每日条数
        /// </summary>
        public int everydayCount { get; set; }
        /// <summary>
        /// 是否定时发布
        /// </summary>
        public bool isAutoPub { get; set; }
        /// <summary>
        /// 小时
        /// </summary>
        public int pubHour { get; set; }
        /// <summary>
        /// 分钟
        /// </summary>
        public int pubMin { get; set; }
        /// <summary>
        /// 是否正在发布
        /// </summary>
        public bool isPubing { get; set; }
        public int userId { get; set; }
        public string username { get; set; }
    }
}
