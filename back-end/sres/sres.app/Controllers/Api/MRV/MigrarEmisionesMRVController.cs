using sres.be.MRV;
using sres.ln.MRV;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api.MRV
{
    [RoutePrefix("api/mrv/migraremisiones")]
    public class MigrarEmisionesMRVController : ApiController
    {
        MigrarEmisionesLN migrarLN = new MigrarEmisionesLN();

        [Route("obtenermigraremisiones")]
        [HttpGet]
        public MigrarEmisionesBE ObtenerMigrarEmisiones(string idIniciativas, string rucLogin, int idUsuarioLogin)
        {
            return migrarLN.ObtenerMigrarEmisiones(idIniciativas, rucLogin, idUsuarioLogin);
        }
    }
}
