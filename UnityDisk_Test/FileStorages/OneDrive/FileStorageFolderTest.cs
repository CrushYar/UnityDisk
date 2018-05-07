using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive;
namespace UnityDisk_Test.FileStorages.OneDrive
{
    [TestClass]
    public class FileStorageFolderTest
    {
        [TestMethod]
        public async Task Can_SignIn()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn("shazhko.artem@gmail.com");
            FileStorageFolder folder=new FileStorageFolder(new AccountProjection(
                new UnityDisk.Accounts.Account(account)));
            
            Assert.IsNotNull(account.Size);
            Assert.IsTrue(account.Size.TotalSize > 0);
            Assert.IsTrue(account.Size.UsedSize > 0);
            Assert.IsTrue(account.Size.FreelSize > 0);
            Assert.IsNotNull(account.Login);
            Assert.IsNotNull(account.Id);
            Assert.IsNotNull(account.Token);
            Assert.AreEqual(account.Status, ConnectionStatusEnum.Connected);
        }
    }
}
