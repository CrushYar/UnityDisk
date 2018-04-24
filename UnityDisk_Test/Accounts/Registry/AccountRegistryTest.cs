using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public void Can_Load()
        {
            string expectedLogin = "login1",
                 expectedServerName = "pCloud",
                 expectedToken = "123456987";
            SpaceSize expectedSize = new SpaceSize(){TotalSize = 100, FreelSize = 70,UsedSize = 30};
            var accountSettingsStub = new[]
            {
                new AccountSettingsItem()
                {
                    Login = expectedLogin, ServerName = expectedServerName, Token = expectedToken
                },
            };
         
            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(accountSettingsStub);
            _mockAccount.SetupGet(acc => acc.LoadConnector(expectedServerName)).Returns(true);
            _mockAccount.SetupGet(acc => acc.Clone()).Returns(_mockAccount.Object);
            _mockAccount.SetupGet(acc => acc.Size).Returns(expectedSize);

            _mockAccountContainer.SetupGet(container => container.GetInstance<IAccount>()).Returns(_mockAccount.Object);
            _mockSettingsContainer.SetupGet(container => container.GetInstance<IAccountSettings>()).Returns(_mockAccountSettings.Object);

            _accountRegistry = new AccountRegistry(_mockSettingsContainer.Object, _mockAccountContainer.Object);

            _accountRegistry.LoadedEvent += (o, e) =>
            {
                Assert.IsNotNull(e.Accounts);
                Assert.AreEqual(e.Accounts.Length, 1);
                _mockAccount.Verify(acc => acc.LoadConnector(expectedServerName), Occurred.Once());
                _mockAccount.Verify(acc => acc.Clone(), Occurred.Once());
                _mockAccount.VerifySet(acc => acc.Login, expectedLogin, Occurred.Once());
                _mockAccount.VerifySet(acc => acc.ServerName, expectedServerName, Occurred.Once());
                _mockAccount.VerifySet(acc => acc.Token, expectedToken, Occurred.Once());
                Assert.AreEqual(e.Size, expectedSize);
            };
            _accountRegistry.Load();
        }

        [TestMethod]
        public void Can_Find()
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
                new AccountSettingsItem()
                {
                    Login = "login2", ServerName = "Tandex", Token = "9875452"
                },
            };

            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(accountSettingsStub);
            _mockAccount.SetupGet(acc => acc.LoadConnector(expectedServerName)).Returns(true);
            _mockAccount.SetupGet(acc => acc.Clone()).Returns(_mockAccount.Object);

            _mockAccountContainer.SetupGet(container => container.GetInstance<IAccount>()).Returns(_mockAccount.Object);
            _mockSettingsContainer.SetupGet(container => container.GetInstance<IAccountSettings>()).Returns(_mockAccountSettings.Object);

            _accountRegistry = new AccountRegistry(_mockSettingsContainer.Object, _mockAccountContainer.Object);

            _accountRegistry.Load();

            IAccount accountFound = _accountRegistry.Find(expectedLogin);

            _mockAccount.Verify(acc=>acc.Clone(),Occurred.Exactly(2));
            Assert.IsNotNull(accountFound);
            
        }

        [TestMethod]
        public void Can_Registry()
        {
            string expectedLogin = "login1",
                expectedServerName = "pCloud",
                expectedToken = "123456987";
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 70, UsedSize = 30 };
            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(Array.Empty<IAccountSettingsItem>());
            _mockAccount.SetupGet(acc => acc.LoadConnector(expectedServerName)).Returns(true);
            _mockAccount.SetupGet(acc => acc.Clone()).Returns(_mockAccount.Object);
            _mockAccount.SetupGet(acc => acc.Size).Returns(expectedSize);
            _mockAccount.SetupGet(acc => acc.Login).Returns(expectedLogin);

            // _mockAccountContainer.SetupGet(container => container.GetInstance<IAccount>()).Returns(_mockAccount.Object);
            _mockSettingsContainer.SetupGet(container => container.GetInstance<IAccountSettings>()).Returns(_mockAccountSettings.Object);

            _accountRegistry = new AccountRegistry(_mockSettingsContainer.Object, _mockAccountContainer.Object);

            _accountRegistry.Registry(_mockAccount.Object);
            Assert.AreEqual(_accountRegistry.Size, expectedSize);
            IAccount accountFound = _accountRegistry.Find(expectedLogin);

            _mockAccount.Verify(acc => acc.Clone(), Occurred.Exactly(3));
            Assert.IsNotNull(accountFound);
        }
        [TestMethod]
        public void Can_SaveSettings()
        {
            string expectedLogin = "login1",
                expectedServerName = "pCloud",
                expectedToken = "123456987";
            var expectedAccountSettings = new[]
            {
                new AccountSettingsItem()
                {
                    Login = expectedLogin, ServerName = expectedServerName, Token = expectedToken
                }
            };

            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 70, UsedSize = 30 };
            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(Array.Empty<IAccountSettingsItem>());
            _mockAccount.SetupGet(acc => acc.LoadConnector(expectedServerName)).Returns(true);
            _mockAccount.SetupGet(acc => acc.Clone()).Returns(_mockAccount.Object);
            _mockAccount.SetupGet(acc => acc.Login).Returns(expectedLogin);
            _mockAccount.SetupGet(acc => acc.ServerName).Returns(expectedServerName);
            _mockAccount.SetupGet(acc => acc.Token).Returns(expectedToken);
            _mockAccount.SetupGet(acc => acc.Size).Returns(expectedSize);
            _mockAccount.SetupGet(acc => acc.Login).Returns(expectedLogin);

            _mockSettingsContainer.SetupGet(container => container.GetInstance<IAccountSettings>()).Returns(_mockAccountSettings.Object);

            _accountRegistry = new AccountRegistry(_mockSettingsContainer.Object, _mockAccountContainer.Object);

            _accountRegistry.Registry(_mockAccount.Object);
            _mockAccountSettings.Verify(settings=>settings.SaveAccounts(Param.Is<IAccountSettingsItem[]>(items=>items.SequenceEqual(expectedAccountSettings))),Occurred.Once());
        }
        [TestMethod]
        public void Can_Delete()
        {
            string expectedLogin = "login1",
                expectedServerName = "pCloud",
                expectedToken = "123456987";
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 70, UsedSize = 30 };
            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(Array.Empty<IAccountSettingsItem>());
            _mockAccount.SetupGet(acc => acc.LoadConnector(expectedServerName)).Returns(true);
            _mockAccount.SetupGet(acc => acc.Clone()).Returns(_mockAccount.Object);
            _mockAccount.SetupGet(acc => acc.Size).Returns(expectedSize);
            _mockAccount.SetupGet(acc => acc.Login).Returns(expectedLogin);

            // _mockAccountContainer.SetupGet(container => container.GetInstance<IAccount>()).Returns(_mockAccount.Object);
            _mockSettingsContainer.SetupGet(container => container.GetInstance<IAccountSettings>()).Returns(_mockAccountSettings.Object);

            _accountRegistry = new AccountRegistry(_mockSettingsContainer.Object, _mockAccountContainer.Object);

            _accountRegistry.Registry(_mockAccount.Object);
            Assert.IsTrue(_accountRegistry.Delete(expectedLogin));
            Assert.AreEqual(_accountRegistry.Size, new SpaceSize());
            IAccount accountFound = _accountRegistry.Find(expectedLogin);

            _mockAccount.Verify(acc => acc.Clone(), Occurred.Exactly(4));
            Assert.IsNull(accountFound);
        }
    }
}
