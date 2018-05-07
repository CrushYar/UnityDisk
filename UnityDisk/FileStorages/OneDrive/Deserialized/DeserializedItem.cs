using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace UnityDisk.FileStorages.OneDrive.Deserialized
{
    [DataContract]
    public class DeserializedItem
    {
        [DataMember]
        public string createdDateTime { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public string lastModifiedDateTime { get; set; }
        [DataMember]
        public string name { get; set; }
        [DataMember]
        public ulong size { get; set; }
        [DataMember]
        public string webUrl { get; set; }
        [DataMember]
        [JsonProperty(PropertyName = "@microsoft.graph.downloadUrl")]
        public string downloadUrl { get; set; }
        [DataMember]
        public DeserializedType file { get; set; }
        [DataMember]
        public DeserializedFolder folder { get; set; }
        [DataMember]
        public DeserializedPath parentReference { get; set; }
        [DataMember]
        public DeserializedPackage package { get; set; }
    }

    [DataContract]
    public class DeserializedPath
    {
        [DataMember]
        public string path { get; set; }
    }
    [DataContract]
    public class DeserializedPackage
    {
        [DataMember]
        public string type { get; set; }
    }
    [DataContract]
    public class DeserializedType
    {
        [DataMember]
        public string mimeType { get; set; }
    }
    [DataContract]
    public class DeserializedFolder
    {
        [DataMember]
        public int childCount { get; set; }
    }
}
