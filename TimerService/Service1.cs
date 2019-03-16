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
using System.Timers;

namespace TimerService
{
    public partial class Service1 : ServiceBase
    {
        Timer timer;//定时发布
        Timer timer1;//凌晨置零
        Timer timer2;//读标题
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer(300 * 1000);
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            timer.Start();
            WriteLog("timer服务启动");

            timer1 = new Timer(60 * 1000);
            timer1.Elapsed += new ElapsedEventHandler(Timer1_Elapsed);
            timer1.Start();
            WriteLog("timer1服务启动");

            timer2 = new Timer(60 * 1000);
            timer2.Elapsed += new ElapsedEventHandler(Timer2_Elapsed);
            timer2.Start();
            WriteLog("timer2服务启动");
        }
        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
            timer1.Stop();
            timer1.Dispose();
            WriteLog("服务停止");
        }

        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string html = NetHelper.HttpGet("http://39.105.196.3:1874/PublishHandler.ashx?action=roundsetting", "", Encoding.UTF8);
            if (html.Contains("成功"))
                WriteLog("timer服务执行了一次");
            else
                WriteLog("timer服务执行失败");
        }
        protected void Timer1_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (DateTime.Now.Hour == 23 && DateTime.Now.Minute == 59)
            {
                string html = NetHelper.HttpGet("http://39.105.196.3:1874/PublishHandler.ashx?action=uptodaycount", "", Encoding.UTF8);
                if (html.Contains("成功"))
                    WriteLog("timer1服务执行了一次");
                else
                    WriteLog("timer1服务执行失败");
            }
        }
        protected void Timer2_Elapsed(object sender, ElapsedEventArgs e)
        {
            string html = NetHelper.HttpGet("http://39.105.196.3:1874/PublishHandler.ashx?action=obtaintitle", "", Encoding.UTF8);
            if (html.Contains("成功"))
                WriteLog("timer2服务执行了一次");
            else
                WriteLog("timer2服务执行失败");
        }
        protected void WriteLog(string str)
        {
            string filePath = AppDomain.CurrentDomain.BaseDirectory + "Log.txt";
            StreamWriter sw = null;
            if (!File.Exists(filePath))
            {
                sw = File.CreateText(filePath);
            }
            else
            {
                sw = File.AppendText(filePath);
            }
            sw.Write(str + DateTime.Now.ToString() + Environment.NewLine);
            sw.Close();
        }
    }
}
