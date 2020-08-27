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
    [RoutePrefix("api/indicador")]
    public class IndicadorController : ApiController
    {
        IndicadorLN indLN = new IndicadorLN();

        [Route("buscarindicador")]
        [HttpGet]
        public List<IndicadorBE> BuscarIndicador(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return indLN.ListaBusquedaIndicador(new IndicadorBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("guardarindicador")]
        public bool GuardarIndicador(IndicadorBE entidad)
        {
            return indLN.GuardarIndicador(entidad);
        }
    }
}
