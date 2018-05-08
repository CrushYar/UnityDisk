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
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "link")]
        public DeserializedLink Link { get; set; }
    }

    [DataContract]
    public class DeserializedLink
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
        [DataMember(Name = "webUrl")]
        public string WebUrl { get; set; }
        [DataMember(Name = "application")]
        public DeserializedApplication Application { get; set; }
    }
    [DataContract]
    public class DeserializedApplication
    {
        [DataMember(Name = "displayName")]
        public string DisplayName { get; set; }
    }
}
