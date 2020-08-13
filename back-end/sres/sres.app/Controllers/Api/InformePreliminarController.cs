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
    [RoutePrefix("api/informepreliminar")]
    public class InformePreliminarController : ApiController
    {
        InformePreliminarLN informeLN = new InformePreliminarLN();

        [Route("generarinformepreliminar")]
        [HttpPost]
        public List<InscripcionBE> listaInscripcionConvocatoriaEvaluador(ConvocatoriaBE entidad)
        {
            return informeLN.listaInscripcionConvocatoriaEvaluador(entidad);
        }
    }
}
