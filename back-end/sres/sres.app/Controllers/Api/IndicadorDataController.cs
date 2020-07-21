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
    [RoutePrefix("api/indicadordata")]
    public class IndicadorDataController : ApiController
    {
        IndicadorDataLN indicadordataLN = new IndicadorDataLN();

        [Route("calcular")]
        public List<IndicadorDataBE> Calcular(IndicadorBE entidad)
        {
            return indicadordataLN.Calcular(entidad.LIST_INDICADORDATA);
        }
    }
}
