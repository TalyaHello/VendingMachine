using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine
{
    public class MoneyStack
    {
        public MoneyStack(Banknote banknote, int amount) {
            Banknote = banknote;
            Amount = amount;
        }
        public Banknote Banknote { get; set; }
        public int Amount { get; set; }
    }
}
