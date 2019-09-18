using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Ports;
using ClassLibrary;
using System.Text.RegularExpressions;
using System.Threading;

namespace iAboutMoney
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        Form2 form2= new Form2();

        SerialPort myPort = new SerialPort();        
        MoneyHelper moneyHelper = new MoneyHelper();


        /// <summary>
        /// Load datas to smsArray[]
        /// Setup myPort
        /// Call TemperatureMet()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadDatasFromFile();
            SetDate();
            try
            {
                myPort.BaudRate = 9600;
                myPort.PortName = "COM3";
                myPort.Open();
            }
            catch (Exception)  { }           
            
            timer.Interval = 30000;
            timer.Enabled = true;
            timer.Tick += new EventHandler(TemperatureMet);
        }

        /// <summary>
        /// Set Year
        /// </summary>
        private void SetDate()
        {
            var dateAndTime = DateTime.Now;
            moneyHelper.Year = dateAndTime.Year;            
        }
        
        /// <summary>
        /// Read temperature from myPort to text
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void TemperatureMet(object sender,EventArgs e)
        {            
            try
            {
                string temp = myPort.ReadLine();
                Temperature.Text = temp.ToString();
            }
            catch(Exception)
            {
                Temperature.Text = "n/a";
            }            
        }

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
                    moneyHelper.FilePath = item;
                }
            }

            string information = File.ReadAllText(moneyHelper.FilePath);
            moneyHelper.SmsArray = Regex.Split(information, @"<sms protocol=");
        }

        /// <summary>
        /// Foreach in smsArray for Balances in Chart
        /// </summary>
        /// <param name="date">Look for which month</param>
        /// <param name="list">Add balance to list</param>
        public void LoadBalancesForChart(string date, List<int> list)
        {

            foreach (var item in moneyHelper.SmsArray)
            {
                if (item.Contains(date))
                {
                    string dataEgy = moneyHelper.GetBetween(item, "Egy:+", ",-HUF");
                    string dataEgy2 = dataEgy.Replace(".", "");
                    if (int.TryParse(dataEgy2, out int egyenlegEgy))
                    {
                        list.Add(egyenlegEgy);
                    }

                    string dataEgyenleg = moneyHelper.GetBetween(item, "Egyenleg: +", " HUF");
                    string dataEgyenleg2 = dataEgyenleg.Replace(".", "");
                    if (int.TryParse(dataEgyenleg2, out int egyenlegEgyenleg))
                    {
                        list.Add(egyenlegEgyenleg);
                    }
                }
            }
        }



        /// <summary>
        /// Drawing the actual year Chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            List<int> balanceList = new List<int>();            

            Graphics graphics = e.Graphics;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Green, 2);

            try
            {
                //January
                LoadBalancesForChart(moneyHelper.Year + ". jan. ", balanceList);
                int janB = balanceList.Last();
                //February
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". febr. ", balanceList);
                int febrB = balanceList.Last();
                int resPointFebrY = ((febrB - janB) / 10000) / 2;
                int febrY = 201 - resPointFebrY;
                graphics.DrawLine(pen, 12, 201, 26, febrY);
                //March  
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". márc. ", balanceList);
                int marchB = balanceList.Last();
                int resPointMarchY = ((marchB - febrB) / 10000) / 2;
                int marchY = febrY - resPointMarchY;
                graphics.DrawLine(pen, 26, febrY, 40, marchY);
                //April
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". ápr. ", balanceList);
                int aprilB = balanceList.Last();
                int resPointAprilY = ((aprilB - marchB) / 10000) / 2;
                int aprilY = marchY - resPointAprilY;
                graphics.DrawLine(pen, 40, marchY, 54, aprilY);
                //May
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". máj. ", balanceList);
                int mayB = balanceList.Last();
                int resPointMayY = ((mayB - aprilB) / 10000) / 2;
                int mayY = aprilY - resPointMayY;
                graphics.DrawLine(pen, 54, aprilY, 68, mayY);
                //June
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". jún. ", balanceList);
                int juneB = balanceList.Last();
                int resPointJuneY = ((juneB - mayB) / 10000) / 2;
                int juneY = mayY - resPointJuneY;
                graphics.DrawLine(pen, 68, mayY, 82, juneY);
                //July
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". júl. ", balanceList);
                int julyB = balanceList.Last();
                int resPointJulyY = ((julyB - juneB) / 10000) / 2;
                int julyY = juneY - resPointJulyY;
                graphics.DrawLine(pen, 82, juneY, 96, julyY);
                //August
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". aug. ", balanceList);
                int augustB = balanceList.Last();
                int resPointAugustY = ((augustB - julyB) / 10000) / 2;
                int augustY = julyY - resPointAugustY;
                graphics.DrawLine(pen, 96, julyY, 110, augustY);
                //September
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". szept. ", balanceList);
                int septemberB = balanceList.Last();
                int resPointSeptemberY = ((septemberB - augustB) / 10000) / 2;
                int septemberY = augustY - resPointSeptemberY;
                graphics.DrawLine(pen, 110, augustY, 124, septemberY);
                //October 
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". okt. ", balanceList);
                int octoberB = balanceList.Last();                
                int resPointoctoberY = (octoberB - septemberB) / 10000;
                int octoberY = septemberY - resPointoctoberY;
                graphics.DrawLine(pen, 124, septemberY, 138, octoberY);
                //November
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". nov. ", balanceList);
                int novemberB = balanceList.Last();
                int resPointNovemberY = (novemberB - octoberB) / 10000;
                int novemberY = octoberY - resPointNovemberY;
                graphics.DrawLine(pen, 124, octoberY, 138, novemberY);
                //December
                balanceList.Clear();
                LoadBalancesForChart(moneyHelper.Year + ". dec. ", balanceList);
                int decemberB = balanceList.Last();
                int resPointDecemberY = (decemberB - novemberB) / 10000;
                int decemberY = novemberY - resPointDecemberY;
                graphics.DrawLine(pen, 124, novemberY, 138, decemberY);
            }
            catch (Exception) { }

            graphics.Dispose();
        }



        /// <summary>
        /// Open Form2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SwitchToSecondForm_Click(object sender, EventArgs e)
        {
            form2.Show();
        }



        /// <summary>
        /// Exit Click
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }

}










        
        



