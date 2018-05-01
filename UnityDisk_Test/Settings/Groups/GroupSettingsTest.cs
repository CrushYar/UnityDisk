using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.GroupTree;
using UnityDisk.Settings;
using UnityDisk.Settings.Accounts;
using UnityDisk.Settings.Groups;

namespace UnityDisk_Test.Settings.Groups
{
    [TestClass]
    public class GroupSettingsTest
    {
        private Mock<ISettings> _mockService;
        private string _parameterName;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockService = Mock.Create<ISettings>();
            _parameterName = "GroupTreeSettings";
        }
        [TestMethod]
        public void Can_LoadGroupTree()
        {
            string expected = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>Container1</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container2</Name>\r\n          <IsActive>false</IsActive>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group2</Name>\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n      <Name>Group</Name>\r\n      <Items>\r\n        <string>Account1</string>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";
            var forSave = new GroupSettingsContainer()
            {
                Items = new List<GroupSettingsItem>()
                {
                    new GroupSettingsContainer()
                    {
                        Name = "Container1",
                        Items = new List<GroupSettingsItem>()
                        {
                            new GroupSettingsContainer() {Name = "Container2"},
                            new GroupSettingsGroup() {Name = "Group2"}
                        },
                        IsActive = true
                    },
                    new GroupSettingsGroup() {Name = "Group", Items = new List<string>() {"Account1"}}
                }
            };
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, forSave);

