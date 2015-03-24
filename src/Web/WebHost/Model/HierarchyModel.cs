using SE.DSP.Pop.Entity.Enumeration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SE.DSP.Pop.Web.WebHost.Model
{
    public class HierarchyModel
    {
        public long Id{get;set;}
        public HierarchyModel[] Children{get;set;}
        public string Code{get;set;}
        public string Comment{get;set;}
        public long CustomerId{get;set;}
        public string Name{get;set;}
        public long ParentId{get;set;}
        public long SpId{get;set;}
        public HierarchyType Type{get;set;}
        public long Version{get;set;}
    }
}