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
    [RoutePrefix("api/parametro")]
    public class ParametroController : ApiController
    {
        ParametroLN paramLN = new ParametroLN();
        [Route("buscarparametro")]
        [HttpGet]
        public List<ParametroBE> BuscarCaso(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return paramLN.ListaBusquedaParametro(new ParametroBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenerparametro")]
        [HttpGet]
        public ParametroBE ObtenerParametro(int idparametro)
        {
            return paramLN.ObtenerParametro(new ParametroBE() { ID_PARAMETRO = idparametro });
        }

        [Route("guardarparametro")]
        public bool GuardarParametro(ParametroBE entidad)
        {
            return paramLN.GuardarParametro(entidad);
        }

        [Route("obtenerallparametrolista")]
        [HttpGet]
        public List<ParametroBE> ObtenerParametroLista()
        {
            return paramLN.ObtenerParametroLista();
        }
    }
}
