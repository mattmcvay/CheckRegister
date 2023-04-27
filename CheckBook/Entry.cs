using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckBook
{
    public class Entry
    {
        public int TransactionNum { get; set; }
        public string Type { get; set; }
        public DateTime Date { get; set; }
        public int CheckNum { get; set; }
        public string Description { get; set; }
        public Decimal Withdrawal { get; set; }
        public Decimal Deposit { get; set; }

        public string fullInfo
        {
            get
            {
                return $"{Type} {Date} {CheckNum} {Description} {Withdrawal} {Deposit}";
            }
        }
    }
}
