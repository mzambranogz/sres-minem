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
    [RoutePrefix("api/factorvalor")]
    public class FactorValorController : ApiController
    {
        FactorValorLN factorLN = new FactorValorLN();
        [Route("buscarfactorvalor")]
        [HttpGet]
        public List<ComponenteBE> BuscarComponenteFactor(string busqueda, int registros, int pagina, string columna, string orden)
        {
            return factorLN.ListaBusquedaComponenteFactor(new ComponenteBE() { CANTIDAD_REGISTROS = registros, ORDER_BY = columna, ORDER_ORDEN = orden, PAGINA = pagina, BUSCAR = busqueda == null ? "" : busqueda });
        }

        [Route("obtenerfactorvalor")]
        [HttpGet]
        public FactorBE ObtenerFactorValor(int id)
        {
            return factorLN.getFactorValor(new FactorBE() { ID_FACTOR = id});
        }

        [Route("guardarfactorvalor")]
        public bool GuardarFactorValor(FactorBE entidad)
        {
            return factorLN.GuardarFactorValor(entidad);
        }
    }
}
