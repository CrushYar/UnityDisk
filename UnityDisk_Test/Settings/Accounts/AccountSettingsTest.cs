using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts;
using UnityDisk.Settings;
using UnityDisk.Settings.Accounts;
namespace UnityDisk_Test.Settings.Accounts
{
    [TestClass]
    public class AccountSettingsTest
    {
        private Mock<ISettings> _mockService;
        private AccountSettings _settings;
        private string _parameterName;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockService = Mock.Create<ISettings>();
            _parameterName = "accountSettings";
            _settings = new AccountSettings(_mockService.Object, _parameterName);
        }

        [TestMethod]
        public void Can_SaveAccounts()
        {
            string expected =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<ArrayOfAccountSettingsItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <AccountSettingsItem>\r\n    <Login>myLogin</Login>\r\n    <Token>123</Token>\r\n    <ServerName>Yandex</ServerName>\r\n  </AccountSettingsItem>\r\n</ArrayOfAccountSettingsItem>";
            _settings.SaveAccounts(new[]
            {
                new AccountSettingsItem() {Login = "myLogin", ServerName = "Yandex", Token = "123"},
            });
            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expected), Occurred.Once());
        }
        [TestMethod]
        public void Can_LoadAccounts()
        {
            _mockService = Mock.Create<ISettings>();
            _parameterName = "accountSettings";
            _settings = new AccountSettings(_mockService.Object, _parameterName);

            string stub =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<ArrayOfAccountSettingsItem xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <AccountSettingsItem>\r\n    <Login>myLogin</Login>\r\n    <Token>123</Token>\r\n    <ServerName>Yandex</ServerName>\r\n  </AccountSettingsItem>\r\n</ArrayOfAccountSettingsItem>";
            var expected = new[]
            {
                new AccountSettingsItem() {Login = "myLogin", ServerName = "Yandex", Token = "123"},
            };
            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(stub);
            IAccountSettingsItem[] actuality = _settings.LoadAccounts();
            CollectionAssert.AreEqual(expected, actuality);
        }
    }
}
