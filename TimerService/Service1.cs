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
        Timer timer;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            timer = new Timer(120*1000);
            timer.Elapsed += new ElapsedEventHandler(Timer_Elapsed);
            timer.Start();
            WriteLog("服务启动");
        }
        protected override void OnStop()
        {
            timer.Stop();
            timer.Dispose();
            WriteLog("服务停止");
        }

        protected void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            string html = NetHelper.HttpGet("http://39.105.196.3:1874/PublishHandler.ashx?action=roundsetting", "", Encoding.UTF8);
            WriteLog("服务执行了一次");
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
