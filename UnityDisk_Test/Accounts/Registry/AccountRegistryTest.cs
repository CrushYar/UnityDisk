using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
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
        private IUnityContainer _accountContainerStub;
        private Mock<IAccount> _mockAccount;
        private Mock<IAccountProjection> _mockAccountProjection;
        private Mock<IFileStorageAccount> _mockFileStorageAccount;
        private AccountRegistry _accountRegistry;

        string expectedLogin = "login1",
            expectedServerName = "pCloud",
            expectedToken = "123456987";
        SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 70, UsedSize = 30 };

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockAccountSettings = Mock.Create<IAccountSettings>();
            _mockAccount = Mock.Create<IAccount>();
            _mockAccountProjection = Mock.Create<IAccountProjection>();
            _mockFileStorageAccount = Mock.Create<IFileStorageAccount>();
            _accountContainerStub=new UnityContainer();
            _accountContainerStub.RegisterInstance<IAccount>(_mockAccount.Object);
            _accountContainerStub.RegisterInstance<IAccountProjection>(new AccountProjection());

            _mockAccount.SetupGet(acc => acc.LoadConnector(expectedServerName)).Returns(true);
            _mockAccount.SetupGet(acc => acc.Clone()).Returns(_mockAccount.Object);
            _mockAccount.SetupGet(acc => acc.Groups).Returns(new List<string>());
            _mockAccount.SetupGet(acc => acc.Login).Returns(expectedLogin);
            _mockAccount.SetupGet(acc => acc.Token).Returns(expectedToken);
            _mockAccount.SetupGet(acc => acc.ServerName).Returns(expectedServerName);
            _mockAccount.SetupGet(acc => acc.Size).Returns(expectedSize);
        }

        [TestMethod]
        public void Can_Load()
        {
           
            var accountSettingsStub = new[]
            {
                new AccountSettingsItem()
                {
                    Login = expectedLogin, ServerName = expectedServerName, Token = expectedToken
                },
            };
            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(accountSettingsStub);


            _accountRegistry = new AccountRegistry(_mockAccountSettings.Object, _accountContainerStub);

            _accountRegistry.LoadedEvent += (o, e) =>
            {
                Assert.IsNotNull(e.Accounts);
                Assert.AreEqual(e.Accounts.Length, 1);
                _mockAccount.Verify(acc => acc.LoadConnector(expectedServerName), Occurred.Once());
                _mockAccount.VerifySet(acc => acc.Login, expectedLogin, Occurred.Once());
                _mockAccount.VerifySet(acc => acc.Token, expectedToken, Occurred.Once());
                Assert.AreEqual(e.Size, expectedSize);
            };
            _accountRegistry.Load();
        }

        [TestMethod]
        public void Can_Find()
        {
          
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


            _accountRegistry = new AccountRegistry(_mockAccountSettings.Object, _accountContainerStub);

            _accountRegistry.Load();

            IAccountProjection accountFound = _accountRegistry.Find(expectedLogin);

            _mockAccount.VerifySet(acc => acc.Login, expectedLogin, Occurred.Once());
            _mockAccount.VerifySet(acc => acc.Token, expectedToken, Occurred.Once());
            Assert.AreEqual(expectedLogin, accountFound.Login);
            Assert.AreEqual(expectedServerName, accountFound.ServerName);
            Assert.AreEqual(expectedToken, accountFound.Token);
            Assert.IsNotNull(accountFound);
        }

        [TestMethod]
        public void Can_Registry()
        {
           
            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(Array.Empty<IAccountSettingsItem>());

            _accountRegistry = new AccountRegistry(_mockAccountSettings.Object, _accountContainerStub);

            _accountRegistry.Registry(_mockAccount.Object);
            Assert.AreEqual(_accountRegistry.Size, expectedSize);
            IAccountProjection accountFound = _accountRegistry.Find(expectedLogin);

            Assert.IsNotNull(accountFound);
        }
        [TestMethod]
        public void Can_SaveSettings()
        {
       
            var expectedAccountSettings = new[]
            {
                new AccountSettingsItem()
                {
                    Login = expectedLogin, ServerName = expectedServerName, Token = expectedToken
                }
            };

            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(Array.Empty<IAccountSettingsItem>());
           
            _accountRegistry = new AccountRegistry(_mockAccountSettings.Object, _accountContainerStub);

            _accountRegistry.Registry(_mockAccount.Object);
            _mockAccountSettings.Verify(settings => settings.SaveAccounts(Param.Is<IAccountSettingsItem[]>(items => items.SequenceEqual(expectedAccountSettings))), Occurred.Once());
        }
        [TestMethod]
        public void Can_Delete()
        {  
            _mockAccountSettings.SetupGet(settings => settings.LoadAccounts()).Returns(Array.Empty<IAccountSettingsItem>());
     
            _accountRegistry = new AccountRegistry(_mockAccountSettings.Object, _accountContainerStub);

            _accountRegistry.Registry(_mockAccount.Object);
            Assert.IsTrue(_accountRegistry.Delete(expectedLogin));
            Assert.AreEqual(_accountRegistry.Size, new SpaceSize());
            IAccountProjection accountFound = _accountRegistry.Find(expectedLogin);

            Assert.IsNull(accountFound);
        }
    }
}
