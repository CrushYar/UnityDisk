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
        [DataMember(Name = "createdDateTime")]
        public string CreatedDateTime { get; set; }
        [DataMember(Name = "id")]
        public string Id { get; set; }
        [DataMember(Name = "lastModifiedDateTime")]
        public string LastModifiedDateTime { get; set; }
        [DataMember(Name = "name")]
        public string Name { get; set; }
        [DataMember(Name = "size")]
        public ulong Size { get; set; }
        [DataMember(Name = "webUrl")]
        public string WebUrl { get; set; }
        [DataMember(Name = "@microsoft.graph.downloadUrl")]
        public string DownloadUrl { get; set; }
        [DataMember(Name = "file")]
        public DeserializedType File { get; set; }
        [DataMember(Name = "folder")]
        public DeserializedFolder Folder { get; set; }
        [DataMember(Name = "parentReference")]
        public DeserializedPath ParentReference { get; set; }
        [DataMember(Name = "package")]
        public DeserializedPackage Package { get; set; }
    }

    [DataContract]
    public class DeserializedPath
    {
        [DataMember(Name = "path")]
        public string Path { get; set; }
    }
    [DataContract]
    public class DeserializedPackage
    {
        [DataMember(Name = "type")]
        public string Type { get; set; }
    }
    [DataContract]
    public class DeserializedType
    {
        [DataMember(Name = "mimeType")]
        public string MimeType { get; set; }
    }
    [DataContract]
    public class DeserializedFolder
    {
        [DataMember(Name = "childCount")]
        public int ChildCount { get; set; }
    }
}
