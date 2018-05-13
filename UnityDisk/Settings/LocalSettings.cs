using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.Settings
{
   public class LocalSettings:ISettings
    {
        public Stream GetValueAsStream(string parameterName, Encoding encoding = null)
        {
            throw new NotImplementedException();
        }

        public string GetValueAsString(string parameterName)
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            return (string)localSettings.Values[parameterName];
        }

        public void SetValueAsStream(string parameterName, Stream value, Encoding encoding = null)
        {
            throw new NotImplementedException();
        }

        public void SetValueAsString(string parameterName, string value)
        {
            Windows.Storage.ApplicationDataContainer localSettings =
                Windows.Storage.ApplicationData.Current.LocalSettings;
            localSettings.Values[parameterName] = value;
        }
    }
}
