using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive;

namespace UnityDisk_Test.FileStorages.OneDrive
{
    [TestClass]
    public class FileStorageFileTest
    {
        private string _login = "shazhko.artem@gmail.com";

        [TestInitialize]
        public async Task BeforeEachTest()
        {
            await UnityDisk.RemoteInitialization.Start();
        }
        [TestMethod]
        public async Task Can_RenameFile()
        {
            string expectedName = "ForTestRename_DONE.txt";
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "ForTestRename.txt",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await file.Rename(expectedName);
            Assert.AreEqual(file.Name, expectedName);
        }
        [TestMethod]
        public async Task Can_LoadPreviewImage()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "Untitled.png",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await file.LoadPreviewImage();
            await Task.Delay(2000);

            Assert.IsNotNull(file.PreviewImage);
        }
        [TestMethod]
        public async Task Can_LoadPublicUrl()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "Untitled.png",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await file.CreatePublicUrl();

            file = new FileStorageFile(new FileBuilder()
            {
                Name = "Untitled.png",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await file.LoadPublicUrl();

            Assert.IsFalse(String.IsNullOrEmpty(file.PublicUrl));
            Assert.IsFalse(String.IsNullOrEmpty(file.PublicUrlId));
        }
        [TestMethod]
        public async Task Can_CreatePublicUrl()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "Untitled.png",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await file.CreatePublicUrl();

            Assert.IsFalse(String.IsNullOrEmpty(file.PublicUrl));
            Assert.IsFalse(String.IsNullOrEmpty(file.PublicUrlId));
        }
        [TestMethod]
        public async Task Can_DeletePublicUrl()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "Untitled.png",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await file.CreatePublicUrl();
            await file.DeletePublicUrl();

            Assert.IsTrue(String.IsNullOrEmpty(file.PublicUrl));
            Assert.IsTrue(String.IsNullOrEmpty(file.PublicUrlId));
        }
    }
}
