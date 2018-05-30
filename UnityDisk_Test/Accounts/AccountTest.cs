using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts;
using UnityDisk.FileStorages;


namespace UnityDisk_Test.Accounts
{
    [TestClass]
    public class AccountTest
    {
        private Mock<IFileStorageAccount> _mockService;
        private Account _account;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockService = Mock.Create<IFileStorageAccount>();
            _account = new Account(_mockService.Object);
        }

        /// <summary>
        /// Все параметры аккаунта должны зависеть от аккаунта файлового хранилища
        /// </summary>
        [TestMethod]
        public void BindingParameters_ChangeFileStorageAccount_GetChangeInAccount()
        {
            string expectedToken = "simpleToken";
            string expectedLogin = "test@gmail.com";
            DateTime expectedCreaDate = new DateTime(2018, 4, 22, 2, 7, 0);
            string expectedServerName = "OneDrive";
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 1000, FreeSize = 300, UsedSize = 700 };
            ConnectionStatusEnum expectedStatus = ConnectionStatusEnum.Connected;

            _mockService.SetupGet(account => account.Token).Returns(expectedToken);
            _mockService.SetupGet(account => account.Login).Returns(expectedLogin);
            _mockService.SetupGet(account => account.ServerName).Returns(expectedServerName);
            _mockService.SetupGet(account => account.Status).Returns(expectedStatus);
            _mockService.SetupGet(account => account.Size).Returns(expectedSize);

            Assert.AreEqual(_account.Token, expectedToken);
            Assert.AreEqual(_account.Login, expectedLogin);
            Assert.AreEqual(_account.ServerName, expectedServerName);
            Assert.AreEqual(_account.Size, expectedSize);
            Assert.AreEqual(_account.Status, expectedStatus);

        }

        [TestMethod]
        public async Task SignIn_RequestForSignIn_GetToken()
        {
            string token = "simpleToken";

            _mockService.Setup(fileStorage => fileStorage.SignIn(token)).Returns(Task.Run(() => { }));

            await _account.SignIn(token);

            _mockService.Verify(fileStorage => fileStorage.SignIn(token), Occurred.Once());
        }

        [TestMethod]
        public async Task SignOut_RequestForSignOut_DeleteToken()
        {
            _mockService.Setup(fileStorage => fileStorage.SignOut()).Returns(Task.Run(() => { }));

            await _account.SignOut();

            _mockService.Verify(fileStorage => fileStorage.SignOut(), Occurred.Once());
        }

        [TestMethod]
        public async Task Update_RequestForUpdate_GetNewInfo()
        {
            _mockService.Setup(fileStorage => fileStorage.Update()).Returns(Task.Run(() => { }));

            await _account.Update();

            _mockService.Verify(fileStorage => fileStorage.Update(), Occurred.Once());
        }
        [TestMethod]
        public void Clone_RequestForClone_GetCloneObject()
        {
            string expectedToken = "simpleToken";
            string expectedLogin = "test@gmail.com";
            DateTime expectedCreaDate = new DateTime(2018, 4, 22, 2, 7, 0);
            string expectedServerName = "OneDrive";
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 1000, FreeSize = 300, UsedSize = 700 };
            List<string> expectedGroups=new List<string>(){"Group1","Group2"};

            ConnectionStatusEnum expectedStatus = ConnectionStatusEnum.Connected;

            _mockService.SetupGet(account => account.Token).Returns(expectedToken);
            _mockService.SetupGet(account => account.Login).Returns(expectedLogin);
            _mockService.SetupGet(account => account.ServerName).Returns(expectedServerName);
            _mockService.SetupGet(account => account.Status).Returns(expectedStatus);
            _mockService.SetupGet(account => account.Size).Returns(expectedSize);
            _mockService.SetupGet(account => account.Clone()).Returns(_mockService.Object);
            _account.Groups.Add("Group1");
            _account.Groups.Add("Group2");

            var accountClone = _account.Clone();

            CollectionAssert.AreEqual((List<string>)_account.Groups, expectedGroups);
            Assert.AreEqual(accountClone.Token, expectedToken);
            Assert.AreEqual(accountClone.Login, expectedLogin);
            Assert.AreEqual(accountClone.ServerName, expectedServerName);
            Assert.AreEqual(accountClone.Size, expectedSize);
            Assert.AreEqual(accountClone.Status, expectedStatus);
        }
    }
}
