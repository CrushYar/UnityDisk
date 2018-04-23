using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using StructureMap;
using UnityDisk;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages;
using UnityDisk.Settings;
using UnityDisk.Settings.Accounts;
using UnityDisk;
namespace UnityDisk_Test.Accounts.Registry
{
    [TestClass]
    public class AccountRegistryTest
    {
        private Mock<IAccountSettings> _mockAccountSettings;
        private Mock<IContainer> _mockSettingsContainer;
        private Mock<IContainer> _mockAccountContainer;
        private Mock<IAccount> _mockAccount;
        private Mock<IFileStorageAccount> _mockFileStorageAccount;
        private AccountRegistry _accountRegistry;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockAccountSettings = Mock.Create<IAccountSettings>();
            _mockSettingsContainer = Mock.Create<IContainer>();
            _mockAccountContainer = Mock.Create<IContainer>();
            _mockAccount = Mock.Create<IAccount>();
            _mockFileStorageAccount = Mock.Create<IFileStorageAccount>();
        }
       
        [TestMethod]
        public async Task Can_Load()
        {
            string expectedLogin = "login1", 
                expectedServerName = "pCloud",
                expectedToken = "123456987";
            var accountSettingsStub = new[]
            {
                new AccountSettingsItem()
                {
                    Login = expectedLogin, ServerName = expectedServerName, Token = expectedToken
                },
            };
            Mock<IFileStorageAccount> fileStorageStub=Mock.Create<IFileStorageAccount>();
            fileStorageStub.SetupGet(acc => acc.Login).Returns(expectedLogin);
            fileStorageStub.SetupGet(acc => acc.ServerName).Returns(expectedServerName);
            fileStorageStub.SetupGet(acc => acc.Token).Returns(expectedToken);
            var expected = new[]
            {
               new Account(fileStorageStub.Object) 
            };

            _mockAccountSettings.SetupGet(settings=>settings.LoadAccounts()).Returns(accountSettingsStub);
            _mockAccount.SetupGet(acc => acc.LoadConnector(expectedServerName)).Returns(true);
            _mockAccount.SetupGet(acc => acc.Clone()).Returns(_mockAccount.Object);
            _mockAccount.SetupGet(acc => acc.Login).Returns(expectedLogin);
            _mockAccount.SetupGet(acc => acc.ServerName).Returns(expectedServerName);
            _mockAccount.SetupGet(acc => acc.Token).Returns(expectedToken);

            _mockAccountContainer.SetupGet(container => container.GetInstance<IAccount>()).Returns(_mockAccount.Object);
            _mockSettingsContainer.SetupGet(container => container.GetInstance<IAccountSettings>()).Returns(_mockAccountSettings.Object);

            _accountRegistry = new AccountRegistry(_mockSettingsContainer.Object, _mockAccountContainer.Object);

            _accountRegistry.LoadedEvent += (o, e) =>
            {
                CollectionAssert.AreEqual(expected, e.Accounts);
            };
            _accountRegistry.Load();
            _mockAccount.VerifySet(acc => acc.Login, expectedLogin, Occurred.Once());
            _mockAccount.VerifySet(acc => acc.ServerName, expectedServerName, Occurred.Once());
            _mockAccount.VerifySet(acc => acc.Token, expectedToken, Occurred.Once());
        }
    }
}
