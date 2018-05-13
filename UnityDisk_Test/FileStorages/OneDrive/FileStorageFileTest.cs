﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Core;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Core;
using Windows.UI.Xaml.Media.Imaging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages.OneDrive;
using UnityDisk.StorageItems;

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
        public async Task Can_DeleteFile()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "ForTestDeleteFile.txt",
                Path = "/drive/root:"
            })
            {
                Account = new AccountProjection(
                    new UnityDisk.Accounts.Account(account))
            };

            await file.Delete();
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
        public async Task Can_MoveFile()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "ForTestMove2.docx",
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
            await file.Move(folderTo);
            Assert.AreEqual(expectedPath, file.Path);
        }
        [TestMethod]
        public async Task Can_CopyFile()
        {
            UnityDisk.FileStorages.OneDrive.Account account = new UnityDisk.FileStorages.OneDrive.Account();
            await account.SignIn(_login);
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "ForTestCopy1.exe",
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
                await file.Copy(folderTo) as UnityDisk.FileStorages.IFileStorageFolder;

            Assert.IsNull(result);
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
        [TestMethod]
        public async Task Can_Parse()
        {
            var account = new UnityDisk.Accounts.Account(new UnityDisk.FileStorages.OneDrive.Account()
            {
                Login = "shazhko.artem@gmail.com",
                Size = new SpaceSize() {TotalSize = 100, FreelSize = 30, UsedSize = 70},
                Id = "idAccount",
                Token = "accountToken",
                Status = ConnectionStatusEnum.Connected
            });
            FileStorageFile file = new FileStorageFile(new FileBuilder()
            {
                Name = "Untitled.png",
                Path = "/drive/root:",
                Type = StorageItemTypeEnum.Image,
                Size = 195544,
                DownloadUrl = "downloadUrl",
                Id ="idFile",
                PublicUrl = "publicUrl"
            })
            {
                Account = new AccountProjection(account)
            };
            string jsonData= file.ToString();
            IUnityContainer container = UnityDisk.ContainerConfiguration.GetContainer().Container;
            var accountRegistry = container.Resolve<UnityDisk.Accounts.Registry.IAccountRegistry>();
            accountRegistry.Registry(account);
            var result = FileStorageFile.Parse(jsonData);
        }
    }
}