            _settings.SaveGroupTree();
            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expected), Occurred.Once());
        }
        [TestMethod]
        public void Can_SaveGroupTree()
        {
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, null);

            string stub =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>Container1</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container2</Name>\r\n          <IsActive>false</IsActive>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group2</Name>\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n      <Name>Group</Name>\r\n      <Items>\r\n        <string>Account1</string>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";
            var expected = new GroupSettingsContainer()
            {
                Items = new List<GroupSettingsItem>()
                {
                    new GroupSettingsContainer()
                    {
                        Name = "Container1",
                        Items = new List<GroupSettingsItem>()
                        {
                            new GroupSettingsContainer() {Name = "Container2", Items = new List<GroupSettingsItem>()},
                            new GroupSettingsGroup() {Name = "Group2", Items = new List<string>()}
                        },
                        IsActive = true
                    },
                    new GroupSettingsGroup() {Name = "Group", Items = new List<string>() {"Account1"}}
                }
            };
            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(stub);
            GroupSettingsContainer actuality = _settings.LoadGroupTree();
            Assert.IsNotNull(actuality);
            Assert.IsNotNull(actuality.Items);
            Assert.AreEqual(expected, actuality);
        }
        [TestMethod]
        public void Can_AddGroup()
        {
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, null);
            string expectedStringForSave =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n      <Name>Group</Name>\r\n      <Items>\r\n        <string>Account1</string>\r\n        <string>Account2</string>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";

            var expected = new GroupSettingsGroup() { Items = new List<string>() { "Account1", "Account2" }, Name = "Group" };
            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(String.Empty);
            GroupSettingsContainer actuality = _settings.LoadGroupTree();

            _settings.Add(new List<string>(), expected);

            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expectedStringForSave), Occurred.Once());

            Assert.IsNotNull(actuality);
            Assert.IsNotNull(actuality.Items);
        }
        [TestMethod]
        public void Can_AddContainer()
        {
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, null);
            string expectedStringForSave =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items />\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";

            var expected = new GroupSettingsContainer()
            {
                Name = "MainContainer", IsActive = true,
                Items = new List<GroupSettingsItem>()
                {
                    new GroupSettingsGroup()
                    {
                        Items = new List<string>() {"Account1", "Account2"},
                        Name = "Group"
                    },
                    new GroupSettingsContainer()
                    {
                        Name = "Container",
                        Items = new List<GroupSettingsItem>(),
                        IsActive = true
                    }
                }
            };

            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(String.Empty);
            GroupSettingsContainer actuality = _settings.LoadGroupTree();

            _settings.Add(new List<string>(), expected);

            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expectedStringForSave),
                Occurred.Once());

            Assert.IsNotNull(actuality);
            Assert.IsNotNull(actuality.Items);
        }
        [TestMethod]
        public void Can_Delete()
        {
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, null);
            string stubSettings =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items />\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";
            string expectedStringForSave = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items />\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";

            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(stubSettings);
            GroupSettingsContainer actuality = _settings.LoadGroupTree();

            _settings.Delete(new List<string>() { "MainContainer" }, "Group", GroupTreeTypeEnum.Group);

            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expectedStringForSave),
                Occurred.Once());
        }
        [TestMethod]
        public void Can_Rename()
        {
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, null);
            string stubSettings =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items />\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";
            string expectedStringForSave = "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>NewName for Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items />\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";

            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(stubSettings);
            GroupSettingsContainer actuality = _settings.LoadGroupTree();

            _settings.Rename(new List<string>() { "MainContainer" }, "Group", GroupTreeTypeEnum.Group, "NewName for Group");

            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expectedStringForSave),
                Occurred.Once());
        }
        [TestMethod]
        public void Can_Move()
        {
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, null);
            string stubSettings =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items>\r\n            <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n              <Name>Group2</Name>\r\n              <Items>\r\n                <string>Account1</string>\r\n              </Items>\r\n            </GroupSettingsItem>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";
            string expectedStringForSave =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items />\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group2</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";
           
            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(stubSettings);
            GroupSettingsContainer actuality = _settings.LoadGroupTree();

            _settings.Move(new List<string>() { "MainContainer" , "Container" }, "Group2", GroupTreeTypeEnum.Group,new List<string>(){ "MainContainer" });

            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expectedStringForSave),
                Occurred.Once());
        }
        [TestMethod]
        public void Can_Copy()
        {
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, null);
            string stubSettings =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items>\r\n            <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n              <Name>Group2</Name>\r\n              <Items>\r\n                <string>Account1</string>\r\n              </Items>\r\n            </GroupSettingsItem>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";
            string expectedStringForSave =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items>\r\n            <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n              <Name>Group2</Name>\r\n              <Items>\r\n                <string>Account1</string>\r\n              </Items>\r\n            </GroupSettingsItem>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group2</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";

            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(stubSettings);
            GroupSettingsContainer actuality = _settings.LoadGroupTree();

            _settings.Copy(new List<string>() { "MainContainer", "Container" }, "Group2", GroupTreeTypeEnum.Group, new List<string>() { "MainContainer" });

            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expectedStringForSave),
                Occurred.Once());
        }
        [TestMethod]
        public void Can_SetActive()
        {
            GroupSettings _settings = new GroupSettings(_mockService.Object, _parameterName, null);
            string stubSettings =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>true</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items>\r\n            <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n              <Name>Group2</Name>\r\n              <Items>\r\n                <string>Account1</string>\r\n              </Items>\r\n            </GroupSettingsItem>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";
            string expectedStringForSave =
                "<?xml version=\"1.0\" encoding=\"utf-8\"?>\r\n<GroupSettingsContainer xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xmlns:xsd=\"http://www.w3.org/2001/XMLSchema\">\r\n  <Name>Room</Name>\r\n  <IsActive>false</IsActive>\r\n  <Items>\r\n    <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n      <Name>MainContainer</Name>\r\n      <IsActive>false</IsActive>\r\n      <Items>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n          <Name>Group</Name>\r\n          <Items>\r\n            <string>Account1</string>\r\n            <string>Account2</string>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n        <GroupSettingsItem xsi:type=\"GroupSettingsContainer\">\r\n          <Name>Container</Name>\r\n          <IsActive>true</IsActive>\r\n          <Items>\r\n            <GroupSettingsItem xsi:type=\"GroupSettingsGroup\">\r\n              <Name>Group2</Name>\r\n              <Items>\r\n                <string>Account1</string>\r\n              </Items>\r\n            </GroupSettingsItem>\r\n          </Items>\r\n        </GroupSettingsItem>\r\n      </Items>\r\n    </GroupSettingsItem>\r\n  </Items>\r\n</GroupSettingsContainer>";

            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(stubSettings);
            GroupSettingsContainer actuality = _settings.LoadGroupTree();

            _settings.SetActive(new List<string>() , "MainContainer", false);

            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, expectedStringForSave),
                Occurred.Once());
        }
    }
}
