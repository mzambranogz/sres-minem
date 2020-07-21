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
    [RoutePrefix("api/parametrodetallerelacion")]
    public class ParametroDetalleRelacionController : ApiController
    {
        ParametroDetalleRelacionLN paramdetrelLN = new ParametroDetalleRelacionLN();

        [Route("filtrar")]
        public List<ParametroDetalleBE> Calcular(ParametroDetalleBE entidad)
        {
            return paramdetrelLN.FiltrarParametroDetalle(entidad.PARAMDETREL);
        }
    }
}
