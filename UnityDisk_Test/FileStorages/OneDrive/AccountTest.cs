using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts;
using Account = UnityDisk.FileStorages.OneDrive.Account;

namespace UnityDisk_Test.FileStorages.OneDrive
{
    [TestClass]
    public class AccountTest
    {
        private string _login = "shazhko.artem@gmail.com";
        private AuthenticationResult _authenticationResult ;
        [TestInitialize]
        public async Task BeforeEachTest()
        {
            try
            {
                _authenticationResult =
                    await UnityDisk.App.PublicClientApp.AcquireTokenSilentAsync(UnityDisk.App.Scopes,
                        UnityDisk.App.PublicClientApp.Users.FirstOrDefault());
            }
            catch (MsalUiRequiredException ex)
            {
                Debug.WriteLine($"MsalUiRequiredException: {ex.Message}");
                try
                {
                    _authenticationResult = await UnityDisk.App.PublicClientApp.AcquireTokenAsync(UnityDisk.App.Scopes);
                }
                catch (MsalException msalex)
                {
                    Debug.WriteLine($"Error Acquiring Token:{System.Environment.NewLine}{msalex}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Error Acquiring Token Silently:{System.Environment.NewLine}{ex}");
            }
        }

        [TestMethod]
        public async Task Can_SignIn()
        {
            Account account = new Account();
            await account.SignIn(_login);
            Assert.IsNotNull(account.Size);
            Assert.IsTrue(account.Size.TotalSize > 0);
            Assert.IsTrue(account.Size.UsedSize > 0);
            Assert.IsTrue(account.Size.FreelSize > 0);
            Assert.IsNotNull(account.Login);
            Assert.IsNotNull(account.Id);
            Assert.IsNotNull(account.Token);
            Assert.AreEqual(account.Status, ConnectionStatusEnum.Connected);
        }

        [TestMethod]
        public async Task Can_Update()
        {
            Account account = new Account();
            await account.SignIn(_login);
            account.Size=null;
            await account.Update();
            Assert.IsNotNull(account.Size);
            Assert.IsTrue(account.Size.TotalSize > 0);
            Assert.IsTrue(account.Size.UsedSize > 0);
            Assert.IsTrue(account.Size.FreelSize > 0);
            Assert.AreEqual(account.Status, ConnectionStatusEnum.Connected);
        }
    }
}
