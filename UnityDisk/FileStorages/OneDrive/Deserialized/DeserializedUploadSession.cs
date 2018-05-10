using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive.Deserialized
{
    [DataContract]
    public class DeserializedUploadSession
    {
        [DataMember(Name = "uploadUrl")]
        public string UploadUrl { get; set; }
        [DataMember(Name = "nextExpectedRanges")]
        public string[] NextExpectedRanges { get; set; }
    }
}
