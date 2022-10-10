using System.Collections.ObjectModel;

namespace VendingMachine.Model
{
    public class User
    {
        public User() {
            // кошелек пользователя
            _userWallet = new ObservableCollection<MoneyStack> 
                (Banknote.Banknotes.Select(b => new MoneyStack(b, 10)));
            UserWallet = new ReadOnlyObservableCollection<MoneyStack>(_userWallet);
            // продукты пользователя
            UserBuyings = new ReadOnlyObservableCollection<ProductStack>(_userBuyings);
        }
        public int UserSumm { get; set; }
        public ReadOnlyObservableCollection<MoneyStack> UserWallet { get; }
        private readonly ObservableCollection<MoneyStack> _userWallet;

        public ReadOnlyObservableCollection<ProductStack> UserBuyings { get; }
        private readonly ObservableCollection<ProductStack> _userBuyings = new ObservableCollection<ProductStack>();
    }
}