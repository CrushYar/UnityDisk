using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive.Deserialized
{
    [DataContract]
    public class DeserializedAccount
    {
        [DataMember]
        public string userPrincipalName { get; set; }
        [DataMember]
        public string id { get; set; }
        [DataMember]
        public ForDeserializedSize quota { get; set; }
    }
    [DataContract]
    public class ForDeserializedSize
    {
        [DataMember]
        public ulong deleted { get; set; }
        [DataMember]
        public ulong remaining { get; set; }
        [DataMember]
        public string state { get; set; }
        [DataMember]
        public ulong total { get; set; }
        [DataMember]
        public ulong used { get; set; }
    }
}
