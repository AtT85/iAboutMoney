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
        public string SavingTimeFilePath { get; set; } = "savingTime.dat";
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
    }
}
