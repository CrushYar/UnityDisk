using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts.Registry;
using UnityDisk.FileStorages;
using UnityDisk.Settings.Accounts;

namespace UnityDisk_Test.Accounts.Registry
{
    [TestClass]
    public class AccountRegistryTest
    {
        private Mock<IAccountSettings> _mockService;
        private AccountRegistry _accountRegistry;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockService = Mock.Create<IAccountSettings>();
            _accountRegistry = new AccountRegistry(_mockService.Object);
        }

        [TestMethod]
        public async Task Can_Load()
        {
            var expected = new[]
            {
                new AccountSettingsItem() {Login = "login1", ServerName = "pCloud", Token = "123456987"},
                new AccountSettingsItem() {Login = "login2", ServerName = "Google", Token = "935548855"}
            };
            _mockService.SetupGet(settings=>settings.LoadAccounts())
                .Returns(expected);

            _accountRegistry.Load();
        }
    }
}
