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
using System.Threading;
using System.Net.Mail;


namespace iAboutMoney
{
    public partial class Form2 : Form
    {
        DateHelper dateHelper = new DateHelper();

        static DataContext dataContext = new DataContext(Database.ConnectionString);
        Table<MoneyInfoTable> moneyDb = dataContext.GetTable<MoneyInfoTable>();

        static SqlConnection sqlCon = new SqlConnection(Database.ConnectionString);
        //SqlCommand sqlCmd;

        public Form2()
        {
            InitializeComponent();
        }


        private void Form2_Load(object sender, EventArgs e)
        {            
            labelMonth.Text = DateHelper.Month;
            labelBalanceActual.Text = LoadActualBalance().ToString("C0");
            WriteAndReadClass.LoadDatasFromFile();
            DropboxClass.MoveFileFromMainFolder();

            LoadMonthData();

            LoadSum();
        } 

        private void DataCollectorToReport()
        {
            int income, credit, fueling, otherExpense, summary;            

            if( DateHelper.Month=="April" || DateHelper.Month == "July" || DateHelper.Month == "October" || DateHelper.Month == "January" )
            {

            }
        }
        

        
        private void LoadMonthData()
        {
            string searchTime;
            switch (DateHelper.Month)
            {
                case "January":
                    searchTime =  ".01."+ DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "February":
                    searchTime = ".02." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "March":
                    searchTime = ".03." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "April":
                    searchTime = ".04." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "May":
                    searchTime = ".05." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "June":
                    searchTime = ".06." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "July":
                    searchTime = ".07." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "August":
                    searchTime = ".08." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "September":
                    searchTime = ".09." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "October":
                    searchTime = ".10." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "November":
                    searchTime = ".11." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
                case "December":
                    searchTime = ".12." + DateHelper.Year;
                    labelIncome.Text = LoadActualIncome(searchTime).ToString("C0");
                    labelExpense.Text = LoadActualExpense(searchTime).ToString("C0");
                    labelCredit.Text = LoadActualCredit(searchTime).ToString("C0");
                    labelFueling.Text = LoadActualFueling(searchTime).ToString("C0");
                    break;
            }
        }        

        
        private int LoadActualIncome(string s)
        {
            List<int> monthlyIncomeList = new List<int>();
            int income;
           
            foreach (var item in WriteAndReadClass.SmsArray)
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
                        //string haviBevetel3 = SmsFileWorker.GetBetween(item, ":+", ",-HUF; Közl");
                        //string haviBevetelReplaced3 = haviBevetel3.Replace(".", "");
                        //if (int.TryParse(haviBevetelReplaced3, out int intHaviBevetel3))
                        //{
                        //    monthlyIncomeList.Add(intHaviBevetel3);
                        //}
                    }                    
                }
            }

            income = monthlyIncomeList.Sum();

            return income;
        }       

        
        private int LoadActualExpense(string s)
        {          
            
            List<int> otherExpenseList = new List<int>();
            int otherExpense;

            foreach (var item in WriteAndReadClass.SmsArray)
            {
                if (item.Contains(s))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") || item.Contains("khitel Központ Zrt.") || item.Contains("SHELL")
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

            return otherExpense;            
        }


        private int LoadActualCredit(string s)
        {
            List<int> monthlyCreditList = new List<int>();
            int credit;

            foreach (var item in WriteAndReadClass.SmsArray)
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

            return credit;

        }

        private int LoadActualFueling(string s)
        {

            List<int> monthlyFuelingList = new List<int>();
            int fueling;

            List<int> monthlyFuelingListAuchan = new List<int>();
            List<int> monthlyFuelingListAuchanStorno = new List<int>();
            int monthlyFuelingAuchan;
            int monthlyFuelingAuchanStorno;
            int realTankolasAuchan;

            foreach (var item in WriteAndReadClass.SmsArray)
            {
                if (item.Contains(s))
                {

                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") && !item.Contains("AUCHAN AUTBEN"))
                    {

                    }
                    else if (item.Contains("SHELL") || item.Contains("MOL TÖLTÖ") ||
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

            return fueling;
        }

        /// <summary>
        /// Set Balance label with SmsArray
        /// </summary>
        private int LoadActualBalance()
        {
            List<int> actualBalanceList = new List<int>();

            foreach (var item in WriteAndReadClass.SmsArray)
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
        

        ///TODO Parallel async
        //DATABASE        
        /// <summary>
        /// Yearly income from database for set income label 
        /// </summary>
        /// <param name="y">searched year</param>
        private async Task<int> YearlyIncomeFromDatabAsync(int y)
        {
            List<int> ListYearIncome = new List<int>();
            int intYearlyIncome;

            var varYearlyIncome = await Task.Run(() => from c in moneyDb where (c.Year == y && c.Type == "Income") select c.Money);

            foreach (var item in varYearlyIncome)
            {
                ListYearIncome.Add(item);
            }

            intYearlyIncome = ListYearIncome.Sum();

            return intYearlyIncome;
        }

        /// <summary>
        /// Yearly expense from database for set expense label
        /// </summary>
        /// <param name="y">searched year</param>
        private async Task<int> YearlyExpenseFromDatabAsync(int y)
        {
            List<int> ListYearExpense = new List<int>();
            int intYearlyExpense;

            var varYearlyExpense = await Task.Run(() => from c in moneyDb where (c.Year == y && c.Type == "Expense") select c.Money);

            foreach (var item in varYearlyExpense)
            {
                ListYearExpense.Add(item);
            }

            intYearlyExpense = ListYearExpense.Sum();

            return intYearlyExpense;
        }

        /// <summary>
        /// Yearly credit from database for set credit label
        /// </summary>
        /// <param name="y">searched year</param>
        private async Task<int> YearlyCreditFromDatabAsync(int y)
        {
            List<int> ListYearCredit = new List<int>();
            int intYearlyCredit;

            var varYearlyCredit = await Task.Run(() => from c in moneyDb where (c.Year == y && c.Type == "Credit") select c.Money);

            foreach (var item in varYearlyCredit)
            {
                ListYearCredit.Add(item);
            }

            intYearlyCredit = ListYearCredit.Sum();

            return intYearlyCredit;
        }

        /// <summary>
        /// Yearly fueling from database for set fueling label
        /// </summary>
        /// <param name="y">searched year</param>
        private async Task<int> YearlyFuelingFromDatabAsync(int y)
        {
            List<int> ListYearFueling = new List<int>();
            int intYearlyFueling;

            var varYearlyFueling = await Task.Run(() => from c in moneyDb where (c.Year == y && c.Type == "Fueling") select c.Money);

            foreach (var item in varYearlyFueling)
            {
                ListYearFueling.Add(item);
            }

            intYearlyFueling = ListYearFueling.Sum();

            return intYearlyFueling;
        }

        /// <summary>
        /// Monthly income from database for set income label
        /// </summary>
        /// <param name="y">searched year</param>
        /// <param name="m">searched month</param>
        private async Task<int> MonthlyIncomeFromDatabAsync(int y, string m)
        {
            List<int> monthlyIncomeList = new List<int>();
            int intMonthlyIncome;            
          
            var varMonthlyIncome = await Task.Run(() => from c in moneyDb where (c.Year==y && c.Month == m && c.Type == "Income") select c.Money);

            foreach (var item in varMonthlyIncome)
            {
                monthlyIncomeList.Add(item);
            }

            intMonthlyIncome = monthlyIncomeList.Sum();

            return intMonthlyIncome;
        }

        /// <summary>
        /// Monthly expense from database for set expense label
        /// </summary>
        /// <param name="y">searched year</param>
        /// <param name="m">searched month</param>
        private async Task<int> MonthlyExpenseFromDatabAsync(int y, string m)
        {
            List<int> monthlyExpenseList = new List<int>();
            int intMonthlyExpense;

            var varMonthlylyExpense = await Task.Run(() => from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Expense") select c.Money);

            foreach (var item in varMonthlylyExpense)
            {
                monthlyExpenseList.Add(item);
            }

            intMonthlyExpense = monthlyExpenseList.Sum();

            return intMonthlyExpense;
        }

        /// <summary>
        /// Monthly credit from database for set credit label
        /// </summary>
        /// <param name="y">searched year</param>
        /// <param name="m">searched month</param>
        private async Task<int> MonthlyCreditFromDatabAsync(int y, string m)
        {
            List<int> monthlyCreditList = new List<int>();
            int intMonthlyCredit;
            
            var varMonthlylyCredit = await Task.Run(() => from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Credit") select c.Money);

            foreach (var item in varMonthlylyCredit)
            {
                monthlyCreditList.Add(item);
            }

            intMonthlyCredit = monthlyCreditList.Sum();

            return intMonthlyCredit;
        }

        /// <summary>
        /// Monthly fueling from database for set fueling label
        /// </summary>
        /// <param name="y">searched year</param>
        /// <param name="m">searched month</param>
        private async Task<int> MonthlyFuelingFromDatabAsync(int y, string m)
        {
            List<int> monthlyFuelingList = new List<int>();
            int intMonthlyFueling;

            var varMonthlyFueling = await Task.Run(() => from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Fueling") select c.Money);

            foreach (var item in varMonthlyFueling)
            {
               monthlyFuelingList.Add(item);
            }

            intMonthlyFueling = monthlyFuelingList.Sum();

            return intMonthlyFueling;
        }

        



        //NAVIGATiON
        /// <summary>
        /// Set Month label to actual Year
        /// Yearly(Income&Expense&Credit&Fueling)FromDatab
        /// LoadSum
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Year_Click(object sender, EventArgs e)
        {
            labelMonth.Text = DateHelper.Year.ToString();
            int income = await YearlyIncomeFromDatabAsync(DateHelper.Year);
            labelIncome.Text = income.ToString("C0");
            int expense = await YearlyExpenseFromDatabAsync(DateHelper.Year);
            labelExpense.Text = expense.ToString("C0");
            int credit = await YearlyCreditFromDatabAsync(DateHelper.Year);
            labelCredit.Text = credit.ToString("C0");
            int fueling = await YearlyFuelingFromDatabAsync(DateHelper.Year);
            labelFueling.Text = fueling.ToString("C0");

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
            labelMonth.Text = DateHelper.Month;
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
        private async void Prev_Click(object sender, EventArgs e)
        {

            if (int.TryParse(labelMonth.Text, out int year))
            {
                int searchYear = year-1;
                int income = await YearlyIncomeFromDatabAsync(searchYear);
                labelIncome.Text = income.ToString("C0");
                int expense = await YearlyExpenseFromDatabAsync(searchYear);
                labelExpense.Text = expense.ToString("C0");
                int credit = await YearlyCreditFromDatabAsync(searchYear);
                labelCredit.Text = credit.ToString("C0");
                int fueling = await YearlyFuelingFromDatabAsync(searchYear);
                labelFueling.Text = fueling.ToString("C0");

                LoadSum();
                labelMonth.Text = searchYear.ToString();
            }
            string month;
            switch(labelMonth.Text)
            {                
                case "February":
                    labelMonth.Text = "January";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "January";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "March":
                    labelMonth.Text = "February";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "February";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "April":
                    labelMonth.Text = "March";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "March";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "May":
                    labelMonth.Text = "April";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "April";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "June":
                    labelMonth.Text = "May";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "May";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "July":
                    labelMonth.Text = "June";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "June";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "August":
                    labelMonth.Text = "July";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "July";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "September":
                    labelMonth.Text = "August";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "August";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "October":
                    labelMonth.Text = "September";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "September";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "November":
                    labelMonth.Text = "October";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "October";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "December":
                    labelMonth.Text = "November";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "November";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
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
        private async void Next_Click(object sender, EventArgs e)
        {

            if (int.TryParse(labelMonth.Text, out int year))
            {
                int searchYear = year + 1;
                int income = await YearlyIncomeFromDatabAsync(searchYear);
                labelIncome.Text = income.ToString("C0");
                int expense = await YearlyExpenseFromDatabAsync(searchYear);
                labelExpense.Text = expense.ToString("C0");
                int credit = await YearlyCreditFromDatabAsync(searchYear);
                labelCredit.Text = credit.ToString("C0");
                int fueling = await YearlyFuelingFromDatabAsync(searchYear);
                labelFueling.Text = fueling.ToString("C0");

                LoadSum();
                labelMonth.Text = searchYear.ToString();
            }

            string month;

            switch (labelMonth.Text)
            {
                case "January":
                    labelMonth.Text = "February";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "February";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "February":
                    labelMonth.Text = "March";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "March";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "March":
                    labelMonth.Text = "April";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "April";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "April":
                    labelMonth.Text = "May";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "May";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "May":
                    labelMonth.Text = "June";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "June";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "June":
                    labelMonth.Text = "July";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "July";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "July":
                    labelMonth.Text = "August";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "August";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "August":
                    labelMonth.Text = "September";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "September";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "September":
                    labelMonth.Text = "October";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "October";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "October":
                    labelMonth.Text = "November";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "November";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
                        LoadSum();

                    }
                    break;
                case "November":
                    labelMonth.Text = "December";
                    if (labelMonth.Text == DateHelper.Month)
                    {
                        LoadMonthData();
                        LoadSum();
                    }
                    else
                    {
                        month = "December";
                        int income = await MonthlyIncomeFromDatabAsync(DateHelper.Year, month);
                        labelIncome.Text = income.ToString("C0");
                        int expense = await MonthlyExpenseFromDatabAsync(DateHelper.Year, month);
                        labelExpense.Text = expense.ToString("C0");
                        int credit = await MonthlyCreditFromDatabAsync(DateHelper.Year, month);
                        labelCredit.Text = credit.ToString("C0");
                        int fueling = await MonthlyFuelingFromDatabAsync(DateHelper.Year, month);
                        labelFueling.Text = fueling.ToString("C0");
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
            if (File.Exists(DateHelper.SavingTimeFilePath))
            {
                dateHelper.SavedMonthList = File.ReadAllLines(DateHelper.SavingTimeFilePath).ToList();
            }
        }

        /// <summary>
        /// ReadSavingTime
        /// if not save in this month LoadSavingData
        /// </summary>
        private void Save()
        {
            ReadSavingTime();      
            
            foreach (var item in dateHelper.SavedMonthList)
            {
                if (item == DateHelper.Month)
                {
                    WriteAndReadClass.SavedToDatabase = true;                    
                }
            }

            if (WriteAndReadClass.SavedToDatabase == false)
            {
                var task = Task.Run((Func<Task>)DropboxClass.Download);
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

            switch (DateHelper.Month)
            {
                case "January":
                    searchString = ".12.";
                    SaveToDatabase(DateHelper.Year-1, searchString, "December");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "February":
                    searchString = ".01.";
                    SaveToDatabase(DateHelper.Year, searchString, "January");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "March":
                    searchString = ".02.";
                    SaveToDatabase(DateHelper.Year, searchString, "February");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "April":
                    searchString = ".03.";
                    SaveToDatabase(DateHelper.Year, searchString, "March");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "May":
                    searchString = ".04.";
                    SaveToDatabase(DateHelper.Year, searchString, "April");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "June":
                    searchString = ".05.";
                    SaveToDatabase(DateHelper.Year, searchString, "May");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "July":
                    searchString = ".06.";
                    SaveToDatabase(DateHelper.Year, searchString, "Juny");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "August":
                    searchString = ".07.";
                    SaveToDatabase(DateHelper.Year, searchString, "July");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "September":
                    searchString = ".08.";
                    SaveToDatabase(DateHelper.Year, searchString, "August");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "October":
                    searchString = ".09.";
                    SaveToDatabase(DateHelper.Year, searchString, "September");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "November":
                    searchString = ".10.";
                    SaveToDatabase(DateHelper.Year, searchString, "October");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
                    break;
                case "December":
                    searchString = ".11.";
                    SaveToDatabase(DateHelper.Year, searchString, "November");
                    WriteAndReadClass.WriteToFile(DateHelper.SavingTimeFilePath, DateHelper.Month);
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
            foreach (var item in WriteAndReadClass.SmsArray)
            {
                if (item.Contains(month+year))
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
                        //string haviBevetel3 = SmsFileWorker.GetBetween(item, ":+", ",-HUF; Közl");
                        //string haviBevetelReplaced3 = haviBevetel3.Replace(".", "");
                        //if (int.TryParse(haviBevetelReplaced3, out int intHaviBevetel3))
                        //{
                        //    monthlySaveIncomeList.Add(intHaviBevetel3);
                        //}
                    }
                }
            }

            intSaveIncome = monthlySaveIncomeList.Sum();

            try
            {
                sqlCon.Open();
                SqlCommand sqlCmd = new SqlCommand(Database.SqlComm, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Year", DateHelper.Year);
                sqlCmd.Parameters.AddWithValue("@Month", monthToDatabase);
                sqlCmd.Parameters.AddWithValue("@Money", intSaveIncome);
                sqlCmd.Parameters.AddWithValue("@Type", "Income");
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }
            catch (Exception) { }

            //Credit
            List<int> monthlySaveCreditList = new List<int>();

            foreach (var item in WriteAndReadClass.SmsArray)
            {
                if (item.Contains(month+year))
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
                SqlCommand sqlCmd = new SqlCommand(Database.SqlComm, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Year", DateHelper.Year);
                sqlCmd.Parameters.AddWithValue("@Month", monthToDatabase);
                sqlCmd.Parameters.AddWithValue("@Money", item);
                sqlCmd.Parameters.AddWithValue("@Type", "Credit");
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
            }

            //Expense
            List<int> monthlySaveExpenseList = new List<int>();

            foreach (var item in WriteAndReadClass.SmsArray)
            {
                if (item.Contains(month + year))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") || item.Contains("khitel Központ Zrt.") || item.Contains("SHELL")
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
                SqlCommand sqlCmd = new SqlCommand(Database.SqlComm, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Year", DateHelper.Year);
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

            foreach (var item in WriteAndReadClass.SmsArray)
            {
                if (item.Contains(month+year))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") && !item.Contains("AUCHAN AUTBEN"))
                    {

                    }
               else if (item.Contains("SHELL") || item.Contains("MOL TÖLTÖ") ||
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
                SqlCommand sqlCmd = new SqlCommand(Database.SqlComm, sqlCon);
                sqlCmd.Parameters.AddWithValue("@Year", DateHelper.Year);
                sqlCmd.Parameters.AddWithValue("@Month", monthToDatabase);
                sqlCmd.Parameters.AddWithValue("@Money", item);
                sqlCmd.Parameters.AddWithValue("@Type", "Fueling");
                sqlCmd.ExecuteNonQuery();
                sqlCon.Close();
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
            var task = Task.Run((Func<Task>)DropboxClass.Download);
            task.Wait();
            DownloadFinishLabel.Visible = true;
            WriteAndReadClass.LoadDatasFromFile();
            DropboxClass.MoveFileFromMainFolder();

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
