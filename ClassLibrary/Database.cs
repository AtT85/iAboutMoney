using System;
using System.Collections.Generic;
using System.Text;


namespace ClassLibrary
{
    public static class Database
    {
        public static string ConnectionString { get; }= @"Data Source = (LocalDB)\MSSQLLocalDB;AttachDbFilename=
            C:\Users\totha\Source\Repos\LibraryiAboutMoney\Database\MoneyInfoDatabase.mdf;Integrated Security = True";
        public static string SqlComm { get; } = "INSERT INTO MoneyInfoTable (Year, Month, Money, Type) " +
            "VALUES (@Year, @Month, @Money, @Type)";
    }
}
