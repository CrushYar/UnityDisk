using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Unity;
using UnityDisk.BackgroundOperation;
using UnityDisk.FileStorages;

namespace UnityDisk.Settings.BackgroundOperations
{
   public class BackgroundOperationDispatcherSettings:IBackgroundOperationDispatcherSettings
   {
       private readonly ISettings _settings;
       private readonly FileStorages.FactoryRagistry.IFactoryRagistry _factoryRagistry;
       public BackgroundOperationDispatcherSettings(ISettings settings)
       {
           _settings = settings;
           IUnityContainer container = ContainerConfiguration.GetContainer().Container;
           _factoryRagistry = container.Resolve<FileStorages.FactoryRagistry.IFactoryRagistry>();
       }
       public BackgroundOperationDispatcherSettings(ISettings settings, FileStorages.FactoryRagistry.IFactoryRagistry factoryRagistry)
       {
           _settings = settings;
           _factoryRagistry = factoryRagistry;
       }
        public void SaveOperations(string paramName, IList<IBackgroundOperation> backgroundOperations)
        {
            var items = new BackgroundOperationSettingsItem[backgroundOperations.Count];

            int i = 0;
            foreach (var operation in backgroundOperations)
            {
                items[i]=new BackgroundOperationSettingsItem()
                {
                    ServerName = operation.RemoteFile.Account.ServerName,
                    State = operation.ToString(),
                    Action=operation.Action
                };
                i++;
            }

            _settings.SetValueAsString(paramName,JsonConvert.SerializeObject(items));
        }

        public IList<IBackgroundOperation> LoadOperations(string paramName)
        {
           string value= _settings.GetValueAsString(paramName);
            BackgroundOperationSettingsItem[] items=new BackgroundOperationSettingsItem[0];
            items =JsonConvert.DeserializeObject(value, items.GetType()) as BackgroundOperationSettingsItem[];

            if(items==null)
                throw new InvalidOperationException("Item did not create the public url");

            List<IBackgroundOperation> result=new List<IBackgroundOperation>(items.Length);
            foreach (var item in items)
            {
                var operation=_factoryRagistry.ParseBackgroundOperation(item.Action, item.State, item.ServerName);
                if(operation!=null)
                    result.Add(operation);
            } 

            return result;
        }
    }
}
