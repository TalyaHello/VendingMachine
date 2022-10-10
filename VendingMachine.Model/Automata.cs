using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VendingMachine.Model
{
    public class Automata
    {
        public Automata() {
            //деньгохранилище автомата
            _automataBank = new ObservableCollection<MoneyStack>
                (Banknote.Banknotes.Select(b => new MoneyStack(b, 100)));
            AutomataBank = new ReadOnlyObservableCollection<MoneyStack>(_automataBank);
            //продукты автомата
            _productsInAutomata =
                new ObservableCollection<ProductStack>(Product.Products.Select(p => new ProductStack(p, 100)));
            ProductsInAutomata = new ReadOnlyObservableCollection<ProductStack>(_productsInAutomata);
        }
        public ReadOnlyObservableCollection<MoneyStack> AutomataBank { get; }
        private readonly ObservableCollection<MoneyStack> _automataBank;

        public ReadOnlyObservableCollection<ProductStack> ProductsInAutomata { get; }
        private readonly ObservableCollection<ProductStack> _productsInAutomata;
        public int Credit { get; set; }
    }
}
