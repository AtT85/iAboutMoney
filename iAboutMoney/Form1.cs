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
            this.Location = new Point(screenWidth - 190, 20);
        }

        Form2 form2= new Form2();
        
        SerialPort myPort = new SerialPort();        
        DateHelper dateHelper = new DateHelper();


        /// <summary>
        /// Load datas to smsArray[]
        /// Setup myPort
        /// Call TemperatureMet()
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Form1_LoadAsync(object sender, EventArgs e)
        {
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



            dateHelper.SetDate();
            WriteAndReadClass.LoadDatasFromFile();

            if (WriteAndReadClass.ShouldDownloadFile())
            {
                await DropboxClass.Download();
                WriteAndReadClass.WriteToFile( WriteAndReadClass.ShouldDownloadFilePath, DateHelper.Day.ToString());                
            }

            DropboxClass.MoveFileFromMainFolder();

            WriteAndReadClass.LoadDatasFromFile();
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
                SmsFileWorker.LoadBalancesForChart(".12." + (DateHelper.Year-1), balanceList);
                int decemberB = balanceList.Last();
                SmsFileWorker.LoadBalancesForChart(".01." + DateHelper.Year, balanceList);
                int janB = balanceList.Last();
                int[] points = SmsFileWorker.GetTheTwoChartPoints(decemberB, janB, 201);
                graphics.DrawLine(pen, 12, 201, 25, points[1]);
                //February
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".02." + DateHelper.Year, balanceList);
                int febrB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints(janB, febrB, 201);
                graphics.DrawLine(pen, 25, points[0], 38, points[1]);
                //March  
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".03." + DateHelper.Year, balanceList);
                int marchB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( febrB, marchB, points[1]);
                graphics.DrawLine(pen, 38, points[0], 51, points[1]);
                //April
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".04." + DateHelper.Year, balanceList);
                int aprilB = balanceList.Last();
                points= SmsFileWorker.GetTheTwoChartPoints( marchB, aprilB, points[1]);
                graphics.DrawLine(pen, 51, points[0], 64, points[1]);
                //May
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".05." + DateHelper.Year, balanceList);
                int mayB = balanceList.Last();
                points= SmsFileWorker.GetTheTwoChartPoints( aprilB, mayB, points[1]);
                graphics.DrawLine(pen, 64, points[0], 77, points[1]);
                //June
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".06." + DateHelper.Year, balanceList);
                int juneB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( mayB, juneB, points[1]);
                graphics.DrawLine(pen, 77, points[0], 90, points[1]);
                //July
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".07." + DateHelper.Year, balanceList);
                int julyB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( juneB, julyB, points[1]);
                graphics.DrawLine(pen, 90, points[0], 103, points[1]);
                //August
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".08." + DateHelper.Year, balanceList);
                int augustB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( julyB, augustB, points[1]);
                graphics.DrawLine(pen, 103, points[0], 116, points[1]);
                //September
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".09." + DateHelper.Year, balanceList);
                int septemberB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( augustB, septemberB, points[1]);
                graphics.DrawLine(pen, 116, points[0], 129, points[1]);
                //October 
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".10." + DateHelper.Year, balanceList);
                int octoberB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( septemberB, octoberB, points[1]);
                graphics.DrawLine(pen, 129, points[0], 142, points[1]);
                //November
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".11." + DateHelper.Year, balanceList);
                int novemberB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( octoberB, novemberB, points[1]);
                graphics.DrawLine(pen, 142, points[0], 155, points[1]);
                //December
                balanceList.Clear();
                SmsFileWorker.LoadBalancesForChart(".12." + DateHelper.Year, balanceList);
                decemberB = balanceList.Last();
                points = SmsFileWorker.GetTheTwoChartPoints( novemberB, decemberB, points[1]);
                graphics.DrawLine(pen, 155, points[0], 167, points[1]);
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
            form2.Location = new Point(screenWidth - 190, 20);
            
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










        
        



