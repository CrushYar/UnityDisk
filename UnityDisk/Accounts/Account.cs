﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.FileStorages;

namespace UnityDisk.Accounts
{
    public class Account : Accounts.IAccount
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

        public Account(IFileStorageAccount fileStorageAccount)
        {
            this._fileStorageAccount = fileStorageAccount;
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
    }
}
