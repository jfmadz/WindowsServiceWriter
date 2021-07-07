using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;

namespace WindowsServiceWriter
{
    public partial class Service1 : ServiceBase
    {
        private Timer timer = null;
        public Service1()
        {
            InitializeComponent();
        }

        protected override void OnStart(string[] args)
        {
            Log.writeEventLog("Timer is set for 5 seconds");
            timer = new Timer();
            this.timer.Interval = 5000; // execute every 5 seconds
            this.timer.Elapsed += new ElapsedEventHandler(this.timer_tick);
            this.timer.Enabled = true;
            Log.writeEventLog("Service Started");
            Log.writeEventLog("*******");
            Log.writeEventLog("****Timer is set to true for next cycle");
            Log.writeEventLog("*******");
        }

        private void timer_tick(object sender, ElapsedEventArgs e)
        {
            Log.writeEventLog("Timer and operations are running....");

            Log.writeEventLog("Operation Completed Successfully");


            string url = "https://en.wikipedia.org/wiki/List_of_programmers";
            var response = CallUrl(url).Result;
            var linkList = ParseHtml(response);
            WriteToTxt(linkList);

        }
        private static async Task<string> CallUrl(string fullUrl)//making the request
        {
            HttpClient client = new HttpClient();
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls;
            client.DefaultRequestHeaders.Accept.Clear();
            var response = client.GetStringAsync(fullUrl);
            return await response;
        }

        private List<string> ParseHtml(string html)// restricting the results
        {
            HtmlDocument htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);
            var programmerLinks = htmlDoc.DocumentNode.Descendants("li")
                    .Where(node => !node.GetAttributeValue("class", "").Contains("tocsection")).ToList();

            List<string> wikiLink = new List<string>();

            foreach (var link in programmerLinks)
            {
                if (link.FirstChild.Attributes.Count > 0)
                    wikiLink.Add("https://en.wikipedia.org/" + link.FirstChild.Attributes[0].Value);
            }

            return wikiLink;

        }

        private void WriteToTxt(List<string> links)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var link in links)
            {
                sb.AppendLine(link);
            }

            System.IO.File.WriteAllText("links.txt", sb.ToString()); //chaange file extension to csv if needeed
        }

        protected override void OnStop()
        {
            Log.writeEventLog("****--Event: OnStop--****");
            Log.writeEventLog("Attempt to Shut Down the Service");
            timer.Stop();
            timer = null;
            Log.writeEventLog("Service Shut Down by User");

        }
    }
}
