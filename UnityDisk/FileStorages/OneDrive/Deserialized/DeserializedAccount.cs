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
        [DataMember(Name = "userPrincipalName")]
        public string UserPrincipalName { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "quota")]
        public ForDeserializedSize Quota { get; set; }
    }
    [DataContract]
    public class ForDeserializedSize
    {
        [DataMember(Name = "deleted")]
        public ulong Deleted { get; set; }
        [DataMember(Name = "remaining")]
        public ulong Remaining { get; set; }
        [DataMember(Name = "state")]
        public string State { get; set; }
        [DataMember(Name = "total")]
        public ulong Total { get; set; }
        [DataMember(Name = "used")]
        public ulong Used { get; set; }
    }
}
