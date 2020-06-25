using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using sres.be;
using sres.ln;

namespace sres.app.Controllers.Api
{
    [RoutePrefix("api/anno")]
    public class AnnoController : ApiController
    {
        AnnoLN annoLN = new AnnoLN();

        [Route("obtenerallanno")]
        [HttpGet]
        public List<AnnoBE> ObtenerAnno()
        {
            try
            {
                return annoLN.getAllAnno();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
