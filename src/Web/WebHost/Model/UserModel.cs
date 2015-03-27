using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SE.DSP.Pop.BL.API.DataContract;

namespace SE.DSP.Pop.Web.WebHost.Model
{ 
    public class UserModel
    {
        public long Id { get; set; }

        public string RealName { get; set; }

        public long UserType { get; set; }

        public string UserTypeName { get; set; }

        public string Password { get; set; }

        public UserPreviligedCustomerModel[] Customers { get; set; }

        public string Title { get; set; }

        public string Telephone { get; set; }

        public string Email { get; set; }

        public string Comment { get; set; }

        public string Name { get; set; }

        public int DemoStatus { get; set; }

        public long SpId { get; set; }

        public int SpStatus { get; set; }

        public long? Version { get; set; }

        public string GetCookieContent()
        {
            var cookie = string.Format("{0}|{1}|{2}", this.Name, this.Id, this.SpId);
            return cookie;
        }
    }
}