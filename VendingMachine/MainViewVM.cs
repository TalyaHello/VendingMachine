﻿using Prism.Commands;
using Prism.Mvvm;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using VendingMachine.Model;

namespace VendingMachine
{
    public class MainViewVM : BindableBase {
        public MainViewVM()
        {
            _manager = new PurchaseManager();
            _user = _manager.User;
            _user.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(UserSumm)); };
            //преобразовать коллекцию в конструкторе 
            UserWallet = new ObservableCollection<MoneyVM>(_user.UserWallet.Select(ms => new MoneyVM(ms, _manager)));
            //преобразовывать каждый добавленный или удаленный элемент из модели
            Watch<MoneyStack, MoneyVM>(_user.UserWallet, UserWallet, uw => uw.MoneyStack);

            //покупки пользователя
            UserBuyings = new ObservableCollection<ProductVM>(_user.UserBuyings.Select(ub => new ProductVM(ub)));
            Watch<ProductStack, ProductVM>(_user.UserBuyings, UserBuyings, ub => ub.ProductStack);

            _automata = _manager.Automata;
            _automata.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Credit)); };
            //деньги автомата
            AutomataBank = new ObservableCollection<MoneyVM>(_automata.AutomataBank.Select(a => new MoneyVM(a)));
            Watch<MoneyStack, MoneyVM>(_automata.AutomataBank, AutomataBank, a => a.MoneyStack);
            //товары автомата
            ProductsInAutomata = new ObservableCollection<ProductVM>(_automata.ProductsInAutomata.Select(ap => new ProductVM(ap, _manager)));
            Watch<ProductStack, ProductVM>(_automata.ProductsInAutomata, ProductsInAutomata, p => p.ProductStack);
            GetChange = new DelegateCommand(() => {
                _manager.GetChange();
            });
        }
        private PurchaseManager _manager;
        public int UserSumm => _user.UserSumm;
        public ObservableCollection<MoneyVM> UserWallet { get; }
        public ObservableCollection<ProductVM> UserBuyings { get; }
        public DelegateCommand GetChange { get; }
        public int Credit { get; }
        public ObservableCollection<MoneyVM> AutomataBank { get; }
        public ObservableCollection<ProductVM> ProductsInAutomata { get; }

        private User _user;

        private Automata _automata;
        private static void Watch<T, T2>(ReadOnlyObservableCollection<T> collToWatch, 
                                         ObservableCollection<T2> collToUpdate, 
                                         Func<T2, object> modelProperty)
        {
            ((INotifyCollectionChanged)collToWatch).CollectionChanged += (s, a) => {
                if (a.NewItems?.Count == 1) collToUpdate.Add((T2)Activator.CreateInstance(typeof(T2), (T)a.NewItems[0]));
                if (a.OldItems?.Count == 1) collToUpdate.Remove(collToUpdate.First(mv => modelProperty(mv) == a.OldItems[0]));
            };
        }
    }
    public class ProductVM : BindableBase {
        public ProductVM(ProductStack productStack, PurchaseManager manager = null)
        {
            ProductStack = productStack;
            productStack.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Amount)); };
            if (manager != null)
                BuyCommand = new DelegateCommand(() => {
                    manager.BuyProduct(ProductStack.Product);
                });
        }
        public ProductStack ProductStack { get; }
        public Visibility IsBuyVisible => BuyCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public DelegateCommand BuyCommand { get; }
        public string Name => ProductStack.Product.Name;
        public string Price => $"({ProductStack.Product.Price} rub.)";
        public Visibility IsAmountVisible => BuyCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public int Amount => ProductStack.Amount;
    }
    public class MoneyVM : BindableBase {
        public MoneyVM(MoneyStack moneyStack, PurchaseManager manager = null){
            MoneyStack = moneyStack;
            if (manager != null)
                InsertCommand = new DelegateCommand(() => {
                    manager.InsertMoney(MoneyStack.Banknote);
                });
            moneyStack.PropertyChanged += (s, a) => { RaisePropertyChanged(nameof(Amount)); };
        }
        public MoneyStack MoneyStack { get; }
        public Visibility IsInsertVisible => InsertCommand == null ? Visibility.Collapsed : Visibility.Visible;
        public DelegateCommand InsertCommand { get; }
        public string Name => MoneyStack.Banknote.Name;
        public int Amount => MoneyStack.Amount;
    }
}
