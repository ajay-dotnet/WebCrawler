using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text.RegularExpressions;

namespace WebCrawler
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Enter the uri to download all files from:");
            var remoteUri = Console.ReadLine(); // @"https://archive.ics.uci.edu/ml/machine-learning-databases/iris/";
            var htmlText = GetWebText(remoteUri);
            List<string> fileNames = ExtractFiles(htmlText);
            DownloadFiles(fileNames, remoteUri);
            Console.WriteLine("Download Done!! Press any key to exit.");
            Console.ReadKey();
        }

        private static List<string> ExtractFiles(string htmlText)
        {
            var urls = new List<string>();
            var regex = "<a href=\"([a-zA-Z0-9.]+)\"";
            MatchCollection mc = Regex.Matches(htmlText, regex);
            foreach (Match item in mc)
            {
                urls.Add(item.Groups[1].Value);
            }

            return urls;
        }

        public static void DownloadFiles(List<string> fileNames, string remoteUri)
        {
            foreach (var file in fileNames)
            {
                DownloadFile(remoteUri + file, file);
            }
        }

        public static void DownloadFile(string url, string filename)
        {
            using (var client = new WebClient())
            {
                client.DownloadFile(url, filename);
            }
        }

        private static string GetWebText(string url)
        {
            ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
            request.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/535.2 (KHTML, like Gecko) Chrome/15.0.874.121 Safari/535.2";
            WebResponse response = request.GetResponse();
            Stream stream = response.GetResponseStream();
            StreamReader reader = new StreamReader(stream);
            string htmlText = reader.ReadToEnd();
            return htmlText;
        }
    }
}
