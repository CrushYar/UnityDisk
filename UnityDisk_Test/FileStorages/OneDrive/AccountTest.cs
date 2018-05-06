using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts;
using Account = UnityDisk.FileStorages.OneDrive.Account;

namespace UnityDisk_Test.FileStorages.OneDrive
{
    [TestClass]
    public class AccountTest
    {
        [TestMethod]
        public async Task Can_SignIn()
        {
            Account account = new Account();
            await account.SignIn("EwA4A8l6BAAURSN/FHlDW5xN74t6GzbtsBBeBUYAAVFhctzMLMe1AjWlU8lCac9ek38DK/aT+pktRoGXigkgG9hFdfRITX0ypWOgtKAzyo5OnFNvovc2/ehyBnf4eg6olJ2c+l70nKvbkfRNz/9YfyqIp5E5unVi/BGtq4/O3Ik9Pqj4TyoTShya7S2rsiemSz7S/2f9kj539+yBQRFiFpzqc7bxPTSnwzCelkr1wlAt991qY6QDKggK3BqTGCnVKwDQVKnzImF9jfNfxstzb5BT/wetsXPTy8r0/UXFuleodyWcsNvGrMjeheBU8x/UQ2Et7YBCY9NECMGWuv9BCRLjRNVgWO1jERVS+f47Cm4Ld0423N1b1urmsjFR7jADZgAACOaey3JEzyBhCAIUapd2xHGt0Y5ju168M9zU6HSujj0vPrsb2UnYuwowDobNHnF3q7bced7ZPM5WypB2dcq1z7fJQ8uksIhxYlnf9OKgaYzPrcnHUBK2btMR0jkd+GhN7GdcuCB8Hk4xo4le1VA9EGW7KSdNz77HWxe+6LYnS8jXOrg/5Xl74lXO3sknkUy22x4ueeAnSENC+iAgXt4m2Dq92fSSIXVdttQ7DzE54WNpY/ySPZF7Hw+zqFAmnEWFDcqD+3LbkuKlDlGSSaYzql5JQlh6nePkjZiMbV0O1WJvWxqDNJt02RRG8GkTGAabkwqvmA9ZS0ELUvuELUhPE1jIbjGR+xUSf+zFkk9zznKbu3pQuYSXhMcJko41CX6vE7Pb71cIhYoeFOlqIeua4vg341QmePIFyvG3ZJ7d7D4fUiN+XlGzJJsta55UxHjy1hAAVdcg5tRtVPTXnTPpMwQ01S5k1UCZT+O/P5B5EJMd0nkwwDXJyibyR+F+QXiH6rJGw/ZOJyDJgTKvCR/lkTWOd0JJkOdbm/oby8rB8cMlC1pQ/LbrMrkiPv424WGaw0pssW4zWDg47WVhCXGn2n4T9RdBMJNdi8PFmT47XAHvjRUouKaPL7oXPXBwGql8kKLPXA7Uy7yYEA1TM3XvSBX8/jq9tNNg7tZF5H8DQ45tZDlHGcoz6I8Rwfr3oPnUh5rSPQI=");
            Assert.IsNotNull(account.Size);
            Assert.IsTrue(account.Size.TotalSize > 0);
            Assert.IsTrue(account.Size.UsedSize > 0);
            Assert.IsTrue(account.Size.FreelSize > 0);
            Assert.IsNotNull(account.Login);
            Assert.IsNotNull(account.Id);
            Assert.IsNotNull(account.Token);
            Assert.AreEqual(account.Status,ConnectionStatusEnum.Connected);
        }
        [TestMethod]
        public async Task Can_Update()
        {
            Account account = new Account();
            await account.SignIn("EwA4A8l6BAAURSN/FHlDW5xN74t6GzbtsBBeBUYAAVFhctzMLMe1AjWlU8lCac9ek38DK/aT+pktRoGXigkgG9hFdfRITX0ypWOgtKAzyo5OnFNvovc2/ehyBnf4eg6olJ2c+l70nKvbkfRNz/9YfyqIp5E5unVi/BGtq4/O3Ik9Pqj4TyoTShya7S2rsiemSz7S/2f9kj539+yBQRFiFpzqc7bxPTSnwzCelkr1wlAt991qY6QDKggK3BqTGCnVKwDQVKnzImF9jfNfxstzb5BT/wetsXPTy8r0/UXFuleodyWcsNvGrMjeheBU8x/UQ2Et7YBCY9NECMGWuv9BCRLjRNVgWO1jERVS+f47Cm4Ld0423N1b1urmsjFR7jADZgAACOaey3JEzyBhCAIUapd2xHGt0Y5ju168M9zU6HSujj0vPrsb2UnYuwowDobNHnF3q7bced7ZPM5WypB2dcq1z7fJQ8uksIhxYlnf9OKgaYzPrcnHUBK2btMR0jkd+GhN7GdcuCB8Hk4xo4le1VA9EGW7KSdNz77HWxe+6LYnS8jXOrg/5Xl74lXO3sknkUy22x4ueeAnSENC+iAgXt4m2Dq92fSSIXVdttQ7DzE54WNpY/ySPZF7Hw+zqFAmnEWFDcqD+3LbkuKlDlGSSaYzql5JQlh6nePkjZiMbV0O1WJvWxqDNJt02RRG8GkTGAabkwqvmA9ZS0ELUvuELUhPE1jIbjGR+xUSf+zFkk9zznKbu3pQuYSXhMcJko41CX6vE7Pb71cIhYoeFOlqIeua4vg341QmePIFyvG3ZJ7d7D4fUiN+XlGzJJsta55UxHjy1hAAVdcg5tRtVPTXnTPpMwQ01S5k1UCZT+O/P5B5EJMd0nkwwDXJyibyR+F+QXiH6rJGw/ZOJyDJgTKvCR/lkTWOd0JJkOdbm/oby8rB8cMlC1pQ/LbrMrkiPv424WGaw0pssW4zWDg47WVhCXGn2n4T9RdBMJNdi8PFmT47XAHvjRUouKaPL7oXPXBwGql8kKLPXA7Uy7yYEA1TM3XvSBX8/jq9tNNg7tZF5H8DQ45tZDlHGcoz6I8Rwfr3oPnUh5rSPQI=");
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
