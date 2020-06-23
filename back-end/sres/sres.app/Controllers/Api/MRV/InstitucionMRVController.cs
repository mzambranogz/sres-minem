using sres.be.MRV;
using sres.ln.MRV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/mrv/institucion")]
    public class InstitucionMRVController : ApiController
    {
        [Route("obtenerinstitucionporruc")]
        [HttpGet]
        public InstitucionBE ObtenerInstitucionPorRuc(string ruc)
        {
            return InstitucionLN.ObtenerInstitucionPorRuc(ruc);
        }
    }
}
