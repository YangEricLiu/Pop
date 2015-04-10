using System;
using System.Runtime.Serialization;
namespace SE.DSP.Foundation.Infrastructure.BE.Enumeration
{
    ////[DataContract(Name = "HierarchyType")]
    public enum HierarchyType
    {
        ////[EnumMember(Value="-1")]
        Customer = -1,

        ////[EnumMember(Value = "0")]
        Organization = 0,

        ////[EnumMember(Value = "1")]
        Site = 1,

        ////[EnumMember(Value = "2")]
        Building = 2,

        ////[EnumMember(Value = "3")]
        Room = 3,

        ////[EnumMember(Value = "4")]
        Cabinet = 4,

        ////[EnumMember(Value = "5")]
        Device = 5
    }
}