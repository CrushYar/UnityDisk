﻿using System;
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

        [TestMethod]
        public void BindingParametersTest()
        {
            string expectedToken = "simpleToken";
            string expectedLogin = "test@gmail.com";
            DateTime expectedCreaDate = new DateTime(2018, 4, 22, 2, 7, 0);
            string expectedServerName = "OneDrive";
            SpaceSize expectedSize = new SpaceSize(){TotalSize = 1000, FreelSize = 300, UsedSize = 700};
            ConnectionStatusEnum expectedStatus = ConnectionStatusEnum.Connected;

            _mockService.SetupGet(account => account.Token).Returns(expectedToken);
            _mockService.SetupGet(account => account.Login).Returns(expectedLogin);
            _mockService.SetupGet(account => account.CreateDate).Returns(expectedCreaDate);
            _mockService.SetupGet(account => account.ServerName).Returns(expectedServerName);
            _mockService.SetupGet(account => account.Status).Returns(expectedStatus);
            _mockService.SetupGet(account => account.Size).Returns(expectedSize);

            Assert.AreEqual(_account.Token, expectedToken);
            Assert.AreEqual(_account.Login, expectedLogin);
            Assert.AreEqual(_account.CreateDate, expectedCreaDate);
            Assert.AreEqual(_account.ServerName, expectedServerName);
            Assert.AreEqual(_account.Size, expectedSize);
            Assert.AreEqual(_account.Status, expectedStatus);

        }
    }
}