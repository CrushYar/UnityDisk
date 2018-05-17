using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts;
using Account = UnityDisk.FileStorages.OneDrive.Account;

namespace UnityDisk_Test.FileStorages.OneDrive
{
    [TestClass]
    public class AccountTest
    {
        private string _login = "shazhko.artem@gmail.com";

        [TestInitialize]
        public async Task BeforeEachTest()
        {
            await UnityDisk.RemoteInitialization.Start();
        }

        [TestMethod]
        public async Task Can_SignIn()
        {
            Account account = new Account();
            await account.SignIn(_login);
            Assert.IsNotNull(account.Size);
            Assert.IsTrue(account.Size.TotalSize > 0);
            Assert.IsTrue(account.Size.UsedSize > 0);
            Assert.IsTrue(account.Size.FreeSize > 0);
            Assert.IsNotNull(account.Login);
            Assert.IsNotNull(account.Id);
            Assert.IsNotNull(account.Token);
            Assert.AreEqual(account.Status, ConnectionStatusEnum.Connected);
        }

        [TestMethod]
        public async Task Can_Update()
        {
            Account account = new Account();
            await account.SignIn(_login);
            account.Size=null;
            await account.Update();
            Assert.IsNotNull(account.Size);
            Assert.IsTrue(account.Size.TotalSize > 0);
            Assert.IsTrue(account.Size.UsedSize > 0);
            Assert.IsTrue(account.Size.FreeSize > 0);
            Assert.AreEqual(account.Status, ConnectionStatusEnum.Connected);
        }
    }
}
