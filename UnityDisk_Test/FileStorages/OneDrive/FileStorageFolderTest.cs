using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage.Pickers;
using Windows.UI.Core;
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
                Name = "ForTestDeleteFolder",
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
            string expectedName = "ForTestRename_DONE";
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestRename",
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
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };
            string expectedPath = "/drive/root:/ForTestMove";
            await folder.Move(folderTo);
            Assert.AreEqual(expectedPath, folder.Path);
        }

        [TestMethod]
        public async Task Can_CopyFolder()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestCopy1",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };
            FileStorageFolder folderTo = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestCopy2",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            UnityDisk.FileStorages.IFileStorageFolder result =
                await folder.Copy(folderTo) as UnityDisk.FileStorages.IFileStorageFolder;
            string expectedPath = "/drive/root:/ForTestCopy2";

            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPath, result.Path);
            Assert.AreEqual(folder.Name,result.Name);
        }
        [TestMethod]
        public async Task Can_LoadPublicUrl()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestLoadPublicUrl",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await folder.CreatePublicUrl();

            folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestLoadPublicUrl",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };
            await folder.LoadPublicUrl();

            Assert.IsFalse(String.IsNullOrEmpty(folder.PublicUrl));
            Assert.IsFalse(String.IsNullOrEmpty(folder.PublicUrlId));
        }
        [TestMethod]
        public async Task Can_CreatePublicUrl()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestCreatePublicUrl",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await folder.CreatePublicUrl();

            Assert.IsFalse(String.IsNullOrEmpty(folder.PublicUrl));
            Assert.IsFalse(String.IsNullOrEmpty(folder.PublicUrlId));
        }
        [TestMethod]
        public async Task Can_DeletePublicUrl()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "ForTestDeletePublicUrl",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await folder.CreatePublicUrl();
            await folder.DeletePublicUrl();

            Assert.IsTrue(String.IsNullOrEmpty(folder.PublicUrl));
            Assert.IsTrue(String.IsNullOrEmpty(folder.PublicUrlId));
        }
        [TestMethod]
        public async Task Can_CreateFolder()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFolder folder = new FileStorageFolder(new FolderBuilder()
            {
                Name = "",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };
            string expectedName = "ForTestCreateFolder";
           var newFolder= await folder.CreateFolder(expectedName);

            Assert.IsNotNull(newFolder);
            Assert.AreEqual(expectedName,newFolder.Name);
        }
    }
}
