using Prism.Mvvm;
using System.Collections.ObjectModel;

namespace VendingMachine.Model
{
    public class User : BindableBase
    {
        public User() {
            // кошелек пользователя
            _userWallet = new ObservableCollection<MoneyStack> 
                (Banknote.Banknotes.Select(b => new MoneyStack(b, 10)));
            UserWallet = new ReadOnlyObservableCollection<MoneyStack>(_userWallet);
            // продукты пользователя
            UserBuyings = new ReadOnlyObservableCollection<ProductStack>(_userBuyings);
        }

        public ReadOnlyObservableCollection<MoneyStack> UserWallet { get; }
        private readonly ObservableCollection<MoneyStack> _userWallet;

        public ReadOnlyObservableCollection<ProductStack> UserBuyings { get; }
        private readonly ObservableCollection<ProductStack> _userBuyings = new ObservableCollection<ProductStack>();

        //сумма наличности пользователя
        public int UserSumm
        {
            get { return _userWallet.Select(b => b.Banknote.Nominal * b.Amount).Sum(); }
        }

        //если такой MoneyStack в наличии, то попробовать вытащить из него одну купюру/монету
        //вернуть false в случае неудачи
        internal bool GetBanknote(Banknote banknote)
        {
            if (_userWallet.FirstOrDefault(ms => ms.Banknote.Equals(banknote))?.PullOne() ?? false)
            {
                RaisePropertyChanged(nameof(UserSumm)); //обновилась сумма наличности пользователя!
                return true;
            }
            return false;
        }

        internal void AddProduct(Product product) {
            var stack = _userBuyings.FirstOrDefault(b => b.Product == product);
            if (stack == null)
                _userBuyings.Add(new ProductStack(product, 1));
            else
                stack.PushOne();
        }

        internal void AppendMoney(IEnumerable<MoneyStack> change)
        {
            foreach (var ms in change)
                for (int i = 0; i < ms.Amount; ++i)
                    UserWallet.First(m => Equals(m.Banknote.Nominal, ms.Banknote.Nominal)).PushOne();
            RaisePropertyChanged(nameof(UserSumm));
        }
    }
}