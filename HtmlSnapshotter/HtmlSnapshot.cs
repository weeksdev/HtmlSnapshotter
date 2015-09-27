using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WatiN.Core;

namespace HtmlSnapshotter
{
    class HtmlSnapshot
    {
        private Browser browser;

        public Browser Browser
        {
            get { return browser; }
            set { browser = value; }
        }

        private bool recursive = true;

        public bool Recursive
        {
            get { return recursive; }
            set { recursive = value; }
        }

        private int waitTimeOnLoad;

        public int WaitTimeOnLoad
        {
            get { return waitTimeOnLoad; }
            set { waitTimeOnLoad = value; }
        }
        private List<string> visitedPages;

        public List<string> VisitedPages
        {
            get { return visitedPages; }
            set { visitedPages = value; }
        }
        
        private string startingAddress;

        public string StartingAddress
        {
            get { return startingAddress; }
            set { startingAddress = value; }
        }
        private string savePath;

        public string SavePath
        {
            get { return savePath; }
            set { savePath = value; }
        }
        public HtmlSnapshot(string startingAddress, string savePath, int waitTimeOnLoad)
        {
            this.StartingAddress = startingAddress;
            if (!savePath.EndsWith("\\"))
            {
                savePath = savePath + "\\";
            }
            this.SavePath = savePath;
            this.WaitTimeOnLoad = waitTimeOnLoad;
            this.VisitedPages = new List<string>();
            this.Browser = new IE(this.StartingAddress);
            System.Threading.Thread.Sleep(this.WaitTimeOnLoad);
            this.VisitedPages.Add(this.StartingAddress);
            this.SaveSnapshot(this.startingAddress);
        }
        public void Shoot()
        {
            //obtain #! links from page
            this.Browser.DomContainer.Links.ToList().ForEach(a =>
            {
                if (a.Url.ToLower().StartsWith(this.StartingAddress.ToLower() + "/#!") && !this.VisitedPages.Contains(a.Url))
                {
                    LoadUrl(a.Url);
                    SaveSnapshot(a.Url);
                }
                else
                {
                    Console.WriteLine("Skipping Url: {0}", a.Url);
                }
            });
        }
        public void Close()
        {
            this.Browser.Close();
            this.Browser = null;
        }
        public void LoadUrl(string url)
        {
            Console.WriteLine("Loading: {0}", url);
            this.Browser.GoTo(url);
            System.Threading.Thread.Sleep(this.WaitTimeOnLoad);
            this.VisitedPages.Add(url);
            if (this.Recursive)
            {
                this.Shoot();
            }
        }
        public void SaveSnapshot(string url)
        {
            string tempPath = this.SavePath + url.Replace(this.StartingAddress,"").Replace("/#!","").Replace("/","\\") + ".html";
            Console.WriteLine("Saving Snapshot: {0}", tempPath);
            System.IO.File.WriteAllText(tempPath, this.Browser.Html.ToString());
        }
    }
}
