﻿//----------------------
// <auto-generated>
//     Generated using the NSwag toolchain v13.15.5.0 (NJsonSchema v10.6.6.0 (Newtonsoft.Json v9.0.0.0)) (http://NSwag.org)
// </auto-generated>
//----------------------


using StreamChat.Core.InternalDTO.Responses;
using StreamChat.Core.InternalDTO.Events;
using StreamChat.Core.InternalDTO.Models;

namespace StreamChat.Core.InternalDTO.Requests
{
    using System = global::System;

    [System.CodeDom.Compiler.GeneratedCode("NJsonSchema", "13.15.5.0 (NJsonSchema v10.6.6.0 (Newtonsoft.Json v9.0.0.0))")]
    public enum CheckPushRequestPushProviderType
    {

        [System.Runtime.Serialization.EnumMember(Value = @"firebase")]
        Firebase = 0,

        [System.Runtime.Serialization.EnumMember(Value = @"apn")]
        Apn = 1,

        [System.Runtime.Serialization.EnumMember(Value = @"huawei")]
        Huawei = 2,

        [System.Runtime.Serialization.EnumMember(Value = @"xiaomi")]
        Xiaomi = 3,

    }

}
