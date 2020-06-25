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
        ConvocatoriaLN convocatoriaLN = new ConvocatoriaLN();

        [Route("guardarconvocatoria")]
        [HttpPost]
        public ConvocatoriaBE GuardarConvocatoria(ConvocatoriaBE obj)
        {
            try
            {
                return convocatoriaLN.RegistroConvocatoria(obj);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }  
        
    }
}
