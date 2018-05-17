using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityDisk.Accounts;
using UnityDisk.Annotations;

namespace UnityDisk.ViewModel
{
    public class AccountManagerViewModel:INotifyPropertyChanged
    {
        private string _userName;
        private ulong _totalSize;
        private ulong _usedSize;
        private ulong _freeSize;
        private ulong _count;
        private Accounts.Registry.IAccountRegistry _accountRegistry;
        private IUnityContainer _container;

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

        public AccountManagerViewModel()
        {
            _container = ContainerConfiguration.GetContainer().Container;
            _accountRegistry= _container.Resolve<Accounts.Registry.IAccountRegistry>();

            _accountRegistry.ChangedSizeEvent += (o, e) =>
            {
                TotalSize = e.NewSize.TotalSize;
                UsedSize = e.NewSize.TotalSize;
                FreeSize = e.NewSize.FreeSize;
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
