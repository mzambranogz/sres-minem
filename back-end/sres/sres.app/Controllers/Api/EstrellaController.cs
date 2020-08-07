using sres.be;
using sres.ln;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.IO;
using System.Web;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/estrella")]
    public class EstrellaController : ApiController
    {
        EstrellaLN estrellaLN = new EstrellaLN();

        [Route("obtenerallestrella")]
        [HttpGet]
        public List<EstrellaBE> BuscarUsuario()
        {
            return estrellaLN.listarEstrellas();
        }

    }
}
