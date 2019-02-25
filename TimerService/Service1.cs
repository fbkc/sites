using AutoSend;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Text;

namespace TimerService
{
    public partial class Service1 : ServiceBase
    {
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {

            timer1.Interval = 60 * 1000;  //设置计时器事件间隔执行时间

            ////timer1.Elapsed += new System.Timers.ElapsedEventHandler(timer1_Elapsed);

            timer1.Enabled = true;

            //if (!EventLog.SourceExists("OnStart222"))
            //{
            //    EventLog.CreateEventSource("OnStart222", "jason");
            //}

            //EventLog.WriteEntry("OnStart222", "开始任务了");

            string start = string.Format("{0}-{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), "程序启动了。");
            Log(start);
        }

        protected override void OnStop()
        {
            this.timer1.Enabled = false;
            //EventLog.WriteEntry("OnStart222", "任务结束");
            string start = string.Format("{0}-{1}", DateTime.Now.ToString("yyyyMMddHHmmss"), "程序停止了。");
            Log(start);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            string html = NetHelper.HttpGet("http://vip.100dh.cn/PublishHandler.ashx?action=roundsetting", "", Encoding.UTF8);
        }
        void Log(string str)
        {
            string path = "C://6.txt";
            using (StreamWriter sw = File.AppendText(path))
            {
                sw.WriteLine(str);
            }
        }
    }
}
