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

            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 30, UsedSize = 70 };

            var expectedContainer = new ContainerProjection(new Container()
            {
                Name = "Root",
                Size = expectedSize,
                Items = new List<IGroupTreeItem>()
                {
                    new Container()
                    {
                        Name = "Container1",
                        IsActive = true,
                        Items = new List<IGroupTreeItem>()
                        {
                            new Container(){Name = "Container2", Size = new SpaceSize()},
                            new Group(){Name = "Group2", Size = new SpaceSize()}
                        },Size = new SpaceSize()
                    },
                    new Group(){Name = "Group", Size = expectedSize, Items = new List<IAccountProjection>()
                    {
                        _accountStub.Object
                    }}
                }
            });
            _mockService.Setup(settings => settings.LoadGroupTree()).Returns(groupSettingsStub);
            _accountRegistryStub.Setup(accountRegistry => accountRegistry.Find("Account1")).Returns(_accountStub.Object);
            _accountStub.SetupGet(accountProjection => accountProjection.Size).Returns(() => expectedSize);
            _accountStub.SetupGet(accountProjection => accountProjection.Login).Returns(() => "Account1");
            _groupContainerStub = new UnityContainer();
            _groupContainerStub.RegisterInstance<IGroupSettings>(_mockService.Object);
            _groupContainerStub.RegisterInstance<IAccountRegistry>(_accountRegistryStub.Object);
            _groupContainerStub.RegisterType<IContainer, Container>(new InjectionConstructor());
            _groupContainerStub.RegisterType<IGroup, Group>(new InjectionConstructor());

            GroupTreeRegistry registry = new GroupTreeRegistry(_groupContainerStub);

            registry.Initialization();

            IContainerProjection rootProjection = registry.GetContainerProjection(new List<string>(), null);
            Assert.IsTrue(expectedContainer.Equals(rootProjection));
        }
        [TestMethod]
        public void Can_Add()
        {
            GroupSettingsContainer groupSettingsStub = null;

            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 30, UsedSize = 70 };

            var forAdd = new Container()
            {
                Name = "Container1",
                IsActive = true,
                Items = new List<IGroupTreeItem>()
                {
                    new Container() {Name = "Container2", Size = new SpaceSize()},
                    new Group() {Name = "Group2", Size = expectedSize}
                },
                Size = expectedSize
            };

            var expectedContainer = new ContainerProjection(new Container()
            {
                Name = "Root",
                Size = expectedSize,
                Items = new List<IGroupTreeItem>()
                {
                    forAdd
                }
            });
            _mockService.Setup(settings => settings.LoadGroupTree()).Returns(groupSettingsStub);
            _groupContainerStub = new UnityContainer();
            _groupContainerStub.RegisterInstance<IGroupSettings>(_mockService.Object);
            _groupContainerStub.RegisterInstance<IAccountRegistry>(_accountRegistryStub.Object);
            _groupContainerStub.RegisterType<IContainer, Container>(new InjectionConstructor());
            _groupContainerStub.RegisterType<IGroup, Group>(new InjectionConstructor());

            GroupTreeRegistry registry = new GroupTreeRegistry(_groupContainerStub);

            registry.Initialization();
            registry.Add(new List<string>(), forAdd);

            IContainerProjection rootProjection = registry.GetContainerProjection(new List<string>(), null);
            Assert.IsTrue(expectedContainer.Equals(rootProjection));
        }

        [TestMethod]
        public void Can_Delete()
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
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 30, UsedSize = 70 };

            var expectedContainer = new ContainerProjection(new Container()
            {
                Name = "Root",
                Size = expectedSize,
                Items = new List<IGroupTreeItem>()
                {
                    new Container()
                    {
                        Name = "Container1",
                        IsActive = true,
                        Items = new List<IGroupTreeItem>()
                        {
                            new Container() {Name = "Container2", Size = new SpaceSize()},
                        },
                        Size = new SpaceSize()
                    },
                    new Group() {Name = "Group", Size = expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object}}
                }
            });
            _mockService.Setup(settings => settings.LoadGroupTree()).Returns(groupSettingsStub);
            _accountRegistryStub.Setup(accountRegistry => accountRegistry.Find("Account1")).Returns(_accountStub.Object);
            _accountStub.SetupGet(accountProjection => accountProjection.Size).Returns(() => expectedSize);
            _accountStub.SetupGet(accountProjection => accountProjection.Login).Returns(() => "Account1");

            _groupContainerStub = new UnityContainer();
            _groupContainerStub.RegisterInstance<IGroupSettings>(_mockService.Object);
            _groupContainerStub.RegisterInstance<IAccountRegistry>(_accountRegistryStub.Object);
            _groupContainerStub.RegisterType<IContainer, Container>(new InjectionConstructor());
            _groupContainerStub.RegisterType<IGroup, Group>(new InjectionConstructor());

            GroupTreeRegistry registry = new GroupTreeRegistry(_groupContainerStub);

            registry.Initialization();
            registry.Delete(new List<string>() { "Container1" }, "Group2", GroupTreeTypeEnum.Group);

            IContainerProjection rootProjection = registry.GetContainerProjection(new List<string>(), null);
            Assert.IsTrue(expectedContainer.Equals(rootProjection));
        }
        [TestMethod]
        public void Can_Rename()
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
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 30, UsedSize = 70 };

            var expectedContainer = new ContainerProjection(new Container()
            {
                Name = "Root",
                Size = expectedSize,
                Items = new List<IGroupTreeItem>()
                {
                    new Container()
                    {
                        Name = "Container1",
                        IsActive = true,
                        Items = new List<IGroupTreeItem>()
                        {
                            new Container() {Name = "Container2", Size = new SpaceSize()},
                            new Group(){Name = "Group3", Size = new SpaceSize() }
                        },
                        Size = new SpaceSize()
                    },
                    new Group() {Name = "Group", Size = expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object}}
                }
            });
            _mockService.Setup(settings => settings.LoadGroupTree()).Returns(groupSettingsStub);
            _accountRegistryStub.Setup(accountRegistry => accountRegistry.Find("Account1")).Returns(_accountStub.Object);
            _accountStub.SetupGet(accountProjection => accountProjection.Size).Returns(() => expectedSize);
            _accountStub.SetupGet(accountProjection => accountProjection.Login).Returns(() => "Account1");

            _groupContainerStub = new UnityContainer();
            _groupContainerStub.RegisterInstance<IGroupSettings>(_mockService.Object);
            _groupContainerStub.RegisterInstance<IAccountRegistry>(_accountRegistryStub.Object);
            _groupContainerStub.RegisterType<IContainer, Container>(new InjectionConstructor());
            _groupContainerStub.RegisterType<IGroup, Group>(new InjectionConstructor());

            GroupTreeRegistry registry = new GroupTreeRegistry(_groupContainerStub);

            registry.Initialization();
            registry.Rename(new List<string>() { "Container1" }, "Group2", GroupTreeTypeEnum.Group, "Group3");

            IContainerProjection rootProjection = registry.GetContainerProjection(new List<string>(), null);
            Assert.IsTrue(expectedContainer.Equals(rootProjection));
        }
        [TestMethod]
        public void Can_Move()
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
                            new GroupSettingsGroup() {Name = "Group2", Items = new List<string>() {"Account1"}}
                        },
                        IsActive = true
                    },
                    new GroupSettingsGroup() {Name = "Group", Items = new List<string>() {"Account1"}}
                }
            };
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 80, UsedSize = 20 };

            var expectedContainer = new ContainerProjection(new Container()
            {
                Name = "Root",
                Size = expectedSize,
                Items = new List<IGroupTreeItem>()
                {
                    new Container()
                    {
                        Name = "Container1",
                        IsActive = true,
                        Items = new List<IGroupTreeItem>()
                        {
                            new Container() {Name = "Container2", Size = new SpaceSize()},
                        },
                        Size = new SpaceSize()
                    },
                    new Group() {Name = "Group", Size =  expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object}},
                    new Group(){Name = "Group2", Size =  expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object} }

                }
            });
            _mockService.Setup(settings => settings.LoadGroupTree()).Returns(groupSettingsStub);
            _accountRegistryStub.Setup(accountRegistry => accountRegistry.Find("Account1")).Returns(_accountStub.Object);
            _accountStub.SetupGet(accountProjection => accountProjection.Size).Returns(() => expectedSize);
            _accountStub.SetupGet(accountProjection => accountProjection.Login).Returns(() => "Account1");

            _groupContainerStub = new UnityContainer();
            _groupContainerStub.RegisterInstance<IGroupSettings>(_mockService.Object);
            _groupContainerStub.RegisterInstance<IAccountRegistry>(_accountRegistryStub.Object);
            _groupContainerStub.RegisterType<IContainer, Container>(new InjectionConstructor());
            _groupContainerStub.RegisterType<IGroup, Group>(new InjectionConstructor());

            GroupTreeRegistry registry = new GroupTreeRegistry(_groupContainerStub);

            registry.Initialization();
            registry.Move(new List<string>() { "Container1" }, "Group2", GroupTreeTypeEnum.Group, new List<string>());

            IContainerProjection rootProjection = registry.GetContainerProjection(new List<string>(), null);
            Assert.IsTrue(expectedContainer.Equals(rootProjection));
        }
        [TestMethod]
        public void Can_Copy()
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
                            new GroupSettingsGroup() {Name = "Group2", Items = new List<string>() {"Account1"}}
                        },
                        IsActive = true
                    },
                    new GroupSettingsGroup() {Name = "Group", Items = new List<string>() {"Account1"}}
                }
            };
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 80, UsedSize = 20 };

            var expectedContainer = new ContainerProjection(new Container()
            {
                Name = "Root",
                Size = expectedSize,
                Items = new List<IGroupTreeItem>()
                {
                    new Container()
                    {
                        Name = "Container1",
                        IsActive = true,
                        Items = new List<IGroupTreeItem>()
                        {
                            new Container() {Name = "Container2", Size = new SpaceSize()},
                            new Group(){Name = "Group2", Size =  expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object} }
                        },
                        Size = expectedSize
                    },
                    new Group() {Name = "Group", Size =  expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object}},
                    new Group(){Name = "Group2", Size =  expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object} }
                }
            });
            _mockService.Setup(settings => settings.LoadGroupTree()).Returns(groupSettingsStub);
            _accountRegistryStub.Setup(accountRegistry => accountRegistry.Find("Account1")).Returns(_accountStub.Object);
            _accountStub.SetupGet(accountProjection => accountProjection.Size).Returns(() => expectedSize);
            _accountStub.SetupGet(accountProjection => accountProjection.Login).Returns(() => "Account1");

            _groupContainerStub = new UnityContainer();
            _groupContainerStub.RegisterInstance<IGroupSettings>(_mockService.Object);
            _groupContainerStub.RegisterInstance<IAccountRegistry>(_accountRegistryStub.Object);
            _groupContainerStub.RegisterType<IContainer, Container>(new InjectionConstructor());
            _groupContainerStub.RegisterType<IGroup, Group>(new InjectionConstructor());

            GroupTreeRegistry registry = new GroupTreeRegistry(_groupContainerStub);

            registry.Initialization();
            registry.Copy(new List<string>() { "Container1" }, "Group2", GroupTreeTypeEnum.Group, new List<string>());

            IContainerProjection rootProjection = registry.GetContainerProjection(new List<string>(), null);
            Assert.IsTrue(expectedContainer.Equals(rootProjection));
        }
        [TestMethod]
        public void Can_SetActive()
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
                            new GroupSettingsGroup() {Name = "Group2", Items = new List<string>() {"Account1"}}
                        },
                        IsActive = true
                    },
                    new GroupSettingsGroup() {Name = "Group", Items = new List<string>() {"Account1"}}
                }
            };
            SpaceSize expectedSize = new SpaceSize() { TotalSize = 100, FreelSize = 80, UsedSize = 20 };

            var expectedContainer = new ContainerProjection(new Container()
            {
                Name = "Root",
                Size = expectedSize,
                Items = new List<IGroupTreeItem>()
                {
                    new Container()
                    {
                        Name = "Container1",
                        IsActive = false,
                        Items = new List<IGroupTreeItem>()
                        {
                            new Container() {Name = "Container2", Size = new SpaceSize()},
                            new Group(){Name = "Group2", Size =  expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object} }
                        },
                        Size = expectedSize
                    },
                    new Group() {Name = "Group", Size =  expectedSize, Items = new List<IAccountProjection>(){_accountStub.Object}},
                }
            });
            _mockService.Setup(settings => settings.LoadGroupTree()).Returns(groupSettingsStub);
            _accountRegistryStub.Setup(accountRegistry => accountRegistry.Find("Account1")).Returns(_accountStub.Object);
            _accountStub.SetupGet(accountProjection => accountProjection.Size).Returns(() => expectedSize);
            _accountStub.SetupGet(accountProjection => accountProjection.Login).Returns(() => "Account1");

            _groupContainerStub = new UnityContainer();
            _groupContainerStub.RegisterInstance<IGroupSettings>(_mockService.Object);
            _groupContainerStub.RegisterInstance<IAccountRegistry>(_accountRegistryStub.Object);
            _groupContainerStub.RegisterType<IContainer, Container>(new InjectionConstructor());
            _groupContainerStub.RegisterType<IGroup, Group>(new InjectionConstructor());

            GroupTreeRegistry registry = new GroupTreeRegistry(_groupContainerStub);

            registry.Initialization();
            registry.SetActive(new List<string>(), "Container1", false);

            IContainerProjection rootProjection = registry.GetContainerProjection(new List<string>(), null);
            Assert.IsTrue(expectedContainer.Equals(rootProjection));
        }
    }
}
