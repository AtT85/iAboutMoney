using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Linq.Mapping;
using System.Data.Linq;

namespace ClassLibrary
{
    [Table]
    public class MoneyInfoTable
    {
        [Column]
        public int Year;
        [Column]
        public string Month;
        [Column]
        public int Money;
        [Column]
        public string Type;
    }
}
