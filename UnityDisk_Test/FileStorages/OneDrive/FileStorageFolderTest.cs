using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive;
namespace UnityDisk_Test.FileStorages.OneDrive
{
    [TestClass]
    public class FileStorageFolderTest
    {
        private string _login = "shazhko.artem@gmail.com";

        [TestInitialize]
        public async Task BeforeEachTest()
        {
            await UnityDisk.RemoteInitialization.Start();
        }

        [TestMethod]
        public async Task Can_LoadDirectory()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder=new FileStorageFolder(new AccountProjection(
                new UnityDisk.Accounts.Account(account)));
            var list=await folder.LoadDirectory();

            Assert.IsNotNull(list);
            Assert.IsTrue(list.Count>0);
        }
        [TestMethod]
        public async Task Can_DeleteFolder()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "TestFolder",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await folder.Delete();
        }
        [TestMethod]
        public async Task Can_RenameFolder()
        {
            string expectedName = "TestFolder3";
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "TestFolder2",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await folder.Rename(expectedName);
            Assert.AreEqual(folder.Name, expectedName);
        }
        [TestMethod]
        public async Task Can_MoveFolder()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestMove2",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };
            FileStorageFolder folderTo = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestMove",
                Path = "/drive/root:/ForTestMove"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await folder.Move(folderTo);
            Assert.AreEqual(folder.Path, folderTo.Path);
        }
    }
}
