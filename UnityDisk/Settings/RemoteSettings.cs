using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace UnityDisk.Settings
{
    public class RemoteSettings : ISettings
    {
        public Stream GetValueAsStream(string parameterName, Encoding encoding = null)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;

            encoding = encoding ?? Encoding.UTF8;
            var bytes = encoding.GetBytes((string)roamingSettings.Values[parameterName]);
            return new MemoryStream(bytes);
        }

        public string GetValueAsString(string parameterName)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            return (string)roamingSettings.Values[parameterName];
        }

        public void SetValueAsStream(string parameterName, Stream value,Encoding encoding=null)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            encoding = encoding ?? Encoding.UTF8;
            using (var stream = new StreamReader(value, encoding))
            {
                roamingSettings.Values[parameterName] = stream.ReadToEnd();
            }
        }

        public void SetValueAsString(string parameterName, string value)
        {
            Windows.Storage.ApplicationDataContainer roamingSettings =
                Windows.Storage.ApplicationData.Current.RoamingSettings;
            roamingSettings.Values[parameterName] = value;
        }
    }
}
