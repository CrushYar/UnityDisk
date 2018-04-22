using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityDisk.Accounts;

namespace UnityDisk.FileStorages.OneDrive
{
    public class Account : IFileStorageAccount
    {
        public string Login { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public DateTime CreateDate { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public SpaceSize Size { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string ServerName { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string Token { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool IsFree { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ConnectionStatusEnum Status { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public IFileStorageAccount Clone()
        {
            throw new NotImplementedException();
        }

        public Task SignIn(string key)
        {
            throw new NotImplementedException();
        }

        public Task SignOut()
        {
            throw new NotImplementedException();
        }

        public Task Update()
        {
            throw new NotImplementedException();
        }
    }
}
