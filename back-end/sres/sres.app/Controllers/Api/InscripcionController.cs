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
    [RoutePrefix("api/inscripcion")]
    public class InscripcionController : ApiController
    {
        InscripcionLN inscripcionLN = new InscripcionLN();

        [Route("existeinscripcion")]
        [HttpGet]
        public bool ExisteInscripcion(int idConvocatoria, int idInstitucion)
        {
            InscripcionBE inscripcion = inscripcionLN.ObtenerInscripcionPorConvocatoriaInstitucion(idConvocatoria, idInstitucion);
            return inscripcion != null;
        }

        [Route("guardarinscripcion")]
        [HttpPost]
        public bool GuardarInscripcion (InscripcionBE inscripcion)
        {
            return inscripcionLN.GuardarInscripcion(inscripcion);
        }
    }
}
