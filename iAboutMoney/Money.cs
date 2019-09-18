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
        //TODO: Database throw away from main folder
        //TODO: Screen resolution
        DropboxClass dropbox = new DropboxClass();
        MoneyHelper moneyHelper2 = new MoneyHelper();

        

        SqlCommand cmd;
        SqlConnection con;
        SqlDataAdapter da = new SqlDataAdapter();

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            SetDate();

            LoadDatasFromFile();
            LoadMonthDatas();
            LoadSum();
            WriteSavedMonthToFile();
        }

        
        
        public void LoadDatasFromFile()
        {
            DirectoryInfo d = new DirectoryInfo(@"C:\Users\totha\Source\Repos\iAboutMoney\iAboutMoney\bin\Debug");
            FileInfo[] Files = d.GetFiles("*.xml");
            foreach (var item in Files)
            {
                if (item.Name.Contains("sms-"))
                {                    
                    File.Move(item.Name, @"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Dropbox\"+item.Name);
                }
            }
            
            var files = Directory.GetFiles(@"C:\Users\totha\Source\Repos\LibraryiAboutMoney\Dropbox", "*.xml");
            foreach (var item in files)
            {
                if (item.Contains("sms-"))
                {
                    moneyHelper2.FilePath = item;
                }
            }
            
            string information = File.ReadAllText(moneyHelper2.FilePath);
            moneyHelper2.SmsArray = Regex.Split(information, @"<sms protocol=");
        }
        


        /// <summary>
        /// Setup date and month label
        /// </summary>
        private void SetDate()
        {
            var dateAndTime = DateTime.Now;
            moneyHelper2.Year = dateAndTime.Year;

            string tempMonth = dateAndTime.Month.ToString();
            switch(tempMonth)
            {
                case "1":
                    moneyHelper2.Month = "January";
                    break;
                case "2":
                    moneyHelper2.Month = "February";
                    break;
                case "3":
                    moneyHelper2.Month = "March";
                    break;
                case "4":
                    moneyHelper2.Month = "April";
                    break;
                case "5":
                    moneyHelper2.Month = "May";
                    break;
                case "6":
                    moneyHelper2.Month = "June";
                    break;
                case "7":
                    moneyHelper2.Month = "July";
                    break;
                case "8":
                    moneyHelper2.Month = "August";
                    break;
                case "9":
                    moneyHelper2.Month = "September";
                    break;
                case "10":
                    moneyHelper2.Month = "October";
                    break;
                case "11":
                    moneyHelper2.Month = "November";
                    break;
                case "12":
                    moneyHelper2.Month = "December";
                    break;
            }
            labelMonth.Text = moneyHelper2.Month.ToString();
        } 
        


        /// <summary>
        /// Actual month
        /// </summary>
        private void LoadMonthDatas()
        {
            string searchString;
            LoadActualBalance();
            switch (moneyHelper2.Month)
            {
                case "January":
                    searchString = moneyHelper2.Year + ". jan. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "February":
                    searchString = moneyHelper2.Year + ". febr. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "March":
                    searchString = moneyHelper2.Year + ". márc. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "April":
                    searchString = moneyHelper2.Year + ". ápr. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "May":
                    searchString = moneyHelper2.Year + ". máj. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "June":
                    searchString = moneyHelper2.Year + ". jún. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "July":
                    searchString = moneyHelper2.Year + ". júl. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "August":
                    searchString = moneyHelper2.Year + ". aug. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "September":
                    searchString = moneyHelper2.Year + ". szept. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "October":
                    searchString = moneyHelper2.Year + ". okt. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "November":
                    searchString = moneyHelper2.Year + ". nov. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
                case "December":
                    searchString = moneyHelper2.Year + ". dec. ";
                    LoadIncome(searchString);
                    LoadExpenses(searchString);
                    break;
            }
        }        
        private void LoadIncome(string s)
        {
            List<int> monthlyIncomeList = new List<int>();
            int income;
           
            foreach (var item in moneyHelper2.SmsArray)
            {
                if (item.Contains(s))
                {
                    
                    
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO"))
                    {

                    }
                    else
                    {                        
                        string haviBevetel = moneyHelper2.GetBetween(item, "S:+", ",-HUF;");
                        string haviBevetelReplaced = haviBevetel.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced, out int intHaviBevetel))
                        {
                            monthlyIncomeList.Add(intHaviBevetel);
                        }
                        string haviBevetel2 = moneyHelper2.GetBetween(item, "t: +", " HUF;");
                        string haviBevetelReplaced2 = haviBevetel2.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced2, out int intHaviBevetel2))
                        {
                            monthlyIncomeList.Add(intHaviBevetel2);
                        }
                        string haviBevetel3 = moneyHelper2.GetBetween(item, ":+", ",-HUF; Közl");
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
        private void LoadExpenses(string s)
        {
            int credit;
            int otherExpense;
            int fueling;
            
            //CREDIT
            List<int> monthlyCreditList = new List<int>();

            foreach (var item in moneyHelper2.SmsArray)
            {
                if (item.Contains(s))
                {
                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO"))
                    {

                    }
                    else if (item.Contains("TÖRLESZTÉS") || item.Contains("khitel Központ Zrt."))
                    {                        
                        string haviTorlesztes = moneyHelper2.GetBetween(item, "S:-", ",-HUF");
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

            foreach (var item in moneyHelper2.SmsArray)
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
                            string haviEgyebKiadas = moneyHelper2.GetBetween(item, ": -", " HUF;");
                             string haviEgyebKiadasReplaced = haviEgyebKiadas.Replace(".", "");
                            if (int.TryParse(haviEgyebKiadasReplaced, out int intHaviEgyebKiadas))
                            {
                                otherExpenseList.Add(intHaviEgyebKiadas);
                            }
                            string haviEgyebKiadas2 = moneyHelper2.GetBetween(item, ":-", ",-HUF;");
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
            List<int> monthlyFuelingListAuchan = new List<int>();
            List<int> monthlyFuelingListAuchanStorno = new List<int>();
            int monthlyFuelingAuchan;
            int monthlyFuelingAuchanStorno;
            int realTankolasAuchanAugust;

            foreach (var item in moneyHelper2.SmsArray)
            {
                if (item.Contains(s))
                {

                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") && !item.Contains("AUCHAN AUTBEN"))
                    {

                    }
                    else if (item.Contains("SHELL TÖLTÖ") || item.Contains("MOL TÖLTÖ") ||
                        item.Contains("GP T") || item.Contains("; OMV"))
                    {                        
                         string haviTankolas = moneyHelper2.GetBetween(item, ": -", " HUF;");
                         string haviTankolasReplaced = haviTankolas.Replace(".", "");
                         if (int.TryParse(haviTankolasReplaced, out int intHaviTankolas))
                         {
                            monthlyFuelingList.Add(intHaviTankolas);
                         }
                         string haviTankolas2 = moneyHelper2.GetBetween(item, ":-", ",-HUF;");
                         string haviTankolasReplaced2 = haviTankolas2.Replace(".", "");
                         if (int.TryParse(haviTankolasReplaced2, out int intHaviTankolas2))
                         {
                            monthlyFuelingList.Add(intHaviTankolas2);
                         }
                    }
                    //Fueling Auchan
                    else if (item.Contains("AUCHAN AUTBEN") && !item.Contains("STORNO"))
                    {
                        string haviTankolasAuchan = moneyHelper2.GetBetween(item, ": -", " HUF;");
                        string haviTankolasAuchanReplaced = haviTankolasAuchan.Replace(".", "");
                        if (int.TryParse(haviTankolasAuchanReplaced, out int intHaviTankolasAuchan))
                        {
                            monthlyFuelingListAuchan.Add(intHaviTankolasAuchan);
                        }
                        string haviTankolas2 = moneyHelper2.GetBetween(item, ":-", ",-HUF;");
                        string haviTankolasReplaced2 = haviTankolas2.Replace(".", "");
                        if (int.TryParse(haviTankolasReplaced2, out int intHaviTankolas2))
                        {
                           monthlyFuelingListAuchan.Add(intHaviTankolas2);
                        }
                    }
                    else if (item.Contains("AUCHAN AUTBEN") && item.Contains("STORNO"))
                    {
                        string haviTankolasStorno = moneyHelper2.GetBetween(item, "O: +", " HUF;");
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
            realTankolasAuchanAugust = monthlyFuelingAuchan - monthlyFuelingAuchanStorno;
            monthlyFuelingList.Add(realTankolasAuchanAugust);

            fueling = monthlyFuelingList.Sum();
            labelFueling.Text = fueling.ToString("C0");
        }
       

        private void LoadActualBalance()
        {
            List<int> actualBalanceList = new List<int>();

            foreach (var item in moneyHelper2.SmsArray)
            {
                string dataEgy = moneyHelper2.GetBetween(item, "Egy:+", ",-HUF");
                string dataEgy2 = dataEgy.Replace(".", "");
                if (int.TryParse(dataEgy2, out int egyenlegEgy))
                {
                    actualBalanceList.Add(egyenlegEgy);
                }

                string dataEgyenleg = moneyHelper2.GetBetween(item, "Egyenleg: +", " HUF");
                string dataEgyenleg2 = dataEgyenleg.Replace(".", "");
                if (int.TryParse(dataEgyenleg2, out int egyenlegEgyenleg))
                {
                    actualBalanceList.Add(egyenlegEgyenleg);
                }
            }
            int intActualBalance = actualBalanceList.Last();
            labelBalanceActual.Text = intActualBalance.ToString("C0");
        }

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
        
        //Yearly data from database
        private void YearlyIncomeFromDatab(int y)
        {
            List<int> ListYearIncome = new List<int>();
            int intYearlyIncome;

            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True";
            DataContext da = new DataContext(connString);
            Table<MoneyInfoTable> moneyDb = da.GetTable<MoneyInfoTable>();
            var varYearlyIncome = from c in moneyDb where (c.Year == y && c.Type == "Income") select c.Money;
            foreach (var item in varYearlyIncome)
            {

                ListYearIncome.Add(item);

            }
            intYearlyIncome = ListYearIncome.Sum();
            labelIncome.Text = intYearlyIncome.ToString("C0");
        }
        private void YearlyExpenseFromDatab(int y)
        {
            List<int> ListYearExpense = new List<int>();
            int intYearlyExpense;

            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True";
            DataContext da = new DataContext(connString);
            Table<MoneyInfoTable> moneyDb = da.GetTable<MoneyInfoTable>();
            var varYearlyExpense = from c in moneyDb where (c.Year == y && c.Type == "Expense") select c.Money;
            foreach (var item in varYearlyExpense)
            {

                ListYearExpense.Add(item);

            }
            intYearlyExpense = ListYearExpense.Sum();
            labelExpense.Text = intYearlyExpense.ToString("C0");
        }
        private void YearlyCreditFromDatab(int y)
        {
            List<int> ListYearCredit = new List<int>();
            int intYearlyCredit;

            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True";
            DataContext da = new DataContext(connString);
            Table<MoneyInfoTable> moneyDb = da.GetTable<MoneyInfoTable>();
            var varYearlyCredit = from c in moneyDb where (c.Year == y && c.Type == "Credit") select c.Money;
            foreach (var item in varYearlyCredit)
            {

                ListYearCredit.Add(item);

            }
            intYearlyCredit = ListYearCredit.Sum();
            labelCredit.Text = intYearlyCredit.ToString("C0");
        }
        private void YearlyFuelingFromDatab(int y)
        {
            List<int> ListYearFueling = new List<int>();
            int intYearlyFueling;

            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True";
            DataContext da = new DataContext(connString);
            Table<MoneyInfoTable> moneyDb = da.GetTable<MoneyInfoTable>();
            var varYearlyFueling = from c in moneyDb where (c.Year == y && c.Type == "Fueling") select c.Money;
            foreach (var item in varYearlyFueling)
            {

                ListYearFueling.Add(item);

            }
            intYearlyFueling = ListYearFueling.Sum();
            labelFueling.Text = intYearlyFueling.ToString("C0");
        }

        //Monthly data from Database
        private void MonthlyIncomeFromDatab(int y, string m)
        {
            List<int> monthlyIncomeList = new List<int>();
            int intMonthlyIncome;
            
            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True";
            DataContext da = new DataContext(connString);
            Table<MoneyInfoTable> moneyDb = da.GetTable<MoneyInfoTable>();
            var varMonthlyIncome = from c in moneyDb where (c.Year==y && c.Month == m && c.Type == "Income") select c.Money;
            foreach (var item in varMonthlyIncome)
            {

                monthlyIncomeList.Add(item);

            }
            intMonthlyIncome = monthlyIncomeList.Sum();
            labelIncome.Text = intMonthlyIncome.ToString("C0");
        }
        private void MonthlyExpenseFromDatab(int y, string m)
        {
            List<int> monthlyExpenseList = new List<int>();
            int intMonthlyExpense;

            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True";
            DataContext da = new DataContext(connString);
            Table<MoneyInfoTable> moneyDb = da.GetTable<MoneyInfoTable>();
            var varMonthlylyExpense = from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Expense") select c.Money;
            foreach (var item in varMonthlylyExpense)
            {

                monthlyExpenseList.Add(item);

            }
            intMonthlyExpense = monthlyExpenseList.Sum();
            labelExpense.Text = intMonthlyExpense.ToString("C0");
        }
        private void MonthlyCreditFromDatab(int y, string m)
        {
            List<int> monthlyCreditList = new List<int>();
            int intMonthlyCredit;

            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True";
            DataContext da = new DataContext(connString);
            Table<MoneyInfoTable> moneyDb = da.GetTable<MoneyInfoTable>();
            var varMonthlylyCredit = from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Credit") select c.Money;
            foreach (var item in varMonthlylyCredit)
            {

                monthlyCreditList.Add(item);

            }
            intMonthlyCredit = monthlyCreditList.Sum();
            labelCredit.Text = intMonthlyCredit.ToString("C0");
        }
        private void MonthlyFuelingFromDatab(int y, string m)
        {
            List<int> monthlyFuelingList = new List<int>();
            int intMonthlyFueling;

            string connString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True";
            DataContext da = new DataContext(connString);
            Table<MoneyInfoTable> moneyDb = da.GetTable<MoneyInfoTable>();
            var varMonthlyFueling = from c in moneyDb where (c.Year == y && c.Month == m && c.Type == "Fueling") select c.Money;
            foreach (var item in varMonthlyFueling)
            {

               monthlyFuelingList.Add(item);

            }
            intMonthlyFueling = monthlyFuelingList.Sum();
            labelFueling.Text = intMonthlyFueling.ToString("C0");
        }

        


        //NAVIGATE
        private void Year_Click(object sender, EventArgs e)
        {
            labelMonth.Text = moneyHelper2.Year.ToString();

            YearlyIncomeFromDatab(moneyHelper2.Year);
            YearlyExpenseFromDatab(moneyHelper2.Year);
            YearlyCreditFromDatab(moneyHelper2.Year);
            YearlyFuelingFromDatab(moneyHelper2.Year);

            LoadSum();
        }
        private void Month_Click(object sender, EventArgs e)
        {
            SetDate();

            LoadDatasFromFile();
            LoadMonthDatas();
            LoadSum();            
        }


        private void Prev_Click(object sender, EventArgs e)
        {
            var dateAndTime = DateTime.Now;
            int yearActual = dateAndTime.Year;

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
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "January";
                        MonthlyIncomeFromDatab(yearActual,month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "March":
                    labelMonth.Text = "February";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "February";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "April":
                    labelMonth.Text = "March";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "March";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "May":
                    labelMonth.Text = "April";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "April";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "June":
                    labelMonth.Text = "May";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "May";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "July":
                    labelMonth.Text = "June";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "June";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "August":
                    labelMonth.Text = "July";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "July";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "September":
                    labelMonth.Text = "August";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "August";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "October":
                    labelMonth.Text = "September";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "September";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "November":
                    labelMonth.Text = "October";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "October";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "December":
                    labelMonth.Text = "November";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "November";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
            }
        }
        private void Next_Click(object sender, EventArgs e)
        {
            var dateAndTime = DateTime.Now;
            int yearActual = dateAndTime.Year;

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
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "February";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "February":
                    labelMonth.Text = "March";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "March";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "March":
                    labelMonth.Text = "April";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "April";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "April":
                    labelMonth.Text = "May";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "May";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "May":
                    labelMonth.Text = "June";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "June";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "June":
                    labelMonth.Text = "July";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "July";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "July":
                    labelMonth.Text = "August";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "August";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "August":
                    labelMonth.Text = "September";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "September";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "September":
                    labelMonth.Text = "October";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "October";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "October":
                    labelMonth.Text = "November";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "November";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
                case "November":
                    labelMonth.Text = "December";
                    if (labelMonth.Text == moneyHelper2.Month)
                    {
                        LoadDatasFromFile();
                        LoadMonthDatas();
                        LoadSum();
                    }
                    else
                    {
                        month = "December";
                        MonthlyIncomeFromDatab(yearActual, month);
                        MonthlyExpenseFromDatab(yearActual, month);
                        MonthlyCreditFromDatab(yearActual, month);
                        MonthlyFuelingFromDatab(yearActual, month);
                        LoadSum();

                    }
                    break;
            }
        }

        //TODO: Repair saving to database function
        //SAVING TO DATABASE           
        private void Save()
        {
            ReadSavedMonthFile();
            
            string searchedMonth;
            switch (moneyHelper2.Month)
            {
                case "January":
                    searchedMonth = "December";
                    break;
                case "February":
                    searchedMonth = "January";
                    break;
                case "March":
                    searchedMonth = "February";
                    break;
                case "April":
                    searchedMonth = "March";
                    break;
                case "May":
                    searchedMonth = "April";
                    break;
                case "June":
                    searchedMonth = "May";
                    break;
                case "July":
                    searchedMonth = "June";
                    break;
                case "August":
                    searchedMonth = "July";
                    break;
                case "September":
                    searchedMonth = "August";
                    break;
                case "October":
                    searchedMonth = "September";
                    break;
                case "November":
                    searchedMonth = "October";
                    break;
                case "December":
                    searchedMonth = "November";
                    break;
            }
            foreach (var item in moneyHelper2.SavedMonthList)
            {
                if (item == "August")
                {
                    moneyHelper2.Saved = true;
                }
            }
            if (moneyHelper2.Saved == false)
            {
                LoadSavingDatas();
            }
        }
        private void ReadSavedMonthFile()
        {
            if (File.Exists(moneyHelper2.SavedMonthFilePath))
            {
                moneyHelper2.SavedMonthList = File.ReadAllLines(moneyHelper2.SavedMonthFilePath).ToList();
            }
        }
        private void LoadSavingDatas()
        {
            string searchString;
            switch (moneyHelper2.Month)
            {
                case "January":
                    searchString = "2019. dec. ";
                    SaveToDatabase(searchString, "December");
                    WriteSavedMonthToFile();
                    break;
                case "February":
                    searchString = "2019. jan. ";
                    SaveToDatabase(searchString, "January");
                    WriteSavedMonthToFile();
                    break;
                case "March":
                    searchString = "2019. febr. ";
                    SaveToDatabase(searchString, "February");
                    WriteSavedMonthToFile();
                    break;
                case "April":
                    searchString = "2019. márc. ";
                    SaveToDatabase(searchString, "March");
                    WriteSavedMonthToFile();
                    break;
                case "May":
                    searchString = "2019. ápr. ";
                    SaveToDatabase(searchString, "April");
                    WriteSavedMonthToFile();
                    break;
                case "June":
                    searchString = "2019. máj. ";
                    SaveToDatabase(searchString, "May");
                    WriteSavedMonthToFile();
                    break;
                case "July":
                    searchString = "2019. jún. ";
                    SaveToDatabase(searchString, "Juny");
                    WriteSavedMonthToFile();
                    break;
                case "August":
                    searchString = "2019. júl. ";
                    SaveToDatabase(searchString, "July");
                    WriteSavedMonthToFile();
                    break;
                case "September":
                    searchString = "2019. aug. ";
                    SaveToDatabase(searchString, "August");
                    WriteSavedMonthToFile();
                    break;
                case "October":
                    searchString = "2019. szept. ";
                    SaveToDatabase(searchString, "September");
                    WriteSavedMonthToFile();
                    break;
                case "November":
                    searchString = "2019. okt. ";
                    SaveToDatabase(searchString, "October");
                    WriteSavedMonthToFile();
                    break;
                case "December":
                    searchString = "2019. nov. ";
                    SaveToDatabase(searchString, "November");
                    WriteSavedMonthToFile();
                    break;
            }
        }
        private void SaveToDatabase(string searchMonth, string monthToDatabase)
        {
            List<int> monthlySaveIncomeList = new List<int>();
            int intSaveIncome;

            //Income

            foreach (var item in moneyHelper2.SmsArray)
            {
                if (item.Contains(searchMonth))
                {

                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO"))
                    {

                    }
                    else
                    {
                        //Income
                        string haviBevetel = moneyHelper2.GetBetween(item, "S:+", ",-HUF;");
                        string haviBevetelReplaced = haviBevetel.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced, out int intHaviBevetel))
                        {
                            monthlySaveIncomeList.Add(intHaviBevetel);
                        }
                        string haviBevetel2 = moneyHelper2.GetBetween(item, "t: +", " HUF;");
                        string haviBevetelReplaced2 = haviBevetel2.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced2, out int intHaviBevetel2))
                        {
                            monthlySaveIncomeList.Add(intHaviBevetel2);
                        }
                        string haviBevetel3 = moneyHelper2.GetBetween(item, ":+", ",-HUF; Közl");
                        string haviBevetelReplaced3 = haviBevetel3.Replace(".", "");
                        if (int.TryParse(haviBevetelReplaced3, out int intHaviBevetel3))
                        {
                            monthlySaveIncomeList.Add(intHaviBevetel3);
                        }
                    }

                }
            }
            intSaveIncome = monthlySaveIncomeList.Sum();


            con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True");
            con.Open();
            cmd = new SqlCommand("INSERT INTO MoneyInfoTable (Year, Month, Money, Type) VALUES (@Year, @Month, @Money, @Type)", con);
            cmd.Parameters.AddWithValue("@Year", 2019);
            cmd.Parameters.AddWithValue("@Month", monthToDatabase);
            cmd.Parameters.AddWithValue("@Money", intSaveIncome);
            cmd.Parameters.AddWithValue("@Type", "Income");
            cmd.ExecuteNonQuery();
            con.Close();

            //Credit
            List<int> monthlySaveCreditList = new List<int>();

            foreach (var item in moneyHelper2.SmsArray)
            {
                if (item.Contains(searchMonth))
                {

                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO"))
                    {

                    }
                    else if (item.Contains("TÖRLESZTÉS") || item.Contains("khitel Központ Zrt."))
                    {
                        //Credit
                        string haviTorlesztes = moneyHelper2.GetBetween(item, "S:-", ",-HUF");
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True");
                con.Open();
                cmd = new SqlCommand("INSERT INTO MoneyInfoTable (Year, Month, Money, Type) VALUES (@Year, @Month, @Money, @Type)", con);
                cmd.Parameters.AddWithValue("@Year", 2019);
                cmd.Parameters.AddWithValue("@Month", monthToDatabase);
                cmd.Parameters.AddWithValue("@Money", item);
                cmd.Parameters.AddWithValue("@Type", "Credit");
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //Expense
            List<int> monthlySaveExpenseList = new List<int>();

            foreach (var item in moneyHelper2.SmsArray)
            {
                if (item.Contains(searchMonth))
                {

                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") || item.Contains("khitel Központ Zrt.") || item.Contains("SHELL TÖLTÖ")
                        || item.Contains("MOL TÖLTÖ") || item.Contains("GP T") || item.Contains("AUCHAN AUTBEN") || item.Contains("; OMV"))
                    {

                    }
                    else if (item.Contains("ATM") || item.Contains("s/z") || item.Contains("rty") && item.Contains("s v") || item.Contains("NAPKÖZBENI")
                          || item.Contains("BIZTOSIT") || item.Contains("VAL KAPCS. DIJ") || item.Contains("TUTAL"))
                    {
                        //Expense
                        string haviEgyebKiadas = moneyHelper2.GetBetween(item, ": -", " HUF;");
                        string haviEgyebKiadasReplaced = haviEgyebKiadas.Replace(".", "");
                        if (int.TryParse(haviEgyebKiadasReplaced, out int intHaviEgyebKiadas))
                        {
                            monthlySaveExpenseList.Add(intHaviEgyebKiadas);
                        }
                        string haviEgyebKiadas2 = moneyHelper2.GetBetween(item, ":-", ",-HUF;");
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True");
                con.Open();
                cmd = new SqlCommand("INSERT INTO MoneyInfoTable (Year, Month, Money, Type) VALUES (@Year, @Month, @Money, @Type)", con);
                cmd.Parameters.AddWithValue("@Year", 2019);
                cmd.Parameters.AddWithValue("@Month", monthToDatabase);
                cmd.Parameters.AddWithValue("@Money", item);
                cmd.Parameters.AddWithValue("@Type", "Expense");
                cmd.ExecuteNonQuery();
                con.Close();
            }

            //Fueling
            List<int> monthlySaveFuelingList = new List<int>();
            List<int> monthlySaveFuelingAuchanList = new List<int>();
            List<int> monthlySaveFuelingAuchanStornoList = new List<int>();
            int intSaveFuelingAuchan;
            int intSaveFuelingAuchanStorno;
            int intRealFuelingAuchan;
            foreach (var item in moneyHelper2.SmsArray)
            {
                if (item.Contains(searchMonth))
                {

                    if (item.Contains("SIKERTELEN") || item.Contains("STORNO") && !item.Contains("AUCHAN AUTBEN"))
                    {

                    }
                    else if (item.Contains("SHELL TÖLTÖ") || item.Contains("MOL TÖLTÖ") ||
                        item.Contains("GP T") || item.Contains("; OMV"))
                    {
                        //Tankolas
                        string haviTankolas = moneyHelper2.GetBetween(item, ": -", " HUF;");
                        string haviTankolasReplaced = haviTankolas.Replace(".", "");
                        if (int.TryParse(haviTankolasReplaced, out int intHaviTankolas))
                        {
                            monthlySaveFuelingList.Add(intHaviTankolas);
                        }
                        string haviTankolas2 = moneyHelper2.GetBetween(item, ":-", ",-HUF;");
                        string haviTankolasReplaced2 = haviTankolas2.Replace(".", "");
                        if (int.TryParse(haviTankolasReplaced2, out int intHaviTankolas2))
                        {
                            monthlySaveFuelingList.Add(intHaviTankolas2);
                        }
                    }
                    //Auchan
                    else if (item.Contains("AUCHAN AUTBEN") && !item.Contains("STORNO"))
                    {
                        string haviTankolasAuchan = moneyHelper2.GetBetween(item, ": -", " HUF;");
                        string haviTankolasAuchanReplaced = haviTankolasAuchan.Replace(".", "");
                        if (int.TryParse(haviTankolasAuchanReplaced, out int intHaviTankolasAuchan))
                        {
                            monthlySaveFuelingAuchanList.Add(intHaviTankolasAuchan);
                        }
                        string haviTankolas2 = moneyHelper2.GetBetween(item, ":-", ",-HUF;");
                        string haviTankolasReplaced2 = haviTankolas2.Replace(".", "");
                        if (int.TryParse(haviTankolasReplaced2, out int intHaviTankolas2))
                        {
                            monthlySaveFuelingAuchanList.Add(intHaviTankolas2);
                        }
                    }
                    else if (item.Contains("AUCHAN AUTBEN") && item.Contains("STORNO"))
                    {
                        string haviTankolasStorno = moneyHelper2.GetBetween(item, "O: +", " HUF;");
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
                con = new SqlConnection(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security=True");
                con.Open();
                cmd = new SqlCommand("INSERT INTO MoneyInfoTable (Year, Month, Money, Type) VALUES (@Year, @Month, @Money, @Type)", con);
                cmd.Parameters.AddWithValue("@Year", 2019);
                cmd.Parameters.AddWithValue("@Month", monthToDatabase);
                cmd.Parameters.AddWithValue("@Money", item);
                cmd.Parameters.AddWithValue("@Type", "Fueling");
                cmd.ExecuteNonQuery();
                con.Close();
            }


        }
        private void WriteSavedMonthToFile()
        {
            using (FileStream stream = new FileStream(moneyHelper2.SavedMonthFilePath, FileMode.OpenOrCreate, FileAccess.Write, FileShare.None))
            {
                using (StreamWriter writer = new StreamWriter(stream))
                {
                    writer.WriteLine(moneyHelper2.Month);
                }
            }
        }
        


        //DOWNLOAD FILE
        private void DownloadFileTitle_Click(object sender, EventArgs e)
        {
            var task = Task.Run((Func<Task>)DropboxClass.Run);
            task.Wait();
            DownloadFinishLabel.Visible = true;
            LoadDatasFromFile();
            LoadMonthDatas();
            LoadSum();
        }


       
        private void SwitchToRoutineForm_Click(object sender, EventArgs e)
        {
            Hide();            
        }
        private void ExitForm2_Click(object sender, EventArgs e)
        {
            //Save();
            Application.Exit();
        }



    }
}
