using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/insignia")]
    public class InsigniaController : ApiController
    {
        InsigniaLN insigniaLN = new InsigniaLN();

        [Route("obtenerallinsignia")]
        [HttpGet]
        public List<InsigniaBE> ObtenerAllInsignia()
        {
            return insigniaLN.getAllInsignia();
        }
    }
}
