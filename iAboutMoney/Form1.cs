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
        //TODO: Refactoring 
        //TODO: Create automatic 1/4, 1/2 annual reports 
        public Form1()
        {
            InitializeComponent();

            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            this.Location = new Point(screenWidth - 190, 50);
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
            moneyHelper.SetDate();
            moneyHelper.LoadDatasFromFile();

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




        /// <summary>
        /// Copy sms file out from main folder
        /// Read sms to array
        /// </summary>
        /*public void LoadDatasFromFile()
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
                    moneyHelper.FilePath = item;
                }
            }

            string information = File.ReadAllText(moneyHelper.FilePath);
            MoneyHelper.SmsArray = Regex.Split(information, @"<sms protocol=");
        }

    */


        
        /// <summary>
        /// Drawing the actual year Chart
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Chart_Paint(object sender, PaintEventArgs e)
        {
            Graphics graphics = e.Graphics;
            e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;
            Pen pen = new Pen(Color.Green, 2);
            Pen pen2 = new Pen(Color.Gray, 0.1F);

            List<int> balanceList = new List<int>();

            try
            {
                graphics.DrawLine(pen2, 12, 201, 166, 146);

                //January
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". jan. ", balanceList);
                int janB = balanceList.Last();
                //February
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". febr. ", balanceList);
                int febrB = balanceList.Last();
                int[] points = SmsFileWorker.GetTheTwoChartPoints(janB, febrB, 201);
                graphics.DrawLine(pen, 12, 201, 26, points[1]);
                //March  
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". márc. ", balanceList);
                int marchB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( febrB, marchB, points[1]);
                graphics.DrawLine(pen, 26, points[0], 40, points[1]);
                //April
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". ápr. ", balanceList);
                int aprilB = balanceList.Last();
                points= SmsFileWorker.GetTheTwoChartPoints( marchB, aprilB, points[1]);
                graphics.DrawLine(pen, 40, points[0], 54, points[1]);
                //May
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". máj. ", balanceList);
                int mayB = balanceList.Last();
                points= SmsFileWorker.GetTheTwoChartPoints( aprilB, mayB, points[1]);
                graphics.DrawLine(pen, 54, points[0], 68, points[1]);
                //June
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". jún. ", balanceList);
                int juneB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( mayB, juneB, points[1]);
                graphics.DrawLine(pen, 68, points[0], 82, points[1]);
                //July
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". júl. ", balanceList);
                int julyB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( juneB, julyB, points[1]);
                graphics.DrawLine(pen, 82, points[0], 96, points[1]);
                //August
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". aug. ", balanceList);
                int augustB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( julyB, augustB, points[1]);
                graphics.DrawLine(pen, 96, points[0], 110, points[1]);
                //September
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". szept. ", balanceList);
                int septemberB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( augustB, septemberB, points[1]);
                graphics.DrawLine(pen, 110, points[0], 124, points[1]);
                //October 
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". okt. ", balanceList);
                int octoberB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( septemberB, octoberB, points[1]);
                graphics.DrawLine(pen, 124, points[0], 138, points[1]);
                //November
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". nov. ", balanceList);
                int novemberB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( octoberB, novemberB, points[1]);
                graphics.DrawLine(pen, 124, points[0], 138, points[1]);
                //December
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(MoneyHelper.Year + ". dec. ", balanceList);
                int decemberB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( novemberB, decemberB, points[1]);
                graphics.DrawLine(pen, 124, points[0], 138, points[1]);
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
            int screenWidth = Screen.PrimaryScreen.Bounds.Width;
            form2.Location = new Point(screenWidth - 190, 50);

            form2.Show();
        }

        /// <summary>
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Exit_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

    }

}










        
        



