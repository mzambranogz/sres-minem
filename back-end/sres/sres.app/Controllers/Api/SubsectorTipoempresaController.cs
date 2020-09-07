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
    [RoutePrefix("api/subsectortipoempresa")]
    public class SubsectorTipoempresaController : ApiController
    {
        SubsectorTipoempresaLN subsectipoempLN = new SubsectorTipoempresaLN();

        [Route("listasubsetortipoempresa")]
        [HttpGet]
        public List<SubsectorTipoempresaBE> listaSubsectorTipoempresa(int? idSector)
        {
            List<SubsectorTipoempresaBE> lista = new List<SubsectorTipoempresaBE>();
            try
            {
                lista = subsectipoempLN.listaSubsectorTipoempresa(idSector);
            }
            catch (Exception ex)
            {
                Log.Error(ex);
            }

            return lista;
        }
    }
}
