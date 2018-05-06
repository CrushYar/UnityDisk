using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.FactoryRagistry
{
    /// <summary>
    /// Реестр фабрик серверов
    /// </summary>
    public class FactoryRagistry : IFactoryRagistry
    {
        private readonly Dictionary<string, IFileStorageFactory> _ragistries=new Dictionary<string, IFileStorageFactory>();
        
        /// <summary>
        /// Объект синхронизации
        /// </summary>
        private SpinLock _spinLock = new SpinLock(true);

        private void Lock()
        {
            bool taken = false;
            _spinLock.Enter(ref taken);
        }
        private void UnLock()
        {
            _spinLock.Exit();
        }

        public FactoryRagistry()
        {

        }
        public IFileStorageAccount CreateAccount(string serverName)
        {
            Lock();
            _ragistries.TryGetValue(serverName,out IFileStorageFactory factory);
            UnLock();
            return factory?.CreateAccount(serverName);
        }

        public IFileStorageFolder CreateFolder(string serverName)
        {
            Lock();
            _ragistries.TryGetValue(serverName, out IFileStorageFactory factory);
            UnLock();
            return factory?.CreateFolder(serverName);
        }

        public IFileStorageFile CreateFile(string serverName)
        {
            Lock();
            _ragistries.TryGetValue(serverName, out IFileStorageFactory factory);
            UnLock();
            return factory?.CreateFile(serverName);
        }

        public bool Registry(string serverName, IFileStorageFactory factory)
        {
            bool isContains;
            Lock();
            isContains = _ragistries.ContainsKey(serverName);
            if (!isContains)
            {
                _ragistries.Add(serverName, factory);
            }
            UnLock();
            return !isContains;
        }
    }
}
