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
    [RoutePrefix("api/convocatoria")]
    public class ConvocatoriaController : ApiController
    {
        AnnoLN annoLN = new AnnoLN();
        CriterioLN criterioLN = new CriterioLN();
        RequerimientoLN requerimientoLN = new RequerimientoLN();
        UsuarioLN usuarioLN = new UsuarioLN();

        [Route("obteneranno")]
        [HttpGet]
        public List<AnnoBE> ObtenerAnno()
        {
            try
            {
                return annoLN.getAllAnno();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Route("obtenerrequerimiento")]
        [HttpGet]
        public List<RequerimientoBE> ObtenerRequerimiento()
        {
            try
            {
                return requerimientoLN.getAllRequerimiento();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [Route("obtenercriterio")]
        [HttpGet]
        public List<CriterioBE> ObtenerCriterio()
        {
            try
            {
                return criterioLN.getAllCriterio();
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        [Route("obtenerevaluador")]
        [HttpGet]
        public List<UsuarioBE> ObtenerEvaluador()
        {
            try
            {
                return usuarioLN.getAllEvaluador();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
