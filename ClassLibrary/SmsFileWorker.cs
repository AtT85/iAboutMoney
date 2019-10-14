using System;
using System.Collections.Generic;
using System.Text;

namespace ClassLibrary
{
    public static class SmsFileWorker
    {
        public static string GetBetween(string strSource, string strStart, string strEnd)
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




        public static List<int> LoadBalancesForChart(string date, List<int> list)
        {
            foreach (var item in WriteAndReadClass.SmsArray)
            {
                if (item.Contains(date))
                {
                    string dataEgy = GetBetween(item, "Egy:+", ",-HUF");
                    string dataEgy2 = dataEgy.Replace(".", "");
                    if (int.TryParse(dataEgy2, out int egyenlegEgy))
                    {
                        list.Add(egyenlegEgy);
                    }

                    string dataEgyenleg = GetBetween(item, "Egyenleg: +", " HUF");
                    string dataEgyenleg2 = dataEgyenleg.Replace(".", "");
                    if (int.TryParse(dataEgyenleg2, out int egyenlegEgyenleg))
                    {
                        list.Add(egyenlegEgyenleg);
                    }
                }
            }
            
            return list;
        }



        public static int[] GetTheTwoChartPoints( int lastbalance, int balance, int lastPointInChart)
        {           
            int temp = ((balance - lastbalance)/10000)/ 2;
            int nextPointInChart = lastPointInChart - temp;
            int[] result = new int[2];
            result[0] = lastPointInChart;
            result[1] = nextPointInChart;

            return result;
        }
    }
}
