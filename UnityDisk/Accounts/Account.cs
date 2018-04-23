using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages;
using UnityDisk.FileStorages.FactoryRagistry;
using UnityDisk.Settings.Accounts;

namespace UnityDisk.Accounts
{
    public class Account : IAccount
    {
        private IFileStorageAccount _fileStorageAccount;

        public string Login { get =>_fileStorageAccount.Login; set => _fileStorageAccount.Login=value; }
        public DateTime CreateDate { get => _fileStorageAccount.CreateDate; set => _fileStorageAccount.CreateDate = value; }
        public SpaceSize Size { get => _fileStorageAccount.Size; set => _fileStorageAccount.Size = value; }
        public string ServerName { get => _fileStorageAccount.ServerName; set => _fileStorageAccount.ServerName = value; }
        public string Token { get => _fileStorageAccount.Token; set => _fileStorageAccount.Token = value; }
        public bool IsFree { get => _fileStorageAccount.IsFree; set => _fileStorageAccount.IsFree = value; }
        public ConnectionStatusEnum Status { get => _fileStorageAccount.Status; set => _fileStorageAccount.Status = value; }

        public event EventHandler<SizeChangedEventArg> ChangedSizeEvent;
        public event EventHandler<IAccount> SignedInEvent;
        public event EventHandler<IAccount> SignedOutEvent;

        public Account() { }

        public Account(IFileStorageAccount fileStorageAccount)
        {
            this._fileStorageAccount = fileStorageAccount;
        }

        public bool LoadConnector(string serverName)
        {
            var container = ContainerConfiguration.GetContainer().Container;
            var factory = container.GetInstance<IFactoryRagistry>();
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
            account.CreateDate=CreateDate;
            account.Login = Login;
            account.ServerName = ServerName;
            account.Size=new SpaceSize(Size);
            account.Status = Status;
            account.Token = Token;
            account.IsFree = IsFree;
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
    }
}
