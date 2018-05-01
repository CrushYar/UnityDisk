using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Unity;
using Unity.Injection;
using UnityDisk.Accounts;
using UnityDisk.Accounts.Registry;
using UnityDisk.GroupTree;
using UnityDisk.GroupTree.Registry;
using UnityDisk.Settings;
using UnityDisk.Settings.Groups;

namespace UnityDisk_Test.GroupTree.Registry
{
    [TestClass]
    public class GroupTreeRegistryTest
    {
        private Mock<IGroupSettings> _mockService;
        private Mock<IAccountRegistry> _accountRegistryStub;
        private Mock<IAccountProjection> _accountStub;
        private string _parameterName;
        private IUnityContainer _groupContainerStub;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockService = Mock.Create<IGroupSettings>();
            _parameterName = "GroupTreeSettings";
            _accountRegistryStub = Mock.Create<IAccountRegistry>();
            _accountStub = Mock.Create<IAccountProjection>();
        }

        [TestMethod]
        public void Can_Initialization()
        {
            var groupSettingsStub = new GroupSettingsContainer()
            {
                Name = "Root",
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
            SpaceSize expectedSize = new SpaceSize() {TotalSize = 100, FreelSize = 30, UsedSize = 70};
            _mockService.Setup(settings =>settings.LoadGroupTree()).Returns(groupSettingsStub);
            _accountRegistryStub.Setup(accountRegistry => accountRegistry.Find("Account1")).Returns(_accountStub.Object);
            _accountStub.SetupGet(accountProjection => accountProjection.Size).Returns(() => expectedSize);
            _groupContainerStub =new UnityContainer();
            _groupContainerStub.RegisterInstance<IGroupSettings>(_mockService.Object);
            _groupContainerStub.RegisterInstance<IAccountRegistry>(_accountRegistryStub.Object);
            _groupContainerStub.RegisterType<IContainer, Container>(new InjectionConstructor());
            _groupContainerStub.RegisterType<IGroup, Group>(new InjectionConstructor());

            GroupTreeRegistry registry = new GroupTreeRegistry(_groupContainerStub);

            registry.Initialization();

            IContainerProjection rootProjection = registry.GetContainerProjection(new List<string>(), null);
            IContainerProjection containerProjection = registry.GetContainerProjection(new List<string>(){ "Container1",}, "Container2");
            Assert.IsNotNull(rootProjection);
            Assert.IsNotNull(containerProjection);
            Assert.AreEqual(rootProjection.Name, "Root");
            Assert.AreEqual(rootProjection.Size, expectedSize);
            Assert.AreEqual(containerProjection.Name, "Container2");
            Assert.AreEqual(containerProjection.Size, new SpaceSize());
        }
    }
}
