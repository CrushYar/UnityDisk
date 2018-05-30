using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Annotations;

namespace UnityDisk.ViewModel.Accounts
{
    public class AccountProjectionViewModel:INotifyPropertyChanged
    {
        private string _login;
        private string _serverName;
        private IList<string> _groups;
        private ulong _totalSize;
        private ulong _usedSize;
        private ulong _freeSize;

        public string Login
        {
            get { return _login; }
            set
            {
                _login = value;
                OnPropertyChanged();
            }
        }
        public string ServerName
        {
            get { return _serverName; }
            set
            {
                _serverName = value;
                OnPropertyChanged();
            }
        }
        public IList<string> Groups
        {
            get { return _groups; }
            set
            {
                _groups = value;
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

        public AccountProjectionViewModel() { }
        public AccountProjectionViewModel(UnityDisk.Accounts.Registry.IAccountProjection projection)
        {
            Login = projection.Login;
            ServerName = projection.ServerName;
            Groups = projection.Groups;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
