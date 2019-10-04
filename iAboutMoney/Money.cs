using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Data.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Dropbox.Api;
using System.IO;
using System.Text.RegularExpressions;
using System.Diagnostics;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Globalization;
using ClassLibrary;


namespace iAboutMoney
{
    public partial class Form2 : Form
    {
        MoneyHelper moneyHelper = new MoneyHelper();

        static DataContext dataContext = new DataContext(Database.ConnectionString);
        Table<MoneyInfoTable> moneyDb = dataContext.GetTable<MoneyInfoTable>();

        static SqlConnection sqlCon = new SqlConnection(Database.ConnectionString);
        SqlCommand sqlCmd = new SqlCommand(Database.SqlComm, sqlCon);


        public Form2()
        {
            InitializeComponent();
        }


        private void Form2_Load(object sender, EventArgs e)
        {            
            labelMonth.Text = MoneyHelper.Month;
            labelBalanceActual.Text = LoadActualBalance().ToString("C0");
            moneyHelper.LoadDatasFromFile();
            LoadMonthData();

            LoadSum();            
        } 

        


        
        private void LoadMonthData()
        {
            string searchTime;
            switch (MoneyHelper.Month)
            {
                case "January":
                    searchTime = MoneyHelper.Year + ". jan. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "February":
                    searchTime = MoneyHelper.Year + ". febr. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "March":
                    searchTime = MoneyHelper.Year + ". márc. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "April":
                    searchTime = MoneyHelper.Year + ". ápr. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "May":
                    searchTime = MoneyHelper.Year + ". máj. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "June":
                    searchTime = MoneyHelper.Year + ". jún. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "July":
                    searchTime = MoneyHelper.Year + ". júl. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "August":
                    searchTime = MoneyHelper.Year + ". aug. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "September":
                    searchTime = MoneyHelper.Year + ". szept. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "October":
                    searchTime = MoneyHelper.Year + ". okt. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "November":
                    searchTime = MoneyHelper.Year + ". nov. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
                case "December":
                    searchTime = MoneyHelper.Year + ". dec. ";
                    LoadIncome(searchTime);
                    LoadExpense(searchTime);
                    break;
            }
        }        

