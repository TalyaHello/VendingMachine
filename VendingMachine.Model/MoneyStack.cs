using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine
{
    public class MoneyStack : BindableBase
    {
        public MoneyStack(Banknote banknote, int amount) {
            Banknote = banknote;
            Amount = amount;
        }
        public Banknote Banknote { get; set; }
        private int _amount;
        public int Amount
        {
            get { return _amount; }
            set { SetProperty(ref _amount, value); }
        }
        internal bool PullOne()
        {
            if (Amount > 0)
            {
                --Amount;
                return true;
            }
            return false;
        }
        internal void PushOne() => ++Amount;
    }
}
