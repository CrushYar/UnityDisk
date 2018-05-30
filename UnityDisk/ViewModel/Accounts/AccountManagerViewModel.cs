using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Unity;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.Annotations;
using UnityDisk.ViewModel.Accounts;

namespace UnityDisk.ViewModel.Accounts
{
    public class AccountManagerViewModel : INotifyPropertyChanged
    {
        private string _userName;
        private ulong _totalSize;
        private ulong _usedSize;
        private ulong _freeSize;
        private ulong _count;
        private UnityDisk.Accounts.Registry.IAccountRegistry _accountRegistry;
        private IUnityContainer _container;
        private ICommand _openViewForAddAccountCommand;
        private ICommand _addAccountOneDriveCommand;
        private ICommand _deleteAccountCommand;
        private ObservableCollection<AccountProjectionViewModel> _accountProjections;
        private AccountProjectionViewModel _accountProjectionSelected;
        private Visibility _accountManagerViewVisibility=Visibility.Visible;
        private Visibility _addNewAccountViewVisibility=Visibility.Collapsed;

        public string UserName
        {
            get { return _userName; }
            set
            {
                _userName = value;
                OnPropertyChanged();
            }
        }

        public ulong TotalSize
        {
            get { return _totalSize; }
            set
            {
                _totalSize = value;
                OnPropertyChanged();
            }
        }

        public ulong UsedSize
        {
            get { return _usedSize; }
            set
            {
                _usedSize = value;
                OnPropertyChanged();
            }
        }

        public ulong FreeSize
        {
            get { return _freeSize; }
            set
            {
                _freeSize = value;
                OnPropertyChanged();
            }
        }

        public ulong Count
        {
            get { return _count; }
            set
            {
                _count = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<AccountProjectionViewModel> AccountProjections
        {
            get { return _accountProjections; }
            set
            {
                AccountProjections = value;
                OnPropertyChanged();
            }
        }
        public AccountProjectionViewModel AccountProjectionSelected
        {
            get { return _accountProjectionSelected; }
            set
            {
                _accountProjectionSelected = value;
                if (_accountProjectionSelected?.ServerName == "+")
                {
                    AccountManagerViewVisibility = Visibility.Collapsed;
                    AddNewAccountViewVisibility = Visibility.Visible;
                }
            }
        }
        public Visibility AccountManagerViewVisibility
        {
            get { return _accountManagerViewVisibility; }
            set
            {
                _accountManagerViewVisibility = value;
                OnPropertyChanged();
            }
        }
        public Visibility AddNewAccountViewVisibility
        {
            get { return _addNewAccountViewVisibility; }
            set
            {
                _addNewAccountViewVisibility = value;
                OnPropertyChanged();
            }
        }

        public ICommand OpenViewForAddAccountCommand => _openViewForAddAccountCommand;
        public ICommand AddAccountOneDriveCommand => _addAccountOneDriveCommand;
        public ICommand DeleteAccountCommand => _deleteAccountCommand;

        public AccountManagerViewModel()
        {
            _accountProjections =
                new ObservableCollection<AccountProjectionViewModel>()
                {
                    new AccountProjectionViewModel()
                    {
                        Login = "+",
                        ServerName = "+"
                    }
                };

            _container = ContainerConfiguration.GetContainer().Container;
            _accountRegistry = _container.Resolve<UnityDisk.Accounts.Registry.IAccountRegistry>();

            _accountRegistry.ChangedSizeEvent += (o, e) =>
            {
                TotalSize = e.NewSize.TotalSize;
                UsedSize = e.NewSize.TotalSize;
                FreeSize = e.NewSize.FreeSize;

                AccountProjectionViewModel projection =
                    AccountProjections.FirstOrDefault(accountProjection => accountProjection.Login == e.Account.Login);
                if (projection == null) return;
                projection.TotalSize = e.Account.Size.TotalSize;
                projection.UsedSize = e.Account.Size.UsedSize;
                projection.FreeSize = e.Account.Size.FreeSize;
            };
            _accountRegistry.LoadedEvent += (o, e) =>
            {
                foreach (var account in e.Accounts)
                {
                    AccountProjections.Add(new AccountProjectionViewModel(account));
                }
            };
            _accountRegistry.ChangedRegistryEvent += (o, e) =>
            {
                switch (e.Action)
                {
                    case RegistryActionEnum.Added:
                        AccountProjections.Add(new AccountProjectionViewModel(e.Account));
                        break;
                    case RegistryActionEnum.Removed:
                        AccountProjections.Remove(AccountProjections.First(model => model.Login == e.Account.Login));
                        break;
                    case RegistryActionEnum.Reseted:
                        AccountProjections.Clear();
                        break;
                }
            };
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