        /// <summary>
        /// Set Income label with SmsArray
        /// </summary>
        /// <param name="s">searched time interval</param>
        private void LoadIncome(string s)
        {
            List<int> monthlyIncomeList = new List<int>();
            int income;
           
            foreach (var item in MoneyHelper.SmsArray)
            {
                if (item.Contains(s))
                {                   
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO"))
                    {

                    }
                    else
                    {                        
                        string haviBevetel = SmsFileWorker.GetBetween(item, "S:+", ",-HUF;");
                        string haviBevetelReplaced = haviBevetel.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced, out int intHaviBevetel))
                        {
                            monthlyIncomeList.Add(intHaviBevetel);
                        }
                        string haviBevetel2 = SmsFileWorker.GetBetween(item, "t: +", " HUF;");
                        string haviBevetelReplaced2 = haviBevetel2.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced2, out int intHaviBevetel2))
                        {
                            monthlyIncomeList.Add(intHaviBevetel2);
                        }
                        string haviBevetel3 = SmsFileWorker.GetBetween(item, ":+", ",-HUF; Közl");
                        string haviBevetelReplaced3 = haviBevetel3.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced3, out int intHaviBevetel3))
                        {
                            monthlyIncomeList.Add(intHaviBevetel3);
                        }
                    }                    
                }
            }

            income = monthlyIncomeList.Sum();
            labelIncome.Text = income.ToString("C0");
        }       

        /// <summary>
        /// Set Expense, Credit, Fueling label with SmsArray
        /// </summary>
        /// <param name="s">searched time interval</param>
        private void LoadExpense(string s)
        {            
            //CREDIT
            List<int> monthlyCreditList = new List<int>();
            int credit;

            foreach (var item in MoneyHelper.SmsArray)
            {
                if (item.Contains(s))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO"))
                    {

                    }
                    else if (item.Contains("TÖRLESZTÉS") || item.Contains("khitel Központ Zrt."))
                    {                        
                        string haviTorlesztes = SmsFileWorker.GetBetween(item, "S:-", ",-HUF");
                        string dataTorlesztesReplaced = haviTorlesztes.Replace(".", "");
                        if (int.TryParse(dataTorlesztesReplaced, out int intHaviTorlesztes))
                        {
                            monthlyCreditList.Add(intHaviTorlesztes);
                        }
                    }
                }
            }

            credit = monthlyCreditList.Sum();
            labelCredit.Text = credit.ToString("C0");
            
            //OTHER EXPENSE
            List<int> otherExpenseList = new List<int>();
            int otherExpense;

            foreach (var item in MoneyHelper.SmsArray)
            {
                if (item.Contains(s))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") || item.Contains("khitel Központ Zrt.") || item.Contains("SHELL TÖLTÖ")
                        || item.Contains("MOL TÖLTÖ") || item.Contains("GP T") || item.Contains("AUCHAN AUTBEN") || item.Contains("; OMV"))
                    {

                    }
                    else if (item.Contains("ATM") || item.Contains("s/z") || item.Contains("rty") && item.Contains("s v") || item.Contains("NAPKÖZBENI")
                          || item.Contains("BIZTOSIT") || item.Contains("VAL KAPCS. DIJ") || item.Contains("TUTAL"))
                         {                        
                            string haviEgyebKiadas = SmsFileWorker.GetBetween(item, ": -", " HUF;");
                             string haviEgyebKiadasReplaced = haviEgyebKiadas.Replace(".", "");
                            if (int.TryParse(haviEgyebKiadasReplaced, out int intHaviEgyebKiadas))
                            {
                                otherExpenseList.Add(intHaviEgyebKiadas);
                            }
                            string haviEgyebKiadas2 = SmsFileWorker.GetBetween(item, ":-", ",-HUF;");
                            string haviEgyebKiadasReplaced2 = haviEgyebKiadas2.Replace(".", "");
                            if (int.TryParse(haviEgyebKiadasReplaced2, out int intHaviEgyebKiadas2))
                            {
                                otherExpenseList.Add(intHaviEgyebKiadas2);
                            }
                         }
                }
            }

            otherExpense = otherExpenseList.Sum();
            labelExpense.Text = otherExpense.ToString("C0");

            //FUELING
            List<int> monthlyFuelingList = new List<int>();
            int fueling;

            List<int> monthlyFuelingListAuchan = new List<int>();
            List<int> monthlyFuelingListAuchanStorno = new List<int>();
            int monthlyFuelingAuchan;
            int monthlyFuelingAuchanStorno;
            int realTankolasAuchan;

            foreach (var item in MoneyHelper.SmsArray)
            {
                if (item.Contains(s))
                {

                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") && !item.Contains("AUCHAN AUTBEN"))
                    {

                    }
                    else if (item.Contains("SHELL TÖLTÖ") || item.Contains("MOL TÖLTÖ") ||
                        item.Contains("GP T") || item.Contains("; OMV"))
                    {                        
                         string haviTankolas = SmsFileWorker.GetBetween(item, ": -", " HUF;");
                         string haviTankolasReplaced = haviTankolas.Replace(".", "");
                         if (int.TryParse(haviTankolasReplaced, out int intHaviTankolas))
                         {
                            monthlyFuelingList.Add(intHaviTankolas);
                         }
                         string haviTankolas2 = SmsFileWorker.GetBetween(item, ":-", ",-HUF;");
                         string haviTankolasReplaced2 = haviTankolas2.Replace(".", "");
                         if (int.TryParse(haviTankolasReplaced2, out int intHaviTankolas2))
                         {
                            monthlyFuelingList.Add(intHaviTankolas2);
                         }
                    }
                    //Fueling Auchan
                    else if (item.Contains("AUCHAN AUTBEN") && !item.Contains("STORNO"))
                    {
                        string haviTankolasAuchan = SmsFileWorker.GetBetween(item, ": -", " HUF;");
                        string haviTankolasAuchanReplaced = haviTankolasAuchan.Replace(".", "");
                        if (int.TryParse(haviTankolasAuchanReplaced, out int intHaviTankolasAuchan))
                        {
                            monthlyFuelingListAuchan.Add(intHaviTankolasAuchan);
                        }
                        string haviTankolasAuchan2 = SmsFileWorker.GetBetween(item, ":-", ",-HUF;");
                        string haviTankolasAuchanReplaced2 = haviTankolasAuchan2.Replace(".", "");
                        if (int.TryParse(haviTankolasAuchanReplaced2, out int intHaviTankolasAuchan2))
                        {
                           monthlyFuelingListAuchan.Add(intHaviTankolasAuchan2);
                        }
                    }
                    else if (item.Contains("AUCHAN AUTBEN") && item.Contains("STORNO"))
                    {
                        string haviTankolasStorno = SmsFileWorker.GetBetween(item, "O: +", " HUF;");
                        string haviTankolasStornoReplaced = haviTankolasStorno.Replace(".", "");
                        if (int.TryParse(haviTankolasStornoReplaced, out int intHaviTankolasStorno))
                        {
                            monthlyFuelingListAuchanStorno.Add(intHaviTankolasStorno);
                        }
                    }
                }
            }

            monthlyFuelingAuchan = monthlyFuelingListAuchan.Sum();
            monthlyFuelingAuchanStorno = monthlyFuelingListAuchanStorno.Sum();
            realTankolasAuchan = monthlyFuelingAuchan - monthlyFuelingAuchanStorno;
            monthlyFuelingList.Add(realTankolasAuchan);

            fueling = monthlyFuelingList.Sum();
            labelFueling.Text = fueling.ToString("C0");
        }
       
        /// <summary>
        /// Set Balance label with SmsArray
        /// </summary>
        private int LoadActualBalance()
        {
            List<int> actualBalanceList = new List<int>();

            foreach (var item in MoneyHelper.SmsArray)
            {
                string dataEgy = SmsFileWorker.GetBetween(item, "Egy:+", ",-HUF");
                string dataEgy2 = dataEgy.Replace(".", "");
                if (int.TryParse(dataEgy2, out int egyenlegEgy))
                {
                    actualBalanceList.Add(egyenlegEgy);
                }

                string dataEgyenleg = SmsFileWorker.GetBetween(item, "Egyenleg: +", " HUF");
                string dataEgyenleg2 = dataEgyenleg.Replace(".", "");
                if (int.TryParse(dataEgyenleg2, out int egyenlegEgyenleg))
                {
                    actualBalanceList.Add(egyenlegEgyenleg);
                }
            }

            int intActualBalance = actualBalanceList.Last();

            return intActualBalance;
        }

        /// <summary>
        /// Set Sum label with other labels
        /// </summary>
        private void LoadSum()
        {
            string income = labelIncome.Text;
            int intIncome = int.Parse(income, NumberStyles.Currency);
            string expense = labelExpense.Text;
            int intExpense = int.Parse(expense, NumberStyles.Currency);
            string credit = labelCredit.Text;
            int intCredit = int.Parse(credit, NumberStyles.Currency);
            string fueling = labelFueling.Text;
            int intFueling = int.Parse(fueling, NumberStyles.Currency);

            int sumrResult = intIncome - intExpense - intCredit - intFueling;
            labelSumResult.Text = sumrResult.ToString("C0");
        }          
        


        //DATABASE        
        /// <summary>
        /// Yearly income from database for set income label 
        /// </summary>
        /// <param name="y">searched year</param>
        private void YearlyIncomeFromDatab(int y)
        {
            List<int> ListYearIncome = new List<int>();
            int intYearlyIncome;

            var varYearlyIncome = from c in moneyDb where (c.Year == y && c.Type == "Income") select c.Money;

            foreach (var item in varYearlyIncome)
            {
                ListYearIncome.Add(item);
            }

            intYearlyIncome = ListYearIncome.Sum();
            labelIncome.Text = intYearlyIncome.ToString("C0");
        }

        /// <summary>
        /// Yearly expense from database for set expense label
        /// </summary>
        /// <param name="y">searched year</param>
        private void YearlyExpenseFromDatab(int y)
        {
            List<int> ListYearExpense = new List<int>();
            int intYearlyExpense;

            var varYearlyExpense = from c in moneyDb where (c.Year == y && c.Type == "Expense") select c.Money;

            foreach (var item in varYearlyExpense)
            {
                ListYearExpense.Add(item);
            }

            intYearlyExpense = ListYearExpense.Sum();
            labelExpense.Text = intYearlyExpense.ToString("C0");
        }

        /// <summary>
        /// Yearly credit from database for set credit label
        /// </summary>
        /// <param name="y">searched year</param>
        private void YearlyCreditFromDatab(int y)
        {
            List<int> ListYearCredit = new List<int>();
            int intYearlyCredit;

            var varYearlyCredit = from c in moneyDb where (c.Year == y && c.Type == "Credit") select c.Money;

            foreach (var item in varYearlyCredit)
            {
                ListYearCredit.Add(item);
            }

            intYearlyCredit = ListYearCredit.Sum();
            labelCredit.Text = intYearlyCredit.ToString("C0");
        }

        /// <summary>
        /// Yearly fueling from database for set fueling label
        /// </summary>
        /// <param name="y">searched year</param>
        private void YearlyFuelingFromDatab(int y)
        {
            List<int> ListYearFueling = new List<int>();
            int intYearlyFueling;

            var varYearlyFueling = from c in moneyDb where (c.Year == y && c.Type == "Fueling") select c.Money;

            foreach (var item in varYearlyFueling)
            {
                ListYearFueling.Add(item);
            }

            intYearlyFueling = ListYearFueling.Sum();
            labelFueling.Text = intYearlyFueling.ToString("C0");
        }

        /// <summary>
        /// Monthly income from database for set income label
        /// </summary>
        /// <param name="y">searched year</param>
        /// <param name="m">searched month</param>
        private void MonthlyIncomeFromDatab(int y, string m)
        {
            List<int> monthlyIncomeList = new List<int>();
            int intMonthlyIncome;            
          
            var varMonthlyIncome = from c in moneyDb where (c.Year==y && c.Month == m && c.Type == "Income") select c.Money;

            foreach (var item in varMonthlyIncome)
            {
                monthlyIncomeList.Add(item);
            }

            intMonthlyIncome = monthlyIncomeList.Sum();
            labelIncome.Text = intMonthlyIncome.ToString("C0");
        }

        /// <summary>
        /// Monthly expense from database for set expense label
        /// </summary>
        /// <param name="y">searched year</param>
        /// <param name="m">searched month</param>
        private void MonthlyExpenseFromDatab(int y, string m)
        {
            List<int> monthlyExpenseList = new List<int>();
            int intMonthlyExpense;

            var varMonthlylyExpense = from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Expense") select c.Money;

            foreach (var item in varMonthlylyExpense)
            {
                monthlyExpenseList.Add(item);
            }

            intMonthlyExpense = monthlyExpenseList.Sum();
            labelExpense.Text = intMonthlyExpense.ToString("C0");
        }

        /// <summary>
        /// Monthly credit from database for set credit label
        /// </summary>
        /// <param name="y">searched year</param>
        /// <param name="m">searched month</param>
        private void MonthlyCreditFromDatab(int y, string m)
        {
            List<int> monthlyCreditList = new List<int>();
            int intMonthlyCredit;
            
            var varMonthlylyCredit = from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Credit") select c.Money;

            foreach (var item in varMonthlylyCredit)
            {
                monthlyCreditList.Add(item);
            }

            intMonthlyCredit = monthlyCreditList.Sum();
            labelCredit.Text = intMonthlyCredit.ToString("C0");
        }

        /// <summary>
        /// Monthly fueling from database for set fueling label
        /// </summary>
        /// <param name="y">searched year</param>
        /// <param name="m">searched month</param>
        private void MonthlyFuelingFromDatab(int y, string m)
        {
            List<int> monthlyFuelingList = new List<int>();
            int intMonthlyFueling;

            var varMonthlyFueling = from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Fueling") select c.Money;

            foreach (var item in varMonthlyFueling)
            {
               monthlyFuelingList.Add(item);
            }

            intMonthlyFueling = monthlyFuelingList.Sum();
            labelFueling.Text = intMonthlyFueling.ToString("C0");
        }

        



        //NAVIGATiON
        /// <summary>
        /// Set Month label to actual Year
        /// Yearly(Income&Expense&Credit&Fueling)FromDatab
        /// LoadSum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Year_Click(object sender, EventArgs e)
        {
            labelMonth.Text = MoneyHelper.Year.ToString();

            YearlyIncomeFromDatab(MoneyHelper.Year);
            YearlyExpenseFromDatab(MoneyHelper.Year);
            YearlyCreditFromDatab(MoneyHelper.Year);
            YearlyFuelingFromDatab(MoneyHelper.Year);

            LoadSum();
        }

        /// <summary>
        /// SetDate 
        /// LoadDataFromFile, LoadMonthData 
        /// LoadSum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Month_Click(object sender, EventArgs e)
        {
            labelMonth.Text = MoneyHelper.Month;
            LoadMonthData();
            LoadSum();            
        }

        /// <summary>
        /// Decide year or month
        /// Income, Expense, Credit, Fueling
        /// LoadSum
        /// Set month label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Prev_Click(object sender, EventArgs e)
        {

            if (int.TryParse(labelMonth.Text, out int year))
            {
                int searchYear = year-1;
                YearlyIncomeFromDatab(searchYear);
                YearlyExpenseFromDatab(searchYear);
                YearlyCreditFromDatab(searchYear);
                YearlyFuelingFromDatab(searchYear);

                LoadSum();
                labelMonth.Text = searchYear.ToString();
            }
            string month;
            switch(labelMonth.Text)
            {                
                case "February":
                    labelMonth.Text = "January";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "January";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "March":
                    labelMonth.Text = "February";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "February";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "April":
                    labelMonth.Text = "March";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "March";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "May":
                    labelMonth.Text = "April";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "April";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "June":
                    labelMonth.Text = "May";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "May";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "July":
                    labelMonth.Text = "June";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "June";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "August":
                    labelMonth.Text = "July";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "July";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "September":
                    labelMonth.Text = "August";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "August";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "October":
                    labelMonth.Text = "September";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "September";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "November":
                    labelMonth.Text = "October";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "October";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "December":
                    labelMonth.Text = "November";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "November";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
            }
        }

        /// <summary>
        /// Decide year or month
        /// Income, Expense, Credit, Fueling
        /// LoadSum
        /// Set month label
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Next_Click(object sender, EventArgs e)
        {

            if (int.TryParse(labelMonth.Text, out int year))
            {
                int searchYear = year + 1;
                YearlyIncomeFromDatab(searchYear);
                YearlyExpenseFromDatab(searchYear);
                YearlyCreditFromDatab(searchYear);
                YearlyFuelingFromDatab(searchYear);

                LoadSum();
                labelMonth.Text = searchYear.ToString();
            }

            string month;

            switch (labelMonth.Text)
            {
                case "January":
                    labelMonth.Text = "February";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "February";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "February":
                    labelMonth.Text = "March";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "March";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "March":
                    labelMonth.Text = "April";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "April";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "April":
                    labelMonth.Text = "May";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "May";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "May":
                    labelMonth.Text = "June";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "June";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "June":
                    labelMonth.Text = "July";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "July";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "July":
                    labelMonth.Text = "August";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "August";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "August":
                    labelMonth.Text = "September";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "September";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "September":
                    labelMonth.Text = "October";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "October";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "October":
                    labelMonth.Text = "November";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "November";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
                case "November":
                    labelMonth.Text = "December";
                    if (labelMonth.Text == MoneyHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "December";
                        MonthlyIncomeFromDatab(MoneyHelper.Year, month);
                        MonthlyExpenseFromDatab(MoneyHelper.Year, month);
                        MonthlyCreditFromDatab(MoneyHelper.Year, month);
                        MonthlyFuelingFromDatab(MoneyHelper.Year, month);
                        LoadSum();

                    }
                    break;
            }
        }



        
        //SAVING TO DATABASE    
        /// <summary>
        /// Read saved times to list
        /// </summary>
        private void ReadSavingTime()
        {
            if (File.Exists(MoneyHelper.SavingTimeFilePath))
            {
                moneyHelper.SavedMonthList = File.ReadAllLines(MoneyHelper.SavingTimeFilePath).ToList();
            }
        }

        /// <summary>
        /// ReadSavingTime
        /// if not save in this month LoadSavingData
        /// </summary>
        private void Save()
        {
            ReadSavingTime();            
            
            foreach (var item in moneyHelper.SavedMonthList)
            {
                if (item == MoneyHelper.Month)
                {
                    moneyHelper.Saved = true;                    
                }
            }

            if (moneyHelper.Saved == false)
            {
                var task = Task.Run((Func<Task>)DropboxClass.Run);
                task.Wait();
                LoadSavingData();
            }
        }
        
        /// <summary>
        /// Setup saving month
        /// Call SaveToDatabase, WriteSavingTimeToFile
        /// </summary>
        private void LoadSavingData()
        {
            string searchString;

            switch (MoneyHelper.Month)
            {
                case "January":
                    searchString = ". dec. ";
                    SaveToDatabase(MoneyHelper.Year-1, searchString, "December");
                    WriteSavingTimeToFile();
                    break;
                case "February":
                    searchString = ". jan. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "January");
                    WriteSavingTimeToFile();
                    break;
                case "March":
                    searchString = ". febr. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "February");
                    WriteSavingTimeToFile();
                    break;
                case "April":
                    searchString = ". márc. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "March");
                    WriteSavingTimeToFile();
                    break;
                case "May":
                    searchString = ". ápr. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "April");
                    WriteSavingTimeToFile();
                    break;
                case "June":
                    searchString = ". máj. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "May");
                    WriteSavingTimeToFile();
                    break;
                case "July":
                    searchString = ". jún. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "Juny");
                    WriteSavingTimeToFile();
                    break;
                case "August":
                    searchString = ". júl. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "July");
                    WriteSavingTimeToFile();
                    break;
                case "September":
                    searchString = ". aug. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "August");
                    WriteSavingTimeToFile();
                    break;
                case "October":
                    searchString = ". szept. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "September");
                    WriteSavingTimeToFile();
                    break;
                case "November":
                    searchString = ". okt. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "October");
                    WriteSavingTimeToFile();
                    break;
                case "December":
                    searchString = ". nov. ";
                    SaveToDatabase(MoneyHelper.Year, searchString, "November");
                    WriteSavingTimeToFile();
                    break;
            }
        }

        /// <summary>
        /// Save previous month to database
        /// </summary>
        /// <param name="year">year</param>
        /// <param name="month">previous month</param>
        /// <param name="monthToDatabase">to month field in database</param>
        private void SaveToDatabase(int year, string month, string monthToDatabase)
        {
            List<int> monthlySaveIncomeList = new List<int>();
            int intSaveIncome;

            //Income
            foreach (var item in MoneyHelper.SmsArray)
            {
                if (item.Contains(year + month))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO"))
                    {

                    }
                    else
                    {                        
                        string haviBevetel = SmsFileWorker.GetBetween(item, "S:+", ",-HUF;");
                        string haviBevetelReplaced = haviBevetel.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced, out int intHaviBevetel))
                        {
                            monthlySaveIncomeList.Add(intHaviBevetel);
                        }
                        string haviBevetel2 = SmsFileWorker.GetBetween(item, "t: +", " HUF;");
                        string haviBevetelReplaced2 = haviBevetel2.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced2, out int intHaviBevetel2))
                        {
                            monthlySaveIncomeList.Add(intHaviBevetel2);
                        }
                        string haviBevetel3 = SmsFileWorker.GetBetween(item, ":+", ",-HUF; Közl");
                        string haviBevetelReplaced3 = haviBevetel3.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced3, out int intHaviBevetel3))
                        {
                            monthlySaveIncomeList.Add(intHaviBevetel3);
                        }
                    }
                }
            }

            intSaveIncome = monthlySaveIncomeList.Sum();

            sqlCon.Open();
            sqlCmd.Parameters.AddWithValue("@Year", MoneyHelper.Year);
            sqlCmd.Parameters.AddWithValue("@Month", monthToDatabase);
            sqlCmd.Parameters.AddWithValue("@Money", intSaveIncome);
            sqlCmd.Parameters.AddWithValue("@Type", "Income");
            sqlCmd.ExecuteNonQuery();
            sqlCon.Close();

            //Credit
            List<int> monthlySaveCreditList = new List<int>();

            foreach (var item in MoneyHelper.SmsArray)
            {
                if (item.Contains(year + month))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO"))
                    {

                    }
                    else if (item.Contains("TÖRLESZTÉS") || item.Contains("khitel Központ Zrt."))
                    {
                        string haviTorlesztes = SmsFileWorker.GetBetween(item, "S:-", ",-HUF");
                        string dataTorlesztesReplaced = haviTorlesztes.Replace(".", "");
                        if (int.TryParse(dataTorlesztesReplaced, out int intHaviTorlesztes))
                        {
                            monthlySaveCreditList.Add(intHaviTorlesztes);
                        }
                    }
                }
            }

            foreach (var item in monthlySaveCreditList)
            {
                sqlCon.Open();
                sqlCmd.Parameters.AddWithValue("@Year", MoneyHelper.Year);
                sqlCmd.Parameters.AddWithValue("@Month", monthToDatabase);
                sqlCmd.Parameters.AddWithValue("@Money", item);
                sqlCmd.Parameters.AddWithValue("@Type", "Credit");
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }

            //Expense
            List<int> monthlySaveExpenseList = new List<int>();

            foreach (var item in MoneyHelper.SmsArray)
            {
                if (item.Contains(year + month))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") || item.Contains("khitel Központ Zrt.") || item.Contains("SHELL TÖLTÖ")
                        || item.Contains("MOL TÖLTÖ") || item.Contains("GP T") || item.Contains("AUCHAN AUTBEN") || item.Contains("; OMV"))
                    {

                    }
               else if (item.Contains("ATM") || item.Contains("s/z") || item.Contains("rty") && item.Contains("s v") || item.Contains("NAPKÖZBENI")
                          || item.Contains("BIZTOSIT") || item.Contains("VAL KAPCS. DIJ") || item.Contains("TUTAL"))
                    {
                        string haviEgyebKiadas = SmsFileWorker.GetBetween(item, ": -", " HUF;");
                        string haviEgyebKiadasReplaced = haviEgyebKiadas.Replace(".", "");
                        if (int.TryParse(haviEgyebKiadasReplaced, out int intHaviEgyebKiadas))
                        {
                            monthlySaveExpenseList.Add(intHaviEgyebKiadas);
                        }
                        string haviEgyebKiadas2 = SmsFileWorker.GetBetween(item, ":-", ",-HUF;");
                        string haviEgyebKiadasReplaced2 = haviEgyebKiadas2.Replace(".", "");
                        if (int.TryParse(haviEgyebKiadasReplaced2, out int intHaviEgyebKiadas2))
                        {
                            monthlySaveExpenseList.Add(intHaviEgyebKiadas2);
                        }
                    }
                }
            }

            foreach (var item in monthlySaveExpenseList)
            {
                sqlCon.Open();
                sqlCmd.Parameters.AddWithValue("@Year", MoneyHelper.Year);
                sqlCmd.Parameters.AddWithValue("@Month", monthToDatabase);
                sqlCmd.Parameters.AddWithValue("@Money", item);
                sqlCmd.Parameters.AddWithValue("@Type", "Expense");
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }

            //Fueling
            List<int> monthlySaveFuelingList = new List<int>();
            List<int> monthlySaveFuelingAuchanList = new List<int>();
            List<int> monthlySaveFuelingAuchanStornoList = new List<int>();
            int intSaveFuelingAuchan;
            int intSaveFuelingAuchanStorno;
            int intRealFuelingAuchan;

            foreach (var item in MoneyHelper.SmsArray)
            {
                if (item.Contains(year + month))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") && !item.Contains("AUCHAN AUTBEN"))
                    {

                    }
               else if (item.Contains("SHELL TÖLTÖ") || item.Contains("MOL TÖLTÖ") ||
                        item.Contains("GP T") || item.Contains("; OMV"))
                    {
                        string haviTankolas = SmsFileWorker.GetBetween(item, ": -", " HUF;");
                        string haviTankolasReplaced = haviTankolas.Replace(".", "");
                        if (int.TryParse(haviTankolasReplaced, out int intHaviTankolas))
                        {
                            monthlySaveFuelingList.Add(intHaviTankolas);
                        }
                        string haviTankolas2 = SmsFileWorker.GetBetween(item, ":-", ",-HUF;");
                        string haviTankolasReplaced2 = haviTankolas2.Replace(".", "");
                        if (int.TryParse(haviTankolasReplaced2, out int intHaviTankolas2))
                        {
                            monthlySaveFuelingList.Add(intHaviTankolas2);
                        }
                    }

                    //Auchan
                    else if (item.Contains("AUCHAN AUTBEN") && !item.Contains("STORNO"))
                    {
                        string haviTankolasAuchan = SmsFileWorker.GetBetween(item, ": -", " HUF;");
                        string haviTankolasAuchanReplaced = haviTankolasAuchan.Replace(".", "");
                        if (int.TryParse(haviTankolasAuchanReplaced, out int intHaviTankolasAuchan))
                        {
                            monthlySaveFuelingAuchanList.Add(intHaviTankolasAuchan);
                        }
                        string haviTankolas2 = SmsFileWorker.GetBetween(item, ":-", ",-HUF;");
                        string haviTankolasReplaced2 = haviTankolas2.Replace(".", "");
                        if (int.TryParse(haviTankolasReplaced2, out int intHaviTankolas2))
                        {
                            monthlySaveFuelingAuchanList.Add(intHaviTankolas2);
                        }
                    }
                    else if (item.Contains("AUCHAN AUTBEN") && item.Contains("STORNO"))
                    {
                        string haviTankolasStorno = SmsFileWorker.GetBetween(item, "O: +", " HUF;");
                        string haviTankolasStornoReplaced = haviTankolasStorno.Replace(".", "");
                        if (int.TryParse(haviTankolasStornoReplaced, out int intHaviTankolasStorno))
                        {
                            monthlySaveFuelingAuchanStornoList.Add(intHaviTankolasStorno);
                        }
                    }
                }
            }

            intSaveFuelingAuchan = monthlySaveFuelingAuchanList.Sum();
            intSaveFuelingAuchanStorno = monthlySaveFuelingAuchanStornoList.Sum();
            intRealFuelingAuchan = intSaveFuelingAuchan - intSaveFuelingAuchanStorno;
            monthlySaveFuelingList.Add(intRealFuelingAuchan);

            foreach (var item in monthlySaveFuelingList)
            {
                sqlCon.Open();
                sqlCmd.Parameters.AddWithValue("@Year", MoneyHelper.Year);
                sqlCmd.Parameters.AddWithValue("@Month", monthToDatabase);
                sqlCmd.Parameters.AddWithValue("@Money", item);
                sqlCmd.Parameters.AddWithValue("@Type", "Fueling");
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }
        }

        /// <summary>
        /// Write saving time(month) to file
        /// </summary>
        private void WriteSavingTimeToFile()
        {
            using (FileStream stream = new FileStream(MoneyHelper.SavingTimeFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(MoneyHelper.Month);
                }
            }
        }
        



        //DOWNLOAD FILE
        /// <summary>
        /// Call DropboxClass Run method
        /// Finish label 
        /// LoadDataFromFile, LoadMonthData, LoadSum 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void DownloadFileTitle_Click(object sender, EventArgs e)
        {
            var task = Task.Run((Func<Task>)DropboxClass.Run);
            task.Wait();
            DownloadFinishLabel.Visible = true;
            moneyHelper.LoadDatasFromFile();
            LoadMonthData();
            labelBalanceActual.Text = LoadActualBalance().ToString("C0");
            LoadSum();            
        }




       /// <summary>
       /// Switch to chart
       /// </summary>
       /// <param name="sender"></param>
       /// <param name="e"></param>
        private void SwitchToChartForm_Click(object sender, EventArgs e)
        {
            Hide();            
        }

        /// <summary>
        /// Save
        /// Exit
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ExitForm2_Click(object sender, EventArgs e)
        {
            Save();
            Application.Exit();
        }        
    }
}
