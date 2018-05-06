using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages;
using UnityDisk.FileStorages.FactoryRagistry;
using UnityDisk.Settings.Accounts;

namespace UnityDisk.Accounts
{
    public sealed class Account : IAccount
    {
        private IFileStorageAccount _fileStorageAccount;

        public string Login { get =>_fileStorageAccount.Login; set => _fileStorageAccount.Login=value; }
        public SpaceSize Size { get => _fileStorageAccount.Size; set => _fileStorageAccount.Size = value; }
        public string ServerName => _fileStorageAccount.ServerName;
        public string Token { get => _fileStorageAccount.Token; set => _fileStorageAccount.Token = value; }
        public bool IsFree => Groups.Count==0;
        public ConnectionStatusEnum Status { get => _fileStorageAccount.Status; set => _fileStorageAccount.Status = value; }
        public IList<string> Groups { get; private set; }

        public event EventHandler<SizeChangedEventArg> ChangedSizeEvent;
        public event EventHandler<IAccount> SignedInEvent;
        public event EventHandler<IAccount> SignedOutEvent;

        public Account()
        {
            Groups=new List<string>();
        }

        public Account(IFileStorageAccount fileStorageAccount)
        {
            this._fileStorageAccount = fileStorageAccount;
            Groups = new List<string>();
        }

        public bool LoadConnector(string serverName)
        {
            IUnityContainer container = ContainerConfiguration.GetContainer().Container;
            var factory = container.Resolve<IFactoryRagistry>();
            _fileStorageAccount = factory.CreateAccount(serverName);
            return _fileStorageAccount != null;
        }

        public async Task SignIn(string key)
        {
           await _fileStorageAccount.SignIn(key);
        }

        public async Task SignOut()
        {
            await _fileStorageAccount.SignOut();
        }

        public async Task Update()
        {
            await _fileStorageAccount.Update();
        }

        public IAccount Clone()
        {
            Account account=new Account(_fileStorageAccount.Clone());
            account.Login = Login;
            account.Size=new SpaceSize(Size);
            account.Status = Status;
            account.Token = Token;
            account.Groups = new List<string>(Groups);
            return account;
        }

        public override Boolean Equals(Object o)
        {
            IAccount other = o as IAccount;
            if (other == null) return false;
            return Login.Equals(other.Login)
                   && Token.Equals(other.Token)
                   && ServerName.Equals(other.ServerName);
        }

        public override int GetHashCode()
        {
            return _fileStorageAccount.GetHashCode();
        }
    }
}
