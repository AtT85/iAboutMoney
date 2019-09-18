using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace ClassLibrary
{
    public class MoneyHelper
    {        
        public int Year { get; set; }
        public string Month { get; set; }
        public bool Saved { get; set; }
        public string SavedMonthFilePath { get; set; } = "savedMonthFile.dat";
        public List<string> SavedMonthList { get; set; }
        public string[] SmsArray { get; set; }
        public string FilePath { get; set; }




        public string GetBetween(string strSource, string strStart, string strEnd)
        {
            const int kNotFound = -1;

            var startIdx = strSource.IndexOf(strStart);
            if (startIdx != kNotFound)
            {
                startIdx += strStart.Length;
                var endIdx = strSource.IndexOf(strEnd, startIdx);
                if (endIdx > startIdx)
                {
                    return strSource.Substring(startIdx, endIdx - startIdx);
                }
            }
            return String.Empty;
        }
        /*
        public void LoadDatasFromFile()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\totha\Source\Repos\iAboutMoney\iAboutMoney\bin\Debug");
            FileInfo[] Files = d.GetFiles("*.xml");
            foreach (var item in Files)
            {
                if (item.Name.Contains("sms-"))
                {
                    File.Move(item.Name, @"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Dropbox\" + item.Name);
                }
            }

            var files = Directory.GetFiles(@"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Dropbox", "*.xml");
            foreach (var item in files)
            {
                if (item.Contains("sms-"))
                {
                    FilePath = item;
                }
            }

            string information = File.ReadAllText(FilePath);
            SmsArray = Regex.Split(information, @"<sms protocol=");
        }
        */
       
    }
}
