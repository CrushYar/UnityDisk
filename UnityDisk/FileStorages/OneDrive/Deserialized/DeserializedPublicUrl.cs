using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive.Deserialized
{
    [DataContract]
    public class DeserializedPublicUrl
    {
        [DataMember]
        public DeserializedPublicUrlItem[] value { get; set; }
    }
    [DataContract]
    public class DeserializedPublicUrlItem
    {
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public DeserializedLink link { get; set; }
    }

    [DataContract]
    public class DeserializedLink
    {
        [DataMember]
        public string type { get; set; }
        [DataMember]
        public string webUrl { get; set; }
    }
}
