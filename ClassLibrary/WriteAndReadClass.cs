using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace ClassLibrary
{
    public static class WriteAndReadClass
    {
        public static string[] SmsArray { get; set; }
        public static string FilePath { get; set; }
        public static bool SavedToDatabase { get; set; }
        public static string ShouldDownloadFilePath { get; set; } = @"C:\Users\totha\Source\Repos\LibraryiAboutMoney\ShouldDownload\dayOfDownloading.dat";
        public static bool ShouldDownload { get; set; }

        public static bool ShouldDownloadFile()
        {
            string readedDay;
            using (FileStream stream = new FileStream(ShouldDownloadFilePath, FileMode.OpenOrCreate, FileAccess.Read, FileShare.None))
            {
                using (StreamReader reader = new StreamReader(stream))
                {
                    readedDay = reader.ReadLine();
                    if(readedDay!=DateHelper.Day.ToString())
                    {
                        ShouldDownload = true;
                    }
                }
               
            }
            return ShouldDownload;
        }   

        public static void WriteToFileDayOfDownloading(int day)
        {
            using (FileStream stream = new FileStream(ShouldDownloadFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(day);
                }
            }
        }

        public static void LoadDatasFromFile()
        {

            var files = Directory.GetFiles(@"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Dropbox", "*.xml");
            foreach (var item in files)
            {
                if (item.Contains("sms-"))
                {
                    FilePath = item;
                }
            }

            string allSmsString = File.ReadAllText(FilePath);
            SmsArray = Regex.Split(allSmsString, @"<sms protocol=");
        }
    }
}
