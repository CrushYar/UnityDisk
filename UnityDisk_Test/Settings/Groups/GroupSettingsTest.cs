﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
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
            Assert.AreEqual(expected,actuality);
        }
    }
}