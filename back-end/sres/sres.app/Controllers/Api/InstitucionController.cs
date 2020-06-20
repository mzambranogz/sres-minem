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
    [RoutePrefix("api/institucion")]
    public class InstitucionController : ApiController
    {
        [Route("obtenerinstitucionporruc")]
        [HttpGet]
        public InstitucionBE ObtenerInstitucionPorRuc(string ruc)
        {
            return InstitucionLN.ObtenerInstitucionPorRuc(ruc);
        }

        [Route("obtenerinstitucion")]
        [HttpGet]
        public InstitucionBE ObtenerInstitucion(int idInstitucion)
        {
            return InstitucionLN.ObtenerInstitucion(idInstitucion);
        }
    }
}
