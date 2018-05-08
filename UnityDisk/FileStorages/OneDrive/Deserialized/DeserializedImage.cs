﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive.Deserialized
{
    [DataContract]
    public class DeserializedImage
    {
        [DataMember(Name = "url")]
        public string Url { get; set; }
    }
}
