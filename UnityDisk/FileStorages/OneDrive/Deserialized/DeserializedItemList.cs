﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace UnityDisk.FileStorages.OneDrive.Deserialized
{
    [DataContract]
    class DeserializedItemList
    {
        [DataMember(Name = "value")]
        public DeserializedItem[] Value { get; set; }
    }
}
