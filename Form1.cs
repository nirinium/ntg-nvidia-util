using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Management;
using System.IO;
using HtmlAgilityPack;
using System.Net;

namespace NTG_NVIDIA_UTIL
{
    public partial class Form1 : Form
    {
    public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            //Log start time
            ntgLog1.AppendText(DateTime.Now.ToString("yyyyMMdd_hhmmss") + "\r\n");
            //Poll hardware
            Hardware_Poll();
        }

        private void Hardware_Poll()
        {
            ntgLog1.ReadOnly = true;

            ManagementObjectSearcher myVideoObject = new ManagementObjectSearcher("select * from Win32_VideoController");

            foreach (ManagementObject obj in myVideoObject.Get())
            {
                //Shitty way of doing this im aware.
                ntgLog1.AppendText("Graphics Card  -  " + obj["Name"] + " Status  -  " + obj["Status"] + (Environment.NewLine));
                //ntgLog1.AppendText("Name  -  " + obj["Name"] + (Environment.NewLine));
                //ntgLog1.AppendText("Status  -  " + obj["Status"] + (Environment.NewLine));
                //ntgLog1.AppendText("Caption  -  " + obj["Caption"] + (Environment.NewLine));
                ntgLog1.AppendText("DeviceID  -  " + obj["DeviceID"] + (Environment.NewLine));
                ntgLog1.AppendText("AdapterRAM  -  " + obj["AdapterRAM"] + (Environment.NewLine));
                ntgLog1.AppendText("AdapterDACType  -  " + obj["AdapterDACType"] + (Environment.NewLine));
                ntgLog1.AppendText("Monochrome  -  " + obj["Monochrome"] + (Environment.NewLine));
                ntgLog1.AppendText("DriverVersion  -  " + obj["DriverVersion"] + (Environment.NewLine));
                ntgLog1.AppendText("VideoProcessor  -  " + obj["VideoProcessor"] + (Environment.NewLine));
                ntgLog1.AppendText("VideoArchitecture  -  " + obj["VideoArchitecture"] + (Environment.NewLine));
                ntgLog1.AppendText("VideoMemoryType  -  " + obj["VideoMemoryType"] + "\n\n" );
                ntgLog1.AppendText("InstalledDisplayDrivers  -  " + obj["InstalledDisplayDrivers"] + "\n");
                //*[@id="driverList"]/td[2]/b/a
            }


            toolStripStatusLabel2.Text = "Idle";
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ntgLog1.SaveFile("logfile" + DateTime.Now.ToString("yyyyMMdd_hhmmss") + ".txt", RichTextBoxStreamType.PlainText);
            toolStripStatusLabel2.Text = "Saved log!";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            ntgLog1.SelectAll();
            ntgLog1.Copy();
            toolStripStatusLabel2.Text = "Copied Log to Clipboard!";
        }

        private void button3_Click(object sender, EventArgs e)
        {
            ntgLog1.Clear();
            Hardware_Poll();
            toolStripStatusLabel2.Text = "Manually scanned video properties";
        }

        private void button4_Click(object sender, EventArgs e)
        {
            //Clear Log
            ntgLog1.Clear();

            string nvURL = "https://www.nvidia.com/Download/processFind.aspx?psid=101&pfid=816&osid=57&lid=1&whql=1&lang=en-us&ctk=0";
            HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
            HtmlAgilityPack.HtmlDocument doc = web.Load(nvURL);

            var nvDrivers = doc.DocumentNode.SelectNodes("//*[@id='driverList']/td[3]").ToList();

            ntgLog1.AppendText("Driver Version History: \n");

            foreach (var driver in nvDrivers)
            {
                ntgLog1.AppendText(driver.InnerText + "\n");
                //*[@id='driverList']/td[3] WORKS
                //*[@id='driverList']/td[2]/b/a
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            string url = "https://www.wagnardsoft.com/DDU/download/DDU%20v18.0.0.6.exe";
                using (WebClient wc = new WebClient())
            {
                wc.DownloadProgressChanged += wc_DownloadProgressChanged;
                wc.DownloadFileAsync(new System.Uri(url), "ddu.exe");                                
            }

            
        }
        // Event to track the progress
        void wc_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            progressBar.Value = e.ProgressPercentage;

            //if (File.Exists("ddu.exe"))
            if (progressBar.Value >= 100) //has to be greater than or equal to cant just be equal.. maybe ==? 
            {
                toolStripStatusLabel2.Text = "Download complete!";
            }
        }
    
    }
}
