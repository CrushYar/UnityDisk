using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Accounts.Registry;
using UnityDisk.BackgroundOperation;
using UnityDisk.FileStorages.OneDrive;
using UnityDisk.Settings;
using UnityDisk.Settings.BackgroundOperations;

namespace UnityDisk_Test.Settings.BackgroundOperations
{
    [TestClass]
    public class BackgroundOperationDispatcherSettingsTest
    {
        private Mock<UnityDisk.Accounts.Registry.IAccountProjection> _mockAccount;
        private Mock<UnityDisk.FileStorages.IFileStorageFile> _mockRemoteFile;
        private Mock<ISettings> _mockService;
        private Mock<UnityDisk.FileStorages.FactoryRagistry.IFactoryRagistry> _mockFactoryRegistry;
        private string _parameterName="saveBO";
        private BackgroundOperationDispatcherSettings _dispatcherSettings;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockService = Mock.Create<ISettings>();
            _mockFactoryRegistry = Mock.Create<UnityDisk.FileStorages.FactoryRagistry.IFactoryRagistry>();
            _dispatcherSettings = new BackgroundOperationDispatcherSettings(_mockService.Object, _mockFactoryRegistry.Object);
            _mockAccount = Mock.Create<UnityDisk.Accounts.Registry.IAccountProjection>();
            _mockRemoteFile = Mock.Create<UnityDisk.FileStorages.IFileStorageFile>();
        }
        [TestMethod]
        public void Can_SaveOperations()
        {
            List<IBackgroundOperation> backgroundOperations = new List<IBackgroundOperation>();
            Mock<IBackgroundOperation> mock1 = Mock.Create<IBackgroundOperation>();
            Mock<IBackgroundOperation> mock2 = Mock.Create<IBackgroundOperation>();
            _mockAccount.SetupGet(projection => projection.ServerName).Returns("OneDrive");
            _mockRemoteFile.SetupGet(file => file.Account).Returns(_mockAccount.Object);

            mock1.SetupGet(operation => operation.Action).Returns(BackgroundOperationActionEnum.Download);
            mock1.SetupGet(operation => operation.RemoteFile).Returns(_mockRemoteFile.Object);

            mock2.SetupGet(operation => operation.Action).Returns(BackgroundOperationActionEnum.Upload);
            mock2.SetupGet(operation => operation.RemoteFile).Returns(_mockRemoteFile.Object);

            backgroundOperations.Add(mock1.Object);
            backgroundOperations.Add(mock2.Object);

             _dispatcherSettings.SaveOperations(_parameterName, backgroundOperations);
            _mockService.Verify(settings => settings.SetValueAsString(_parameterName, "[{\"Action\":0,\"ServerName\":\"OneDrive\",\"State\":\"generatedProxy_4\"},{\"Action\":1,\"ServerName\":\"OneDrive\",\"State\":\"generatedProxy_4\"}]"),Occurred.Once());
        }
        [TestMethod]
        public void Can_LoadOperations()
        {
            List<IBackgroundOperation> backgroundOperations = new List<IBackgroundOperation>();
            Mock<UnityDisk.FileStorages.FactoryRagistry.IFactoryRagistry> mockFactory = Mock.Create< UnityDisk.FileStorages.FactoryRagistry.IFactoryRagistry>();

            Mock<IBackgroundOperation> mock1 = Mock.Create<IBackgroundOperation>();
            Mock<IBackgroundOperation> mock2 = Mock.Create<IBackgroundOperation>();
            _mockAccount.SetupGet(projection => projection.ServerName).Returns("OneDrive");
            _mockRemoteFile.SetupGet(file => file.Account).Returns(_mockAccount.Object);

            mock1.SetupGet(operation => operation.Action).Returns(BackgroundOperationActionEnum.Download);
            mock1.SetupGet(operation => operation.RemoteFile).Returns(_mockRemoteFile.Object);

            mock2.SetupGet(operation => operation.Action).Returns(BackgroundOperationActionEnum.Upload);
            mock2.SetupGet(operation => operation.RemoteFile).Returns(_mockRemoteFile.Object);

            mockFactory.Setup(registry => registry.ParseBackgroundOperation(BackgroundOperationActionEnum.Download, "generatedProxy_4","OneDrive")).Returns(mock1.Object);
            mockFactory.Setup(registry => registry.ParseBackgroundOperation(BackgroundOperationActionEnum.Upload, "generatedProxy_4", "OneDrive")).Returns(mock2.Object);

            _mockFactoryRegistry.Setup(ragistry => ragistry.ParseBackgroundOperation(BackgroundOperationActionEnum.Download, "generatedProxy_4", "OneDrive")).Returns(mock1.Object);
            _mockFactoryRegistry.Setup(ragistry => ragistry.ParseBackgroundOperation(BackgroundOperationActionEnum.Upload, "generatedProxy_4", "OneDrive")).Returns(mock2.Object);

            string stubSettings = "[{\"Action\":0,\"ServerName\":\"OneDrive\",\"State\":\"generatedProxy_4\"},{\"Action\":1,\"ServerName\":\"OneDrive\",\"State\":\"generatedProxy_4\"}]";
            _mockService.Setup(settings => settings.GetValueAsString(_parameterName)).Returns(stubSettings);
            var actual= _dispatcherSettings.LoadOperations(_parameterName);
            Assert.IsNotNull(actual);
            Assert.AreEqual(2, actual.Count);

            Assert.AreEqual(mock1.Object, actual[0]);
            Assert.AreEqual(mock2.Object, actual[1]);
        }
    }
}
