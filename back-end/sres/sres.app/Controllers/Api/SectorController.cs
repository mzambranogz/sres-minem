using sres.be;
using sres.ln;
using sres.ut;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/sector")]
    public class SectorController : ApiController
    {
        SectorLN sectorLN = new SectorLN();

        [Route("listarsectorporestado")]
        [HttpGet]
        public List<SectorBE> ListarSectorPorEstado(string flagEstado)
        {
            return sectorLN.ListarSectorPorEstado(flagEstado);
        }
    }
}
