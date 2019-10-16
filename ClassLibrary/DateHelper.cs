using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;


namespace ClassLibrary
{
    public class DateHelper
    {        
        public static int Year { get; set; }
        public static string Month { get; set; }
        public static string Day { get; set; }
        public static string SavingTimeFilePath { get; } = "savingTime.dat";
        public List<string> SavedMonthList { get; set; }
       

        public void SetDate()
        {
            var dateAndTime = DateTime.Now;
            Year = dateAndTime.Year;
            Day = dateAndTime.Day.ToString();

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
        }      

    }
}
