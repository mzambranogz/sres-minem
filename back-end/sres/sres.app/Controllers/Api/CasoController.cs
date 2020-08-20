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
    [RoutePrefix("api/caso")]
    public class CasoController : ApiController
    {
        CasoLN casoLN = new CasoLN();

        [Route("buscarcaso")]
        [HttpGet]
        public List<CasoBE> BuscarCaso(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return casoLN.ListaBusquedaCaso(new CasoBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }
    }
}
