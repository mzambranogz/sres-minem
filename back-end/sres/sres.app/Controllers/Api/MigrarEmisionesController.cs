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
    [RoutePrefix("api/migraremisiones")]
    public class MigrarEmisionesController : ApiController
    {
        MigrarEmisionesLN migrarLN = new MigrarEmisionesLN();

        [Route("grabarmigraremisiones")]
        [HttpPost]
        public bool migrarEmisiones(MigrarEmisionesBE entidad)
        {
            return migrarLN.migrarEmisiones(entidad);
        }
    }
}
