using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace ClassLibrary
{
    public class MoneyHelper
    {        
        public static int Year { get; set; }
        public static string Month { get; set; }
        public bool Saved { get; set; }
        public string SavingTimeFilePath { get; set; } = "savingTime.dat";
        public List<string> SavedMonthList { get; set; }
        public static string[] SmsArray { get; set; }
        public static string FilePath { get; set; }
        public List<int> BalanceList { get; set; }

        public void SetDate()
        {
            var dateAndTime = DateTime.Now;
            Year = dateAndTime.Year;

            string tempMonth = dateAndTime.Month.ToString();
            switch (tempMonth)
            {
                case "1":
                    Month = "January";
                    break;
                case "2":
                    Month = "February";
                    break;
                case "3":
                    Month = "March";
                    break;
                case "4":
                    Month = "April";
                    break;
                case "5":
                    Month = "May";
                    break;
                case "6":
                    Month = "June";
                    break;
                case "7":
                    Month = "July";
                    break;
                case "8":
                    Month = "August";
                    break;
                case "9":
                    Month = "September";
                    break;
                case "10":
                    Month = "October";
                    break;
                case "11":
                    Month = "November";
                    break;
                case "12":
                    Month = "December";
                    break;
            }

            //labelMonth.Text = MoneyHelper.Month.ToString();
        }

        public void LoadDatasFromFile()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\totha\Source\Repos\iAboutMoneyGit\iAboutMoney\bin\Debug");
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

        

    }
}
