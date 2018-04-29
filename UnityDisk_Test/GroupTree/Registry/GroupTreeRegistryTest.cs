using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HyperMock;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using UnityDisk.Settings;

namespace UnityDisk_Test.GroupTree.Registry
{
    [TestClass]
    public class GroupTreeRegistryTest
    {
        private Mock<ISettings> _mockService;
        private string _parameterName;

        [TestInitialize]
        public void BeforeEachTest()
        {
            _mockService = Mock.Create<ISettings>();
            _parameterName = "GroupTreeSettings";
        }
    }
}
