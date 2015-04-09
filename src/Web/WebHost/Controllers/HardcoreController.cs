using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SE.DSP.Pop.Web.WebHost.Controllers
{
    public class HardcoreController : ApiController
    {
        [HttpGet]
        [AllowAnonymous]
        [Route("api/simple/sejazzmock/{id}")]
        public byte[] Get(string id)
        {
            return new byte[] { 0 };
        }

        [HttpPost]
        [AllowAnonymous]
        [Route("api/simple/sejazzmock/{id}")]
        public void Post(string id, [FromBody]byte[] content)
        {
        }
    }
}
